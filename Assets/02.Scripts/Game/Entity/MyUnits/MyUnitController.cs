using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MyUnitController : EntityController, IDamagable
{
    public Sprite sprite;   //아이콘

    #region Component
    public Rigidbody2D Rigid { get; private set; }
    public CapsuleCollider2D capsuleCollider;
    private string _Body = nameof(_Body);
    public SpriteRenderer spriteRenderer;
    public NavMeshAgent Agent;
    public GameObject Target;
    public HealthView healthView;
    #endregion

    #region 정보부
    public MyUnit MyUnit { get; private set; }
    public MyUnitStatus MyUnitStatus { get; private set; }
    #endregion

    private Coroutine DotCor;
    private Coroutine SlowCor;
    private WaitForSeconds dotWFS;
    private WaitForSeconds slowWFS;

    public virtual void AnimationFinishTrigger() => AnimData.StateMachine.CurrentState.AniamtionFinishTrigger();

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Rigid = GetComponent<Rigidbody2D>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    protected override void Update()
    {
        if (AnimData != null)
            AnimData.StateMachine.CurrentState?.Update();
        FlipControll(Target);
    }

    public override void Init(Vector2 position)
    {
        base.Init(position);
        transform.position = position;
        Rigid = GetComponent<Rigidbody2D>();
        AnimData.Init(this);
        MyUnitStatus.Health.OnChangeHealth = healthView.SetHpBar;
        MyUnitStatus.Health.AddValue(0.0f);
        //sethpbar 호출
    }

    /// <summary>
    /// 바디 생성로직
    /// </summary>
    /// <param name="primaryKey">ID값</param>
    /// <param name="name">어드레서블 키이름</param>
    /// <param name="position">생성위치</param>
    public override void TakeRoot(int primaryKey, string name, Vector2 position)
    {
        if (MyUnit == null)
            MyUnit = new MyUnit();

        MyUnit.Init(primaryKey, sprite);

        MyUnitStatus = MyUnit.Status as MyUnitStatus;
        // 초기화부분
        if (body == null)
        {
            Managers.Resource.Instantiate($"{name}{_Body}", go =>
            {
                go.transform.SetParent(transform);
                Fx = go.GetOrAddComponent<ObjectFlash>();
                spriteRenderer = go.GetOrAddComponent<SpriteRenderer>();
                body = go;
                healthView = go.GetComponentInChildren<HealthView>();
                Init(position);
            });
        }
        else
            Init(position);
    }

    /// <summary>
    /// 직격 피해를 입을때 쓸 데미지
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float incomingDamage)
    {
        float defence = Mathf.Max(0f, MyUnitStatus.Defence.GetValue());

        // 부드러운 비율 스케일링
        float damageScale = incomingDamage / (incomingDamage + defence);

        float finalDamage = incomingDamage * damageScale;

        finalDamage = Mathf.Max(finalDamage, 1f); // 최소 1 보장 (선택사항)

        MyUnitStatus.Health.AddValue(-finalDamage);

        Fx.StartBlinkFlash();
    }

    //도트 데미지 적용
    public void TakeDotDamage(float abilityValue, float abilityDuration, float abilityCooldown)
    {
        if (DotCor != null)
        {
            StopCoroutine(DotCor);
            DotCor = null;
        }
        dotWFS = new(abilityCooldown);
        DotCor = StartCoroutine(OnDotDamage(abilityValue, abilityDuration, abilityCooldown));
    }  
    //도트 데미지 코루틴
    private IEnumerator OnDotDamage(float abilityValue, float abilityDuration, float abilityCooldown)
    {
        while(abilityDuration > 0)
        {
            abilityDuration -= Time.deltaTime;
            TakeDamage(abilityValue);
            yield return dotWFS;
        }
    }

    //슬로우 적용
    public void TakeSlow(float abilityValue, float abilityDuration)
    {
        if (SlowCor != null)
        {
            StopCoroutine(SlowCor);
            SlowCor = null;
        }
        slowWFS = new(abilityDuration);
        SlowCor = StartCoroutine(OnSlow(abilityValue, abilityDuration));
    }
    //슬로우 코루틴
    private IEnumerator OnSlow(float abilityValue, float abilityDuration)
    {
        MyUnitStatus.MoveSpeed.SetValueMultiples(abilityValue);
        yield return slowWFS;
    }

    //반사 데미지 적용
    public void ReflectDamage(float damage, float abilityRatio)
    {
        TakeDamage(damage* abilityRatio);
    }

    //실제 트리거에서 호출되는 메서드
    public void ApplyDamage(float amount, AbilityType condition = AbilityType.None, GameObject go = null)
    {
        TakeDamage(amount);
    }
}