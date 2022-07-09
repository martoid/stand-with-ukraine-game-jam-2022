using System.Collections;
using System.Collections.Generic;
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
    }
}
