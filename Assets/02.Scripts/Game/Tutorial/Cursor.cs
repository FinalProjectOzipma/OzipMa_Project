using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : UI_Base
{
    private Vector3 startPos;
    private Vector3 endPos;

    private Sequence seq;

    /// <summary>
    /// 클릭만 만들때: 시작위치 끝위치(시작위치랑 같은값!) 
    /// 드래그만 만들때: 시작위치, 끝위치
    /// 클릭하고 드래그까지 하는거 만들때: 시작위치 끝위치 넣고 뒤에 true
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="both"></param>
    public void Init(Vector3 startPos, Vector3 endPos, bool both = false)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        gameObject.transform.position = startPos;

        //무한반복 및 초기화 할당 및 세팅
        seq = DOTween.Sequence()
        .SetAutoKill(false)
        .SetLoops(-1, LoopType.Restart);

        if (both == true)
        {
            //회전 트윈 추가
            seq.Append(transform
                .DORotate(new Vector3(0f, 0f, 45f), 5f)
                .SetEase(Ease.InOutQuart)
            );

            //드래그 트윈 추가
            seq.Append(transform
                .DOMove(endPos, 5f)
                .SetEase(Ease.OutCubic)
            );
        }
        //드래그 애니메이션
        else if (startPos != endPos)
        {
            seq.Append(transform
                .DOMove(endPos, 5f)
                .SetEase(Ease.InOutQuart));
        }
        //클릭 애니메이션
        else if (startPos == endPos)
        {
            seq.Append(transform
                .DORotate(new Vector3(0f, 0f, 45f), 5f)
                .SetEase(Ease.InOutQuart));
        }

    }

    private void OnDisable()
    {
        seq?.Kill();
        Managers.Resource.Destroy(gameObject);
    }
}