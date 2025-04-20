using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerControlBase : MonoBehaviour
{
    public Tower Tower {  get; private set; }
    public TowerStatus TowerStatus { get; private set; } // 캐싱용
    public Animator Anim { get; private set; }
    public TowerAnimationData AnimData { get; private set; }

    public bool IsPlaced; // 맵에 배치되었는가 
    public Sprite Preview { get; private set; }
    [field: SerializeField] public string Name { get; set; }

    protected LinkedList<EnemyController> detectedEnemies = new(); // 범위 내 적들

    private GameObject body;
    private float attackCooldown = 0f;
    public abstract void Attack(float AttackPower);

    private void Start()
    {
        TakeRoot(0, Name, Vector2.zero);
    }

    public void Init()
    {
        CircleCollider2D range = GetComponent<CircleCollider2D>();
        range.radius = TowerStatus == null ? 1f : TowerStatus.AttackRange.GetValue();
    }

    private void Update()
    {
        if (!IsPlaced) return;
        if(detectedEnemies.Count == 0) return;

        attackCooldown -= Time.deltaTime;
        if (attackCooldown < 0)
        {
            attackCooldown = TowerStatus.AttackCoolDown.GetValue();
            Attack(TowerStatus.Attack.GetValue());
            Anim.SetTrigger(AnimData.AttackHash);
        }
    }

    private void FixedUpdate()
    {
        foreach(var enemy in detectedEnemies)
        {
            if (enemy == null)
            {
                detectedEnemies.Remove(enemy);
            }
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
        // 정보 세팅
        Tower = new Tower();
        Tower.Init(primaryKey, Preview);
        TowerStatus = Tower.TowerStatus;

        Init();

        // 외형 로딩
        Managers.Resource.Instantiate(Name, go => {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.gameObject.GetComponentInParent<EnemyController>();
        if (enemy != null)
        {
            detectedEnemies.AddLast(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnemyController enemy = collision.gameObject.GetComponentInParent<EnemyController>();
        if (enemy != null)
        {
            detectedEnemies.Remove(enemy);
        }
    }
}