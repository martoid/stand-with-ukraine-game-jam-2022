using DG.Tweening;
using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingPot : ActionFinish
{
    public class CookingProcess
    {
        public class IngredientProcess
        {
            public int count;
            public int totalTime;
            public BortschRecipeSO.Ingredient type;
        }
        List<IngredientProcess> Ingredients = new List<IngredientProcess>();
        public void SecondTick()
        {
            foreach (var ingredient in Ingredients)
            {
                ingredient.totalTime += ingredient.count;
            }
        }
        public void Add(BortschRecipeSO.Ingredient ingredient)
        {
            IngredientProcess process = Ingredients.Find(item => item.type == ingredient);
            if (process != null)
            {
                process.count++;
            }
            else
            {
                Ingredients.Add(new IngredientProcess()
                {
                    count = 1,
                    type = ingredient,
                    totalTime = 0,
                });
            }
        }

        public void PrintDebug()
        {
            string recipe = "The recipe currently in the pot";
            foreach (var ingredient in Ingredients)
            {
                recipe += $"Ingredient: {ingredient.type} | Count: {ingredient.count} | Average cook time: {ingredient.totalTime / ingredient.count}\n";
            }
            Debug.Log($"{recipe}");
        }
    }
    [SerializeField] Color initialSoupColor;

    [SerializeField] float waterHeight;
    [SerializeField] float fallVelocityInreasePerSecond;
    [SerializeField] int maxLightSeconds;

    [SerializeField] GameObject liquidParticle;

    [SerializeField] Renderer[] SoupRenders;
    [SerializeField] Transform fireLevel;

    public int remainingFireSeconds { get; set; }

    protected override void Awake()
    {
        base.Awake();
        SetColor(initialSoupColor);
    }
    CookingProcess process;
    IEnumerator Start()
    {
        process = new CookingProcess();

        while(true)
        {
            yield return new WaitForSeconds(1);
            if(remainingFireSeconds > 0)
            {
                process.SecondTick();
                remainingFireSeconds--;
            }
        }
    }
    public override void InteractableDraggedOn(Interactable interactable)
    {
        base.InteractableDraggedOn(interactable);
        if(interactable is not PouredIngredient)
        {
            StartCoroutine(Plop((Ingredient)interactable));
        }
    }
    public void SetColor(Color color)
    {
        foreach (var item in SoupRenders)
        {
            item.sharedMaterial.SetColor("_Tint", color);
        }
    }

    IEnumerator Plop(Ingredient ingredient)
    {
        float velocity = 0;
        while(ingredient.transform.position.y > waterHeight)
        {
            velocity += Time.deltaTime * fallVelocityInreasePerSecond;
            yield return null;

            ingredient.transform.Translate(Vector2.down * velocity * Time.deltaTime);
        }

        SoundManager.instance.PlayEffect(SoundType.splash);

        switch (ingredient.type)
        {
            case BortschRecipeSO.Ingredient.BeetRoot:
                Gameplay.instance.OnActionPerformed.Invoke(Action.beetAdded);
                break;
            case BortschRecipeSO.Ingredient.Salt:
                Gameplay.instance.OnActionPerformed.Invoke(Action.saltAdded);
                break;
            case BortschRecipeSO.Ingredient.Pepper:
                break;
            case BortschRecipeSO.Ingredient.Carrot:
                break;
            case BortschRecipeSO.Ingredient.Potatoe:
                break;
            case BortschRecipeSO.Ingredient.Leaf:
                break;
            case BortschRecipeSO.Ingredient.Garlic:
                break;
            case BortschRecipeSO.Ingredient.Onion:
                break;
            case BortschRecipeSO.Ingredient.Tomatoe:
                break;
            case BortschRecipeSO.Ingredient.Cabbage:
                Gameplay.instance.OnActionPerformed.Invoke(Action.cabbageAdded);
                break;
            default:
                break;
        }

        // This is where the ingredient hits the water
        AddIngredient(ingredient.type);
        Destroy(ingredient.gameObject);
        Instantiate(liquidParticle.gameObject, ingredient.transform.position, Quaternion.identity);
    }

    public void AddIngredient(BortschRecipeSO.Ingredient type)
    {
        process.Add(type);
    }

    [ContextMenu("Present borsch")]
    public void PresentBorsch()
    {
        process.PrintDebug();
    }

    private void Update()
    {
        fireLevel.localScale = new Vector3(1, Mathf.Clamp01((float)remainingFireSeconds / maxLightSeconds), 1);
    }

    private void OnDrawGizmos()
    {
        Vector2 p1 = new Vector2(0, waterHeight);
        Gizmos.DrawLine(p1+Vector2.left, p1+Vector2.right);
    }
}
