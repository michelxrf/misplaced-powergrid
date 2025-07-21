using TMPro;
using UnityEngine;


/// <summary>
/// Shows score on screen.
/// </summary>
public class ScoreHud : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    private LevelManager levelManager;

    private void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
        levelManager.scoreHud = this;
    }
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
