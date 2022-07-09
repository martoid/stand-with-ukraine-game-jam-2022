using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouredIngredient : Ingredient
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] float rotateDistance;
    [SerializeField] float pourDistance;
    [SerializeField] float pourAngle;

    [SerializeField] float pourAmount;

    Vector2 position;
    Quaternion rotation;

    [SerializeField] ParticleSystem pourParticles;

    protected override void Awake()
    {
        base.Awake();

        position = transform.position;
        rotation = transform.rotation;
    }

    public override void DestroyIngredient()
    {
        draggable = false;

        transform.DOMove(position, 1).SetEase(Ease.OutQuint);
        transform.DORotate(rotation.eulerAngles, 1).SetEase(Ease.OutQuint);
        DOVirtual.DelayedCall(1, () =>
        {
            draggable = true;
        });
    }

    private void Update()
    {
        Vector2 distanceVector = Gameplay.instance.cookingPot.transform.position - pourParticles.transform.position;
        float distance = distanceVector.magnitude;

        float progression = Mathf.Clamp01(Mathf.InverseLerp(rotateDistance, 0, distance));
        float pourProgression = Mathf.Clamp01(Mathf.InverseLerp(pourDistance, 0, distance));
        float rotation = progression * pourAngle;
        sr.transform.rotation = Quaternion.Euler(0,0, rotation);

        pourParticles.emissionRate = pourProgression * pourAmount;
    }
}
