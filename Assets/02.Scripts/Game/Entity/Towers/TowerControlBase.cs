using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerControlBase : MonoBehaviour
{
    public Tower Tower {  get; private set; }
    public Animator Anim { get; private set; }
    public TowerAnimationData AnimData { get; private set; }
    public TowerStatus TowerStatus { get; private set; }

    public bool IsPlaced;

    private GameObject body;
    private float attackCooldown = 0f;
    
    public abstract void Attack(float AttackPower);

    protected virtual void Awake()
    {
        Init();
    }

    public void Init()
    {

    }

    private void Update()
    {
        if (!IsPlaced) return;

        attackCooldown -= Time.deltaTime;

        if (attackCooldown < 0)
        {
            attackCooldown = TowerStatus.AttackCoolDown.GetValue();

            Attack(TowerStatus.Attack.GetValue());

            // TODO : Attack 애니메이션 실행 
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
    public void TakeRoot(UserObject Info)
    {
        Tower = Info as Tower;
        TowerStatus = Tower.Status;

        Managers.Resource.Instantiate(Tower.Name, go => {
            body = go;
            body.transform.SetParent(transform);

            if(body.TryGetComponent<TowerBodyBase>(out TowerBodyBase bodyBase))
            {
                Anim = bodyBase.Anim;
                AnimData = bodyBase.AnimData;
            }
        });
    }
}