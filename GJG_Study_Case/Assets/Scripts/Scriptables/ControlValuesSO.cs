using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Control Values", menuName = "ScriptableObjects/Control Values")]
public class ControlValuesSO : ScriptableObject
{
    [Header("Colors")]
    [Range(2, 6)]
    public int k;
    
    [Header("Rows-Columns")]
    [Range(2, 10)]
    public int m;
    [Range(2, 10)]
    public int n;
    [Header("Conditions")]
    public int a;
    public int b;
    public int c;
    
    
    
    public List<Sprite> defaultImage;
    public List<Sprite> secondImage;
    public List<Sprite> thirdImage;
    public List<Sprite> fourthImage;
}
