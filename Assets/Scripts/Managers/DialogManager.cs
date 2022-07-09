using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBackground;
    [SerializeField] TextMeshProUGUI characterTextField;

    [SerializeField] float dialogLifeTime = 4f;

    private void Start()
    {
        Gameplay.instance.OnCharacterSpeak.AddListener(ShowMessage);
    }

    private void ShowMessage(string message)
    {
        characterTextField.text = message;

        StopAllCoroutines();
        StartCoroutine(ShowText());
    }

    IEnumerator ShowText()
    {
        scaleDown.Kill();
        dialogBackground.transform.localScale = new Vector3(1f, 1f, 1f);
        dialogBackground.SetActive(true);
        yield return new WaitForSeconds(dialogLifeTime);
        CloseDialog();
    }

    Tween scaleDown;

    public void CloseDialog()
    {
        print("uhu");
        StopAllCoroutines();
        scaleDown.Kill();
        scaleDown = dialogBackground.transform.DOScale(0, 0.3f).SetEase(Ease.OutExpo).OnComplete(() => {
            dialogBackground.SetActive(false);
        });

    }
}
