using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimationData
{
    #region ParameterName
    private string AttackParameterName = "Attack";
    #endregion

    #region HashProperty
    public int AttackHash { get; private set; }
    #endregion   

    public TowerAnimationData()
    {
        AttackHash = Animator.StringToHash(AttackParameterName);
    }
}
