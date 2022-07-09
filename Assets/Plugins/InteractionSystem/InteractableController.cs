using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace InteractionSystem2D
{
    public class InteractableController : MonoBehaviour
    {
        public static Vector2 cursorWorldPosition;

        [Header("Parameters")]
        [Tooltip("The amount of world distance the cursor needs to travel for interactable to be dragged")]
        public float dragThreshold;
        public LayerMask interactableMask = ~0;
        [Tooltip("Assign a camera to be used for interactable system, if not assigned Camera.Main will be used")]
        public Camera cam;

        Interactable currentInteractable;
        InteractableDragTarget currentDragTarget;
        bool dragging = false;

        bool TryRaycastAndFindClosest<T>(Vector2 worldPosition, out T hit) where T : Component, IPivotable
        {
            RaycastHit2D[] Hits = Physics2D.RaycastAll(worldPosition, Vector2.zero, Mathf.Infinity, interactableMask);
            List<T> HitInteractables = new List<T>();

            float minDistance = float.MaxValue;
            hit = null;
            foreach (var col in Hits)
            {
                if (col.collider.TryGetComponent(out T hitInteractable))
                {
                    float distance = Vector2.Distance(worldPosition, (Vector2)hitInteractable.transform.position + hitInteractable.pickupPosition);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        hit = hitInteractable;
                    }
                    HitInteractables.Add(hitInteractable);
                }
            }

            if(hit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void Update()
        {
            if (dragging && !currentInteractable) dragging = false;

            if(!cam)
            {
                cam = Camera.main;
            }
            Vector2 worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            cursorWorldPosition = worldPoint;

            TryRaycastAndFindClosest(worldPoint, out Interactable hit);

            if (dragging)
            {
                if(TryRaycastAndFindClosest(worldPoint, out InteractableDragTarget target))
                {
                    if(target != currentDragTarget)
                    {
                        if(currentDragTarget)
                        {
                            currentDragTarget.InteractableDragUnhover(currentInteractable);
                        }
                        currentDragTarget = target;
                        currentDragTarget.InteractableDragHover(currentInteractable);
                    }
                }
                else if(currentDragTarget)
                {
                    currentDragTarget.InteractableDragUnhover(currentInteractable);
                    currentDragTarget = null;
                }

                if(!hit)
                {
                    if(currentInteractable.isHovered)
                    {
                        currentInteractable.HoverEnd(worldPoint);
                    }
                }
                else
                {
                    if(!currentInteractable.isHovered && hit == currentInteractable)
                    {
                        currentInteractable.HoverBegin(worldPoint);
                    }
                }

                if(!currentInteractable)
                {
                    dragging = false;
                }
                else
                {
                    currentInteractable.dragTarget = worldPoint;

                    if(Input.GetMouseButtonUp(0))
                    {
                        Debug.DrawLine(currentInteractable.transform.position, currentInteractable.dragTarget, Color.red, 1);

                        if(currentDragTarget)
                        {
                            currentDragTarget.InteractableDraggedOn(currentInteractable);
                        }
                        currentInteractable.EndDrag(worldPoint, currentDragTarget);

                        dragging = false;
                        currentInteractable = null;
                    }
                    else
                    {
                        Debug.DrawLine(currentInteractable.transform.position, currentInteractable.dragTarget);
                        currentInteractable.Dragging(worldPoint);
                    }
                }

            }
            else if (hit)
            {
                if(currentInteractable && hit != currentInteractable)
                {
                    currentInteractable.HoverEnd(worldPoint);
                }


                currentInteractable = hit;
                if(!currentInteractable.isHovered)
                {
                    currentInteractable.HoverBegin(worldPoint);
                }
                currentInteractable.Hovering(worldPoint);


                if(Input.GetMouseButtonDown(0))
                {
                    currentInteractable.dragStartPosition = worldPoint;
                    currentInteractable.ClickBegin(worldPoint);
                }
                else if(Input.GetMouseButtonUp(0))
                {
                    currentInteractable.ClickEnd(worldPoint);
                }
                
                if(Input.GetMouseButton(0))
                {
                    currentInteractable.Clicking(worldPoint);
                    if(Vector2.Distance(currentInteractable.dragStartPosition, worldPoint) >= dragThreshold)
                    {
                        currentInteractable.dragTarget = worldPoint;
                        currentInteractable.BeginDrag(worldPoint);
                        dragging = true;
                    }
                }
            }
            else
            {
                if(currentInteractable)
                {
                    currentInteractable.HoverEnd(worldPoint);
                }
                currentInteractable = null;
            }
        }
    }
}
