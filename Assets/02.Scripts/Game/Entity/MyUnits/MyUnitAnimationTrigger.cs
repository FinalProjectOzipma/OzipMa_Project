using DefaultTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnitAnimationTrigger : MonoBehaviour
{
    public Transform AttackCheck;
    float attackValue;
    protected MyUnitController myUnit => GetComponentInParent<MyUnitController>();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        attackValue = myUnit.MyUnitStatus.AttackRange.GetValue();
    }

    public void AnimationTrigger()
    {
        myUnit.AnimationFinishTrigger();
    }

    public virtual void AttackTrigger()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackCheck.position, attackValue);
    }
}
