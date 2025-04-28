using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MyUnitController : EntityController, IDamagable
{
    public Sprite sprite;   //아이콘

    #region Component
    public Rigidbody2D Rigid { get; private set; }
    private string _Body = nameof(_Body);
    public SpriteRenderer spriteRenderer;
    public NavMeshAgent Agent;
    public GameObject Target;
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
    }

    /// <summary>
    /// 바디 생성로직
    /// </summary>
    /// <param name="primaryKey">ID값</param>
    /// <param name="name">어드레서블 키이름</param>
    /// <param name="position">생성위치</param>
    public override void TakeRoot(int primaryKey, string name, Vector2 position)
    {
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
                Init(position);
            });
        }
        else
            Init(position);
    }

    /// <summary>
    /// 타겟이 공격거리내에 있다면 true
    /// 밖에 있다면 false를 반환
    /// </summary>
    /// <returns></returns>
    public bool IsClose()
    {
        if (!Target.activeSelf || Target == null)
            return false;
        float r = MyUnitStatus.AttackRange.GetValue();

        return  r * r> (Target.transform.position - transform.position).sqrMagnitude;
    }

    /// <summary>
    /// 직격 피해를 입을때 쓸 데미지
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        if (MyUnitStatus.Defence.GetValue() > damage)
        {   
            Util.Log("안아프지렁" + "방어력 :" + MyUnitStatus.Defence.GetValue());
            return;
        }
        Util.Log("Damage: "+ damage.ToString());
        float dam = Mathf.Max(damage - MyUnitStatus.Defence.GetValue(), 0);
        MyUnitStatus.Health.AddValue(-damage);
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
        TakeDamage(damage);
    }

    //실제 트리거에서 호출되는 메서드
    public void ApplyDamage(float amount, AbilityType condition = AbilityType.None, GameObject go = null)
    {
        TakeDamage(amount);
    }

}