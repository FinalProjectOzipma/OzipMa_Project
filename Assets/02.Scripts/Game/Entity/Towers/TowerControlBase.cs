using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerControlBase : MonoBehaviour
{
    [Header("테스트용")]
    public int ID = 1;
    public bool IsPlaced; // 맵에 배치되었는가 
    public string Name { get; set; }

    #region 데이터
    public Tower Tower {  get; protected set; }
    public TowerStatus TowerStatus { get; protected set; } // 캐싱용
    public Animator Anim { get; protected set; }
    public TowerAnimationData AnimData { get; protected set; }
    [field:SerializeField] public Sprite Preview { get; protected set; }
    #endregion

    protected LinkedList<EnemyController> detectedEnemies = new(); // 범위 내 적들
    protected GameObject body; // 현재 나의 외형

    private float attackCooldown = 0f;
    private CircleCollider2D range;

    public abstract void Attack(float AttackPower);

    protected virtual void Start()
    {
        Name = gameObject.name;
    }

    public void Init()
    {
        range = GetComponent<CircleCollider2D>();
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
            StartAnimation(AnimData.AttackHash);
        }
    }

    private void LateUpdate()
    {
        LinkedListNode<EnemyController> node = detectedEnemies.First;
        while(node != null)
        {
            EnemyController enemy = node.Value;
            if (enemy == null || enemy.isActiveAndEnabled == false)
            {
                detectedEnemies.Remove(enemy);
            }
            node = node.Next;
        }
    }

    /// <summary>
    /// 타워 시작시키는 함수 (ex:배치 성공하면 실행)
    /// </summary>
    public void TowerStart()
    {
        IsPlaced = true;
        StartAnimation(AnimData.StartHash);
    }

    /// <summary>
    /// 타워 작동 멈추기
    /// </summary>
    public void TowerStop()
    {
        IsPlaced = false;
        StartAnimation(AnimData.EndHash);
    }

    /// <summary>
    /// Tower 정보 넣어주는 함수
    /// </summary>
    public void TakeRoot(int primaryKey, string name, Tower data)
    {
        // 정보 세팅
        Tower = data;
        Name = name;
        Tower.Sprite = Preview;
        TowerStatus = Tower.TowerStatus;

        attackCooldown = TowerStatus.AttackCoolDown.GetValue();
        TowerStatus.AttackRange.OnChangeValue -= ApplyAttackRange;
        TowerStatus.AttackRange.OnChangeValue += ApplyAttackRange;

        Init();

        // 외형 세팅
        TakeBody();
    }

    /// <summary>
    /// 외형 로딩
    /// </summary>
    protected abstract void TakeBody();
    

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

    public void StartAnimation(int AnimHash)
    {
        if (Anim == null) return;
        Anim?.SetTrigger(AnimHash);
    }

    public void ApplyAttackRange(float newValue)
    {
        if(range == null) range = GetComponent<CircleCollider2D>();
        range.radius = TowerStatus == null ? 1f : newValue;
        Util.Log($"어택레인지 {newValue}만큼으로 늘어났다");
    }
}