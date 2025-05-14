using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TrailObject : Poolable
{
    private SpriteRenderer spr;

    private float alphaVal;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 활성화
    /// </summary>
    /// <param name="sp">이미지</param>
    /// <param name="alphaVal">투명 값 0 ~ 255f</param>
    public void Active(Sprite sp, float alphaVal, int facingDir = 1, int sortingLayer = 0)
    {
        if(sp != null)
        {
            transform.localScale = new Vector2(transform.localScale.x * facingDir, transform.localScale.y);
            spr.sprite = sp;
            spr.color = Color.white;
            spr.sortingOrder = sortingLayer - 1;
            this.alphaVal = alphaVal = Mathf.Clamp(0, 1f, alphaVal / 255f);
        }
    }

    private void Update()
    {
        DestroyTrail(Managers.Wave.CurrentState == Enums.WaveState.Start);

        alphaVal -= Time.deltaTime;
        spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, Mathf.Max(0f, alphaVal));

        if (alphaVal >= 0f) return;

        DestroyTrail(true);
    }

    private void DestroyTrail(bool isActive)
    {
        if(isActive)
        {
            spr.sprite = null;
            if (gameObject.activeInHierarchy) Managers.Resource.Destroy(gameObject);
        }
    }
}
