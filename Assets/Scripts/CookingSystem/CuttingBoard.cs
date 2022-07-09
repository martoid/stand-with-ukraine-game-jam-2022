using InteractionSystem2D;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : ActionFinish
{
    GameObject cuttable;

    [SerializeField] Interactable cuttingAction;
    [SerializeField] int chopsPerVegetable;
    [SerializeField] Transform cutPlacement;

    float chopsLeft;

    protected override void Awake()
    {
        base.Awake();

        cuttingAction.OnClickBegin.AddListener(Chop);
        cuttingAction.gameObject.SetActive(false);
    }
    public override void Prime()
    {
        if(!cuttable)
        {
            base.Prime();
        }
    }
    public override void GiveObject(GameObject go)
    {
        col.enabled = false;
        cuttingAction.gameObject.SetActive(true);

        base.GiveObject(go);
        cuttable = go;

        cuttable.transform.position = cutPlacement.position;
        cuttable.transform.rotation = cutPlacement.rotation;

        chopsLeft = chopsPerVegetable;
    }
    private void Chop(Vector2 arg0)
    {
        if(cuttable)
        {
            chopsLeft--;

            cuttable.transform.localScale = Vector3.one * (float)chopsLeft / chopsPerVegetable;
            if(chopsLeft <= 0)
            {
                cuttingAction.gameObject.SetActive(false);
                col.enabled = true;
                Destroy(cuttable);
            }
        }
    }
}
