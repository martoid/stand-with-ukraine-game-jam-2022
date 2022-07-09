using DG.Tweening;
using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GratedIngredient : Ingredient
{
    bool isGrated = false;
    bool isGrating = false;

    public override void BeginDrag(Vector2 cursorPosition)
    {
        base.BeginDrag(cursorPosition);

        if (isGrated)
        {
            Gameplay.instance.cookingPot.Prime();
        }
        else
        {
            Gameplay.instance.grater.Prime();
        }
    }

    public override void Dragging(Vector2 cursorPosition)
    {
        if (isGrating)
        {
            //var collider = Gameplay.instance.grater.GetComponent<CircleCollider2D>();
            //var min = col.center - col.size * 0.5f;
            //var max = col.center + col.size * 0.5f;

            //transform.position = new Vector2(
            //    Mathf.Clamp(cursorPosition.x, collider., collider.bounds.max.x), 
            //    Mathf.Clamp(cursorPosition.y, collider.bounds.min.y, collider.bounds.max.y));
        }
        else
        {
            base.Dragging(cursorPosition);
        }
    }

    public override void EndDrag(Vector2 cursorPosition, InteractableDragTarget target)
    {
        foreach (var item in FindObjectsOfType<ActionFinish>())
        {
            item.Unprime();
        }

        base.EndDrag(cursorPosition, target);

        if(target == Gameplay.instance.grater)
        {
            Gameplay.instance.grater.inUse = true;

            isGrating = true;

            transform.position = target.transform.position;
        }
    }
}
