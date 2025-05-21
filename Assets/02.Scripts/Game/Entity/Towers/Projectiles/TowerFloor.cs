using System;
using UnityEngine;

public class TowerFloor : MonoBehaviour
{
    public float MoveSpeed = 1f;
    private GameObject body;

    public void Init(string floorKey, float attackPower, Tower ownerTower, Action<EnemyController> applyDamageAction)
    {
        // Body 불러오기
        Managers.Resource.Instantiate($"{floorKey}Body", go =>
        {
            body = go;

            Transform t = go.transform;
            t.SetParent(this.transform);
            t.localPosition = Vector3.zero;

            // Trigger에서 실제 데미지 Apply 처리하기 때문에 정보 넘겨주기
            go.GetComponentInChildren<TowerFloorAnimTrigger>().Init(applyDamageAction, OnAttackFinish);
        });
    }

    private void Update()
    {
        if (Managers.Wave.CurrentState != Enums.WaveState.Playing)
        {
            OnAttackFinish();
        }
    }

    /// <summary>
    /// TowerFloor의 본체와 외형 각각 Destroy
    /// </summary>
    public void OnAttackFinish()
    {
        // Body 삭제
        if (body != null)
        {
            Managers.Resource.Destroy(body);
            body = null;
        }
        // this.gameObject 삭제
        Managers.Resource.Destroy(gameObject);
    }
}
