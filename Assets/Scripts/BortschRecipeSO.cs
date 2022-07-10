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
        OliveOil,
    }

    [System.Serializable]
    public class IngredientData
    {
        public Ingredient type;
        public Color color;
        public int perfectAmount;
        public Vector2Int cookInterval;
    }

    public List<IngredientData> Norms = new List<IngredientData>();
    public Color waterColor;
    public float waterColoringStrenght;

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
}
