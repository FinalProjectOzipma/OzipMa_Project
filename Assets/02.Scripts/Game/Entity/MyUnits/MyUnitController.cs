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
    #endregion

    // 어차피 컨트롤러는 어드레서블 BrainVariant안에 들어가있으니

    public MyUnit MyUnit { get; private set; }
    public MyUnitStatus MyUnitStatus { get; private set; }
    public SpriteRenderer spriteRenderer; 
    public NavMeshAgent Agent;

    public GameObject Target;

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
    }

    // Wave에서 들고있는것
    // Dictionary<string, int> entityKey = new Dictionary<string, int>();

    // Wave에서 컨트롤러에 접근을하고 컨트롤러.Init(int entityKey["랜덤지정된 엔티티이름"], string 랜덤지정된 엔티티이름);
    // 컨트롤러 Init안에서는 Primary = 매개변수; 
    // Name = 매개변수;

    private string _Body = nameof(_Body);

    public override void TakeRoot(int primaryKey, string name, Vector2 position)
    {
        MyUnit = new MyUnit();
        MyUnit.Init(PrimaryKey, sprite);
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
        if (1.0f > (Target.transform.position - transform.position).magnitude)
        {
            return true;
        }
        return false;
    }
}