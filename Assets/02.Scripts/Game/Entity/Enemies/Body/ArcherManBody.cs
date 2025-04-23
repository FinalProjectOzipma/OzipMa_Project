using DefaultTable1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ArcherManBody : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        EnemyController ctrl = GetComponentInParent<EnemyController>();
        ctrl.AnimData = new ArcherManAnimData();
        ctrl.AnimData.Init(ctrl);
    }
}
