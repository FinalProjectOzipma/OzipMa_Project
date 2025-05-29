using UnityEngine;

public class ProjectileAnimationTrigger : MonoBehaviour
{
    private AnimProjectile projectile;

    private void Awake()
    {
        projectile = GetComponentInParent<AnimProjectile>();
    }
    public void AnimationTrigger()
    {
        projectile.AnimationTriggerCalled();
    }
}
