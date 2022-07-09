using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    [SerializeField] Ingredient ingredient;

    Ingredient current;

    private void Awake()
    {
        SpawnIngredient();
    }

    void SpawnIngredient()
    {
        current = Instantiate(ingredient.gameObject, transform.position, Quaternion.identity).GetComponent<Ingredient>();
        current.OnUsedUp.AddListener(() => SpawnIngredient());
    }
}
