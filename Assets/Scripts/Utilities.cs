using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class Utilities
{
#region material properties
    //prevents the material from creating instances
    public static void SetPropertyFloat(this Renderer renderer, string property, float value)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(block);
        block.SetFloat(property, value);
        renderer.SetPropertyBlock(block);
    }
    public static void SetPropertyFloat(this Renderer renderer, string[] properties, float[] values)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(block);
        for (int i = 0; i < properties.Length; i++)
        {
            block.SetFloat(properties[i], values[i]);
        } 
        renderer.SetPropertyBlock(block);
    }
    //
#endregion
    public static ContactPoint2D GetFirstContact(this Collision2D collision)
    {
        return collision.GetContact(0);
    }
    public static T PickRandom<T>(this IEnumerable<T> collection)
    {
        var array = collection.ToArray();
        return array[Random.Range(0, array.Length)];
    }
    public static float PickRandom(this Vector2 range)
    {
        return Mathf.Lerp(range.x, range.y, Random.value);
    }
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
    public static bool TryGetComponentInParent<T>(this Component parentComponent, out T component)
    {
        component = parentComponent.GetComponentInParent<T>();
        return component != null;
    }
    public static bool TryGetComponentInChildren<T>(this Component parentComponent, out T component)
    {
        component = parentComponent.GetComponentInChildren<T>();
        return component != null;
    }

    public static void SetActiveStatNodes<T>(int count, int existingNodes, ref List<T> array) where T : MonoBehaviour
    {
        for (int i = 0; i < existingNodes; i++)
        {
            array[i].gameObject.SetActive(i < count);
        }
    }

    public static void SetOpacity(this SpriteRenderer sr, float opacity)
    {
        Color tmp = sr.color;
        tmp.a = opacity;
        sr.color = tmp;
    }
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}
