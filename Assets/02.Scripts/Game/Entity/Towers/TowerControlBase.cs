using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerControlBase : MonoBehaviour
{
    [Header("테스트용")]
    public int ID = 1;
    public bool IsPlaced; // 맵에 배치되었는가 
    [field: SerializeField] public string Name { get; set; }

    #region 데이터
    public Tower Tower {  get; protected set; }
    public TowerStatus TowerStatus { get; protected set; } // 캐싱용
    public Animator Anim { get; protected set; }
    public TowerAnimationData AnimData { get; protected set; }
    public Sprite Preview { get; protected set; }
    #endregion

    protected LinkedList<EnemyController> detectedEnemies = new(); // 범위 내 적들
    protected GameObject body; // 현재 나의 외형

    private float attackCooldown = 0f;
    public abstract void Attack(float AttackPower);

    private void Start()
    {
        Name = gameObject.name;
        Util.Log(Name);

        // Test용 강제 TakeRoot
        TakeRoot(ID, Name, Vector2.zero);
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
            Anim?.SetTrigger(AnimData.AttackHash);
            Util.Log($"{Name}의 공격");
        }
    }

    private void FixedUpdate()
    {
        foreach(var enemy in detectedEnemies)
        {
            if (enemy == null || enemy.isActiveAndEnabled == false)
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
    public abstract void TakeRoot(int primaryKey, string name, Vector2 position);
    

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