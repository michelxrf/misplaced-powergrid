using UnityEngine;


/// <summary>
/// Blank pieces allow the player to put new pieces in their spot.
/// </summary>
public class BlankTile : MonoBehaviour
{
    MeshFilter meshFilter;

    Mesh originalMesh;
    [SerializeField] Mesh previewMesh;


    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        originalMesh = meshFilter.mesh;
    }

    public void EnablePreview()
    {
        meshFilter.mesh = previewMesh;
    }

    public void DisablePreview()
    {
        meshFilter.mesh = originalMesh;
    }
}
