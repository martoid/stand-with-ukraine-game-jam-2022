using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Action
{
    saltAdded = 0,
    pepperAdded = 1,
    onionAdded = 2,
    carrotAdded = 3,
    cabbageAdded = 4,
    beetAdded = 5,
    ingredientGrab = 6,
}

[Serializable]
public class ActionData
{
    public Action actionKey;
    public string actionMessage;
    public int requiredAmount;
    [HideInInspector] public int timesPerformed = 0;
    [HideInInspector] public bool alreadyShowed = false;
}

public class ActionsManager : MonoBehaviour
{
    [SerializeField] private ActionSO actions;

    private void Start()
    {
        ResetActions();

        Gameplay.instance.OnActionPerformed.AddListener(ActionPerformed);
    }

    public void ActionPerformed(Action key)
    {
        var action = FindAction(key);

        action.timesPerformed++;

        if(action.timesPerformed >= action.requiredAmount && !action.alreadyShowed)
        {
            action.alreadyShowed = true;

            Gameplay.instance.OnCharacterSpeak.Invoke(action.actionMessage);
        }
    }

    private ActionData FindAction(Action key)
    {
        foreach (var action in actions.actions)
        {
            if (action.actionKey == key)
                return action;
        }

        return null;
    }

    private void ResetActions()
    {
        foreach (var action in actions.actions)
        {
            action.alreadyShowed = false;
            action.timesPerformed = 0;
        }
    }
}
