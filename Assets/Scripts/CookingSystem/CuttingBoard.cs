using InteractionSystem2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : ActionFinish
{
    [SerializeField] Animator knifeAnimator;

    public Transform cutPlacement;
    public bool inUse { get; private set; }

    public void AssignIngredient(CuttableIngredient cuttable)
    {
        inUse = true;
        cuttable.OnCut.AddListener(() => knifeAnimator.SetTrigger("Cut"));
        knifeAnimator.SetBool("InUse", true);
    }
    public void RemoveIngredient()
    {
        inUse = false;
        knifeAnimator.SetBool("InUse", false);
    }
    public override void Prime()
    {
        if(!inUse)
        {
            base.Prime();
            knifeAnimator.SetBool("Primed", true);
        }
    }
    public override void Unprime()
    {
        base.Unprime();
        knifeAnimator.SetBool("Primed", false);
    }
}
