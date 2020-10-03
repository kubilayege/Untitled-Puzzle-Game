using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBlock : MonoBehaviour {
    public int stageCount;
    public SpriteRenderer sr;
    public bool Progress () {
        //sr.color = new Color(sr.color.r,
        //sr.color.g,
        //sr.color.b,
        //sr.color.a + ((1f - sr.color.a) / stageCount));
        stageCount--;

        if (stageCount == 0) {
            Destroy (this.gameObject);
            return true;
        }

        return false;
    }
}