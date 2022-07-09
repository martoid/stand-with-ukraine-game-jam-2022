using InteractionSystem2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : ActionFinish
{
    [SerializeField] Animator knifeAnimator;

    public Transform cutPlacement;
    public bool inUse { get; set; }

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
