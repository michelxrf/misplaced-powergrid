using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Controls what happens when player drags a piece from the pieces box in the hud to the game world.
/// </summary>
public class DraggableIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // the piece
    [SerializeField] GameObject gridPrefab;
    
    // used to limit pieces per level, -1 means unlimited
    [SerializeField] int amount = -1;
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] GameObject outOfPiecesIcon;

    [SerializeField] AudioSource buildSound;
    [SerializeField] AudioSource cancelSound;

    BlankTile currentlySelectedTile;

    private Outline outline;
    bool locked = false;

    private LevelManager levelManager;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;

        amountText.text = amount.ToString();

        // init the icon according to it's inital condition set for the level
        if (amount == -1)
        {
            outOfPiecesIcon.SetActive(false);
            amountText.gameObject.SetActive(false);
        }
        else if(amount == 0)
        {
            LockPiece();
        }
        else
        {
            outOfPiecesIcon.SetActive(false);
            amountText.gameObject.SetActive(true);
            amountText.text = amount.ToString();
        }
    }

    private void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
    }

    /// <summary>
    /// Shows the the selected piece as the player drags a new piece to the board.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (locked)
            return;

        levelManager.isDragging = true;

        outline.enabled = true;
    }

    /// <summary>
    /// As the player drags a piece over the board a preview is shown where the piece will be set.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (locked)
            return;

        // cancels tile preview if dragging over UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (currentlySelectedTile != null)
            {
                currentlySelectedTile.DisablePreview();
                currentlySelectedTile = null;
            }
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            BlankTile tile = hit.transform.GetComponent<BlankTile>();
            if (tile != null)
            {
                // player's pointing at a new blank tile
                if (tile != currentlySelectedTile)
                {
                    if (currentlySelectedTile != null)
                    {
                        // disables preview on the previous tile
                        currentlySelectedTile.DisablePreview();
                    }

                    // enables preview in the new blank tile
                    currentlySelectedTile = tile;
                    tile.EnablePreview();
                }
            }
            // player's pointing at a tile that's not a blank
            else
            {
                // will remove the preview
                if (currentlySelectedTile != null)
                {
                    currentlySelectedTile.DisablePreview();
                    currentlySelectedTile = null;
                }
            }
        }
        // Player's pointing at something, but it's not a tile
        // DISCLAIMER: would be more efficient to filter the Raycast with layers. Pressed for time I made this less than ideal solution.
        else
        {
            // disables the preview
            if (currentlySelectedTile != null)
            {
                currentlySelectedTile.DisablePreview();
                currentlySelectedTile = null;   
            }
        }
    }

    /// <summary>
    /// Deals with player releasing the piece in the board, either sets it or cancels its placement.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if(locked)
            return;

        // cancels tile setting if dropping over UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            cancelSound.Play();
            outline.enabled = false;

            if (currentlySelectedTile != null)
            {
                currentlySelectedTile.DisablePreview();
                currentlySelectedTile = null;
            }
            return;
        }

        levelManager.isDragging= false;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            BlankTile tile = hit.transform.GetComponent<BlankTile>();
            if (tile != null)
            {
                // player dropped the piece in a valid position
                if (tile != currentlySelectedTile)
                {
                    if (currentlySelectedTile != null)
                    {
                        currentlySelectedTile.DisablePreview();
                    }
                    currentlySelectedTile = tile;
                }

                SetTile(currentlySelectedTile.gameObject);
                amount--;
                amountText.text = amount.ToString();

                if (amount == 0)
                    LockPiece();
                    
            }
            // player dropped it in a not valid tile, cancels the piece setting
            else
            {
                cancelSound.Play();
                if (currentlySelectedTile != null)
                {
                    currentlySelectedTile.DisablePreview();
                    currentlySelectedTile = null;
                }
            }
        }
        // player dropped somewhere that's not a tile
        // DISCLAIMER: would be more efficient to filter the Raycast with layers. Pressed for time I made this less than ideal solution.
        else
        {
            if (currentlySelectedTile != null)
            {
                cancelSound.Play();
                currentlySelectedTile.DisablePreview();
                currentlySelectedTile = null;
            }
        }

        
        outline.enabled = false;
    }

    /// <summary>
    /// Shows the piece as unavailable in the hud and prevents its use
    /// </summary>
    void LockPiece()
    {
        locked = true;
        outOfPiecesIcon.SetActive(true);
        amountText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Places a new piece in the game world.
    /// </summary>
    /// <param name="blankTile">The blank tile that will be replaced by the new piece.</param>
    private void SetTile(GameObject blankTile)
    {
        buildSound.Play();
        GameObject newTile = Instantiate(gridPrefab, blankTile.transform.position, Quaternion.identity, blankTile.transform.parent);
        levelManager.CountPiece();
        Destroy(blankTile);
    }
}
