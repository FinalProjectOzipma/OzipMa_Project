using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBodyBase : MonoBehaviour
{
    public Animator Anim {  get; private set; }
    public TowerAnimationData AnimData { get; private set;}

    public Vector3 FirePosition
    {
        get => gameObject.transform.GetChild(transform.childCount - 1).position;
    }

    protected virtual void Awake()
    {
        Anim = Util.FindComponent<Animator>(gameObject, "MainSprite");
        AnimData = new();
    }
}
