using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFloor : MonoBehaviour
{
    public float MoveSpeed = 1f;
    private GameObject body;
    private Vector3 dir, targetPos;
    private Animator animator;

    public void Init(string floorKey, Vector3 targetPosition, float attackPower, Tower ownerTower)
    {
        Util.Log($"장판의 타겟 x : {targetPosition.x}, y :{targetPosition.y}");
        targetPos = targetPosition;
        dir = (targetPosition - transform.position).normalized;
        // Body 불러오기
        Managers.Resource.Instantiate($"{floorKey}Body", go =>
        {
            body = go;
            animator = body.GetComponentInChildren<Animator>();
            animator.speed = 0f;

            Transform t = go.transform;
            t.SetParent(this.transform);
            t.localPosition = Vector3.zero;

            // Trigger에서 실제 데미지 Apply 처리
            go.GetComponentInChildren<TowerAnimationTrigger>().Init(attackPower, ownerTower, OnAttackFinish);
        });
    }

    private void Update()
    {
        transform.position += dir * MoveSpeed * Time.deltaTime;

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            transform.position = targetPos;
            animator.speed = 1f;
        }
    }

    public void OnAttackFinish()
    {
        // Body 삭제
        Managers.Resource.Destroy(body);
        // this.gameObject 삭제
        Managers.Resource.Destroy(gameObject);
    }
}
