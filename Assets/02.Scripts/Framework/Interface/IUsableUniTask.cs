using System.Threading;

public interface IUsableUniTask
{
    public CancellationTokenSource DisableCancellation { get; set; } // 비활성화시 취소처리
    public CancellationTokenSource DestroyCancellation { get; set; } // 삭제시 취소처리

    public void TokenEnable();
    public void TokenDisable();
    public void TokenDestroy();
}
