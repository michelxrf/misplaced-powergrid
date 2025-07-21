using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Returns to the tittle screen when button's pressed. Used in a "thanks for playing" screen in the end of the game.
/// </summary>
public class Thanks : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
