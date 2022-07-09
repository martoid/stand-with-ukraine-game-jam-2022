using DG.Tweening;
using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        SoundManager.instance.PlayEffect(SoundType.grabSound, 0.2f);
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
}
