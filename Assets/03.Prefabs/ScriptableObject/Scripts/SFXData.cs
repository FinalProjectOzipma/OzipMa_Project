using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SFXEntry
{
    public string key;
    public AudioClip clip;
}


[CreateAssetMenu(fileName = "SFXData", menuName = "AudioData/SFXData")]
public class SFXData : ScriptableObject
{
    public List<SFXEntry> entries = new List<SFXEntry>();
}
