using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoreBase : MonoBehaviour
{
    public FloatBase Health;
    public FloatBase MaxHelaht;
    public GameObject HpBar;
    private Image hpImage;
    // Start is called before the first frame update

    private void Awake()
    {
        hpImage = HpBar.GetComponent<Image>();
        hpImage.fillAmount = Health.Value / MaxHelaht.Value;
    }

    public void TakeDamge(float damage)
    {
        Health.Value = Mathf.Min(Health.Value-damage,0);
        hpImage.fillAmount = Health.Value / MaxHelaht.Value;

        if(Health.Value == 0)
        {
            Debug.Log("현재 웨이브 다시 시작");
        }
    }

    public void UpgradeHealth(float value)
    {
        Health.AddValue(value);
        MaxHelaht.AddValue(value);
        hpImage.fillAmount = Health.Value / MaxHelaht.Value;
    }
 
}
