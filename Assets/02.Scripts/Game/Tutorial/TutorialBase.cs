using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBase
{
    public Enums.TutorialStep Step;
    protected TutorialController controller;

    public TutorialBase(TutorialController _controller, Enums.TutorialStep step)
    {
        controller = _controller;
        Step = step;
    }

    public abstract bool CheckCondition();
    public abstract void OnStart();
    public virtual void OnEnd()
    {
        controller.Cursor.OffCursor();
        controller.SetCursorActive(false);
        controller.SetDialogueActive(false);

        Managers.Player.LastTutorialStep = Step; // 진행도 저장
    }
}
