using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : UI_Base
{
    // 이 Transform을 통해 자식으로 붙일 수 있음
    public static RectTransform DamageIndicatorRT {  get; private set; }

    private void Awake()
    {
        DamageIndicatorRT = GetComponent<RectTransform>();
    }
}
