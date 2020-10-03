using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    //private void OnMouseDrag()
    //{
    //    float distance = transform.position.z - Camera.main.transform.position.z ;
    //    Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
    //    transform.position = pos;
    //}

    public void OnClickArea()
    {
        switch (GameManager.instance.gameStates)
        {
            case GameManager.GameStates.WAIT_FOR_PLAY:
                GameManager.instance.StartGame();
                break;
            case GameManager.GameStates.PLAY:
                break;
            case GameManager.GameStates.SUCCESS_FINISH:
                break;
            case GameManager.GameStates.FAIL_FINISH:
                break;
            default:
                break;
        }
    }
}
