using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class UniTaskHandler
{
    protected CancellationTokenSource disableCancellation { get; set; } // 비활성화시 취소처리
    protected CancellationTokenSource destroyCancellation { get; set; } // 삭제시 취소처리

    public void TokenEnable()
    {
        if (disableCancellation != null)
        {
            disableCancellation.Dispose();
        }

        disableCancellation = new();
    }

    public void TokenDisable()
    {
        destroyCancellation?.Cancel();
    }

    public void TokenDestroy()
    {
        disableCancellation?.Cancel();
        destroyCancellation?.Dispose();
    }
}
