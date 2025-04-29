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

    public void SetHpBar(float amount, float maxAmount)
    {
        FillRect.fillOrigin = -ctrl.FacDir;
        FillRect.fillAmount = amount / maxAmount;
    }
}
