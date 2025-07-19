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

    BlankTile currentlySelectedTile;
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    /// <summary>
    /// Shows the the selected piece as the player drags a new piece to the board.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        outline.enabled = true;
    }

    /// <summary>
    /// As the player drags a piece over the board a preview is shown where the piece will be set.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
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
        }
    }

    /// <summary>
    /// Deals with player releasing the piece in the board, either sets it or cancels its placement.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
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
            }
        }
        else
        {
            currentlySelectedTile.DisablePreview();
            currentlySelectedTile = null;   
        }

        
        outline.enabled = false;
    }

    private void SetTile(GameObject blankTile)
    {
        GameObject newTile = Instantiate(gridPrefab, blankTile.transform.position, Quaternion.identity, blankTile.transform.parent);
        Destroy(blankTile);
        // TODO: counts score
    }
}
