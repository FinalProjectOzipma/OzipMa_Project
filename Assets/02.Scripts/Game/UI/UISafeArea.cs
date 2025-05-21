using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UISafeArea : MonoBehaviour
{
    private RectTransform m_rectTransform;

    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    private void Update()
    {
        ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        Vector2 safeAreaBottomLeftPos = safeArea.position;

        Vector2 safeAreaTopRightPos = safeAreaBottomLeftPos + safeArea.size;

        Vector2 anchorMin = Vector2.zero;
        anchorMin.x = safeAreaBottomLeftPos.x / Screen.width;
        anchorMin.y = safeAreaBottomLeftPos.y / Screen.height;

        Vector2 anchorMax = Vector2.one;
        anchorMax.x = safeAreaTopRightPos.x / Screen.width;
        anchorMax.y = safeAreaTopRightPos.y / Screen.height;

        m_rectTransform.anchorMin = anchorMin;
        m_rectTransform.anchorMax = anchorMax;
    }
}
