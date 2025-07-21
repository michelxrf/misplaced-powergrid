using System.Collections;
using UnityEngine;


/// <summary>
/// A scripted animation of the splash screen on the game start.
/// </summary>
public class SplashScreenController : MonoBehaviour
{
    public CanvasGroup logo1;
    public CanvasGroup logo2;
    public CanvasGroup mainMenu;

    public float fadeDuration = 1f;
    public float logoDisplayTime = 1f;

    private void Awake()
    {
        logo1.alpha = 0;
        logo2.alpha = 0;
        mainMenu.alpha = 0;
        mainMenu.interactable = false;
    }

    private void Start()
    {
        StartCoroutine(PlaySplashSequence());
    }

    IEnumerator PlaySplashSequence()
    {
        yield return new WaitForSeconds(logoDisplayTime);
        yield return StartCoroutine(FadeInOut(logo1));
        yield return StartCoroutine(FadeInOut(logo2));
        yield return StartCoroutine(FadeIn(mainMenu));
        mainMenu.interactable = true;
    }

    IEnumerator FadeInOut(CanvasGroup canvas)
    {
        yield return StartCoroutine(FadeIn(canvas));
        yield return new WaitForSeconds(logoDisplayTime);
        yield return StartCoroutine(FadeOut(canvas));
    }

    IEnumerator FadeIn(CanvasGroup canvas)
    {
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        canvas.alpha = 1f;
    }

    IEnumerator FadeOut(CanvasGroup canvas)
    {
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }
        canvas.alpha = 0f;
    }
}
