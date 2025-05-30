using UnityEngine;

public class EntityAnimationData
{
    #region ParameterName
    private string idleParameterName = "Idle";
    private string ChaseParameterName = "Chase";
    private string AttackParameterName = "Attack";
    private string DeadParameterName = "Dead";
    private string DarkParameterName = "Dark";
    #endregion

    #region HashProperty
    protected int IdleHash { get; set; }
    protected int ChaseHash { get; set; }
    protected int AttackHash { get; set; }
    protected int DeadHash { get; set; }
    protected int DarkHash { get; set; }
    #endregion

    public StateMachine StateMachine { get; protected set; }

    public virtual void Init(EntityController controller = null)
    {
        IdleHash = Animator.StringToHash(idleParameterName);
        ChaseHash = Animator.StringToHash(ChaseParameterName);
        AttackHash = Animator.StringToHash(AttackParameterName);
        DeadHash = Animator.StringToHash(DeadParameterName);
        DarkHash = Animator.StringToHash(DarkParameterName);

        StateMachine = new();
    }
}
