using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FieldGold : Poolable
{
    [SerializeField] private ParticleSystem particle;
    private SpriteRenderer spr;
    private Sequence seq;
    private WaveManager wave;

    private bool canDestroy;
    private bool isEnd;

    public bool NextGold { get; private set; }

    private void Awake()
    {
        seq = Util.RecyclableSequence();
        spr = GetComponentInChildren<SpriteRenderer>();
        particle.Stop();

        Transform rect = Managers.UI.GetScene<UI_Gold>().GetGoldPoint().transform;
        Vector3 goldVector = Camera.main.ScreenToWorldPoint(rect.position);
        goldVector.z = 0f;

        Vector3 centerVector = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));
        centerVector.z = 0f;

        // 처음 시퀀스가 끝났을때 다음 골드도 실행 시킬 수 있도록 다음골드로 바꾸기
        seq.Append(transform.DOMove(centerVector, 1f)).Join(transform.DOScale(new Vector2(5, 5), 1f)).InsertCallback(0.1f, () =>
        {
            NextGold = true;
        });

        seq.Append(transform.DOMove(goldVector, 0.5f)).Join(transform.DOScale(new Vector2(1, 1), 0.5f)).AppendCallback(() =>
        {
            NextGold = false;
            Managers.Audio.PlaySFX(SFXClipName.Coin);
            canDestroy = true;
            particle.Play(); //파티클 사용할꺼면.. 주석 풀기
            
            // 웨이브 끝나는 조건
            if (wave.FieldGolds.Count == 0)
            {
                wave.Timer = 1f;
                wave.CurrentState = Enums.WaveState.End;
            }
        });
    }

    private void Start()
    {
        wave = Managers.Wave;
    }

    private void Update()
    {
        if(canDestroy && !particle.isPlaying)
        {
            canDestroy = false;
            particle.Pause();
            Managers.Resource.Destroy(gameObject);
        }
    }

    public void Destroy()
    {
        if(wave.FieldGolds.Count > 0)
            StartCoroutine(FadeOut());
    }

    private void OnEnable()
    {
        spr.color = Color.white;
    }

    private IEnumerator FadeOut()
    {
        float alpha = 1f;

        while(alpha > 0f)
        {
            alpha -= Time.deltaTime;
            spr.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        isEnd = false;
        Managers.Resource.Destroy(gameObject);
    }

    public void Play()
    {
        // 플레이 중이 아니면 실행
        if (!seq.IsPlaying())
            seq.Restart();
    }
}
