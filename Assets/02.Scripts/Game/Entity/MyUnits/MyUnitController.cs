using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyUnitController : EntityController
{
    //아이콘
    public Sprite sprite; 
    #region Component
    public Rigidbody2D Rigid { get; private set; }
    private string _Body = nameof(_Body);
    #endregion

    // 어차피 컨트롤러는 어드레서블 BrainVariant안에 들어가있으니

    public MyUnit MyUnit { get; private set; }
    public MyUnitStatus MyUnitStatus { get; private set; }
    public SpriteRenderer spriteRenderer; 
    public NavMeshAgent Agent;

    public GameObject Target;

    public virtual void AnimationFinishTrigger() => AnimData.StateMachine.CurrentState.AniamtionFinishTrigger();

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }

    public override void Init(int primaryKey, string name, Vector2 position, GameObject go = null)
    {
        base.Init(primaryKey, name, position, go);
        transform.position = position;
        AnimData = new MyUnitAnimationData();
        AnimData.Init(this);
    }

    protected override void Update()
    {
        base.Update();
        //타겟의 방향에 따라서 회전시킴
        if (Target != null)
        {
            float dir = Target.transform.position.x - transform.position.x;

            if (!Mathf.Approximately(dir, 0))
            {
                spriteRenderer.flipX = dir < 0;
            }
        }
    }

    // Wave에서 들고있는것
    // Dictionary<string, int> entityKey = new Dictionary<string, int>();

    // Wave에서 컨트롤러에 접근을하고 컨트롤러.Init(int entityKey["랜덤지정된 엔티티이름"], string 랜덤지정된 엔티티이름);
    // 컨트롤러 Init안에서는 Primary = 매개변수; 
    // Name = 매개변수;



    /// <summary>
    /// 바디 생성로직
    /// </summary>
    /// <param name="primaryKey">ID값</param>
    /// <param name="name">어드레서블 키이름</param>
    /// <param name="position">생성위치</param>
    public override void TakeRoot(int primaryKey, string name, Vector2 position)
    {
        MyUnit = new MyUnit();
        MyUnit.Init(PrimaryKey, sprite);

        MyUnitStatus = MyUnit.Status as MyUnitStatus;
        // 초기화부분
        Managers.Resource.Instantiate($"{name}{_Body}", go =>
        {
            go.transform.SetParent(transform);
            Rigid = go.GetOrAddComponent<Rigidbody2D>();
            Init(primaryKey, name, position, go);
        });
    }

    /// <summary>
    /// 타겟이 공격거리내에 있다면 true
    /// 밖에 있다면 false를 반환
    /// </summary>
    /// <returns></returns>
    public bool IsClose()
    {
        if (Target == null)
            return false;
        float r = MyUnitStatus.AttackRange.GetValue();

        return  r * r> (Target.transform.position - transform.position).sqrMagnitude;
    }



    /// <summary>
    /// 피해를 입을때 쓸 데미지
    /// </summary>
    /// <param name="damage">음수 말고 양수로 던져주면 됨</param>
    public void TakeDamage(float damage)
    {
        MyUnitStatus.Health.AddValue(-damage);
        Fx.StartBlinkFlash();
    }
}