using InteractionSystem2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public class Gameplay : MonoBehaviour
{
    public BortschRecipeSO recipe;

    [HideInInspector] public UnityEvent<Action> OnActionPerformed = new UnityEvent<Action>();
    [HideInInspector] public UnityEvent<string> OnCharacterSpeak = new UnityEvent<string>();

    public static Gameplay instance;

    [SerializeField] private string[] randomHelloText;

    public InteractableController interactable;

    public CuttingBoard cuttingBoard;
    public CookingPot cookingPot;
    public Furnance furnance;
    public Grater grater;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnCharacterSpeak.Invoke(randomHelloText[Random.Range(0, randomHelloText.Length)]);
    }

    public bool isDragging()
    {
        return interactable.isDragging();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Speak(string message)
    {
        OnCharacterSpeak.Invoke(message);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
