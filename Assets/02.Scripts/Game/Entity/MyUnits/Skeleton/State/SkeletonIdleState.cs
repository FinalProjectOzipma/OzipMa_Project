using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonIdleState : MyUnitStateBase
{
    public SkeletonIdleState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (controller.Target == null)
        {
            SetTarget();
        }
        else
        {
            if (!controller.Target.activeSelf)
            {
                SetTarget();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        time += Time.deltaTime;
        //타겟이 없다면
        if (controller.Target == null)
        {
            //Util.Log("타겟이 없네요?");
            SetTarget();
        }
        //타겟이 존재하는데
        else
        {
            Util.Log("타겟은 있는데");
            //비활성화 되어있지 않다면
            if (controller.Target.activeSelf)
            {
                //공격범위 외라면
                if (!controller.IsClose())
                {
                    //추격상태로 전환
                    StateMachine.ChangeState(data.ChaseState);
                }
                else
                {
                    //공격범위에 있으면서 공격쿨타임이 돌았다면
                    if (time >= controller.MyUnitStatus.AttackCoolDown.GetValue())
                    {
                        //공격상태로 전환
                        StateMachine.ChangeState(data.AttackState);
                    }
                }
            }
            else
            {
                Util.Log("비활성화 돼있네요?");
                SetTarget();
            }
        }
    }

    public void SetTarget()
    {
        //Managers.Wave에서 남은 적 리스트 가져오기
        List<GameObject> enemys = Managers.Wave.CurEnemyList;
        //적이 없으면 그냥 리턴해버리기
        if (enemys.Count == 0)
        {
            controller.Target = null;
            return;
        }

        float minDistance = float.MaxValue;
        controller.Target = enemys[0];
        //적들과의 거리를 비교하고
        foreach (GameObject enemy in enemys)
        {
            float distance = (controller.transform.position - enemy.transform.position).magnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                controller.Target = enemy;
            }
        }
    }
}
