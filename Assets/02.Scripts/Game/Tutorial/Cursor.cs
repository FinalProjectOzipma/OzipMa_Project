using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public enum Type 
    {
        Click,
        Drag,
    }

    private Vector3 startPos;
    private Vector3 endPos;

    private Tweener tweener;

    public void Init(Vector3 startPos, Vector3 endPos)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        gameObject.transform.position = startPos;

        //무한 드래그 애니메이션
        if (startPos != endPos)
        {
            tweener = transform
                .DOMove(endPos, 5f)
                .SetEase(Ease.InOutQuart)
                .SetLoops(-1, LoopType.Restart)
                .SetAutoKill(false);
        }
        //무한 클릭 애니메이션
        else if (startPos == endPos)
        {
            tweener = transform
                .DORotate(new Vector3(0f, 0f, 45f), 5f)
                .SetEase(Ease.InOutQuart)
                .SetLoops(-1, LoopType.Restart)
                .SetAutoKill(false);
        }
    }

    private void OnDisable()
    {
        tweener.Kill();
        Managers.Resource.Destroy(gameObject);
    }
}