using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the in game menus, like pause pause menu, game end screen and updates the score on the hud.
/// </summary>
public class Hud : MonoBehaviour
{
    [SerializeField] GameObject gameEndScreen;
    [SerializeField] GameObject scoreBox;
    [SerializeField] GameObject gridBox;
    [SerializeField] GameObject pauseScreen;

    [SerializeField] TextMeshProUGUI finalScore;
    private LevelManager levelManager;

    private void Start()
    {
        gridBox.SetActive(true);
        scoreBox.SetActive(true);
        pauseScreen.SetActive(false);
        gameEndScreen.SetActive(false);

        levelManager = FindFirstObjectByType<LevelManager>();

        levelManager.gameEndScreen = gameEndScreen;
        levelManager.pauseScreen = pauseScreen;
        levelManager.scoreScreen = scoreBox;
        levelManager.gridBox = gridBox;
    }

    public void Unpause()
    {
        levelManager.Pause(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {

        SceneManager.LoadScene(0);
    }

    public void Score(int score)
    {
        finalScore.text = score.ToString();
    }

    public void Nextlevel()
    {
        levelManager.NextLevel();
    }
}
