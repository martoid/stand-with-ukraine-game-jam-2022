using DG.Tweening;
using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnance : ActionFinish
{
    public override void InteractableDraggedOn(Interactable interactable)
    {
        base.InteractableDraggedOn(interactable);
        Destroy(interactable.gameObject);
        print("Fire increase");
    }
}
