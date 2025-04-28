using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoreController : Poolable, IDamagable
{
    public Core core;
    public Animator anime;
    public GameObject HpBar;
    private Image hpImage;
    private float spawnY = 2.7f;

    public string coreLevelkey = "CoreLevelKey";
    
    public Vector2 CenterPos { get; private set; }
    // Start is called before the first frame update

    private void Awake()
    {
        core = new Core();

        hpImage = HpBar.GetComponent<Image>();
        hpImage.fillAmount = core.Health.Value / core.MaxHealth.Value;

        float randomX = Random.Range(-2.0f, 2.0f);

        this.gameObject.transform.position = new Vector2(randomX, spawnY);

        CenterPos = GetComponentInChildren<SpriteRenderer>().transform.position;
    }

    public void Init(Core data)
    {
        core.Health = data.Health;
        core.MaxHealth = data.MaxHealth;

        if(PlayerPrefs.HasKey(coreLevelkey))
        {
            core.CoreLevel.SetValue(PlayerPrefs.GetInt(coreLevelkey));
            CoreUpgrade();
        }
        
    }

    public void TakeDamge(float damage)
    {

        core.Health.AddValue(-damage);
        hpImage.fillAmount = core.Health.Value /core.MaxHealth.Value;
        Managers.Audio.audioControler.PlaySFX(SFXClipName.Hit);

        if (core.Health.Value == 0)
        {
            anime.SetBool("IsDestroy", true);
            Managers.Audio.audioControler.PlaySFX(SFXClipName.Dead);
            Debug.Log("현재 웨이브 다시 시작");
        }
    }

    public void UpgradeHealth(float value)
    {
        core.Health.AddValue(value);
        core.MaxHealth.AddValue(value);
        hpImage.fillAmount = core.Health.Value / core.MaxHealth.Value;
    }

    public void SpawnUnit()
    {
        List<IGettable> myUnitsList = Managers.Player.Inventory.GetList<MyUnit>();

        int random = UnityEngine.Random.Range(0, myUnitsList.Count);

        MyUnit myUnit = myUnitsList[random].GetClassAddress<MyUnit>();

        string name = myUnit.Name;

        Managers.Resource.Instantiate($"{name}_Brain", (go) =>
        {
            Managers.Wave.CurMyUnitList.Add(go);
            MyUnitController ctrl = go.GetComponent<MyUnitController>();
            ctrl.Target = GameObject.Find("Test");
            ctrl.TakeRoot(random, $"{name}", transform.position);
        });
    }

    public void ApplyDamage(float amount, AbilityType condition = AbilityType.None, GameObject go = null)
    {
        TakeDamge(amount);
       
    }

    public void CoreUpgrade()
    {
        if (core.CoreLevel.GetValue() < 5)
        {
            Managers.Wave.MainCore.anime.SetInteger("Level", 1);
        }
        else if (core.CoreLevel.GetValue() <= 10)
        {
            Managers.Wave.MainCore.anime.SetInteger("Level", 5);
        }
        else if (core.CoreLevel.GetValue() <= 15)
        {
            Managers.Wave.MainCore.anime.SetInteger("Level", 10);
        }
        else if (core.CoreLevel.GetValue() <= 20)
        {
            Managers.Wave.MainCore.anime.SetInteger("Level", 15);
        }
        else if (core.CoreLevel.GetValue() <= 25)
        {
            Managers.Wave.MainCore.anime.SetInteger("Level", 20);
        }
    }
}
