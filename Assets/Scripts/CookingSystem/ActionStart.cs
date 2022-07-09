using DG.Tweening;
using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionStart : Interactable
{
    public Vector2 distanceFadeBounds = new Vector2(1, 5);

    [SerializeField] SpriteRenderer indicatorSr;
    [SerializeField] Interactable draggedObject;
    [SerializeField] GameObject spawnParticle;

    Collider2D col;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        draggable = false;
    }
    public override void ClickBegin(Vector2 cursorPosition)
    {
        col.enabled = false;
        base.ClickBegin(cursorPosition);
        ingredient = Instantiate(draggedObject.gameObject, cursorPosition, Quaternion.identity).GetComponent<Ingredient>();
        Instantiate(spawnParticle,transform.position, Quaternion.identity);

        DOVirtual.DelayedCall(0.5f, () => col.enabled = true);
    }

    Ingredient ingredient;

    public override void ClickEnd(Vector2 cursorPosition)
    {
        base.ClickEnd(cursorPosition);

        if (ingredient) ingredient.DestroyIngredient();
    }

    private void Update()
    {
        if (Gameplay.instance.isDragging())
        {
            indicatorSr.SetOpacity(0);
        }
        else
        {
            float distance = Vector2.Distance(transform.position, InteractableController.cursorWorldPosition);
            float minOpacity = Mathf.InverseLerp(distanceFadeBounds.y, distanceFadeBounds.x, distance);

            indicatorSr.SetOpacity(minOpacity);
        }
    }
    public override void HoverBegin(Vector2 cursorPosition)
    {
        base.HoverBegin(cursorPosition);
        transform.DOScale(Vector2.one * 1.1f, 0.2f);
    }
    public override void HoverEnd(Vector2 cursorPosition)
    {
        transform.DOScale(Vector2.one * 1.0f, 0.2f);
        base.HoverEnd(cursorPosition);
    }
}
