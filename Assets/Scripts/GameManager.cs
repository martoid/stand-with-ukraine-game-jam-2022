using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-500)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] GameConfigSO gameConfig;

    public static GameConfigSO Config => Instance.gameConfig;

    private void Awake()
    {
        Instance = this;
    }
}
