using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private LevelManager levelManager;
    private void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
    }

    private void Update()
    {
        if((Input.GetKeyDown(KeyCode.Escape)) || (Input.GetKeyDown(KeyCode.Mouse0)) || (Input.GetKeyDown(KeyCode.Return)) || (Input.GetKeyDown(KeyCode.Mouse1)))
        {
            Destroy(gameObject);
        }
    }
}
