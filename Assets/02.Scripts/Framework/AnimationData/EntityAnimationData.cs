using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimationData
{
    #region ParameterName
    private string idleParameterName = "Idle";
    private string ChaseParameterName = "Chase"; 
    private string AttackParameterName = "Attack";
    #endregion

    #region HashProperty
    public int IdleHash { get; private set; }
    public int ChaseHash { get; private set; }
    public int AttackHash { get; private set; }
    #endregion

    public StateMachine StateMachine { get; protected set; }

    public virtual void Init(EntityController controller = null)
    {
        IdleHash = Animator.StringToHash(idleParameterName);
        ChaseHash = Animator.StringToHash(ChaseParameterName);
        AttackHash = Animator.StringToHash(AttackParameterName);

        StateMachine = new();
    }
}
