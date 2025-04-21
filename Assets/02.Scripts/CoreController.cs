using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreController : MonoBehaviour
{
    public Core core;
    public GameObject HpBar;
    private Image hpImage;
    private float spawnY = 2.7f;
    private float InitHp;
    // Start is called before the first frame update

    private void Awake()
    {
        InitHp = 100.0f;
        core = new();

        core.Health.SetValue(InitHp);
        core.MaxHealth.SetValue(InitHp);

        hpImage = HpBar.GetComponent<Image>();
        hpImage.fillAmount = core.Health.Value / core.MaxHealth.Value;

        float randomX = Random.Range(-2.0f, 2.0f);

        this.gameObject.transform.position = new Vector2(randomX, spawnY);
    }

    public void TakeDamge(float damage)
    {

        core.Health.Value = Mathf.Min(core.Health.Value - damage, 0);
        hpImage.fillAmount = core.Health.Value /core.MaxHealth.Value;

        if (core.Health.Value == 0)
        {
            Debug.Log("현재 웨이브 다시 시작");
        }
    }

    public void UpgradeHealth(float value)
    {
        core.Health.AddValue(value);
        core.MaxHealth.AddValue(value);
        hpImage.fillAmount = core.Health.Value / core.MaxHealth.Value;
    }

}
