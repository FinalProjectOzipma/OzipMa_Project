using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class KeyPairCondition
{
    public AbilityType Key;
    public GameObject GameObj;
    public float CoolDown;

    public bool IsExit { get; set; }

    public void ObjectActive(bool active)
    {
        if (GameObj == null) return;
        GameObj.SetActive(active);
        IsExit = !active;
    } 
}
