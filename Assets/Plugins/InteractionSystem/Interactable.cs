using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem2D
{
    [RequireComponent(typeof(Collider2D)), DisallowMultipleComponent, SelectionBase]
    public class Interactable : MonoBehaviour, IPivotable
    {
        /// <summary>
        /// Called when the pointer enters the collider of the interatable.
        /// </summary>
        public UnityEvent<Vector2> OnHoverBegin;

        /// <summary>
        /// Called when the pointer exits the collider of the interactable.
        /// </summary>
        public UnityEvent<Vector2> OnHoverEnd;

        /// <summary>
        /// Called each frame the pointer is inside the collider of the interactable or the interactable is no longer active.
        /// </summary>
        public UnityEvent<Vector2> OnHovering;

        /// <summary>
        /// Called when pointer begins dragging the interactable.
        /// </summary>
        public UnityEvent<Vector2> OnDragBegin;

        /// <summary>
        /// Called when click is released to end the drag or the interactable is no longer active.
        /// First Vector2 is where drag originated from, the second one is where it ended.
        /// </summary>
        public UnityEvent<Vector2, Vector2, InteractableDragTarget> OnDragEnd;

        /// <summary>
        /// Called each frame the interactable is being dragged from, returns current
        /// position of the cursor and amount that it has moved since the last frame.
        /// </summary>
        public UnityEvent<Vector2, Vector2> OnDragging;

        /// <summary>
        /// Called when pointer clicks on the interactable.
        /// </summary>
        public UnityEvent<Vector2> OnClickBegin;

        /// <summary>
        /// Called when pointer click is released while pointer is still on the interactable.
        /// </summary>
        public UnityEvent<Vector2> OnClickEnd;

        /// <summary>
        /// Called every frame the click is held
        /// </summary>
        public UnityEvent<Vector2> OnClickHold;

        public bool draggable { get; set; } = true;

        [Header("Parameters")]
        [Tooltip("Vector that is offsetted from the base position used for calculations")]
        public Vector2 pivotOffset = Vector2.zero;
        public Vector2 dragTarget { get; set; }
        public bool isBeingDragged { get; set; }
        public bool isHovered { get; set; }
        public Vector2 dragStartPosition { get; set; }

        public Vector2 pickupPosition => (Vector2)transform.position + pivotOffset;

        Vector2 lastPosition { get; set; }
        Vector2 moveVelocity { get; set; }

        public virtual void HoverBegin(Vector2 cursorPosition)
        {
            isHovered = true;
            OnHoverBegin.Invoke(cursorPosition);
        }
        public virtual void HoverEnd(Vector2 cursorPosition)
        {
            OnHoverEnd.Invoke(cursorPosition);
            isHovered = false;
        }
        /// <summary>
        /// Called every frame the interatable is hovered over
        /// </summary>
        /// <param name="cursorPosition"></param>
        public virtual void Hovering(Vector2 cursorPosition)
        {
            OnHovering.Invoke(cursorPosition);
        }
        public virtual void BeginDrag(Vector2 cursorPosition)
        {
            lastPosition = cursorPosition;
            isBeingDragged = true;
            OnDragBegin.Invoke(cursorPosition);
        }
        public virtual void EndDrag(Vector2 cursorPosition, InteractableDragTarget target)
        {
            lastPosition = cursorPosition;
            isBeingDragged = false;
            OnDragEnd.Invoke(dragStartPosition, cursorPosition, target);
        }
        /// <summary>
        /// Called every frame the interactable is being dragged
        /// </summary>
        /// <param name="cursorPosition"></param>
        public virtual void Dragging(Vector2 cursorPosition)
        {
            moveVelocity = cursorPosition - lastPosition;
            lastPosition = cursorPosition;
            OnDragging.Invoke(cursorPosition, moveVelocity);
        }
        public virtual void ClickBegin(Vector2 cursorPosition)
        {
            OnClickBegin.Invoke(cursorPosition);
        }
        public virtual void ClickEnd(Vector2 cursorPosition)
        {
            OnClickEnd.Invoke(cursorPosition);
        }
        public virtual void Clicking(Vector2 cursorPosition)
        {
            OnClickHold.Invoke(cursorPosition);
        }

        protected virtual void OnDisable()
        {
            if (isHovered) HoverEnd(lastPosition);
            if (isBeingDragged) EndDrag(lastPosition, null);
        }
    }
}
