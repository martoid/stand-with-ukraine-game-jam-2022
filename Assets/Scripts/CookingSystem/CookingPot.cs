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
            public BortschRecipeSO.Ingredient ingredient;
        }
        List<IngredientProcess> Ingredients;
        public void SecondTick()
        {
            foreach (var ingredient in Ingredients)
            {
                ingredient.totalTime += ingredient.count;
            }
        }
    }


    [SerializeField] float waterHeight;
    [SerializeField] float fallVelocityInreasePerSecond;

    [SerializeField] GameObject liquidParticle;

    int remainingFire = 0;
    IEnumerator Start()
    {
        CookingProcess process = new CookingProcess();

        while(true)
        {
            yield return new WaitForSeconds(1);
            if(remainingFire > 0)
            {
                process.SecondTick();
                remainingFire--;
            }
        }
    }
    public override void InteractableDraggedOn(Interactable interactable)
    {
        base.InteractableDraggedOn(interactable);
        StartCoroutine(Plop(interactable.gameObject));
    }

    IEnumerator Plop(GameObject go)
    {
        float velocity = 0;
        while(go.transform.position.y > waterHeight)
        {
            velocity += Time.deltaTime * fallVelocityInreasePerSecond;
            yield return null;

            go.transform.Translate(Vector2.down * velocity * Time.deltaTime);
        }

        switch (go.GetComponent<Ingredient>().type)
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
        Destroy(go);
        Instantiate(liquidParticle.gameObject, go.transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Vector2 p1 = new Vector2(0, waterHeight);
        Gizmos.DrawLine(p1+Vector2.left, p1+Vector2.right);
    }
}
