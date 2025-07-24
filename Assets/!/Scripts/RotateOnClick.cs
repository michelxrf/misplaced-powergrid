using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Rotates 90 deg on Y once object gets clicked, used to rotate powergrid pieces
/// </summary>
public class RotateOnClick : MonoBehaviour
{
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
    }
    void Update()
    {
        if (levelManager.isGameover || levelManager.isPaused || levelManager.isDragging)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            // prevents rotation when clicking through the UI, it happened if theres a piece under the tile box ui
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    transform.Rotate(0f, 90f, 0);
                    // Tells the game to refresh all connections
                    levelManager.UpdateConnections();
                }
            }
        }
    }
}
