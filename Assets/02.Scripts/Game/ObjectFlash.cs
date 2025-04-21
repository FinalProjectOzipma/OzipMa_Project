using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFlash : MonoBehaviour
{
    public SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Color dotColor;
    [SerializeField] private float flashDuration;
    [SerializeField] private Material flashMat;
    private Material originMat;

    WaitForSeconds waitFor;

    private void Start()
    {
        originMat = sr.material;
        waitFor = new WaitForSeconds(flashDuration);
    }

    public void StartBlinkRed()
    {
        StartCoroutine(DotFx());
    }

    public void StartBlinkFlash()
    {
        StartCoroutine(FlashFX());
    }

    public IEnumerator FlashFX()
    {
        sr.material = flashMat;

        yield return waitFor;

        sr.material = originMat;
    }

    public IEnumerator DotFx()
    {
        sr.color = dotColor;

        yield return waitFor;

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
}
