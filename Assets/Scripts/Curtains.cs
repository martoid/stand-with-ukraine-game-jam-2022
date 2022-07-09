using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtains : Interactable
{
    [SerializeField] Vector2 distanceAllowed;

    Vector2 origin;
    private void Awake()
    {
        origin = transform.position;
    }
    public override void Dragging(Vector2 cursorPosition)
    {
        base.Dragging(cursorPosition);
        transform.position = Vector2.Lerp(transform.position, new Vector2(Mathf.Clamp(cursorPosition.x, origin.x+distanceAllowed.x, origin.x+distanceAllowed.y), origin.y), Time.deltaTime*20);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right*distanceAllowed.y-Vector3.left*distanceAllowed.x);
    }
}
