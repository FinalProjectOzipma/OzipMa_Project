using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public abstract class EntityController : Poolable
{
    protected GameObject body;


    #region Component
    public Animator Anim { get; private set; }
    public CapsuleCollider2D Colider { get; private set; }
    public ObjectFlash Fx { get; set; }

    #endregion

    public Dictionary<int, KeyPairCondition> Conditions { get; set; } = new();
    public Dictionary<int, float> Times { get; set; } = new();
    public AbilityType CurrentCondition { get; set; } = AbilityType.None;
    public EntityAnimationData AnimData { get; set; }

    public bool IsLeft { get; private set; }
    public int FacDir { get; private set; } = 1;

    public bool IsDead { get; set; }

    public virtual void Init(Vector2 position)
    {
        Anim = GetComponentInChildren<Animator>();
        Colider = GetComponent<CapsuleCollider2D>();
        //IsDead = false;
    }

    protected virtual void Update()
    {
        if(AnimData != null)
            AnimData.StateMachine.CurrentState?.Update();
    }

    private void FixedUpdate()
    {
        if (AnimData != null)
            AnimData.StateMachine.CurrentState?.FixedUpdate();
    }

    public void FlipControll(GameObject target = null)
    {
        if(target != null)
        {
            Vector2 pos = target.transform.position;
            Vector2 mePos = transform.position;

            if (pos.x - mePos.x > 0 && IsLeft)
            {
                OnFlip();
            }
            else if (pos.x - mePos.x < 0 && !IsLeft)
            {
                OnFlip();
            }
        }
        else
        {
            OnFlip();
        }
    }

    protected void OnFlip()
    {
        IsLeft = !IsLeft;
        FacDir *= -1;
        transform.Rotate(0f, 180f * FacDir, 0f);
    }

    //Root부분 생성해주는 파트
    public abstract void TakeRoot(int primaryKey, string name, Vector2 position); 
}
