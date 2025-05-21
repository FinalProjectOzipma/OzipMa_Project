public class AnimEffectTrigger : Poolable
{
    public void End()
    {
        Managers.Resource.Destroy(gameObject);
    }
}
