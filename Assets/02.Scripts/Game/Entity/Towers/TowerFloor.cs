using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFloor : MonoBehaviour
{
    private GameObject body;
    public void Init(string floorKey, Vector3 position, float attackPower, Tower ownerTower)
    {
        transform.position = position;
        // Body 불러오기
        Managers.Resource.Instantiate($"{floorKey}Body", go =>
        {
            body = go;

            Transform t = go.transform;
            t.SetParent(this.transform);
            t.localPosition = Vector3.zero;

            // Trigger에서 실제 공격 Apply 처리
            go.GetComponentInChildren<TowerTrigger>().Init(attackPower, ownerTower, OnAttackFinish);
        });
    }

    public void OnAttackFinish()
    {
        // Body 삭제
        Managers.Resource.Destroy(body);
        // this.gameObject 삭제
        Managers.Resource.Destroy(gameObject);
    }
}
