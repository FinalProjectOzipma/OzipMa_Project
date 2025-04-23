using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MyUnitAnimationData : EntityAnimationData
{
    public MyUnitStateBase IdleState { get; protected set; }
    public MyUnitStateBase ChaseState { get; protected set; }
    public MyUnitStateBase AttackState { get; protected set; }
    public MyUnitStateBase DeadState { get; protected set; }
}