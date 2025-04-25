using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardBody : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        EnemyController ctrl = GetComponentInParent<EnemyController>();
        ctrl.AnimData = new WizardAnimData();
        ctrl.AnimData.Init(ctrl);
    }
}
