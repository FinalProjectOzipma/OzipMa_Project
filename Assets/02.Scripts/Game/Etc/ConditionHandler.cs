using UnityEngine;

[System.Serializable]
public class ConditionHandler
{
    public AbilityType Key;
    public GameObject GameObj; // 컨디션 오브젝트
    public float CoolDown;
    public float Duration;

    private float curDuration;
    public float CurDuration
    {
        get { return curDuration; }
        set
        {
            curDuration = Mathf.Max(-1f, value);
        }
    }

    public Transform Attacker { get; set; }
    public bool IsExit { get; set; }


    public void ObjectActive(bool active)
    {
        if (GameObj == null) return;
        GameObj?.SetActive(active);
        IsExit = !active;
    }
}
