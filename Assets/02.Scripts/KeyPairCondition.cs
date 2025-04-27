using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class KeyPairCondition
{
    public AbilityType Key;
    public GameObject GO;
    public float CoolDown;
    public bool IsExit { get; set; }

    public void ObjectActive(bool active)
    {
        GO.SetActive(active);
        IsExit = !active;
    } 
}
