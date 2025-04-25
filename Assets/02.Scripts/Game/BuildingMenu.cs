using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMenu : MonoBehaviour
{
    private BuildingSystem buildingSystem;
    private GameObject towerMenu; // 불러온 타워메뉴창 프리팹 저장
    public float holdTimeThreshold = 1.0f; // 홀드 지연 시간

    private float pressTime = 0f; // 홀드 경과 시간
    private bool hasActivated = false; // 한번만 SetActive(true) 하도록 hasActivated 사용


    private void Start()
    {
        buildingSystem = GetComponent<BuildingSystem>();
    }
    void Update()
    {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject && !hasActivated )
            {
                pressTime += Time.deltaTime;

                if (pressTime >= holdTimeThreshold)
                {
                    // 타워 하위에 TowerMenu 있으면 실행
                    if(towerMenu !=  null)
                    {
                        towerMenu.SetActive(true);
                        hasActivated = true;
                    }
                    // 없으면 새로 생성해서 만듬
                    else
                    {
                        Managers.Resource.Instantiate("TowerMenu", go =>
                        {
                            go.transform.position = this.transform.position;
                            go.transform.SetParent(this.transform);
                            towerMenu = go;                          
                            hasActivated = true;

                            // TowerMenu에 targetTower에 오브젝트 전달
                            UI_TowerMenu menu = towerMenu.GetComponent<UI_TowerMenu>();
                            menu.targetTower = this.gameObject;
                        });
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (towerMenu == null) return;

            pressTime = 0.0f;

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            // 이 오브젝트가 아닌 곳을 클릭했을 때 숨기기
            if (hasActivated)
            {
                if (hit.collider == null || hit.collider.gameObject != gameObject)
                {
                    towerMenu.SetActive(false);
                    hasActivated = false;
                }

            }
        }

    }

}
