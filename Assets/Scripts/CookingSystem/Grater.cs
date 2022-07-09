using DG.Tweening;
using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grater : ActionFinish
{
    public Transform gratePlacement;
    public bool inUse { get; set; }

    public override void Prime()
    {
        if (!inUse)
        {
            base.Prime();
        }
    }
    public override void InteractableDragHover(Interactable interactable)
    {
        base.InteractableDragHover(interactable);
        ((GratedIngredient)interactable).isGrating = true;
    }
    public override void InteractableDragUnhover(Interactable interactable)
    {
        base.InteractableDragUnhover(interactable);
        ((GratedIngredient)interactable).isGrating = false;
    }
}
