using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridConstructer : MonoBehaviour
{
    public GameObject cagePrefab;
    public GameObject gridParent;
    public static GridConstructer instance;
    public Coroutine gridCor;
    public Texture2D[] gridTextures;
    void Awake()
    {
        instance = this;
    }

    public void Init(LevelData level, bool tutorial)
    {

        // level.InitLevel();
        // LevelManager.instance.activeLevelData = level;

        if (gridParent != null)
        {
            Destroy(gridParent);
        }

        gridParent = new GameObject("grid");
        SpriteRenderer gridCerceve = new GameObject("gridCerceve").AddComponent<SpriteRenderer>();
        gridCerceve.sprite = Resources.Load("General Textures/gridCerceve", typeof(Sprite)) as Sprite;
        gridCerceve.sortingOrder = 5;
        gridCerceve.transform.parent = gridParent.transform;
        gridCerceve.transform.position = new Vector3(2.06f, 2.06f, 0f);
        gridCerceve.transform.localScale *= 0.9922501f;


        foreach (var UIText in UIManager.instance.missionCounts)
        {
            UIText.text = "0";
        }
        foreach (var button in UIManager.instance.missionCheckMarks)
        {
            button.gameObject.SetActive(false);
        }
        #region Standart Level Init
        for (int i = 0; i < level.animals.Count; i++)
        {
            level.missions.Add(
                new Mission(
                    level.animals[i],
                    UIManager.instance.missionCounts[level.animalIndex[i]], UIManager.instance.missionCheckMarks[level.animalIndex[i]], level.missionLenghts[i])
            );

            level.missions[i].SetAnimalModel(
                Instantiate(level.animals[i].animal3D,
                    GameManager.instance.animalpositions[level.animalIndex[i]],
                    Quaternion.Euler(8, -165, 0),
                    level.transform),
                Instantiate(cagePrefab,
                    GameManager.instance.animalpositions[level.animalIndex[i]],
                    Quaternion.Euler(8, -165, 0),
                    level.transform)
            );

        }
        foreach (var button in UIManager.instance.missionCheckMarks)
        {
            button.gameObject.SetActive(false);
        }
        foreach (var UIText in UIManager.instance.missionCounts)
        {
            if (UIText.text == "0")
                UIText.transform.parent.gameObject.SetActive(false);
            else
                UIText.transform.parent.gameObject.SetActive(true);
        }

        #endregion

        #region Grid Initilization
        GameManager.cells = new Dictionary<Tuple<int, int>, CellManager>();
        int index = 0;
        int k = 0;
        for (int i = -1; i < 2; i++)
        {
            bool increase = false;
            if ((i + 1) < level.animalIndex.Capacity && (i + 1) == level.animalIndex[k])
            {
                increase = true;
            }
            for (int j = -1; j < 2; j++)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        index++;
                        Sprite newSprite = Sprite.Create(gridTextures[(Mathf.Abs(i) + Mathf.Abs(j)) % 2],
                            new Rect(x * 200, y * 200, 200, 200),
                            new Vector2(0.5f, 0.5f), 100);
                        GameObject spritePart = new GameObject("spritePart" + (x * i + y * j));
                        Tuple<int, int> pos = new Tuple<int, int>((x + 3 * (i + 1)), (y + 3 * (j + 1)));
                        // Debug.Log(pos.Item1 + "," + pos.Item2);
                        spritePart.transform.position = new Vector3((x + 3 * i) * 2, (y + 3 * j) * 2, 1);
                        spritePart.transform.parent = gridParent.transform;
                        spritePart.name = "Cell" + (index);
                        spritePart.gameObject.tag = "Placeable";
                        SpriteRenderer sr = spritePart.AddComponent<SpriteRenderer>();
                        CellManager cm = spritePart.AddComponent<CellManager>();
                        BoxCollider2D bc = spritePart.AddComponent<BoxCollider2D>();
                        sr.sprite = newSprite;
                        bc.size = new Vector2(2f, 2f);
                        GameManager.cells.Add(pos, cm);
                        GameManager.cells[pos].index.x = pos.Item1;
                        GameManager.cells[pos].index.y = pos.Item2;
                        if (!level.blockerPos.ContainsTuple(ref pos))
                        {
                            if ((i + 1) == level.animalIndex[k])
                                GameManager.cells[pos].mission = level.missions[k];
                            else
                                GameManager.cells[pos].mission = null;

                            GameManager.cells[pos].blocked = false;

                        }
                        else
                        {
                            GameManager.cells[pos].mission = null;
                            GameManager.cells[pos].blocked = true;
                            sr.color = Color.black;
                            GameManager.emptyCellSlots--;
                        }

                        GameManager.cells[pos].fruitOnTop = null;
                        GameManager.cells[pos].isFull = false;
                    }
                }
            }
            if (increase)
            {
                k++;
            }
        }
        #endregion 

        AnimateMissions(level);

    }
    private void AnimateMissions(LevelData level)
    {
        foreach (var mission in level.missions)
        {
            mission.AnimateAnimal();
        }
    }

}