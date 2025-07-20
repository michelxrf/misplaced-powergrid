using TMPro;
using UnityEngine;


/// <summary>
/// Shows score on hud
/// </summary>
public class ScoreHud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    private void Start()
    {
        LevelManager.Instance.scoreHud = this;
    }
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
