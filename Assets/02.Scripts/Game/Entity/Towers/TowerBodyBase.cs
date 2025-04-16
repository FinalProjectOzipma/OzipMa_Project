using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBodyBase : MonoBehaviour
{
    public Animator Anim {  get; private set; }
    public TowerAnimationData AnimData { get; private set;}

    protected virtual void Awake()
    {
        Anim = Util.FindComponent<Animator>(gameObject, "MainSprite");
        AnimData = new();
    }

    void Start()
    {
        // TODO : 등장 애니메이션 있으면 실행    
    }
}
