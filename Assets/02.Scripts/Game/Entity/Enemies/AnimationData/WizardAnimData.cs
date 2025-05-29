using UnityEngine;

public class WizardAnimData : EntityAnimationData
{
    private string lightningParameterName = "Lightning";
    private int lightningHash;

    public WizardIdleState IdleState { get; private set; }
    public WizardChasingState ChaseState { get; private set; }
    public WizardShotState AttackState { get; private set; }
    public WizardDeadState DeadState { get; private set; }
    public WizardLightningState LightningState { get; private set; }


    public override void Init(EntityController controller = null)
    {
        base.Init(controller);
        lightningHash = Animator.StringToHash(lightningParameterName);

        IdleState = new WizardIdleState(StateMachine, IdleHash, controller as EnemyController, this);
        ChaseState = new WizardChasingState(StateMachine, ChaseHash, controller as EnemyController, this);
        AttackState = new WizardShotState(StateMachine, AttackHash, controller as EnemyController, this);
        DeadState = new WizardDeadState(StateMachine, DeadHash, controller as EnemyController, this);
        LightningState = new WizardLightningState(StateMachine, lightningHash, controller as EnemyController, this);

        StateMachine.Init(IdleState);
    }
}
