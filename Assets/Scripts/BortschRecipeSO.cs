using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "BorschRecipe", menuName = "Data/Recipe")]
public class BortschRecipeSO : ScriptableObject
{
    public enum Ingredient
    {
        BeetRoot,
        Salt,
        Pepper,
        Carrot,
        Potatoe,
        Leaf,
        Garlic,
        Onion,
        Tomatoe,
        Cabbage,
        Vinegar,
    }

    [System.Serializable]
    public class IngredientData
    {
        [System.Serializable]
        public class Lesson
        {
            public CookingPot.CookingProcess.CookingReport.Problem.Type problemType;
            [Multiline]
            public string[] Sayings;
        }

        public Ingredient type;
        public Color color;
        public int perfectAmount;
        public Vector2Int cookInterval;

        public List<Lesson> Lessons;
    }

    public float missingIngredientPenalty = 9999;
    public float incorrectIngredientCountRelativePenalty = 500;
    public float wrongCookTimeRelativePenalty = 100;

    public List<IngredientData> Norms = new List<IngredientData>();
    public Color waterColor;
    public float waterColoringStrenght;
    public AnimationCurve ratingDistribution;

    public IngredientData GetData(Ingredient ingredient)
    {
        return Norms.Find(item => item.type == ingredient);
    }

    [ContextMenu("Add missing ingredients")]
    public void AddMissingIngredients()
    {
        foreach (var item in Enum.GetValues(typeof(Ingredient)).Cast<Ingredient>())
        {
            if(!Norms.Exists(i => i.type == item))
            {
                Norms.Add(new IngredientData()
                {
                    type = item
                }) ;
            }
        }
    }

    [ContextMenu("Add missing lessons")]
    public void AddMissingLessons()
    {
        foreach (var item in Norms)
        {
            foreach (var tp in Enum.GetValues(typeof(CookingPot.CookingProcess.CookingReport.Problem.Type)).Cast<CookingPot.CookingProcess.CookingReport.Problem.Type>())
            {
                if (!item.Lessons.Exists(i => i.problemType == tp))
                {
                    item.Lessons.Add(new IngredientData.Lesson()
                    {
                        problemType = tp,
                        Sayings = new string[] {"Placeholder"}
                    });
                }
            }
        }
    }
}
