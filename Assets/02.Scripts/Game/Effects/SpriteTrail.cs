using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Threading;
using UnityEngine;

public class SpriteTrail : MonoBehaviour, IUsableUniTask
{
    #region UniTask
    public CancellationTokenSource DisableCancellation { get; set; }
    public CancellationTokenSource DestroyCancellation { get; set; }

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
    #endregion

    // Component
    [SerializeField] private SpriteRenderer spr;

    [Range(0,255)]
    [SerializeField] private float Alpha;

    private float time;
    // Key
    private string TrailObject = nameof(TrailObject);
    
    private bool isActive;

    private Transform trans;

    private int facingDir = 1;
    public int FacingDir 
    {
        get
        {
            return facingDir;
        } 
        set
        {
            facingDir = value;
            Mathf.Clamp(facingDir, -1, 1);
        }

    }

    private float angle = 0;
    public float Angle
    {
        get
        {
            return angle;
        }
        set
        {
            angle = value;
            Mathf.Clamp(facingDir, -180, 180);
        }
    }

    private Vector2 scale;
    public Vector2 Scale { get; set; } = Vector2.one;

    [Header("Trail 시간 간격")]
    [Range(0.3f,1f)]
    [SerializeField] private float trailRate;


    /// <summary>
    /// Update문에서는 사용하지 말아주세요 반드시 한번 호출
    /// </summary>
    /// <param name="active">Update문이 아닌 한번 호출하는 부분 Enter = true or Init = true, Exit = false</param>
    /// <param name="trans">위치, 회전, 크기 정보 넘겨주고 싶으면</param>
    /// <param name="facingDir">좌우로만 컨트롤 하고싶으면</param>
    /// <param name="angle">회전으로만 컨트롤 하고싶으면</param>
    public void SetActive(bool active, Transform trans = null, int facingDir = 1, float angle = 0)
    {
        isActive = active;
        if (isActive)
        {
            FacingDir = facingDir;
            Angle = angle;
            this.trans = trans;
            TokenEnable(); // 토큰 활성화
            StartTrail().Forget();
        }
    }

    private async UniTaskVoid StartTrail()
    {
        while(isActive)
        {
            if (time <= 0f)
            {
                time = trailRate;
                Managers.Resource.Instantiate(TrailObject, (go) => SetTrail(go));
            }
            else
            {
                time -= Time.deltaTime;
            }

            if (DisableCancellation == null || DisableCancellation.IsCancellationRequested || !gameObject.activeInHierarchy)
            {
                TokenDisable();
                break;
            }

            await UniTask.NextFrame(DisableCancellation.Token);
        }

        TokenDisable();
    }

    private void SetTrail(GameObject go)
    {
        if (go == null) return;

        go.transform.position = spr.transform.position; // 위치

        // angle값이 있을때
        if (trans == null)
        {
            go.transform.localRotation = Quaternion.Euler(angle, transform.localRotation.y, transform.localRotation.z); // 회전
            go.transform.localScale = Scale;
        }
        else
        {
            go.transform.rotation = trans.localRotation;
            go.transform.localScale = trans.localScale;
        }

        go.GetComponent<TrailObject>().Active(spr.sprite, Alpha, facingDir, spr.sortingOrder);
    }
}
