using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoreBase : MonoBehaviour
{
    public FloatBase Health = new();
    public FloatBase MaxHealth = new();
    public GameObject HpBar;
    private Image hpImage;
    private float spawnY = 2.7f;
    // Start is called before the first frame update

    private void Awake()
    {
        Health.Init(100.0f);
        MaxHealth.Init(100.0f);

        hpImage = HpBar.GetComponent<Image>();
        hpImage.fillAmount = Health.Value / MaxHealth.Value;

        float randomX = Random.Range(-2.0f,2.0f);

        this.gameObject.transform.position = new Vector2(randomX, spawnY);
    }

    public void TakeDamge(float damage)
    {

        Health.Value = Mathf.Min(Health.Value-damage,0);
        hpImage.fillAmount = Health.Value / MaxHealth.Value;

        if(Health.Value == 0)
        {
            Debug.Log("현재 웨이브 다시 시작");
        }
    }

    public void UpgradeHealth(float value)
    {
        Health.AddValue(value);
        MaxHealth.AddValue(value);
        hpImage.fillAmount = Health.Value / MaxHealth.Value;
    }


 
}
