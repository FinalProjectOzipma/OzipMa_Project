using System;
using UnityEngine;

/// <summary>
/// Tower의 바디에 붙는 애니메이션 트리거
/// </summary>
public class TowerAnimTrigger : MonoBehaviour
{
    public event Action FireAction;
    private static int enemyLayer = -1;

    private void Awake()
    {
        if (enemyLayer < 0)
        {
            enemyLayer = (int)Enums.Layer.Enemy;
        }
    }

    /// <summary>
    /// 애니메이션의 특정 시점에 공격 Fire
    /// </summary>
    public void Fire()
    {
        FireAction?.Invoke();
    }
}
