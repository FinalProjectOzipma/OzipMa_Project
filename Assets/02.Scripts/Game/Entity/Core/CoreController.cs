using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class CoreController : Poolable, IDamagable
{
    public Core core;
    public Animator anime;
    private GameObject body;
    private float spawnY = 2.7f;
    private CapsuleCollider2D coreColider;

    [HideInInspector]public string coreLevelkey = "CoreLevelKey";
    [HideInInspector]public string coreHealthkey = "CoreHealthKey";
    
    //public Vector2 CenterPos { get; private set; }
    // Start is called before the first frame update

    private void Awake()
    {
        core = new Core();
        coreColider = GetComponent<CapsuleCollider2D>();
        //CenterPos = GetComponentInChildren<SpriteRenderer>().transform.position;
    }


    public void Init(float maxHealth)
    {
        if(body == null)
        {
            Managers.Resource.Instantiate("Core_Body", go =>
            {
                body = go;
                body.transform.SetParent(transform);
                body.transform.localPosition = Vector3.zero;

                if (body.TryGetComponent<CoreBase>(out CoreBase coreBase))
                {
                    anime = coreBase.Anim;
                    SetHealth(maxHealth, coreBase);
                    Util.Log("널 일 때 체력 세팅");
                }

            });
        }
        else
        {
            if (body.TryGetComponent<CoreBase>(out CoreBase coreBase))
            {
                SetHealth(maxHealth, coreBase);
                Util.Log("널이 아닐 때 체력 세팅");
                Util.Log("최대체력:" + maxHealth);
            }
        }


        float randomX = Random.Range(-2.0f, 2.0f);
        this.gameObject.transform.position = new Vector2(randomX, spawnY);

        core.CoreLevel.SetValue(Managers.Player.MainCoreData.CoreLevel.GetValue());
        Util.Log("코어레벨"+Managers.Player.MainCoreData.CoreLevel.GetValue().ToString());
        CoreUpgrade();
    }


    
    private void SetHealth(float maxHealth, CoreBase coreBase)
    {
        core.Health.MaxValue = maxHealth;
        core.Health.SetValue(maxHealth);
        core.Health.OnChangeHealth += coreBase.healthView.SetHpBar;
    }


    private void OnEnable()
    {
        if(coreColider != null)
        {
            if (!coreColider.enabled) coreColider.enabled = true;
        }
    }



    public void TakeDamge(float damage)
    {

        if (Managers.Game.IsGodMode) return;

        core.Health.AddValue(-damage);

        Managers.Audio.audioControler.PlaySFX(SFXClipName.Hit);

        if (core.Health.Value == 0)
        {
            anime.SetBool("IsDestroy", true);
            coreColider.enabled = false;
            Managers.Audio.audioControler.PlaySFX(SFXClipName.Dead);
        }
    }



    public void SpawnUnit()
    {
        List<IGettable> myUnitsList = Managers.Player.Inventory.GetList<MyUnit>();
        if (myUnitsList == null) return;
        int random = UnityEngine.Random.Range(0, myUnitsList.Count);

        MyUnit myUnit = myUnitsList[random].GetClassAddress<MyUnit>();

        string name = myUnit.Name;

        Managers.Resource.Instantiate($"{name}_Brain", (go) =>
        {
            Managers.Wave.CurMyUnitList.Add(go);
            MyUnitController ctrl = go.GetComponent<MyUnitController>();
            ctrl.Target = GameObject.Find("Test");
            ctrl.TakeRoot(myUnit.PrimaryKey, $"{name}", transform.position);
        });
    }

    public void ApplyDamage(float amount, AbilityType condition = AbilityType.None, GameObject go = null, DefaultTable.AbilityDefaultValue abilities = null)
    {
        TakeDamge(amount);    
    }

    public void CoreUpgrade()
    {
        if (core.CoreLevel.GetValue() < 5)
        {
            anime.SetInteger("Level", 1);
        }
        else if (core.CoreLevel.GetValue() < 10)
        {
            anime.SetInteger("Level", 6);
        }
        else if (core.CoreLevel.GetValue() < 15)
        {
            anime.SetInteger("Level", 11);
        }
        else if (core.CoreLevel.GetValue() < 20)
        {
            anime.SetInteger("Level", 16);
        }
        else if (core.CoreLevel.GetValue() >= 20)
        {
            anime.SetInteger("Level", 21);
        }
    }
}
