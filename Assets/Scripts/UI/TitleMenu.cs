using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    [SerializeReference] private Animator anim;
    [SerializeReference] GameObject crossImage;

    public void Play()
    {
        crossImage.SetActive(true);
        anim.SetTrigger("end");
        DOVirtual.DelayedCall(1f, () => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });
    }

    public void Quit()
    {
        Application.Quit();
    }
}
