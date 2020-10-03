using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class LevelEditor : EditorWindow
{

    public LevelData level;
    public Animal[] animalTypes = new Animal[3];
    public int[] missionLenghts = new int[3];
    // public Stack<GameObject> activeBlockers1 = new Stack<GameObject>();
    // public Stack<GameObject> activeBlockers2 = new Stack<GameObject>();
    // public Stack<GameObject> activeBlockers3 = new Stack<GameObject>();
    // public Stack<GameObject> otherBlockers = new Stack<GameObject>();

    public int blockerX;
    public int blockerY;
    public List<GameObject> animalObjects = new List<GameObject>();
    public PartTheme partSprites;

    [MenuItem("Window/LevelEditor #&l")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        LevelEditor window = (LevelEditor)EditorWindow.GetWindow(typeof(LevelEditor));

        window.Show();

    }

    public void OnGUI()
    {

        if (level == null && GUILayout.Button("Create New Level"))
        {
            level = new GameObject("Level", typeof(LevelData)).GetComponent<LevelData>();
            level.animals = new List<Animals>();
            level.animalIndex = new List<int>();
            level.blockerPos = new List<MyTuples>();
            Tools.current = Tool.None;
        }

        if (level != null)
        {
            level = (LevelData)EditorGUILayout.ObjectField("Level", level, typeof(LevelData));
        }

        GUILayout.Space(10);

        partSprites = (PartTheme)EditorGUILayout.EnumPopup("Select Level Theme", partSprites);

        GUILayout.Space(10);

        if (level != null)
        {

            GUILayout.BeginHorizontal();

            // if (lblocker != LevelBlocker.None && GUILayout.Button("Spawn Blocker"))
            // {
            //     string path = "Blockers/" + lblocker.ToString();
            //     Vector3 pos = otherBlockers.Count != 0 ? otherBlockers.Peek().transform.position + new Vector3(2, 0, 0) : new Vector3(-6, 10, 0);
            //     if (pos.x > 10)
            //     {
            //         pos.x = -6;
            //         pos.y -= 2;
            //     }
            //     otherBlockers.Push(new GameObject("Blocker" + otherBlockers.Count, typeof(SpriteRenderer)));
            //     BlockerBlock bl = otherBlockers.Peek().AddComponent<BlockerBlock>();
            //     switch (lblocker)
            //     {
            //         case LevelBlocker.None:
            //             break;
            //         case LevelBlocker.GreyMetall:
            //             bl.type = LevelBlocker.GreyMetall;
            //             bl.blockerType = BlockerType.Unbreakable;
            //             break;
            //         case LevelBlocker.Wood_ikili:
            //             bl.breakTimes = 1;
            //             bl.type = LevelBlocker.Wood_ikili;
            //             bl.blockerType = BlockerType.Breakable;
            //             break;
            //         case LevelBlocker.Wood_Uclu:
            //             bl.breakTimes = 2;
            //             bl.type = LevelBlocker.Wood_Uclu;
            //             bl.blockerType = BlockerType.Breakable;
            //             break;
            //         default:
            //             break;
            //     }
            //     bl.GetComponent<SpriteRenderer>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            //     bl.GetComponent<SpriteRenderer>().sortingOrder = 3;
            //     bl.transform.parent = level.transform;
            //     bl.transform.position = pos;
            //     Selection.activeGameObject = bl.gameObject;
            //     Tools.current = Tool.Move;
            // }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            // if (otherBlockers.Count != 0 && GUILayout.Button("Select Last Blocker"))
            // {
            //     Selection.activeGameObject = otherBlockers.Peek();
            //     Tools.current = Tool.Move;
            // }
            // if (otherBlockers.Count != 0 && GUILayout.Button("Remove Last Blocker"))
            // {
            //     DestroyImmediate(otherBlockers.Pop());

            //     if (otherBlockers.Count != 0)
            //     {
            //         Selection.activeGameObject = otherBlockers.Peek();

            //         Tools.current = Tool.Move;
            //     }
            // }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10);
        animalTypes[0] = (Animal)EditorGUILayout.EnumPopup("MissionAnimal 1", animalTypes[0]);
        if (animalTypes.Length != 0 && animalTypes[0] != Animal.None)
        {
            GUILayout.BeginHorizontal();
            missionLenghts[0] = EditorGUILayout.IntField("Animal Mission ", missionLenghts[0]);
            if (!level.animalIndex.Contains(0))
            {
                level.animalIndex.Add(0);
            }
            // if (GUILayout.Button("Select", GUILayout.MaxWidth(120)))
            // {
            //     Selection.activeGameObject = animalObjects[0];

            //     Tools.current = Tool.Rect;
            // }
            // if (mblocker != MissionBlocker.None && GUILayout.Button("Spawn Blocker"))
            // {
            //     string path = "Blockers/" + mblocker.ToString();
            //     Vector3 pos = activeBlockers1.Count != 0 ? activeBlockers1.Peek().transform.position + new Vector3(2, 0, 0) : new Vector3(-6, 10, 0);
            //     if (pos.x > 10)
            //     {
            //         pos.x = -6;
            //         pos.y -= 2;
            //     }
            //     activeBlockers1.Push(new GameObject("Blocker" + activeBlockers1.Count, typeof(SpriteRenderer)));
            //     MissionBlock mb = activeBlockers1.Peek().AddComponent<MissionBlock>();
            //     mb.stageCount = 1; //kırılma sayısı

            //     mb.GetComponent<SpriteRenderer>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            //     mb.GetComponent<SpriteRenderer>().sortingOrder = 2;
            //     mb.transform.parent = level.transform;
            //     mb.transform.position = pos;
            //     Selection.activeGameObject = activeBlockers1.Peek();
            //     Tools.current = Tool.Move;
            // }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            // if (activeBlockers1.Count != 0 && GUILayout.Button("Select Last Blocker"))
            // {
            //     Selection.activeGameObject = activeBlockers1.Peek();
            //     Tools.current = Tool.Move;
            // }
            // if (activeBlockers1.Count != 0 && GUILayout.Button("Remove Last Blocker"))
            // {
            //     DestroyImmediate(activeBlockers1.Pop());

            //     if (activeBlockers1.Count != 0)
            //     {
            //         Selection.activeGameObject = activeBlockers1.Peek();

            //         Tools.current = Tool.Move;
            //     }
            // }
            GUILayout.EndHorizontal();
        }
        else
        {
            if (level != null && level.animalIndex.Contains(0))
            {
                level.animalIndex.Remove(0);
                level.animalIndex.TrimExcess();
                missionLenghts[0] = 0;
            }
        }
        GUILayout.Space(20);
        animalTypes[1] = (Animal)EditorGUILayout.EnumPopup("MissionAnimal 2", animalTypes[1]);
        if (animalTypes.Length != 0 && animalTypes[1] != Animal.None)
        {
            GUILayout.BeginHorizontal();
            missionLenghts[1] = EditorGUILayout.IntField("Animal Mission ", missionLenghts[1]);
            if (!level.animalIndex.Contains(1))
            {
                level.animalIndex.Add(1);
            }
            // if (GUILayout.Button("Select Animal", GUILayout.MaxWidth(120)))
            // {
            //     Selection.activeGameObject = animalObjects[1];

            //     Tools.current = Tool.Rect;
            // }
            // if (mblocker != MissionBlocker.None && GUILayout.Button("Spawn Blocker"))
            // {
            //     string path = "Blockers/" + mblocker.ToString();
            //     Vector3 pos = activeBlockers2.Count != 0 ? activeBlockers2.Peek().transform.position + new Vector3(2, 0, 0) : new Vector3(0, 4, 0);
            //     if (pos.x > 10)
            //     {
            //         pos.x = -6;
            //         pos.y -= 2;
            //     }
            //     activeBlockers2.Push(new GameObject("Blocker" + activeBlockers2.Count, typeof(SpriteRenderer)));
            //     MissionBlock mb = activeBlockers2.Peek().AddComponent<MissionBlock>();
            //     mb.stageCount = 1; //kırılma sayısı
            //     mb.GetComponent<SpriteRenderer>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            //     mb.GetComponent<SpriteRenderer>().sortingOrder = 2;
            //     mb.transform.parent = level.transform;
            //     mb.transform.position = pos;
            //     Selection.activeGameObject = activeBlockers2.Peek();
            //     Tools.current = Tool.Move;
            // }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            // if (activeBlockers2.Count != 0 && GUILayout.Button("Select Blocker"))
            // {
            //     Selection.activeGameObject = activeBlockers2.Peek();
            //     Tools.current = Tool.Move;
            // }
            // if (activeBlockers2.Count != 0 && GUILayout.Button("Remove Blocker"))
            // {
            //     DestroyImmediate(activeBlockers2.Pop());

            //     if (activeBlockers2.Count != 0)
            //     {
            //         Selection.activeGameObject = activeBlockers2.Peek();

            //         Tools.current = Tool.Move;
            //     }
            // }
            GUILayout.EndHorizontal();
        }
        else
        {
            if (level != null && level.animalIndex.Contains(1))
            {
                level.animalIndex.Remove(1);
                level.animalIndex.TrimExcess();
                missionLenghts[1] = 0;
            }
        }
        GUILayout.Space(20);
        animalTypes[2] = (Animal)EditorGUILayout.EnumPopup("MissionAnimal 3", animalTypes[2]);
        if (animalTypes.Length != 0 && animalTypes[2] != Animal.None)
        {
            GUILayout.BeginHorizontal();
            missionLenghts[2] = EditorGUILayout.IntField("Animal Mission ", missionLenghts[2]);
            if (!level.animalIndex.Contains(2))
            {
                level.animalIndex.Add(2);
            }
            // if (GUILayout.Button("Select Animal", GUILayout.MaxWidth(120)))
            // {
            //     Selection.activeGameObject = animalObjects[2];

            //     Tools.current = Tool.Rect;
            // }
            // if (mblocker != MissionBlocker.None && GUILayout.Button("Spawn Blocker"))
            // {
            //     string path = "Blockers/" + mblocker.ToString();
            //     Vector3 pos = activeBlockers3.Count != 0 ? activeBlockers3.Peek().transform.position + new Vector3(2, 0, 0) : new Vector3(6, -2, 0);
            //     if (pos.x > 10)
            //     {
            //         pos.x = -6;
            //         pos.y -= 2;
            //     }
            //     activeBlockers3.Push(new GameObject("Blocker" + activeBlockers3.Count, typeof(SpriteRenderer)));
            //     MissionBlock mb = activeBlockers3.Peek().AddComponent<MissionBlock>();
            //     mb.stageCount = 1; //kırılma sayısı
            //     mb.GetComponent<SpriteRenderer>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
            //     mb.GetComponent<SpriteRenderer>().sortingOrder = 2;
            //     mb.transform.parent = level.transform;
            //     mb.transform.position = pos;
            //     Selection.activeGameObject = activeBlockers3.Peek();
            //     Tools.current = Tool.Move;
            // }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            // if (activeBlockers3.Count != 0 && GUILayout.Button("Select Blocker"))
            // {
            //     Selection.activeGameObject = activeBlockers3.Peek();
            //     Tools.current = Tool.Move;
            // }
            // if (activeBlockers3.Count != 0 && GUILayout.Button("Remove Blocker"))
            // {
            //     DestroyImmediate(activeBlockers3.Pop());

            //     if (activeBlockers3.Count != 0)
            //     {
            //         Selection.activeGameObject = activeBlockers3.Peek();

            //         Tools.current = Tool.Move;
            //     }
            // }
            GUILayout.EndHorizontal();

        }
        else
        {
            if (level != null && level.animalIndex.Contains(2))
            {
                level.animalIndex.Remove(2);
                level.animalIndex.TrimExcess();
                missionLenghts[2] = 0;
            }
        }
        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        blockerX = EditorGUILayout.IntField("X,Y", blockerX);
        blockerY = EditorGUILayout.IntField("", blockerY, GUILayout.Width(100));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add blocker", GUILayout.Height(20)))
        {
            MyTuples pos = new MyTuples(blockerX, blockerY);
            if (!level.blockerPos.ContainsTuple(ref pos))
                level.blockerPos.Add(pos);

            Debug.Log(level.blockerPos.Count);
        }
        if (GUILayout.Button("Remove blocker", GUILayout.Height(20)))
        {
            MyTuples pos = new MyTuples(blockerX, blockerY);
            if (level.blockerPos.ContainsTuple(ref pos))
                level.blockerPos.Remove(pos);

            Debug.Log(level.blockerPos.Count);

        }
        GUILayout.EndHorizontal();

        GUILayout.Space(30);
        if (GUILayout.Button("Get Animals", GUILayout.Height(40)))
        {
            if (level.animals.Count != 0)
                level.animals.Clear();
            foreach (var anim in animalObjects)
            {
                DestroyImmediate(anim);
            }
            animalObjects.Clear();
            Animal[] anims = animalTypes.Where(x => x != Animal.None).ToArray();
            Debug.Log(anims.Length);
            for (int i = 0; i < anims.Length; i++)
            {
                // Sprite texture = Resources.Load("Animals/" + animalTypes[i].ToString(), typeof(Sprite)) as Sprite;
                // SpriteRenderer newAnim = new GameObject(texture.name).AddComponent<SpriteRenderer>();
                // newAnim.sprite = texture;
                // newAnim.transform.parent = level.transform;

                level.animals.Add((Animals)EditorGUIUtility.Load("Assets/Prefabs/Animals/_AnimalData/" + anims[i] + ".asset"));
                GameObject newAnim = Instantiate(level.animals[i].animal3D, new Vector3(i * 4f + (-4f), 15f, 0f), Quaternion.Inverse(Quaternion.identity));

                animalObjects.Add(newAnim.gameObject);
            }
        }
        GUILayout.Space(10);

        if (level != null)
        {
            level.levelNumber = EditorGUILayout.IntField("Level No ", level.levelNumber);
        }

        if (GUILayout.Button("Save Level"))
        {
            string path = "Assets/Prefabs/Levels/Level" + level.levelNumber + ".prefab";
            // level.blockers1 = activeBlockers1.ToArray();
            // level.blockers2 = activeBlockers2.ToArray();
            // level.blockers3 = activeBlockers3.ToArray();
            // level.otherBlockers = otherBlockers.ToArray();
            level.animalObjects = animalObjects;
            var loadedTheme = Resources.LoadAll("Parts/" + partSprites.ToString(), typeof(Sprite));
            level.levelPartSprites = new List<Sprite>();
            foreach (var theme in loadedTheme)
            {
                level.levelPartSprites.Add(theme as Sprite);
            }
            level.missionLenghts = missionLenghts.Where(x => x != 0).ToArray();
            PrefabUtility.SaveAsPrefabAssetAndConnect(level.gameObject, path, InteractionMode.UserAction);
            PrefabUtility.ApplyObjectOverride(level.gameObject, path, InteractionMode.UserAction);

        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(level);
            EditorSceneManager.MarkSceneDirty(level.gameObject.scene);
        }
    }
}