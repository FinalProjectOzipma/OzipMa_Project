using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBodyBase : MonoBehaviour
{
    public float MainSpriteOffset = 0.3f;
    public Animator Anim {  get; private set; }
    public TowerAnimationData AnimData { get; private set;}

    private GameObject mainSprite;

    public Vector3 FirePosition
    {
        get
        {
            Vector3 finalFirePos = gameObject.transform.GetChild(transform.childCount - 1).position;
            finalFirePos.y -= MainSpriteOffset;
            return finalFirePos;
        }
    }

    protected virtual void Awake()
    {
        mainSprite = gameObject.transform.GetChild(0).gameObject;

        Vector3 pos = mainSprite.transform.position;
        pos.y = -MainSpriteOffset;
        mainSprite.transform.position = pos;

        Anim = mainSprite.GetComponent<Animator>();
        AnimData = new();
    }
}
