using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArrowProjectile : Poolable
{
    public float Power;

    private int ownerLayer;
    private Rigidbody2D rigid;
    private BoxCollider2D col;

    private int hitLayer;
    private float ownerAttack;

    private int mapLayer = 1 << 6;
    private int enemyLayer = 1 << 8;
    private int unitLayer = 1 << 9;
    private int coreLayer = 1 << 10;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(GameObject owner, float ownerAttack, Vector2 direction)
    {
        this.ownerLayer = owner.layer;
        this.hitLayer = mapLayer | enemyLayer | unitLayer | coreLayer;
        this.ownerAttack = ownerAttack;
        this.rigid.velocity = direction * Power * Time.deltaTime;

        // 1111 ^ 1011 = 0100
        // 1010 ^ 0101 = 1111
        // 1110 ^ 0111 = 1001

        if (owner.layer == enemyLayer)
        {
            // 1111 ^ 0001 = 1110
            hitLayer ^= enemyLayer;
        }
        else if (owner.layer == unitLayer)
        {
            // 1111 ^ 0010 | 0100 =
            // 1111 ^ 0110 = 1001
            hitLayer ^= unitLayer | coreLayer;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // IF 1110 ^ 0001 = 0000
        // IF 1110 ^ 0010 = 0010
        if((hitLayer & other.gameObject.layer) > 0)
        {
            Managers.Resource.Destroy(gameObject);
        }
    }
}
