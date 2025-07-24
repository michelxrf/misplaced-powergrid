using UnityEngine;


/// <summary>
/// Blank pieces allow the player to put new pieces in their spot.
/// </summary>
public class BlankTile : MonoBehaviour
{
    [SerializeField] GameObject preview;

    private void Awake()
    {
        preview.SetActive(false);
    }

    public void EnablePreview()
    {
        preview.SetActive(true);
    }

    public void DisablePreview()
    {
        preview.SetActive(false);
    }
}
