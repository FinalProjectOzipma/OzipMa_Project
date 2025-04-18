using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyUnitController : EntityController
{
    public Sprite sprite;
    #region Component
    public Rigidbody2D Rigid { get; private set; }
    #endregion

    // 어차피 컨트롤러는 어드레서블 BrainVariant안에 들어가있으니

    public MyUnit MyUnit { get; private set; }
    public MyUnitStatus MyUnitStatus { get; private set; }
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

    // Wave에서 들고있는것
    // Dictionary<string, int> entityKey = new Dictionary<string, int>();

    // Wave에서 컨트롤러에 접근을하고 컨트롤러.Init(int entityKey["랜덤지정된 엔티티이름"], string 랜덤지정된 엔티티이름);
    // 컨트롤러 Init안에서는 Primary = 매개변수; 
    // Name = 매개변수;

    public override void TakeRoot(int primaryKey, string name, Vector2 position)
    {
        MyUnit = new MyUnit();
        MyUnit.Init(PrimaryKey, sprite);
        // 초기화부분
        Managers.Resource.Instantiate(Name, go =>
        {
            go.transform.SetParent(transform);        // 클래스 초기화
            MyUnitStatus = MyUnit.Status as MyUnitStatus;
            Rigid = go.GetComponent<Rigidbody2D>();
            Init(primaryKey, name, position, go);
        });
    }

    ////적 감지후 감지결과를 오브젝트로 전달
    //public void DetectEnemyRaycast()
    //{
    //    //float detectRadius = MyUnitStatus.AttackRange;
    //    float detectRadius = 1.0f;
    //    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRadius, LayerMask.GetMask("Enemy"));

    //    foreach (var hit in hits)
    //    {
    //        Vector2 dir = (hit.transform.position - transform.position).normalized;
    //        float dist = Vector2.Distance(transform.position, hit.transform.position);

    //        // 장애물 무시하고 Raycast
    //        RaycastHit2D ray = Physics2D.Raycast(transform.position, dir, dist, LayerMask.GetMask("Enemy", "Obstacle"));

    //        if (ray.collider != null && ray.collider.gameObject == hit.gameObject)
    //        {
    //            Target = hit.gameObject;
    //            Util.Log(Target.name);
    //        }
    //    }
    //}
}