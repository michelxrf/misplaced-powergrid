using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] AudioSource bgm;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ToggleBGM()
    {
        bgm.mute = !bgm.mute;
    }

    public bool IsBgmMuted()
    {
        return bgm.mute;
    }
}
