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
        mouseNormalized = (mouseNormalized * 2) - (mouseNormalized / 2);

        transform.position = (Vector3)mouseNormalized * paralaxAmount - Vector3.forward;
    }
}
