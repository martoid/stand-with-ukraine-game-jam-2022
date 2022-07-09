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

    Vector2 lastPosition;
    Vector2 deltaPosition;

    public override void Dragging(Vector2 cursorPosition)
    {
        if (isGrating)
        {
            var grater = Gameplay.instance.grater;
            transform.position = new Vector2(
                Mathf.Clamp(cursorPosition.x, grater.transform.position.x - 1f, grater.transform.position.x + 1f),
                Mathf.Clamp(cursorPosition.y, grater.transform.position.y - 1f, grater.transform.position.y + 1f));

            deltaPosition = (Vector2)transform.position - lastPosition;
            lastPosition = transform.position;
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

        if (isGrating)
        {
            OnUsedUp.Invoke();
            OnUsedUp.RemoveAllListeners();
            transform.position = Gameplay.instance.grater.transform.position;
        }
        else
        {
            base.EndDrag(cursorPosition, target);

            if (target == Gameplay.instance.grater)
            {
                Gameplay.instance.grater.inUse = true;

                isGrating = true;

                transform.position = target.transform.position;
            }
        }
    }
}
