using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Controls what happens when player drags a piece from the pieces box in the hud.
/// Most of the time this means the player is setting this piece in the board.
/// </summary>
public class DraggableIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] GameObject gridPrefab;
    [SerializeField] int amount = -1;
    [SerializeField] TextMeshProUGUI amountText;
    [SerializeField] GameObject outOfPiecesIcon;
    [SerializeField] Texture2D dragCursor;

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

        Cursor.SetCursor(dragCursor, new Vector2(0,0), CursorMode.Auto);

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

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            BlankTile tile = hit.transform.GetComponent<BlankTile>();
            if (tile != null)
            {
                if (tile != currentlySelectedTile)
                {
                    if (currentlySelectedTile != null)
                    {
                        currentlySelectedTile.DisablePreview();
                    }

                    currentlySelectedTile = tile;
                    tile.EnablePreview();
                }
            }
            else
            {
                if (currentlySelectedTile != null)
                {
                    currentlySelectedTile.DisablePreview();
                    currentlySelectedTile = null;
                }
            }
        }
        else
        {
            Debug.Log("Beep");
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

        levelManager.isDragging= false;

        Cursor.SetCursor(null, new Vector2(0, 0), CursorMode.Auto);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            BlankTile tile = hit.transform.GetComponent<BlankTile>();
            if (tile != null)
            {
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

    void LockPiece()
    {
        locked = true;
        outOfPiecesIcon.SetActive(true);
        amountText.gameObject.SetActive(false);
    }

    private void SetTile(GameObject blankTile)
    {
        buildSound.Play();
        GameObject newTile = Instantiate(gridPrefab, blankTile.transform.position, Quaternion.identity, blankTile.transform.parent);
        levelManager.CountPiece();
        Destroy(blankTile);
    }
}
