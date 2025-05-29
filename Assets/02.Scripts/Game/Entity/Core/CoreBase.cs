using UnityEngine;

public class CoreBase : MonoBehaviour
{
    //public float MainSpriteOffset = 0.0f;
    public Animator Anim { get; private set; }

    public HealthView healthView;

    private GameObject mainSprite;

    protected virtual void Awake()
    {
        mainSprite = gameObject.transform.GetChild(0).gameObject;

        Anim = mainSprite.GetComponent<Animator>();
    }
}
