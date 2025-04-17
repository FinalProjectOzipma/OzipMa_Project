using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimationData
{
    #region ParameterName
    private string idleParameterName = "Idle";
    private string MoveParameterName = "Move"; 
    private string AttackParameterName = "Attack";
    private string ChaseParameterName = "Chase";
    #endregion

    #region HashProperty
    public int IdleHash { get; private set; }
    public int MoveHash { get; private set; }
    public int AttackHash { get; private set; }
    public int ChaseHash { get; private set; } 
    #endregion

    public StateMachine StateMachine { get; protected set; }

    public virtual void Init(EntityController controller = null)
    {
        IdleHash = Animator.StringToHash(idleParameterName);
        MoveHash = Animator.StringToHash(MoveParameterName);
        AttackHash = Animator.StringToHash(AttackParameterName);
        ChaseHash = Animator.StringToHash(ChaseParameterName);

        StateMachine = new();
    }
}
