using DG.Tweening;
using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Interactable
{
    [SerializeField] SpriteRenderer indicator;
    [SerializeField] Vector2 distanceFadeBounds;

    Vector2 origin;
    private void Awake()
    {
        origin = transform.position;
    }

    public override void BeginDrag(Vector2 cursorPosition)
    {
        base.BeginDrag(cursorPosition);
        Gameplay.instance.cookingPot.Prime();
    }
    public override void EndDrag(Vector2 cursorPosition, InteractableDragTarget target)
    {
        base.EndDrag(cursorPosition, target);
        Gameplay.instance.cookingPot.Unprime();

        if(target is CookingPot)
        {
            ((CookingPot)target).ConsumeSoup();
        }

        draggable = false;
        transform.DOMove(origin, 0.5f).OnComplete(() => draggable = true);
    }
    public override void Dragging(Vector2 cursorPosition)
    {
        base.Dragging(cursorPosition);
        transform.position = cursorPosition;
    }
    private void Update()
    {
        if (Gameplay.instance.isDragging())
        {
            indicator.SetOpacity(0);
        }
        else
        {
            float distt = Vector2.Distance(transform.position, InteractableController.cursorWorldPosition);
            float minOpacity = Mathf.InverseLerp(distanceFadeBounds.y, distanceFadeBounds.x, distt);

            indicator.SetOpacity(minOpacity);
        }
    }
}
