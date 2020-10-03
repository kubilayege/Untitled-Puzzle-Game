using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Timer
{
    static float resultFirst = -1;
    static float resultSecond;
    static float defaultTime = 20;
    //public static void TimeCheck(Action<string,bool> onComplete,string args,bool durum)
    public static bool TimeCheck()
    {
        resultSecond = DateTime.Now.Hour * 10000 + DateTime.Now.Minute * 100 + DateTime.Now.Second;
        
        //Debug.Log("Time: " + (resultSecond - resultFirst) + "--" + PlayerDataController.data.adsTime);

        if (PlayerDataController.data.adsTime <= 1)
        {
            PlayerDataController.data.adsTime = defaultTime;
        }

        if (resultSecond - resultFirst >= PlayerDataController.data.adsTime)
        {
            Reset();
            return true;
        }

        return false;
    }
    public static IEnumerator TimeSteps(float totalTime, float stepTime, Action onStart, Action<float> onUpdate, Action onComplete)
    {
        var elapsed = 0.0f;

        if (onStart != null)
            onStart();

        while (elapsed < totalTime)
        {

            if (onUpdate != null)
                onUpdate(elapsed);

            yield return new WaitForSeconds(stepTime);
            elapsed += stepTime;
        }
        if (onComplete != null)
        {
            onComplete();
        }
    }

    public static void Reset()
    {
        resultFirst = DateTime.Now.Hour * 10000 + DateTime.Now.Minute * 100 + DateTime.Now.Second;
    }
}
