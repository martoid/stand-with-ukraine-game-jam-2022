using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingPot : ActionFinish
{
    [SerializeField] float waterHeight;
    [SerializeField] float fallVelocityInreasePerSecond;

    [SerializeField] GameObject liquidParticle;

    public override void GiveObject(GameObject go)
    {
        base.GiveObject(go);
        StartCoroutine(Plop(go));
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
