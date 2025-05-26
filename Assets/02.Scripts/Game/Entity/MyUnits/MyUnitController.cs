using UnityEngine;
using UnityEngine.AI;

public class MyUnitController : EntityController, IDamagable
{
    #region Component
    public Rigidbody2D Rigid { get; private set; }
    public SpriteRenderer spriteRenderer;
    public NavMeshAgent Agent;
    public SpriteTrail ST;
    #endregion

    #region 정보부
    public MyUnit MyUnit { get; private set; }
    public GameObject Target;
    public Sprite sprite;   //아이콘
    private string _Body = nameof(_Body);
    #endregion

    //죽었을때 처리
    public virtual void AnimationFinishTrigger() => AnimData.StateMachine.CurrentState.AnimationFinishTrigger();
    public virtual void AnimationFinishProjectileTrigger() => AnimData.StateMachine.CurrentState.AnimationFinishProjectileTrigger();
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Rigid = GetComponent<Rigidbody2D>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;

        // 컨디션 초기화 (도트데미지)
        Conditions.Add((int)AbilityType.Explosive, new ExplosiveCondition<MyUnitController>(this));
    }

    protected override void Update()
    {
        if (AnimData != null)
            AnimData.StateMachine.CurrentState?.Update();
    }

    public override void Init(Vector2 position)
    {
        base.Init(position);
        transform.position = position;
        Target = null;
        Body.GetComponent<EntityBodyBase>().Enable(); // 죽었을때 Disable 처리해줘야됨
    }

    /// <summary>
    /// 바디 생성로직
    /// </summary>
    /// <param name="primaryKey">ID값</param>
    /// <param name="name">어드레서블 키이름</param>
    /// <param name="position">생성위치</param>
    public override void TakeRoot(int primaryKey, string name, Vector2 position)
    {
        entityName = name;

        if (MyUnit == null)
            MyUnit = new MyUnit();

        MyUnit.Init(primaryKey, sprite);

        MyUnit.Status.InitHealth();

        Status = MyUnit.Status;

        // 초기화부분
        if (Body == null)
        {
            Managers.Resource.Instantiate($"{name}{_Body}", go =>
            {
                go.transform.SetParent(transform);
                Fx = go.GetOrAddComponent<ObjectFlash>();
                spriteRenderer = go.GetOrAddComponent<SpriteRenderer>();
                ST = go.GetComponent<SpriteTrail>();
                Body = go;
                Init(position);
            });
        }
        else
            Init(position);
    }


    #region 데미지 입는 메서드

    /// <summary>
    /// 실제 데미지 입는 메서드
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float incomingDamage)
    {
        float defence = Mathf.Max(0f, Status.Defence.GetValue());

        // 부드러운 비율 스케일링
        float damageScale = incomingDamage / (incomingDamage + defence);

        float finalDamage = incomingDamage * damageScale;

        Managers.Resource.Instantiate("DamageTxt", go =>
        {
            go.GetComponent<Damage>().Init(finalDamage, 18, Body.transform.position);
        });

        finalDamage = Mathf.Max(finalDamage, 1f); // 최소 1 보장 (선택사항)

        Status.Health.AddValue(-finalDamage);

        Fx.StartBlinkFlash();
    }

    /// <summary>
    /// 반사 데미지 적용
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="abilityRatio"></param>
    public void ReflectDamage(float damage, float abilityRatio)
    {
        TakeDamage(damage * abilityRatio);
    }

    /// <summary>
    /// 애니메이션 트리거에서 호출되는 메서드
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="condition"></param>
    /// <param name="go"></param>
    /// <param name="abilities"></param>
    public void ApplyDamage(float amount, AbilityType condition = AbilityType.None, GameObject go = null, DefaultTable.AbilityDefaultValue abilities = null)
    {
        TakeDamage(amount);
        ApplyCondition(condition, amount, go, abilities);
    }
    #endregion
}