using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EntityProjectile : Poolable
{
    #region Component
    private Rigidbody2D rigid;
    [SerializeField] protected SpriteTrail spTrail;
    #endregion
    
    public float Speed;

    private int ownerLayer;
    private BoxCollider2D col;
    private Vector2 dir;

    private int hitLayer;
    private float ownerAttack;

    public GameObject Owner;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 초기 위치, attacker 공격력, 타겟 위치, controller 좌우 방향 그냥 FacDir 넣으셈 
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="ownerAttack"></param>
    /// <param name="targetPos"></param>
    public virtual void Init(GameObject owner, float ownerAttack, Vector2 targetPos)
    {
        Owner = owner.transform.parent.gameObject;
        transform.position = owner.transform.position;
        
        this.ownerLayer = owner.layer;
        this.hitLayer = (int)Enums.Layer.Map | (int)Enums.Layer.Enemy | (int)Enums.Layer.MyUnit | (int)Enums.Layer.Core;
        this.ownerAttack = ownerAttack;

        float angle = Util.GetAngle(transform.position, targetPos);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.Rotate(Vector3.forward * angle);

        spTrail?.SetActive(true, transform);

        dir = (targetPos - (Vector2)owner.transform.position).normalized;


        if ((1 << owner.layer) == (int)Enums.Layer.Enemy)
        {
            hitLayer -= (int)Enums.Layer.Enemy;
        }
        else if ((1 << owner.layer) == (int)Enums.Layer.MyUnit)
        {
            // 1111 ^ 0010 | 0100 =
            // 1111 ^ 0110 = 1001
            hitLayer -= (int)Enums.Layer.MyUnit + (int)Enums.Layer.Core;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        //웨이브 종료조건
        //TODO : 나중에 현재 웨이브상태 수정되면 다시 조건을 상태를 활용하도록 수정하기
        if (Managers.Wave.CurrentState == Enums.WaveState.End)
        {
            if (gameObject.activeInHierarchy)
            {
                Managers.Resource.Destroy(gameObject);
                spTrail?.SetActive(false);
            }
        }
    }

    protected virtual void Move()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + (Vector3)(dir * Speed), Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // IF 1110 ^ 0001 = 0000
        // IF 1110 ^ 0010 = 0010
        int otherLayer = 1 << other.gameObject.layer;

        if ((hitLayer & otherLayer) > 0) // 같은 레이어 무시
        {
            if (otherLayer != (int)Enums.Layer.Map) // 벽 레이어가 아니면 
            {
                other.GetComponentInParent<IDamagable>().ApplyDamage(ownerAttack, AbilityType.None, Owner);
                Managers.Audio.audioControler.PlaySFX(SFXClipName.Hit);
            }

            OnPoolDestroy(otherLayer);
        }
    }

    protected virtual void OnPoolDestroy(int hitLayer)
    {
        if (gameObject.activeInHierarchy)
        {
            Managers.Resource.Destroy(gameObject);
            spTrail?.SetActive(false);
        }
    }
}
