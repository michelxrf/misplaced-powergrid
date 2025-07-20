using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] AudioSource bgm;

    Dictionary<int, int> scores = new Dictionary<int, int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ToggleBGM()
    {
        bgm.mute = !bgm.mute;
    }

    public bool IsBgmMuted()
    {
        return bgm.mute;
    }

    public void RegisterScore(int level, int score)
    {
        scores[level] = score;
    }
}
