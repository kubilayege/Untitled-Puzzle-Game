using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animal", menuName = "Animals/CreateNewAnimal")]
public class Animals : ScriptableObject
{
    //public GameObject colorPanel;
    public GameObject animal3D;
    //Temp
    public Sprite animalTexture;

    public Vector3 cageScale;

}