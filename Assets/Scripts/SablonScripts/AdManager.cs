using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    public static AdManager instance;
    public GameObject Simulator;
    public enum AD_STATES
    {
        READY,
        NOT_READY,
        COMPLETE,
        NOT_COMPLETE
    }

    public Action<string, AD_STATES> onComplete;

    bool isIntersititialOpen = false;
    Coroutine testCoroutine;

    #region TEST_VARIABLE_AREA

    #endregion
    public void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public bool IsRewardedReady()
    {
        return PlayerDataController.data.isTest || AMR.AMRSDK.isRewardedVideoReady();
    }
    public bool IsIntersititialReady()
    {
        return PlayerDataController.data.isTest || AMR.AMRSDK.isInterstitialReady();
    }
    public void ShowIntersititial(string whichAd, Action<string, AD_STATES> onState, Action<string, AD_STATES> onCmplt)
    {

        if (onCmplt != null)
        {
            onComplete = onCmplt;
        }

        PlayerDataController.SaveData("whichVideoFor", whichAd);

        if (PlayerDataController.data.isTest && Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android && Timer.TimeCheck())
        {
            if (onState != null)
            {
                onState(PlayerDataController.data.whichVideoFor, AD_STATES.READY);
            }
            isIntersititialOpen = true;
            Simulator.SetActive(true);
            Simulator.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Intersitial Ad";
            Simulator.transform.Find("FinishItNow").gameObject.SetActive(true);
            AMR.AMRSdkConfig.instance.OnInterstitialShow();

            if (testCoroutine != null)
                StopCoroutine(testCoroutine);
            testCoroutine = StartCoroutine(TestAdsDelay(PlayerDataController.data.intersitialDurationForTest));
            return;
        }
        if (AMR.AMRSDK.isInterstitialReady() && Timer.TimeCheck())
        {
            if (onState != null)
            {
                onState("", AD_STATES.READY);
            }
        }
        else
        {
            if (onState != null)
            {
                onState("", AD_STATES.NOT_READY);
            }
        }
    }
    public void ShowRewarded(string rewardName, Action<string, AD_STATES> onState, Action<string, AD_STATES> onCmplt)
    {

        if (onCmplt != null)
        {
            onComplete = onCmplt;
        }

        if (PlayerDataController.data.isTest && Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
        {
            if (onState != null)
            {
                onState(rewardName, AD_STATES.READY);
            }
            isIntersititialOpen = false;
            PlayerDataController.SaveData("whichVideoFor", rewardName);
            Debug.Log("==========" + PlayerDataController.GetData<string>("whichVideoFor").ToString());
            Simulator.SetActive(true);
            Simulator.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Rewarded Video Ad";

            AMR.AMRSdkConfig.instance.OnVideoShow();

            if (testCoroutine != null)
                StopCoroutine(testCoroutine);

            Simulator.transform.Find("FinishItNow").gameObject.SetActive(true);

            testCoroutine = StartCoroutine(TestAdsDelay(PlayerDataController.data.rewardedDurationForTest));
            return;
        }

        if (AMR.AMRSDK.isRewardedVideoReady())
        {
            if (onState != null)
            {
                onState(rewardName, AD_STATES.READY);
            }
        }
        else
        {
            if (onState != null)
            {
                onState(rewardName, AD_STATES.NOT_READY);
            }
        }
    }
    IEnumerator TestAdsDelay(float duration)
    {
        int elapsed = (int)duration;
        while (elapsed > -1)
        {
            Simulator.transform.Find("Time").GetComponent<Text>().text = "(" + elapsed + " sn)";
            elapsed--;
            yield return new WaitForSeconds(1);
        }

        Simulator.transform.Find("Time").GetComponent<Text>().text = "";

        if (isIntersititialOpen)
        {
            Simulator.transform.Find("FinishItNow").gameObject.SetActive(false);
        }
        else
        {
            RunVideo();
        }
    }
    public void FinishItNow()
    {
        if (testCoroutine != null)
            StopCoroutine(testCoroutine);

        Simulator.transform.Find("Time").GetComponent<Text>().text = "";

        if (isIntersititialOpen)
        {
            Simulator.transform.Find("FinishItNow").gameObject.SetActive(false);
        }
        else
        {
            RunVideo();
        }
    }
    public void CloseSimulator()
    {
        if (testCoroutine != null)
            StopCoroutine(testCoroutine);

        if (isIntersititialOpen)
        {
            RunIntersititial();
        }
        else
        {
            AMR.AMRSdkConfig.instance.OnVideoDismiss();
            Simulator.SetActive(false);
        }
    }

    public void RunVideo()
    {
        AMR.AMRSdkConfig.instance.OnVideoComplete();
        Simulator.SetActive(false);
    }
    public void RunIntersititial()
    {
        AMR.AMRSdkConfig.instance.OnInterstitialDismiss();
        Simulator.SetActive(false);
    }
}
