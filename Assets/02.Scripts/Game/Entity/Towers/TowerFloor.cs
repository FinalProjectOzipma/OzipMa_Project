using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFloor : MonoBehaviour
{
    public void Init(string floorKey, Vector3 position, float attackPower, Tower ownerTower)
    {
        // Body 불러오기
        Managers.Resource.Instantiate($"{floorKey}Body", go =>
        {
            Transform t = go.transform;
            t.SetParent(this.transform);
            t.position = position;
            t.localPosition = Vector3.zero;

            // Trigger에서 실제 공격 Apply 처리
            go.GetComponentInChildren<TowerTrigger>().Init(attackPower, ownerTower);
        });
    }
}
