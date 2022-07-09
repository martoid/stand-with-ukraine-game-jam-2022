using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : MonoBehaviour
{
    private void Update()
    {
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.R))
        {
            Gameplay.instance.Restart();
        }

#endif
    }
}
