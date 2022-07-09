using DG.Tweening;
using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CuttableIngredient : Ingredient
{
    public UnityEvent OnCut;

    [SerializeField] SpriteRenderer sr;

    [SerializeField] Sprite[] CutSprites;
    int cutsLeft;

    bool cutting = false;

    protected override void Awake()
    {
        cutsLeft = CutSprites.Length;
    }
    public override void BeginDrag(Vector2 cursorPosition)
    {
        base.BeginDrag(cursorPosition);
        if(cutsLeft > 0)
        {
            Gameplay.instance.cuttingBoard.Prime();
        }
        else
        {
            Gameplay.instance.cuttingBoard.RemoveIngredient();
            Gameplay.instance.cookingPot.Prime();
        }
    }
    public override void ClickBegin(Vector2 cursorPosition)
    {
        base.ClickBegin(cursorPosition);
        if  (cutting && cutsLeft > 0)
        {
            OnCut.Invoke();
            sr.sprite = CutSprites[CutSprites.Length - cutsLeft];
            SoundManager.instance.PlayEffect(SoundType.choppingSound);
            cutsLeft--;
            if (cutsLeft <= 0)
            {
                draggable = true;
            }
        }
    }
    public override void EndDrag(Vector2 cursorPosition, InteractableDragTarget target)
    {
        foreach (var item in FindObjectsOfType<ActionFinish>())
        {
            item.Unprime();
        }

        base.EndDrag(cursorPosition, target);

        if(target == Gameplay.instance.cuttingBoard)
        {
            Gameplay.instance.cuttingBoard.AssignIngredient(this);
            draggable = false;
            cutting = true;

            transform.position = target.transform.position;
        }
    }
}
