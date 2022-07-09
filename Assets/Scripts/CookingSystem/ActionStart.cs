using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionStart : Interactable
{
    public Vector2 distanceFadeBounds = new Vector2(1, 5);

    [SerializeField] SpriteRenderer indicatorSr;
    [SerializeField] Interactable draggedObject;

    Collider2D col;
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        draggable = false;
    }
    public override void ClickBegin(Vector2 cursorPosition)
    {
        base.ClickBegin(cursorPosition);
        Instantiate(draggedObject.gameObject, cursorPosition, Quaternion.identity);
    }
    private void Update()
    {
        float distance = Vector2.Distance(transform.position, InteractableController.cursorWorldPosition);
        float minOpacity = Mathf.InverseLerp(distanceFadeBounds.y, distanceFadeBounds.x, distance);

        indicatorSr.SetOpacity(minOpacity);
    }
}
