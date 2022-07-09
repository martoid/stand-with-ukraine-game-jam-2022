using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem2D
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractableDragTarget : MonoBehaviour, IPivotable
    {
        public UnityEvent<Interactable> OnInteractableDraggedOn;
        public UnityEvent<Interactable> OnInteractableDragHoverBegin;
        public UnityEvent<Interactable> OnInteractableDragHoverEnd;

        public Vector2 pivotOffset;
        public Vector2 pickupPosition => pivotOffset;

        public virtual void InteractableDraggedOn(Interactable interactable)
        {
            OnInteractableDraggedOn.Invoke(interactable);
        }
        public virtual void InteractableDragHover(Interactable interactable)
        {
            OnInteractableDragHoverBegin.Invoke(interactable);
        }
        public virtual void InteractableDragUnhover(Interactable interactable)
        {
            OnInteractableDragHoverEnd.Invoke(interactable);
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.DrawSphere((Vector2)transform.position + pivotOffset, 0.05f);
        }
    }
}
