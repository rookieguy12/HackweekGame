using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achievement", menuName = "Create an Achievement file")]
public class Achievement : ScriptableObject
{
    [Header("收集:")]
    int cherryMaxCollection;
    int diamondMaxCollection;
    int rabitTotalCollection;
}
