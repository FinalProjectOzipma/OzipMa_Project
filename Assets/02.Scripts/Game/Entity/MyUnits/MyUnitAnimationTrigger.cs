using DefaultTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
        if (myUnit.Target == null)
        {
            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackCheck.position, attackValue);
    }

    protected bool IsReflect(GameObject target)
    {
        EnemyController enemy = target.GetComponent<EnemyController>();

        if (enemy.Enemy.AtkType == AtkType.ReflectDamage)
        {
            return true;
        }
        return false;
    }
}
