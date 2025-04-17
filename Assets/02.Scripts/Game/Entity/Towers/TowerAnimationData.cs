using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimationData
{
    #region ParameterName
    private string IdleParameterName = "Idle";
    private string AttackParameterName = "Attack";
    #endregion

    #region HashProperty
    public int IdleHash { get; private set; }
    public int AttackHash { get; private set; }
    #endregion   

    public TowerAnimationData()
    {
        IdleHash = Animator.StringToHash(IdleParameterName);
        AttackHash = Animator.StringToHash(AttackParameterName);
    }
}
