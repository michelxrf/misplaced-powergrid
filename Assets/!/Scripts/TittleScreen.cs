using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Controls the menu in the tittle screen, like setting the level and starting the game on click
/// </summary>
public class TittleScreen : MonoBehaviour
{
    public int level = 1;


    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject noMusic;
    [SerializeField] GameObject noSound;

    private void Start()
    {
        noMusic.SetActive(GameManager.Instance.IsBgmMuted());
        noSound.SetActive(AudioListener.volume == 0f);

        // used to just refresh the level to the selector
        PreviousLevel();
    }

    /// <summary>
    /// Increments the picked level and updates it on display
    /// </summary>
    public void NextLevel()
    {
        level++;

        level = Mathf.Clamp(level, 1, 7);
        levelText.text = $"Level {level}";
    }

    /// <summary>
    /// Decrements the picked level and updates it on display
    /// </summary>
    public void PreviousLevel()
    {
        level--;

        level = Mathf.Clamp(level, 1, 7);
        levelText.text = $"Level {level}";
    }

    /// <summary>
    /// Loads the chosen level
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(level);
    }

    /// <summary>
    /// Closes the game
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Toggles music on and off
    /// </summary>
    public void ToggleMusic()
    {
        GameManager.Instance.ToggleBGM();
        noMusic.SetActive(GameManager.Instance.IsBgmMuted());
    }

    /// <summary>
    /// Toggles all sounds on and off
    /// </summary>
    public void ToggleSound()
    {
        noSound.SetActive(!noSound.activeInHierarchy);
        AudioListener.volume = noSound.activeInHierarchy ? 0 : 1;
    }
}
