using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InteractionSystem2D;

public class CameraController : MonoBehaviour
{
    [SerializeField] float paralaxAmount;

    private void Update()
    {
        Vector2 mouseNormalized = Input.mousePosition / Screen.height;
        mouseNormalized = ((mouseNormalized) - (Vector2.one/2)) * 2;

        transform.position = (Vector3)mouseNormalized * paralaxAmount - Vector3.forward*10;
    }
}
