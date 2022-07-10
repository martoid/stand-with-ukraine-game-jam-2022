using DG.Tweening;
using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CookingPot : ActionFinish
{
    public class CookingProcess
    {
        public class CookingReport
        {
            public class Problem
            {
                public enum Type { TooMuch, NotEnough, Overcooked, Undercooked, MissingIngredient }

                public int amount { get; private set; }
                public Type type { get; private set; }
                public BortschRecipeSO.Ingredient ingredient { get; private set; }
                public float penalty { get; private set; }

                public Problem(BortschRecipeSO.Ingredient ingredient, Type type, int amount = 0)
                {
                    this.ingredient = ingredient;
                    this.type = type;
                    this.amount = amount;

                    penalty = CalculatePriority();
                }
                private float CalculatePriority()
                {
                    var data = recipe.Norms.Find(item => item.type == ingredient);
                    switch (type)
                    {
                        case Type.TooMuch:
                        case Type.NotEnough:
                            return  (amount/(float)data.perfectAmount) * recipe.incorrectIngredientCountRelativePenalty;
                        case Type.Overcooked:
                        case Type.Undercooked:
                            int interval = data.cookInterval.y - data.cookInterval.x;
                            return ((float)amount / interval) * recipe.wrongCookTimeRelativePenalty; 
                        case Type.MissingIngredient:
                            return recipe.missingIngredientPenalty;
                    }
                    return 0;
                }
            }


            public List<Problem> Problems = new List<Problem>();
            public CookingReport(CookingProcess process)
            {
                foreach (var item in recipe.Norms)
                {
                    List<IngredientProcess> ingredientsOfType = process.Ingredients.FindAll(ing => ing.type == item.type);
                    if (ingredientsOfType.Count <= 0) Problems.Add(new Problem(item.type, Problem.Type.MissingIngredient));

                    int amountDifference = ingredientsOfType.Count - item.perfectAmount;
                    if (amountDifference > 0)
                    {
                        Problems.Add(new Problem(item.type, Problem.Type.TooMuch, amountDifference));
                    }
                    else
                    {
                        Problems.Add(new Problem(item.type, Problem.Type.NotEnough, -amountDifference));
                    }


                    int overcooked = 0;
                    int undercooked = 0;
                    foreach (var ing in ingredientsOfType)
                    {
                        if(ing.timeCooked > item.cookInterval.y)
                        {
                            overcooked++;
                        }
                        else if(ing.timeCooked < item.cookInterval.x)
                        {
                            undercooked++;
                        }
                    }

                    if (overcooked > 0) Problems.Add(new Problem(item.type, Problem.Type.Overcooked, overcooked));
                    if (undercooked > 0) Problems.Add(new Problem(item.type, Problem.Type.Undercooked, undercooked));

                    Problems = Problems.OrderByDescending(item => item.penalty).ToList();
                }
            }

            public void PrintToConsole()
            {
                float penalty = Problems.Sum(item => item.penalty);
                float rating = recipe.ratingDistribution.Evaluate(penalty);
                string sum = $"<b>These are the problems with the soup (Penalty:{penalty}, Rating:{rating*10}/10):</b> \n"; 
                foreach (var problem in Problems)
                {
                    sum += $"{problem.penalty}, {problem.ingredient}, {problem.type}, {problem.amount}\n";
                }
                Debug.Log($"{sum}");
            }

            internal void TeachALesson()
            {
                var problem = Problems[0];
                Gameplay.instance.Speak(recipe.Norms.Find(norm => norm.type == problem.ingredient).Lessons.Find(lesson => lesson.problemType == problem.type).Sayings.PickRandom());
                
            }
        }
        public class IngredientProcess
        {
            public int timeCooked;
            public BortschRecipeSO.Ingredient type;
        }
        private static BortschRecipeSO recipe => Gameplay.instance.recipe;

        List<IngredientProcess> Ingredients = new List<IngredientProcess>();
        public void SecondTick()
        {
            foreach (var ingredient in Ingredients)
            {
                ingredient.timeCooked++;
            }
        }
        public void Add(BortschRecipeSO.Ingredient ingredient)
        {
            Ingredients.Add(new IngredientProcess()
            {
                timeCooked = 0,
                type = ingredient
            });
        }

        public Color GetColor()
        {
            Vector3 color = new Vector3();
            float totalPoints = 0;

            foreach (var ingredient in Ingredients)
            {
                var data = recipe.GetData(ingredient.type);
                // Max single color 1, min 0
                color += new Vector3(data.color.r, data.color.g, data.color.b) * data.color.a;
                totalPoints += data.color.a;
            }
            Color waterColor = recipe.waterColor;
            color += new Vector3(waterColor.r, waterColor.g, waterColor.b) * recipe.waterColoringStrenght;
            totalPoints += recipe.waterColoringStrenght;


            return new Color(color.x/totalPoints, color.y/totalPoints, color.z/totalPoints);
        }

        internal CookingReport GetReport()
        {
            return new CookingReport(this);
        }
    }
    [SerializeField] float waterHeight;
    [SerializeField] float fallVelocityInreasePerSecond;
    [SerializeField] int maxLightSeconds;
    [SerializeField] int maxSoupAmount;
    [SerializeField] Vector2 soupLevelBounds;

    [SerializeField] GameObject liquidParticle;

    [SerializeField] Renderer[] SoupRenders;
    [SerializeField] Transform fireLevel;
    [SerializeField] Transform soupLevel;

    [SerializeField] ParticleSystem bubbles;
    [SerializeField] SpriteRenderer waves;
    [SerializeField] AudioSource boilingSource;

    float baseBoilingSound;
    float bubblesBaseEmission;
    float wavesBaseAmplitude;

    float currentTemprature = 0;
    float targetTemperature = 0;

    int soupLeft;
    Color targetColor;
    Color currentColor;

    public int remainingFireSeconds { get; set; }

    CookingProcess process;
    protected override void Awake()
    {
        base.Awake();
        bubblesBaseEmission = bubbles.emissionRate;
        wavesBaseAmplitude = waves.material.GetFloat("_Amplitude");
        baseBoilingSound = boilingSource.volume;
    }
    IEnumerator Start()
    {
        RestartGame();

        while(true)
        {
            yield return new WaitForSeconds(1);
            if(remainingFireSeconds > 0)
            {
                process.SecondTick();
                remainingFireSeconds--;
                targetTemperature = 1;
            }
            else
            {
                targetTemperature = 0;
            }
        }
    }
    public override void InteractableDraggedOn(Interactable interactable)
    {
        base.InteractableDraggedOn(interactable);
        if(interactable is not PouredIngredient && interactable is not Plate)
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
    public void SetTemperature(float normalizedTemp)
    {
        bubbles.emissionRate = Mathf.Lerp(0, bubblesBaseEmission, normalizedTemp);
        waves.material.SetFloat("_Amplitude", Mathf.Lerp(0, wavesBaseAmplitude, normalizedTemp));
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

        targetColor = process.GetColor();
    }

    private void Update()
    {
        fireLevel.localScale = new Vector3(1, Mathf.Clamp01((float)remainingFireSeconds / maxLightSeconds), 1);

        float soupTarget = Mathf.Lerp(soupLevelBounds.x, soupLevelBounds.y, (float)soupLeft/maxSoupAmount);
        soupLevel.localPosition = Vector3.Lerp(soupLevel.localPosition, new Vector3(0, soupTarget, 0), Time.deltaTime*20);

        //currentTemprature = Mathf.Lerp(currentTemprature, targetTemperature, 5 * Time.deltaTime);
        currentTemprature = Mathf.MoveTowards(currentTemprature, targetTemperature, Time.deltaTime * 2);
        boilingSource.volume = baseBoilingSound * currentTemprature;
        SetTemperature(currentTemprature);

        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * 5);
        SetColor(currentColor);
    }

    private void OnDrawGizmos()
    {
        Vector2 p1 = new Vector2(0, waterHeight);
        Gizmos.DrawLine(p1+Vector2.left, p1+Vector2.right);
    }

    [ContextMenu("ConsumeSoup")]
    public CookingProcess ConsumeSoup()
    {
        soupLeft--;

        if(soupLeft <= 0)
        {
            RestartGame();
        }

        var report = process.GetReport();

        report.PrintToConsole();
        report.TeachALesson();
        return process;
    }

    public void RestartGame()
    {
        soupLevel.localPosition = new Vector3(0, 0, 0);
        process = new CookingProcess();
        currentColor = process.GetColor();
        targetColor = currentColor;
        SetColor(currentColor);
        soupLeft = maxSoupAmount;
    }
}
