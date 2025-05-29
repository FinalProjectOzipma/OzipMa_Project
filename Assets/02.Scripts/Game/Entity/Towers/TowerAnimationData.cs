using UnityEngine;

public class TowerAnimationData
{
    #region ParameterName
    private string StartParameterName = "Start";
    private string AttackParameterName = "Attack";
    private string EndParameterName = "End";
    #endregion

    #region HashProperty
    public int StartHash { get; private set; }
    public int AttackHash { get; private set; }
    public int EndHash { get; private set; }
    #endregion   

    public TowerAnimationData()
    {
        StartHash = Animator.StringToHash(StartParameterName);
        AttackHash = Animator.StringToHash(AttackParameterName);
        EndHash = Animator.StringToHash(EndParameterName);
    }
}
