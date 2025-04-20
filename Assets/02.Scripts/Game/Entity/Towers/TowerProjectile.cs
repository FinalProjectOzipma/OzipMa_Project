using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    public EnemyController Target { get; set; }
    private Transform targetTransform {  get; set; }
    private Rigidbody2D rb;
    private float speed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(float attackPower, Tower ownerTower, EnemyController target)
    {
        Target = target;
        targetTransform = target.transform;
        //TODO 
    }

    private void Update()
    {
        if (targetTransform == null) return;
        Vector2 dir = (targetTransform.position- transform.position).normalized;
        rb.velocity = dir * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.gameObject.GetComponentInParent<EnemyController>();
        if (enemy == Target)
        {
            // TODO : 공격 Apply
            Util.LogWarning("공격");
            Managers.Resource.Destroy(gameObject);
        }
    }
}
