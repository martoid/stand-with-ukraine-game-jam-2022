using InteractionSystem2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : ActionFinish
{
    public Transform cutPlacement;
    public bool inUse { get; set; }

    public override void Prime()
    {
        if(!inUse)
        {
            base.Prime();
        }
    }
}
