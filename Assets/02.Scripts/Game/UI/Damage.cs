using DG.Tweening;
using TMPro;
using UnityEngine;

public class Damage : Poolable
{
    [SerializeField] private TextMeshProUGUI DamageTxt;
    private Tweener tweener;

    /// <summary>
    /// 데미지, 최종 폰트사이즈, 내 위치
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="endSize"></param>
    /// <param name="StartPos"></param>
    public void Init(float damage, int endSize, Vector3 StartPos, EntityController controller)
    {
        if (controller is MyUnitController) DamageTxt.color = new Color(1f, 0.5f, 0f);
        else DamageTxt.color = Color.red;

        DamageTxt.text = damage.ToString("N2");
        DamageTxt.fontSize = 72;

        Vector3 uiPos = Camera.main.WorldToScreenPoint(StartPos);
        transform.SetParent(DamageIndicator.DamageIndicatorRT, false);

        transform.position = uiPos;

        // CanvasGroup이 없다면 추가
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
        cg.alpha = 1f;

        float moveAmount = 200f;     // 올라갈 거리
        float duration = 1f;         // 애니메이션 시간

        Sequence uiseq = DOTween.Sequence();
        uiseq.Append(transform.DOMoveY(transform.position.y + moveAmount, duration).SetEase(Ease.OutQuad));
        uiseq.Join(cg.DOFade(0f, duration));
        uiseq.OnComplete(AnimTrigger); // 애니메이션이 끝나면 리소스 매니저를 통해 삭제
    }

    public void AnimTrigger()
    {


        Managers.Resource.Destroy(gameObject);
    }
}