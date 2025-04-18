using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerControlBase : MonoBehaviour
{
    public Tower Tower {  get; private set; }
    public Animator Anim { get; private set; }
    public TowerAnimationData AnimData { get; private set; }
    public TowerStatus TowerStatus { get; private set; }

    public bool IsPlaced; // 맵에 배치되었는가 
    public Sprite Preview { get; private set; }

    private GameObject body;
    private float attackCooldown = 0f;
    
    public abstract void Attack(float AttackPower);

    protected virtual void Awake()
    {
        Init();

        //Test용 강제 TakeRoot
        //TakeRoot(null);
    }

    public void Init()
    {
        CircleCollider2D range = GetComponent<CircleCollider2D>();
        range.radius = TowerStatus == null ? 1f : TowerStatus.AttackRange.GetValue();
    }

    private void Update()
    {
        if (!IsPlaced) return;

        attackCooldown -= Time.deltaTime;

        if (attackCooldown < 0)
        {
            attackCooldown = TowerStatus.AttackCoolDown.GetValue();

            Attack(TowerStatus.Attack.GetValue());

            Anim.SetTrigger(AnimData.AttackHash);
        }
    }

    /// <summary>
    /// 타워 동작 시작시키는 함수 (ex:배치 성공하면 실행)
    /// </summary>
    public void TowerStart()
    {
        IsPlaced = true;
    }

    /// <summary>
    /// Tower 정보 넣어주는 함수
    /// </summary>
    /// <param name="Info">Tower 데이터</param>
    public void TakeRoot(int primaryKey, string name, Vector2 position)
    {
        // 정보 받기
        Tower = new Tower();
        Tower.Init(primaryKey, Preview);
        TowerStatus = Tower.Status;

        Init();

        // Body 로딩
        Managers.Resource.Instantiate("BodyTest", go => {
            body = go;
            body.transform.SetParent(transform);
            body.transform.localPosition = Vector3.zero;

            if (body.TryGetComponent<TowerBodyBase>(out TowerBodyBase bodyBase))
            {
                Anim = bodyBase.Anim;
                AnimData = bodyBase.AnimData;
            }
        });
    }
}