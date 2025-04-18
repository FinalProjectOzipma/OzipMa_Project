using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    public int PrimaryKey { get; set; }
    public string Name { get; set; }

    public Animator Anim { get; private set; }
    public EntityAnimationData AnimData { get; protected set; }

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

    //Root부분 생성해주는 파트
    public abstract void TakeRoot(int primaryKey, string name, Vector2 position);

    /// <summary>
    /// 타겟이 공격거리내에 있다면 true
    /// 밖에 있다면 false를 반환
    /// </summary>
    /// <returns></returns>
    public bool IsClose()
    {
        if (controller.MyUnitStatus.AttackRange.GetValue() > (controller.Target.transform.position - controller.transform.position).magnitude)
        {
            return true;
        }
        return false;
    }
}
