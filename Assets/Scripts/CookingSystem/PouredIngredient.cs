using DG.Tweening;
using InteractionSystem2D;
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
    [SerializeField] float pourPerSecond;

    [SerializeField] SpriteRenderer indicator;
    [SerializeField] Vector2 distanceFadeBounds;

    Vector2 position;
    Quaternion rotation;

    [SerializeField] ParticleSystem pourParticles;

    float currentPour = 0;

    AudioSource pourSound;
    float targetVolume = 0;

    private void Start()
    {
        var pourType = type == BortschRecipeSO.Ingredient.Vinegar ? SoundType.pourLiquid : SoundType.pourSalt;
        pourSound = SoundManager.instance.GetSoundObject(pourType);
        pourSound.playOnAwake = false;
        pourSound.transform.parent = gameObject.transform;
        pourSound.Play();
    }

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

        SoundManager.instance.PlayEffect(SoundType.releaseBottle);
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

        currentPour += pourProgression * pourPerSecond * Time.deltaTime;

        int toPot = Mathf.FloorToInt(currentPour);
        currentPour -= toPot;

        // Yes, pretty dumb
        for (int i = 0; i < toPot; i++)
        {
            Gameplay.instance.cookingPot.AddIngredient(type);
        }
        targetVolume = pourProgression;
        pourSound.volume = Mathf.Lerp(pourSound.volume, targetVolume, Time.deltaTime * 20);

        if (Gameplay.instance.isDragging())
        {
            indicator.SetOpacity(0);
        }
        else
        {
            float distt = Vector2.Distance(transform.position, InteractableController.cursorWorldPosition);
            float minOpacity = Mathf.InverseLerp(distanceFadeBounds.y, distanceFadeBounds.x, distt);

            indicator.SetOpacity(minOpacity);
        }
    }
    public override void BeginDrag(Vector2 cursorPosition)
    {
        base.BeginDrag(cursorPosition);
        Gameplay.instance.cookingPot.Prime();
        SoundManager.instance.PlayEffect(SoundType.grabSound);
    }
    public override void EndDrag(Vector2 cursorPosition, InteractableDragTarget target)
    {
        targetVolume = 0;
        base.EndDrag(cursorPosition, target);
        Gameplay.instance.cookingPot.Unprime();
        DestroyIngredient();
    }

    public override void HoverBegin(Vector2 cursorPosition)
    {
        base.HoverBegin(cursorPosition);
        transform.DOScale(Vector2.one * 1.1f, 0.2f);
    }
    public override void HoverEnd(Vector2 cursorPosition)
    {
        transform.DOScale(Vector2.one * 1.0f, 0.2f);
        base.HoverEnd(cursorPosition);
    }
}
