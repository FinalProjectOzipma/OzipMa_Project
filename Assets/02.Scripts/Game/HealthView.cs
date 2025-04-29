using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    public Image FillRect;
    public EntityHealth health;

    private EntityController ctrl;

    private void Start()
    {
        ctrl = GetComponentInParent<EntityController>();
    }

    private void Update()
    {
        FillRect.fillOrigin = -ctrl.FacDir;
    }

    /// <summary>
    /// 체력바 업데이트
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="maxAmount"></param>
    public void SetHpBar(float amount, float maxAmount)
    {
        FillRect.fillAmount = amount / maxAmount;
    }

    private void OnDisable()
    {
        FillRect.fillAmount = 1f;
    }
}
