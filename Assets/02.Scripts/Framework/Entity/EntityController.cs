using DefaultTable;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public abstract class EntityController : Poolable
{
    protected string entityName;
    public GameObject Body { get; protected set; } // 나중에 EntityBodyBase로 만들어서 DeadState때 GetComponent호출 줄이기

    public StatusBase Status { get; protected set; }
    #region Component
    public Animator Anim { get; private set; }
    public CapsuleCollider2D Colider { get; private set; }
    public ObjectFlash Fx { get; set; }

    #endregion

    public Dictionary<int, IConditionable> Conditions { get; set; } = new();
    public Dictionary<int, ConditionHandler> ConditionHandlers { get; set; } = new();
    public Dictionary<int, float> Times { get; set; } = new();


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

    public void ApplyCondition(AbilityType condition, float damage, GameObject atker = null, AbilityDefaultValue values = null)
    {
        int iCondition = (int)condition;
        if (Times.ContainsKey(iCondition))
        {
            if (Times[iCondition] <= 0f)
            {
                Times[iCondition] = ConditionHandlers[iCondition].CoolDown;
                ConditionHandlers[iCondition].Attacker = atker.transform;

                if (Conditions.ContainsKey(iCondition))
                    Conditions[iCondition].Execute(damage, values);
                else
                    Util.LogWarning($"{entityName}에 현재 키로 된 Conditions가 없습니다.");
            }
        }
        else
        {
            // 실수 방지 경고로그
            if (Conditions.ContainsKey(iCondition))
                Util.LogWarning($"{entityName} 바디에 {condition}키를 추가해주세요");
        }
    }

    public void EntityDestroy()
    {
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        float alpha = 1;
        EntityBodyBase body = Body.GetComponent<EntityBodyBase>();
        body.Disable();

        while(alpha > 0f)
        {
            body.Spr.color = new Color(1f, 1f, 1f, alpha);
            alpha = Mathf.Max(0, alpha - Time.deltaTime);
            yield return null;
        }

        Managers.Resource.Destroy(gameObject);
    }
}
