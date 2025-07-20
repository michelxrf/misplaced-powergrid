using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hud : MonoBehaviour
{
    [SerializeField] GameObject gameEndScreen;
    [SerializeField] GameObject scoreBox;
    [SerializeField] GameObject gridBox;
    [SerializeField] GameObject pauseScreen;

    [SerializeField] TextMeshProUGUI finalScore;

    private void Start()
    {
        gridBox.SetActive(true);
        scoreBox.SetActive(true);
        pauseScreen.SetActive(false);
        gameEndScreen.SetActive(false);

        LevelManager.Instance.gameEndScreen = gameEndScreen;
        LevelManager.Instance.pauseScreen = pauseScreen;
        LevelManager.Instance.scoreScreen = scoreBox;
        LevelManager.Instance.gridBox = gridBox;
    }

    public void Unpause()
    {
        LevelManager.Instance.Pause(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {

        SceneManager.LoadScene(1);
    }

    public void Score(int score)
    {
        finalScore.text = score.ToString();
    }
}
