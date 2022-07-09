using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CurtainSystem : MonoBehaviour
{
    [SerializeField] List<Transform> Curtains;
    [SerializeField] Volume volume;

    [SerializeField] Vector2 distance;

    private void Update()
    {
        float dis = Vector2.Distance(Curtains[0].position, Curtains[1].position);

        float progress = Mathf.Clamp01(Mathf.InverseLerp(distance.y, distance.x, dis));
        volume.weight = progress;
    }
}
