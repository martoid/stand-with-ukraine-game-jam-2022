using DG.Tweening;
using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFinish : InteractableDragTarget
{
    [SerializeField] SpriteRenderer indicatorSr;

    protected Collider2D col;
    protected virtual void Awake()
    {
        col = GetComponent<Collider2D>();

        col.enabled = false;
        indicatorSr.SetOpacity(0);
        indicatorSr.transform.localScale = Vector3.one;
    }
    public virtual void Prime()
    {
        col.enabled = true;
        indicatorSr.DOFade(1, 0.5f);
    }
    public virtual void Unprime()
    {
        col.enabled = false;
        indicatorSr.DOFade(0, 0.5f);
    }
    public override void InteractableDragHover(Interactable interactable)
    {
        base.InteractableDragHover(interactable);
        indicatorSr.transform.DOScale(2, 0.2f);
    }
    public override void InteractableDragUnhover(Interactable interactable)
    {
        base.InteractableDragUnhover(interactable);
        indicatorSr.transform.DOScale(1, 0.2f);
    }
    public virtual void GiveObject(Ingredient go)
    {

    }
}
