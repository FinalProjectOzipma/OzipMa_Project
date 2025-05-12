using System;
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
    public GameObject AttackRangeObj;
    [field:SerializeField] public Sprite Preview { get; protected set; }
    #endregion

    protected LinkedList<EnemyController> detectedEnemies = new(); // 범위 내 적들
    protected GameObject body; // 현재 나의 외형
    protected TowerBodyBase towerBodyBase;

    private float attackCooldown = 0f;
    private CircleCollider2D range;

    /// <summary>
    /// 공격 유형별 Attack 구현
    /// </summary>
    /// <param name="AttackPower">기본 공격력</param>
    public abstract void Attack(float AttackPower);

    /// <summary>
    /// 외형 로딩
    /// </summary>
    protected abstract void TakeBody();

    protected virtual void Start()
    {
        Name = gameObject.name;
    }

    public void Init()
    {
        ApplyAttackRange(TowerStatus.AttackRange.GetValue());
        if(AttackRangeObj == null) // 인스펙터창에서 넣어줬으나 혹시 없다면 찾아줌
        {
            AttackRangeObj = transform.Find("AttackRangeSprite").gameObject;
        }
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
        Init();
        IsPlaced = true;
        StartAnimation(AnimData.StartHash);
    }

    /// <summary>
    /// 타워 작동 멈추기
    /// </summary>
    public void TowerStop()
    {
        IsPlaced = false;
        StartAnimation(AnimData.EndHash); // 타워 보관 애니메이션 실행
        ShowRangeIndicator(); // 타워 사거리 표시
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

    public TowerBodyBase GetTowerBodyBase()
    {
        if(towerBodyBase != null) return towerBodyBase;
        if(body != null)
        {
            return body.GetComponent<TowerBodyBase>();
        }
        return null;
    }
    public void ShowRangeIndicator()
    {
        AttackRangeObj.SetActive(true);
    }
    public void HideRangeIndicator()
    {
        AttackRangeObj.SetActive(false);
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

    protected void StartAnimation(int AnimHash)
    {
        if (Anim == null) return;
        Anim?.SetTrigger(AnimHash);
    }

    protected void ApplyAttackRange(float newValue)
    {
        if(range == null) range = GetComponent<CircleCollider2D>();
        range.radius = TowerStatus == null ? 1f : newValue;
        AttackRangeObj.transform.localScale = Vector3.one * newValue; // 타워 사거리 표시기 업데이트
        Util.Log($"{Name} 타워의 어택레인지 {newValue}만큼으로 세팅됐다");
    }
}