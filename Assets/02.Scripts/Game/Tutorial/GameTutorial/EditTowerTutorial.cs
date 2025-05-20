using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 타워 편집 튜토리얼
/// </summary>
public class EditTowerTutorial : TutorialBase
{
    public EditTowerTutorial(TutorialController _controller) : base(_controller)
    {
    }

    public override bool CheckCondition()
    {
        return true;
    }

    public override void OnStart()
    {
        // 임시
        Vector3 startPos = BuildingSystem.Instance.GridObjectMap.Keys.First();

        foreach (Vector3Int emptyPoint in BuildingSystem.Instance.GetCurMapHandler().BuildHighlightList)
        {
            if (startPos != emptyPoint && BuildingSystem.Instance.GridObjectMap.ContainsKey(emptyPoint))
            {
                // 다른 빈공간을 endPos로 사용
            }
        }

        //controller.Dialogue.EnQueueText("타워는 최대 2개까지 설치가 됩니다. 이번에는 위치를 옮기는법을 알아볼까요?");
        //controller.Dialogue.EnQueueText("");
    }
}
