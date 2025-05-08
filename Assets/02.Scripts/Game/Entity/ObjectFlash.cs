using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ObjectFlash : MonoBehaviour, IUsableUniTask
{
    public CancellationTokenSource DisableCancellation { get; set; }
    public CancellationTokenSource DestroyCancellation { get; set; }

    public SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Color dotColor;
    [Range(0.001f, 10f)]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material flashMat;
    private Material originMat;


    private void Start()
    {
        originMat = sr.material;
        flashDuration *= 1000f;

        TokenEnable();
    }

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        if (originMat != null)
        {
            sr.material = originMat;
            sr.color = Color.white;
        }
    }

    public void StartBlinkRed()
    {
        if (gameObject.activeInHierarchy)
            DotFx().Forget();
        else
            TokenDisable();
    }

    public void StartBlinkFlash()
    {
        if (gameObject.activeInHierarchy)
            FlashFX().Forget();
        else
            TokenDisable();
    }

    private async UniTaskVoid FlashFX()
    {
        sr.material = flashMat;

        await UniTask.Delay((int)flashDuration, false, PlayerLoopTiming.Update, DisableCancellation.Token);

        sr.material = originMat;
    }

    private async UniTaskVoid DotFx()
    {
        sr.color = dotColor;

        await UniTask.Delay((int)flashDuration, false, PlayerLoopTiming.Update, DisableCancellation.Token);

        sr.color = Color.white;
    }


    public void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;
    }

    public void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void TokenEnable()
    {
        if (DisableCancellation != null)
        {
            DisableCancellation.Dispose();
        }

        DisableCancellation = new();
    }

    public void TokenDisable()
    {
        DestroyCancellation?.Cancel();
    }

    public void TokenDestroy()
    {
        DisableCancellation?.Cancel();
        DestroyCancellation?.Dispose();
    }
}
