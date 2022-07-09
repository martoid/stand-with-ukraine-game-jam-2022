using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    public static Gameplay instance;

    public InteractableController interactable;

    private void Awake()
    {
        instance = this;
    }

    public bool isDragging()
    {
        return interactable.isDragging();
    }
}
