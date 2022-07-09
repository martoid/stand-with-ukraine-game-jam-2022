using DG.Tweening;
using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableInredient : Ingredient
{
    public override void BeginDrag(Vector2 cursorPosition)
    {
        base.BeginDrag(cursorPosition);
        Gameplay.instance.furnance.Prime();
    }

    public override void EndDrag(Vector2 cursorPosition, InteractableDragTarget target)
    {
        foreach (var item in FindObjectsOfType<ActionFinish>())
        {
            item.Unprime();
        }

        base.EndDrag(cursorPosition, target);

        if (target == Gameplay.instance.furnance)
        {
            draggable = false;

            transform.position = target.transform.position;
        }
    }
}
