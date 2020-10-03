using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using AMR;

public class SplashScreenController : MonoBehaviour
{
    public GameObject splashParent;
    public Image splashImage;
    //public string sceneName;
    private float fadeInDuration;
    private float fadeOutDuration;
    private float stayDuration;

    //public GameObject progress;
    //public Image progressImage;

    public AsyncOperation Operation;

    IEnumerator Start()
    {

        //progress.SetActive(false);

        fadeInDuration = 1.0f;
        fadeOutDuration = 2.0f;
        stayDuration = 2.0f;
        splashImage.canvasRenderer.SetAlpha(0.0f);
        
        yield return new WaitForSeconds(0.2f);

        FadeIn();
        yield return new WaitForSeconds(fadeInDuration);
        yield return new WaitForSeconds(stayDuration);
        
        FadeOut();
        yield return new WaitForSeconds(fadeOutDuration);

        Timer.Reset();

        AMRSDK.loadBanner(Enums.AMRSDKBannerPosition.BannerPositionBottom, true);

        splashParent.SetActive(false);

        //SceneManager.LoadScene(sceneName);
        //StartCoroutine(LoadProgress());
    }

    void FadeIn()
    {
        splashImage.CrossFadeAlpha(1.0f,fadeInDuration,false);
    }

    void FadeOut()
    {
        splashImage.CrossFadeAlpha(0.0f, fadeOutDuration, false);
    }

    IEnumerator LoadProgress()
    {
        //progress.SetActive(true);
        //progressImage.fillAmount = 0;
        //Operation = SceneManager.LoadSceneAsync(sceneName);
        //while (!Operation.isDone) {
        //    progressImage.fillAmount = Operation.progress;
        //    yield return null;
        //}
        yield break;
    }
}
