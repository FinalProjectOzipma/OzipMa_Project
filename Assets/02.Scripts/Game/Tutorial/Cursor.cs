using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public enum CursorType 
{
    Drag,
    Click,
    ClickDrag,
    ClickDragClick,
}

public class Cursor : UI_Base
{
    private Sequence seq;
    [SerializeField] private RectTransform CursorObj;

    private void OnEnable()
    {
        seq.Restart();
    }

    /// <summary>
    /// 클릭만 만들때: 시작위치 끝위치(시작위치랑 같은값!) 
    /// 드래그만 만들때: 시작위치, 끝위치
    /// 클릭하고 드래그까지 하는거 만들때: 시작위치 끝위치 넣고 뒤에 true
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="both"></param>
    public void Init(Vector3 startPos, Vector3 endPos, CursorType type)
    {
        gameObject.transform.localPosition = startPos;
        //방향 초기화
        //CursorObj.rotation = Quaternion.identity;
        CursorObj.localRotation = Quaternion.Euler(Vector3.zero);
        
        //기존에 존재하는 시퀀스 삭제
        seq?.Kill();

        //무한반복 및 초기화 할당 및 세팅
        seq = DOTween.Sequence()
        .SetAutoKill(false)
        .SetLoops(-1, LoopType.Restart);

        //두투윈 타입 지정
        switch (type) 
        {
            //드래그 타입
            case CursorType.Drag:
                seq = seq.Append(transform
                    .DOLocalMove(endPos, 3f)
                    .SetEase(Ease.InOutQuart));
                break;
            
            //클릭타입
            case CursorType.Click:
                seq.Append(transform
                    .DORotate(new Vector3(0f, 0f, 45f), 3f)
                    .SetEase(Ease.InOutQuart));
                break;

            //클릭하고 드래그타입
            case CursorType.ClickDrag:
                //회전 트윈 추가
                seq.Append(transform
                    .DORotate(new Vector3(0f, 0f, 30f), 1.5f)
                    .SetEase(Ease.InOutQuart)
                );
                //드래그 트윈 추가
                seq.Append(transform
                    .DOLocalMove(endPos, 3f)
                    .SetEase(Ease.OutCubic)
                );
                break;

            //클릭하고 드래그한 다음 클릭하는 거 
            case CursorType.ClickDragClick:
                //회전 트윈 추가
                seq.Append(transform
                    .DORotate(new Vector3(0f, 0f, 45f), 3f)
                    .SetEase(Ease.InOutQuart)
                );

                //드래그 트윈 추가
                seq.Append(transform
                    .DOLocalMove(endPos, 2f)
                    .SetEase(Ease.OutCubic)
                );

                //회전 트윈 추가
                seq.Append(transform
                    .DORotate(new Vector3(0f, 0f, 70f), 2f)
                    .SetEase(Ease.InOutQuart)
                );
                break;
        }
    }

    public void OffCursor()
    {
        seq?.Kill();
        //Managers.Resource.Destroy(gameObject);
    }
}