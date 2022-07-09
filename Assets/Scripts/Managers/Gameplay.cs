using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    public static Gameplay instance;

    public InteractableController interactable;

    public CuttingBoard cuttingBoard;
    public CookingPot cookingPot;
    public Furnance furnance;
    public Grater grater;

    private void Awake()
    {
        instance = this;
    }

    public bool isDragging()
    {
        return interactable.isDragging();
    }
}
