using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

[Serializable]
public abstract class EntityController : MonoBehaviour
{
    [SerializeField]
    public int PrimaryKey { get; set; }
    public string Name { get; set; }

    #region Component
    public Animator Anim { get; private set; }
    public ObjectFlash Fx { get; set; }

    #endregion
    public EntityAnimationData AnimData { get; protected set; }

    public bool IsLeft { get; private set; }
    public int FacDir { get; private set; }

    public virtual void Init(int primaryKey, string name, Vector2 position, GameObject go = null)
    {
        Anim = GetComponentInChildren<Animator>();

        PrimaryKey = primaryKey;
        Name = name;
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

        if (pos.x - mePos.x > 0f && IsLeft)
        {
            OnFlip();
        }
        else if (pos.x - mePos.x < 0f && !IsLeft)
        {
            OnFlip();
        }
    }

    protected void OnFlip()
    {
        IsLeft = !IsLeft;
        FacDir *= -1;
        transform.Rotate(Vector2.up * 180);
    }

    //Root부분 생성해주는 파트
    public abstract void TakeRoot(int primaryKey, string name, Vector2 position); 
}
