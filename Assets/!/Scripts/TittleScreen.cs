using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        PreviousLevel();
    }
    public void NextLevel()
    {
        level++;

        level = Mathf.Clamp(level, 1, 7);
        levelText.text = $"Level {level}";
    }

    public void PreviousLevel()
    {
        level--;

        level = Mathf.Clamp(level, 1, 7);
        levelText.text = $"Level {level}";
    }

    public void StartGame()
    {
        SceneManager.LoadScene(level);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToggleMusic()
    {
        GameManager.Instance.ToggleBGM();
        noMusic.SetActive(GameManager.Instance.IsBgmMuted());
    }

    public void ToggleSound()
    {
        noSound.SetActive(!noSound.activeInHierarchy);
        AudioListener.volume = noSound.activeInHierarchy ? 0 : 1;
    }
}
