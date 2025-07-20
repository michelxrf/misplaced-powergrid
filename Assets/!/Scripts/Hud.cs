using UnityEngine;
using UnityEngine.SceneManagement;

public class Hud : MonoBehaviour
{
    [SerializeField] GameObject gameEndScreen;
    [SerializeField] GameObject scoreBox;
    [SerializeField] GameObject gridBox;
    [SerializeField] GameObject pauseScreen;

    private void Start()
    {
        LevelManager.Instance.gameEndScreen = gameEndScreen;
        LevelManager.Instance.pauseScreen = pauseScreen;
        LevelManager.Instance.scoreScreen = scoreBox;
        LevelManager.Instance.gridBox = gridBox;
    }

    public void Unpause()
    {
        LevelManager.Instance.Pause(false);
    }

    public void Quit()
    {
        SceneManager.LoadScene(1);
    }
}
