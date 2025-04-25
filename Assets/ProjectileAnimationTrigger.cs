using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAnimationTrigger : MonoBehaviour
{
    private AnimProjectile projectile;

    private void Awake()
    {
        projectile = GetComponent<AnimProjectile>();
    }
    public void AnimationTrigger()
    {
        
    }
}
