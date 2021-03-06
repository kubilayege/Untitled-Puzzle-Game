﻿using System;
using System.Collections.Generic;
using UnityEngine;
class BombBooster : Booster
{

    public void Awake()
    {
        Player.onPlayerTryHold.AddListener(TryHoldingBooster);
        Player.PlayerHolding.AddListener(MoveHoldingBooster);
        Player.onPlayerRelease.AddListener(ReleaseHoldingBooster);
    }

    public void ReleaseHoldingBooster()
    {
        if (GameManager.holdingBooster != null)
        {
            BombBooster cl = GameManager.holdingBooster.GetComponent<BombBooster>();
            if (cl == null)
                return;
        }
        else
        {
            return;
        }

        GameManager.playerState = Player.States.Empty;
        RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(GameManager.holdingBooster.transform.position,
                                                                GameManager.holdingBooster.transform.forward),
                                                                10);
        if (hit.collider != null)
        {
            CellManager cell = hit.transform.GetComponent<CellManager>();
            if (cell != null && cell.fruitOnTop == null)
            {
                this.Place(hit.collider.GetComponent<CellManager>().index);
            }
            else
            {
                Destroy(GameManager.holdingBooster);
            }
        }
        else
        {
            Destroy(GameManager.holdingBooster);
        }
        GameManager.holdingBooster = null;
    }

    public void MoveHoldingBooster()
    {
        if (GameManager.holdingBooster == null)
        {
            return;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 pos = GameManager.holdingBooster.transform.position;
        float distance = 0f - mousePos.z;
        Vector3 targetPos = new Vector3(mousePos.x, mousePos.y, -1);

        GameManager.holdingBooster.transform.position = Vector3.Lerp(pos, targetPos + new Vector3(0, 5f, 0), 15 * Time.deltaTime);
    }

    public void TryHoldingBooster()
    {
        Vector2 rayOrigin = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Camera.main.transform.forward);
        if (hit.transform != null)
        {
            if (hit.transform.TryGetComponent<BombBooster>(out BombBooster booster))
            {

                GameManager.holdingBooster = Instantiate(hit.transform.gameObject, hit.transform.position, hit.transform.rotation, LevelManager.instance.activeLevel.transform);
                GameManager.holdingBooster.transform.localScale = new Vector3(0.42f, 0.42f, 0.42f);
                GameManager.playerState = Player.States.Holding;

            }
        }
    }
    public override void Place(CellManager.Pos pos)
    {
        // List<BlockerBlock> breakableBlocks = new List<BlockerBlock>();

        for (int j = -1; j < 2; j++)
        {
            for (int i = -1; i < 2; i++)
            {
                Tuple<int, int> delP = new Tuple<int, int>(pos.x + i, pos.y + j);
                if (delP.Item1 < 0 || delP.Item1 > 8 || delP.Item2 < 0 || delP.Item2 > 8)
                {
                    continue;
                }
                //GameManager.GetBlockersOnTheSides(breakableBlocks, delP);

                if (GameManager.cells[delP].RemoveAndDeleteObjectOnTop())
                {
                    Debug.Log("ended: " + delP.Item1 + " ; " + delP.Item2);
                    return;
                }
            }
        }

        // foreach (var block in breakableBlocks)
        // {
        //     block.Break();
        // }

        Destroy(GameManager.holdingBooster);
    }
}
