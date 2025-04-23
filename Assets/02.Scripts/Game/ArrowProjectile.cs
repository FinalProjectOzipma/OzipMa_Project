using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArrowProjectile : Poolable
{
    #region Component
    private Rigidbody2D rigid;
    private SpriteRenderer spr;
    #endregion
    
    public float Speed;

    private int ownerLayer;
    private BoxCollider2D col;
    private Vector2 dir;

    private int hitLayer;
    private float ownerAttack;

    private int mapLayer = 1 << 6;
    private int enemyLayer = 1 << 8;
    private int unitLayer = 1 << 9;
    private int coreLayer = 1 << 10;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spr = GetComponentInChildren<SpriteRenderer>();
    }

    public void Init(GameObject owner, float ownerAttack, Vector2 targetPos, int factingDir)
    {
        transform.position = owner.transform.position;

        this.ownerLayer = owner.layer;
        this.hitLayer = mapLayer | enemyLayer | unitLayer | coreLayer;
        this.ownerAttack = ownerAttack;


        float angle = Util.GetAngle(transform.position, targetPos);
        spr.flipX = (factingDir < 0) ? true : false;
        spr.transform.rotation = Quaternion.Euler(Vector3.zero);
        spr.transform.Rotate(Vector3.forward * angle);
        dir = (targetPos - (Vector2)owner.transform.position).normalized;
        // 1111 ^ 1011 = 0100
        // 1010 ^ 0101 = 1111
        // 1110 ^ 0111 = 1001

        if ((1 << owner.layer) == enemyLayer)
        {
            // 1111 ^ 0001 = 1110
            hitLayer -= enemyLayer;
        }
        else if ((1 << owner.layer) == unitLayer)
        {
            // 1111 ^ 0010 | 0100 =
            // 1111 ^ 0110 = 1001
            hitLayer -= unitLayer + coreLayer;
        }
    }

    private void FixedUpdate()
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
            if(otherLayer != mapLayer) // 벽 레이어가 아니면 
                other.GetComponentInParent<IDamagable>().ApplyDamage(ownerAttack);

            Managers.Resource.Destroy(gameObject); 
        }
    }
}
