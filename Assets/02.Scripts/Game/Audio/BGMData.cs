using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BGMEntry
{
    public string key;
    public AudioClip clip;
}

[CreateAssetMenu(fileName = "BGMData", menuName = "AudioData/BGMData")]
public class BGMData : ScriptableObject
{
    public List<BGMEntry> entries = new List<BGMEntry>();
}
