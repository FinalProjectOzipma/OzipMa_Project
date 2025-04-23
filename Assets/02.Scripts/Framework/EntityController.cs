using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : Poolable
{
    #region Component
    public Animator Anim { get; private set; }
    public BoxCollider2D BoxCol { get; private set; }
    public ObjectFlash Fx { get; set; }

    #endregion
    public EntityAnimationData AnimData { get; set; }

    public bool IsLeft { get; private set; }
    public int FacDir { get; private set; }

    public bool IsDead { get; set; }

    public virtual void Init(Vector2 position, GameObject go = null)
    {
        Anim = GetComponentInChildren<Animator>();
        BoxCol = GetComponentInChildren<BoxCollider2D>();
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

    public void FlipControll(GameObject target)
    {
        if (target == null)
            return;

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

    protected void OnFlip()
    {
        IsLeft = !IsLeft;
        FacDir *= -1;
        transform.rotation = Quaternion.Euler(new Vector3(0, 180f * FacDir, 0));
    }

    //Root부분 생성해주는 파트
    public abstract void TakeRoot(int primaryKey, string name, Vector2 position); 
}
