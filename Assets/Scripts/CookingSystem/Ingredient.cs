using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ingredient : Interactable
{
    public UnityEvent OnUsedUp;

    [SerializeField] GameObject destroyParticle;

    public BortschRecipeSO.Ingredient type;

    protected Collider2D col;
    protected virtual void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    public override void BeginDrag(Vector2 cursorPosition)
    {
        base.BeginDrag(cursorPosition);
    }
    public override void Dragging(Vector2 cursorPosition)
    {
        base.Dragging(cursorPosition);
        transform.position = cursorPosition;
    }
    public override void EndDrag(Vector2 cursorPosition, InteractableDragTarget target)
    {
        base.EndDrag(cursorPosition, target);
        OnUsedUp.Invoke();
        OnUsedUp.RemoveAllListeners();
        if(!target)
        {
            Instantiate(destroyParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
