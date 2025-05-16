using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FieldReward : Poolable
{
    private string Gold = nameof(Gold);
    private string Gem = nameof(Gem);

    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Animator animator;

    private SpriteRenderer spr;
    private Sequence seq;
    private WaveManager wave;

    private bool canDestroy;
    private bool isEnd;

    public long Value { get; set; }
    public bool NextReward { get; private set; }

    private int whatIsReward;
    private Vector3 rewardVector;
    /// <summary>
    /// 0 : 골드, 1 : 젬
    /// </summary>
    public int WhatIsReward
    {
        get { return whatIsReward; }
        set
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            whatIsReward = Mathf.Clamp(value, 0, 1);
            animator.SetBool(Gem, whatIsReward == (int)RewardID.Gem);
            animator.SetBool(Gold, whatIsReward == (int)RewardID.Gold);

            if (whatIsReward == 0)
            {
                Transform rect = Managers.UI.GetScene<UI_Gold>().GetGoldPoint().transform;
                rewardVector = Camera.main.ScreenToWorldPoint(rect.position);
            }
            else
            {
                Transform rect = Managers.UI.GetScene<UI_Gem>().GetGemPoint().transform;
                rewardVector = Camera.main.ScreenToWorldPoint(rect.position);
            }
            rewardVector.z = 0f;
        }
    }
    // 첫번째는 골드, 두번재는 젬
    Color[] ptColors = { new Color(255f / 255f, 252f / 255f, 170f / 255f, 100f / 255f), new Color(170f / 255f, 244f / 255f, 255f / 255f, 100f / 255f) };

    enum RewardID
    {
        Gold,
        Gem
    }


    private void Awake()
    {
        seq = Util.RecyclableSequence();
        spr = GetComponentInChildren<SpriteRenderer>();
        particle.Stop();



        Vector3 centerVector = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));
        centerVector.z = 0f;

        // 처음 시퀀스가 끝났을때 다음 보상도 실행 시킬 수 있도록 다음보상으로 바꾸기
        seq.Append(transform.DOMove(centerVector, 1f)).Join(transform.DOScale(new Vector2(5, 5), 1f)).InsertCallback(0.1f, () =>
        {
            NextReward = true;
        });

        seq.Append(DOTween.To(() => 0f, t => { transform.position = Vector3.Lerp(centerVector, rewardVector, t); }, 1f, 0.5f)).Join(transform.DOScale(new Vector2(1, 1), 0.5f)).AppendCallback(() =>
        {
            NextReward = false;
            Managers.Audio.PlaySFX(SFXClipName.Coin);
            canDestroy = true;

            var main = particle.main;
            main.startColor = ptColors[whatIsReward];
            particle.Play(); //파티클 사용할꺼면.. 주석 풀기

            if (whatIsReward == (int)RewardID.Gold)
            {
                Managers.Wave.CurrentGold += Value;
                Managers.Player.AddGold(Value);
            }
            else
            {
                Managers.Wave.CurrentGem += Value;
                Managers.Player.AddGem(Value);
            }


            Value = 0;
            // 웨이브 끝나는 조건
            if (wave.FieldRewards.Count == 0)
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
        if (canDestroy && !particle.isPlaying)
        {
            canDestroy = false;
            particle.Pause();
            Managers.Resource.Destroy(gameObject);
        }
    }

    public void Destroy()
    {
        if (wave.FieldRewards.Count > 0)
            StartCoroutine(FadeOut());
    }

    private void OnEnable()
    {
        spr.color = Color.white;
    }

    private IEnumerator FadeOut()
    {
        float alpha = 1f;

        while (alpha > 0f)
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
