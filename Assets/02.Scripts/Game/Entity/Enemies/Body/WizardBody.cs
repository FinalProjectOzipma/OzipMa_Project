using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBody : MonoBehaviour
{
    private EnemyController ctrl;
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        ctrl = GetComponentInParent<EnemyController>();
        ctrl.AnimData = new WizardAnimData();
        ctrl.AnimData.Init(ctrl);
    }

    protected void OnEnable()
    {
        if (ctrl != null)
        {
            WizardAnimData data = ctrl.AnimData as WizardAnimData;
            ctrl.AnimData.StateMachine.ChangeState(data.IdleState);
        }
    }
}
