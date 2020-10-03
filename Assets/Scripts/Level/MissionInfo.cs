using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionInfo : MonoBehaviour
{
    public Image missionAnimalImage;
    public Text missionCountText;

    public void Init(Sprite texture, int Count)
    {
        missionAnimalImage.sprite = texture;
        missionCountText.text = Count.ToString();
    }

}
