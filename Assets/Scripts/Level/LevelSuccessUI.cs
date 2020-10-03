using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSuccessUI : MonoBehaviour
{
    public Text levelResultText;
    public Text watchAdText;
    public static LevelSuccessUI main;
    private void Awake()
    {
        main = this;
    }
    public void UpdateText()
    {
        levelResultText.text = "YOU WON " + Player.main.goldAccuiredThisLevel + " COINS!";
        watchAdText.text = "GET\n " + Player.main.goldAccuiredThisLevel * 2 + " COINS!";

    }
}
