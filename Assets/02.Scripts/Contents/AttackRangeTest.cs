using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeTest : MonoBehaviour
{
    [Header("수진넴 이걸 좌우드래그하면 편해요 ㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎㅎ")]
    public float AttackRatio;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AttackRatio);
    }
}
