using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioSO", menuName = "Data/AudioSO", order = 1)]
public class AudioSO : ScriptableObject
{
    public Sound[] sounds;
}