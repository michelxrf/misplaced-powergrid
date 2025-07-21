using UnityEngine;

/// <summary>
/// Destroys the tutorial screen when player press any key
/// </summary>
public class Tutorial : MonoBehaviour
{
    private void Update()
    {
        if((Input.GetKeyDown(KeyCode.Escape)) || (Input.GetKeyDown(KeyCode.Mouse0)) || (Input.GetKeyDown(KeyCode.Return)) || (Input.GetKeyDown(KeyCode.Mouse1)))
        {
            Destroy(gameObject);
        }
    }
}
