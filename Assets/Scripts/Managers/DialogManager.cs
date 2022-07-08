using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum DialogKey
{
    carrotNotChopped = 0,
    carrotCorrect = 1,
    onionNotChopped = 2,
}

[Serializable]
public class DialogText
{
    public DialogKey key;
    public string text;
}

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    [SerializeField] DialogSO dialogSO;

    [SerializeField] TextMeshProUGUI characterTextField;

    [HideInInspector] public UnityEvent<DialogKey> OnCharacterSpeak = new UnityEvent<DialogKey>();

    [SerializeField] float typingSpeed = 0.02f;

    private void Awake()
    {
        instance = this;

        OnCharacterSpeak.AddListener(ShowMessage);
    }

    private void ShowMessage(DialogKey key)
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(FindText(key)));
    }

    IEnumerator TypeSentence(string sentence)
    {
        characterTextField.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            characterTextField.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private string FindText(DialogKey key)
    {
        foreach (var message in dialogSO.messages)
        {
            if (message.key == key)
                return message.text;
        }

        return null;
    }
}
