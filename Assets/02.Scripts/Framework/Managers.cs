using System.Collections;
using System.Resources;
using UnityEditor.EditorTools;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance { get; private set; }
    public static MonoBehaviour MonoInstance { get; private set; }

    //public static readonly AudioManager Audio = new();
    //public static readonly CameraManager Camera = new();
    public static readonly GameManager Game = new();
    //public static readonly InputManager Input = new();
    public static readonly PoolManager Pool = new();
    public static readonly ResourceManager Resource = new();
    public static readonly SceneManager Scene = new();
    public static readonly UIManager UI = new();
    public static readonly DataManager Data = new();
    public static readonly WaveManager Wave = new();

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

        Game.Initialize();
        Pool.Initialize();
        Data.Initialize();
        Scene.Initialize();
        Wave.Initialize();
    }

    public new static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return MonoInstance.StartCoroutine(coroutine);
    }

    public new static void StopCoroutine(Coroutine coroutine)
    {
        MonoInstance.StopCoroutine(coroutine);
    }
}