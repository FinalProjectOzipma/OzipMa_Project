using Firebase.Database;
using System;
using System.Collections;
using System.Resources;
using System.Threading.Tasks;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Audio;

public class Managers : MonoBehaviour
{
    public static Managers Instance { get; private set; }
    public static MonoBehaviour MonoInstance { get; private set; }

    public static readonly AudioManager Audio = new();
    //public static readonly CameraManager Camera = new();
    public static readonly GameManager Game = new();
    //public static readonly InputManager Input = new();
    public static readonly PoolManager Pool = new();
    public static readonly ResourceManager Resource = new();
    public static readonly SceneManager Scene = new();
    public static readonly UIManager UI = new();
    public static readonly DataManager Data = new();
    public static readonly WaveManager Wave = new();
    public static readonly PlayerManager Player = new();
    public static readonly UpgradeManager Upgrade = new();
    public static readonly EffectManager Effect = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        MonoInstance = this;
        DontDestroyOnLoad(this);

        // 로컬 캐시 비활설화
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);

        Data.Initialize();
        Pool.Initialize();
        TestInit();
        Scene.Initialize();
        Effect.Initialize();
        Upgrade.Intialize();

    }
    

    private void Update()
    {
        Scene.CurrentScene?.Update();
    }

    public new static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return MonoInstance.StartCoroutine(coroutine);
    }

    public new static void StopCoroutine(Coroutine coroutine)
    {
        MonoInstance.StopCoroutine(coroutine);
    }


    public void TestInit()
    {
        Scene.GameScene.SingletonAction += Audio.Initialize;
        Scene.GameScene.SingletonAction += Player.Initialize;
        Scene.GameScene.SingletonAction += Wave.Initialize;
        Scene.GameScene.SingletonAction += Game.Initialize;

        Scene.PhnMyUnitScene.SingletonAction += Audio.Initialize;
        Scene.PhnMyUnitScene.SingletonAction += Player.Initialize;
        Scene.PhnMyUnitScene.SingletonAction += Wave.Initialize;
        Scene.PhnMyUnitScene.SingletonAction += Game.Initialize;
    }


    void OnApplicationQuit()
    {
        Data.SaveGameData();
    }

}