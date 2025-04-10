using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimationData
{
    #region ParameterName
    private string idleParameterName = "Idle";
    private string MoveParameterName = "Move"; 
    private string AttackParameterName = "Attack";
    #endregion

    #region HashProperty
    public int IdleHash { get; private set; }
    public int MoveHash { get; private set; }
    public int AttackHash { get; private set; }
    #endregion

    //StateMachine
    public StateMachine StateMachine { get; private set; }

    public virtual void Init(EntityController controller = null)
    {
        IdleHash = Animator.StringToHash(idleParameterName);
        MoveHash = Animator.StringToHash(MoveParameterName);
        AttackHash = Animator.StringToHash(AttackParameterName);

        StateMachine = new();
    }
}
