using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food",menuName ="Create Food")]
public class Food : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public int Price;
}
