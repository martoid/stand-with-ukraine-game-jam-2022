using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTools : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            DialogManager.instance.OnCharacterSpeak.Invoke(DialogKey.carrotNotChopped);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            DialogManager.instance.OnCharacterSpeak.Invoke(DialogKey.carrotCorrect);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DialogManager.instance.OnCharacterSpeak.Invoke(DialogKey.onionNotChopped);
        }
    }
}
