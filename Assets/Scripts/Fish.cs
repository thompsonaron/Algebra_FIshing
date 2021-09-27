using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// creates ability to right click and create Fish ScriptableObject in editor directly
[CreateAssetMenu(fileName = "New Fish", menuName = "Fish")]
public class Fish : ScriptableObject
{
    public new string fishName;
    public Sprite image;
    public int totalCaught;
}
