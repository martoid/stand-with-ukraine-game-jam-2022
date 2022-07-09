using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionStart : Interactable
{
    public Vector2 distanceFadeBounds = new Vector2(1, 5);

    [SerializeField] SpriteRenderer indicatorSr;
    [SerializeField] List<ActionFinish> Finishers;
    [SerializeField] GameObject draggedObject;
    [SerializeField] GameObject dissapearParticle;

    GameObject objectInstance;

    public override void BeginDrag(Vector2 cursorPosition)
    {
        base.BeginDrag(cursorPosition);

        Finishers.ForEach(item => item.Prime());

        objectInstance = Instantiate(draggedObject, cursorPosition, Quaternion.identity);
    }
    public override void EndDrag(Vector2 cursorPosition, InteractableDragTarget target)
    {
        base.EndDrag(cursorPosition, target);
        Finishers.ForEach(item => item.Unprime());

        if(target)
        {
            ((ActionFinish)target).GiveObject(objectInstance);
        }
        else
        {
            Instantiate(dissapearParticle, objectInstance.transform.position, Quaternion.identity);
            Destroy(objectInstance);
        }
        objectInstance = null;
    }
    public override void Dragging(Vector2 cursorPosition)
    {
        base.Dragging(cursorPosition);
        objectInstance.transform.position = cursorPosition;
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, InteractableController.cursorWorldPosition);
        float minOpacity = Mathf.InverseLerp(distanceFadeBounds.y, distanceFadeBounds.x, distance);

        indicatorSr.SetOpacity(minOpacity);
    }
}
