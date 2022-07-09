using DG.Tweening;
using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnance : ActionFinish
{
    [SerializeField] int burnTimePerPlank = 20;
    public override void InteractableDraggedOn(Interactable interactable)
    {
        base.InteractableDraggedOn(interactable);
        Destroy(interactable.gameObject);
        Gameplay.instance.cookingPot.remainingFireSeconds += burnTimePerPlank;
    }
}
