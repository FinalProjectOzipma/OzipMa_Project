using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Damage: Poolable
{
    [SerializeField] private TextMeshProUGUI DamageTxt;
    private Tweener tweener;

    /// <summary>
    /// 데미지, 최종 폰트사이즈, 내 위치
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="endSize"></param>
    /// <param name="StartPos"></param>
    public void Init(float damage, int endSize, Vector3 StartPos)
    {
        DamageTxt.text = damage.ToString("N2");
        DamageTxt.fontSize = 72;

        Vector3 uiPos = Camera.main.WorldToScreenPoint(StartPos);
        transform.SetParent(DamageIndicator.DamageIndicatorRT, false);

        transform.position = uiPos;
    }

    public void AnimTrigger()
    {
        Managers.Resource.Destroy(gameObject);
    }
}