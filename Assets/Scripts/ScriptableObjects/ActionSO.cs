using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionSO", menuName = "Data/ActionSO", order = 1)]
public class ActionSO : ScriptableObject
{
    public ActionData[] actions;
}