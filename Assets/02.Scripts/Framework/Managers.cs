using System.Collections;
using System.Resources;
using UnityEditor.EditorTools;
using UnityEngine;

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

        Data.Initialize();
        Pool.Initialize();
        TestInit();
        Scene.Initialize();
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

        Scene.YgmLoadingScene.SingletonAction += Audio.Initialize;
        Scene.YgmLoadingScene.SingletonAction += Player.Initialize;
        Scene.YgmLoadingScene.SingletonAction += Wave.Initialize;
        Scene.YgmLoadingScene.SingletonAction += Game.Initialize;

        Scene.PhnMyUnitScene.SingletonAction += Audio.Initialize;
        Scene.PhnMyUnitScene.SingletonAction += Player.Initialize;
        Scene.PhnMyUnitScene.SingletonAction += Wave.Initialize;
        Scene.PhnMyUnitScene.SingletonAction += Game.Initialize;

        Scene.PydTowerScene.SingletonAction += Audio.Initialize;
        Scene.PydTowerScene.SingletonAction += Player.Initialize;
        Scene.PydTowerScene.SingletonAction += Wave.Initialize;
        Scene.PydTowerScene.SingletonAction += Game.Initialize;

        Scene.BsyEnemyScene.SingletonAction += Audio.Initialize;
        Scene.BsyEnemyScene.SingletonAction += Player.Initialize;
        Scene.BsyEnemyScene.SingletonAction += Wave.Initialize;
        Scene.BsyEnemyScene.SingletonAction += Game.Initialize;
    }
}