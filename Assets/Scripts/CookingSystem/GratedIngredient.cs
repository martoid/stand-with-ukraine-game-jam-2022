using DG.Tweening;
using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GratedIngredient : Ingredient
{
    [SerializeField] SpriteRenderer sr;

    bool isGrated = false;
    public bool isGrating = false;

    Vector2 lastPosition;
    Vector2 deltaPosition;

    float grateProgress = 0;
    [SerializeField] Sprite[] gratingSprites;
    [SerializeField] float gratingSpeed;
    [SerializeField] ParticleSystem grateParticles;

    public override void BeginDrag(Vector2 cursorPosition)
    {
        base.BeginDrag(cursorPosition);

        if (isGrated)
        {
            Gameplay.instance.cookingPot.Prime();
        }
        else
        {
            Gameplay.instance.grater.Prime();
        }
    }
    public override void Dragging(Vector2 cursorPosition)
    {
        base.Dragging(cursorPosition);
        if (isGrating)
        {
            grateParticles.enableEmission = true;

            deltaPosition = (Vector2)transform.position - lastPosition;
            lastPosition = transform.position;

            grateProgress += deltaPosition.magnitude * gratingSpeed;
            grateProgress = Mathf.Clamp01(grateProgress);

            int index = Mathf.FloorToInt(grateProgress * (gratingSprites.Length-1));
            sr.sprite = gratingSprites[index];

            if (grateProgress >= 1)
            {
                isGrating = false;
                isGrated = true;
                Gameplay.instance.grater.inUse = false;
                Gameplay.instance.cookingPot.Prime();
                Gameplay.instance.grater.Unprime();
            }
        }
        else
        {
            grateParticles.enableEmission = false;
        }
    }

    public override void EndDrag(Vector2 cursorPosition, InteractableDragTarget target)
    {
        foreach (var item in FindObjectsOfType<ActionFinish>())
        {
            item.Unprime();
        }

        if (isGrating)
        {
            OnUsedUp.Invoke();
            OnUsedUp.RemoveAllListeners();
            Gameplay.instance.grater.Unprime();
            DestroyIngredient();
        }
        else
        {
            base.EndDrag(cursorPosition, target);
        }
    }
}
