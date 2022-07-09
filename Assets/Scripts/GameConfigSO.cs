using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Data/Config")]
public class GameConfigSO : ScriptableObject
{
    public float moveForce = 1000;
    public float maxThrowForce = 1000;
}
