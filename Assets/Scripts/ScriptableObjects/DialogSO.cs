using UnityEngine;

[CreateAssetMenu(fileName = "DialogSO", menuName = "Data/DialogSO", order = 1)]
public class DialogSO : ScriptableObject
{
    public DialogText[] messages;
}