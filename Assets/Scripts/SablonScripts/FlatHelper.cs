using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlatHelper
{
    static System.Random random = new System.Random();
    public enum VibrationType
    {
        Light,
        Medium,
        Success,
        Failure
    }
    public static IEnumerator EaseSnap(this GameObject part, Vector3 targetPos, Vector3 targetScale, float time, bool isShape)
    {
        if (isShape)
        {
            foreach (var partCollider in part.GetComponentsInChildren<BoxCollider2D>())
            {
                partCollider.enabled = false;
            }
        }
        else
        {
            part.GetComponent<BoxCollider2D>().enabled = false;
        }

        float elapsedTime = 0.0f;
        float rate = 1f / time;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime * rate;
            part.transform.localScale = Vector3.Lerp(part.transform.localScale, targetScale, elapsedTime);
            part.transform.position = Vector3.Lerp(part.transform.position, targetPos, elapsedTime);
            yield return null;
            // part.transform.position = Vector3.Lerp(part.transform.position, targetPos, 15 * elapsedTime);
            // part.transform.localScale = Vector3.Lerp(part.transform.localScale, targetScale, 15 * elapsedTime);

            // elapsedTime += 0.01f;
            // yield return new WaitForSeconds(0.01f);

        }

        if (isShape)
        {
            foreach (var partCollider in part.GetComponentsInChildren<BoxCollider2D>())
            {
                partCollider.enabled = true;
            }
        }
        else
        {
            part.GetComponent<BoxCollider2D>().enabled = true;
        }

        if (!(Mathf.Abs(part.transform.localScale.x - targetScale.x) > 0.01f))
        {
            part.transform.localScale = targetScale;
            part.transform.position = targetPos;
        }
        yield return null;
    }

    public static IEnumerator EaseSnap(this GameObject part, Vector3 targetScale, float time, bool isShape)
    {
        if (isShape)
        {
            foreach (var partCollider in part.GetComponentsInChildren<BoxCollider2D>())
            {
                partCollider.enabled = false;
            }
        }
        else
        {
            part.GetComponent<BoxCollider2D>().enabled = false;
        }


        float elapsedTime = 0.0f;
        float rate = 1f / time;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime * rate;
            part.transform.localScale = Vector3.Lerp(part.transform.localScale, targetScale, elapsedTime);
            // part.transform.localScale = Vector3.Lerp(part.transform.localScale, targetScale, 15 * elapsedTime);
            // elapsedTime += 0.005f;
            yield return null;
        }
        if (isShape)
        {
            foreach (var partCollider in part.GetComponentsInChildren<BoxCollider2D>())
            {
                partCollider.enabled = true;
            }
        }
        else
        {
            part.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (!(Mathf.Abs(part.transform.localScale.x - targetScale.x) > 0.01f))
        {
            part.transform.localScale = targetScale;
        }

        yield return null;
    }

    public static IEnumerator EaseScale(this TMPro.TextMeshProUGUI part, Vector3 targetScale, float time, Action onEnd)
    {
        float elapsedTime = 0.0f;
        float rate = 1f / time;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime * rate;
            part.rectTransform.localScale = Vector3.Lerp(part.rectTransform.localScale, targetScale, elapsedTime);
            yield return null;
        }
        onEnd();
    }
    public static IEnumerator EaseScale(this GameObject part, Vector3 targetScale, float time, Action onEnd)
    {
        float elapsedTime = 0.0f;
        float rate = 1f / time;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime * rate;
            part.transform.localScale = Vector3.Lerp(part.transform.localScale, targetScale, elapsedTime);
            yield return null;
        }
        onEnd();
    }
    public static IEnumerator EaseMove(this GameObject part, Vector3 targetPos, float time, Action onEnd)
    {
        float elapsedTime = 0.0f;
        float rate = 1f / time;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime * rate;
            part.transform.position = Vector3.Lerp(part.transform.position, targetPos, elapsedTime);
            yield return null;
        }
        onEnd();
    }
    public static IEnumerator EaseMove(this TMPro.TextMeshProUGUI part, Vector3 targetPos, float time, Action onEnd)
    {
        float elapsedTime = 0.0f;
        float rate = 1f / time;
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime * rate;
            part.rectTransform.position = Vector3.Lerp(part.rectTransform.position, targetPos, elapsedTime);
            yield return null;
        }
        onEnd();
    }
    public static IEnumerator EaseRotate(this GameObject obj, Quaternion targetRot, float time)
    {
        float elapsedTime = 0.0f;
        float rate = 1f / time;
        Debug.Log(elapsedTime);
        while (elapsedTime < 1.0f)
        {
            elapsedTime += Time.deltaTime * rate;
            obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, targetRot, elapsedTime);
            yield return null;
        }

    }



    public static bool ContainsTuple(this List<MyTuples> tuples, ref MyTuples tupleToCheck)
    {
        if (tuples.Count == 0)
        {
            return false;
        }
        foreach (var tuple in tuples)
        {
            if (tuple.x == tupleToCheck.x && tuple.y == tupleToCheck.y)
            {
                tupleToCheck = tuple;
                return true;
            }
        }
        return false;
    }
    public static bool ContainsTuple(this List<MyTuples> tuples, ref Tuple<int, int> tupleToCheck)
    {

        foreach (var tuple in tuples)
        {
            if (tuple.x == tupleToCheck.Item1 && tuple.y == tupleToCheck.Item2)
            {
                return true;
            }
        }
        return false;
    }

    public static MyTuples GetTuple(this List<MyTuples> tuples, int x, int y)
    {
        foreach (var tuple in tuples)
        {
            if (tuple.x == x && tuple.y == y)
            {
                return tuple;
            }
        }
        return default(MyTuples);
    }
    public static bool CastsRayToGet<T>(this GameObject obj, out T cm)
    {
        cm = default(T);
        if (obj == null)
            return false;
        RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(obj.transform.position, Camera.main.transform.forward), 10000f);
        Debug.DrawRay(obj.transform.position, Camera.main.transform.forward, Color.red, 0.3f);
        if (hit.transform != null)
        {
            cm = hit.transform.GetComponent<T>();
            if (cm != null)
                return true;
            else
                return false;
        }
        return false;
    }
    public static T GetRandomEnumType<T>()
    {
        Array values = Enum.GetValues(typeof(T));
        T randomType = (T)values.GetValue(random.Next(values.Length));
        return randomType;
    }
    public static Vector3 Interpolate(Vector3 start, Vector3 end, float t)
    {
        return Vector3.Lerp(start, end, t);
    }
    public static int ToInt(object value)
    {
        var result = 0;

        if (value.GetType() == typeof(bool))
        {
            result = (bool)value == true ? 1 : 0;
        }
        else if (value.GetType() == typeof(string))
        {
            result = int.Parse(value.ToString());
        }
        return result;
    }
    public static bool ToBool(object value)
    {

        var result = false;

        if (value.GetType() == typeof(int))
        {
            result = int.Parse(value.ToString()) == 1 ? true : false;
        }
        else if (value.GetType() == typeof(string))
        {
            result = value.ToString().ToLower() == "true" ? true : false;
        }
        return result;
    }
    public static void ShuffleList<T>(this List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static void ShuffleArray<T>(this T[] array)
    {
        System.Random rng = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = (T)array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }
    public static T FromJson<T>(string jsonFile)
    {
        return JsonUtility.FromJson<T>(jsonFile);
    }
    public static string ToJson<T>(object lis)
    {
        var str = "";

        List<T> list = new List<T>((IEnumerable<T>)lis);

        for (int i = 0; i < list.Count; i++)
        {
            str += JsonUtility.ToJson(list[i]);
            if (i < list.Count - 1)
                str += "|";
        }
        return str;
    }
    public static string ToJson(object obj)
    {
        return JsonUtility.ToJson(obj);
    }
    public static void Vibrate(VibrationType vibrationType)
    {
        switch (vibrationType)
        {
            case VibrationType.Light:
                if (PlayerDataController.data.isVibrationOpen)
                    Taptic.Light();
                break;
            case VibrationType.Medium:
                if (PlayerDataController.data.isVibrationOpen)
                    Taptic.Medium();
                break;
            case VibrationType.Success:
                if (PlayerDataController.data.isVibrationOpen)
                    Taptic.Success();
                break;
            case VibrationType.Failure:
                if (PlayerDataController.data.isVibrationOpen)
                    Taptic.Failure();
                break;
            default:
                break;
        }
    }
}