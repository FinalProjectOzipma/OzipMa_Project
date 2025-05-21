using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBase
{
    protected TutorialController controller;

    public TutorialBase(TutorialController _controller)
    {
        controller = _controller;
    }

    public abstract bool CheckCondition();
    public abstract void OnStart();
    public virtual void OnEnd()
    {
        controller.Cursor.OffCursor();
        controller.SetCursorActive(false);
        controller.SetDialogueActive(false);
    }
}
