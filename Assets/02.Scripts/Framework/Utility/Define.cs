using UnityEngine;

public class Define
{
    public static Color GlowGreen = new Color(169f / 255f, 1f, 107f / 255f, 100f / 255f);
    public static Color GlowRed = new Color(1f, 61f / 255f, 61f / 255f, 100f / 255f);

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        Click,
    }

    public enum CameraMode
    {
        QuarterView,
    }
}
