using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 endPos;

    private Tweener movingTweener;
    public void Init(Vector3 startPos, Vector3 endPos)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        gameObject.transform.position = startPos;

        //무한 드래그
        movingTweener = transform
            .DOMove(endPos, 5f)
            .SetEase(Ease.InOutQuart)
            .SetLoops(-1, LoopType.Restart)
            .SetAutoKill(false);
    }

    private void OnDisable()
    {
        movingTweener.Kill();
        Managers.Resource.Destroy(gameObject);
    }
}