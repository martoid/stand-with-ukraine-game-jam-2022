using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ingredient : Interactable
{
    public UnityEvent OnUsedUp;

    [SerializeField] GameObject destroyParticle;
    [SerializeField] float rotationOverSpeed = 10;

    public BortschRecipeSO.Ingredient type;

    protected Collider2D col;

    protected Vector2 lastPos;
    protected Vector2 velocity;

    protected Quaternion targetRotation;

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
            DestroyIngredient();
        }
    }

    public virtual void DestroyIngredient()
    {
        Instantiate(destroyParticle, transform.position, Quaternion.identity);
        SoundManager.instance.PlayEffect(SoundType.discard);
        Destroy(gameObject);
    }
    private void Update()
    {
        velocity = (Vector2)transform.position - lastPos;
        lastPos = transform.position;
        targetRotation = Quaternion.Euler(0, 0, rotationOverSpeed * -velocity.x);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 20);
    }
}
