using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PotRefill : Interactable
{
    [SerializeField] SpriteRenderer indicator;
    [SerializeField] Vector2 distanceFadeBounds;
    [SerializeField] TextMeshPro text;

    private void Awake()
    {
        draggable = false;
        text.alpha = 0;
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

    public override void ClickBegin(Vector2 cursorPosition)
    {
        base.ClickBegin(cursorPosition);

        Gameplay.instance.cookingPot.RestartGame();

        transform.DOScale(1.2f, 0.25f);
        text.DOFade(0, 0.25f);
    }
    public override void HoverBegin(Vector2 cursorPosition)
    {
        base.HoverBegin(cursorPosition);
        transform.DOScale(1.1f, 0.25f);
        text.DOFade(1, 0.5f);
    }
    public override void HoverEnd(Vector2 cursorPosition)
    {
        base.HoverEnd(cursorPosition);
        transform.DOScale(1.0f, 0.25f);
        text.DOFade(0, 0.5f);
    }
}
