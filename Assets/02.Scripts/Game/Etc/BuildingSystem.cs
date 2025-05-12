using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance {  get; private set; }
    public DragController DragController { get; private set; }
    public Dictionary<Vector3Int, int> GridObjectMap { get; private set; } = new();

    [SerializeField] private LayerMask CanDragLayerMask;
    [SerializeField] private LayerMask TowerBuildLayerMask;
    private Tilemap map;
    private Camera cam;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Init();
    }

    public void Init()
    {
        map = Util.FindComponent<Tilemap>(Managers.Scene.CurrentScene.CurrentMap, "TowerBuildArea");
        cam = Camera.main;
        DragController = GetComponent<DragController>();
        Util.Log($"그리드 : {map.transform.parent.parent.gameObject.name}");
    }

    /// <summary>
    /// 배치되어 시작하는 타워들 설치
    /// </summary>
    /// <param name="gridObjectMap">배치 위치/ID 데이터</param>
    public void BuildingInit(Dictionary<Vector3Int, int> gridObjectMap = null)
    {
        if (gridObjectMap != null && gridObjectMap.Count > 0)
        {
            // Load된 데이터로 세팅된 타워로 배치해야 함. 
            foreach(Vector3Int point in gridObjectMap.Keys)
            {
                int primaryKey = gridObjectMap[point];
                Tower towerInfo = Managers.Player.Inventory.GetItem<Tower>(primaryKey);

                string towerName = $"{towerInfo.Name}Tower";
                Managers.Resource.Instantiate(towerName, go => 
                {
                    go.transform.position = map.GetCellCenterWorld(point);
                    BuildingSystem.Instance.AddPlacedMapCell(point, primaryKey);
                    go.GetComponent<TowerControlBase>().TakeRoot(primaryKey, towerName, towerInfo);
                });
            }
        }
    }

    /// <summary>
    /// 그리드에서 eventPosition에 해당되는 Cell의 Center Position을 반환
    /// </summary>
    /// <param name="eventPosition">변환할 Position</param>
    /// <returns>Cell의 Center Position</returns>
    public Vector3 UpdatePosition(Vector2 eventPosition)
    {
        Vector3Int cell = map.WorldToCell(cam.ScreenToWorldPoint(eventPosition));
        return map.GetCellCenterWorld(cell);
    }

    /// <summary>
    /// 타워 배치 가능 여부 확인
    /// </summary>
    /// <param name="mousePoint">터치 포인트</param>
    /// <returns>배치 가능 여부</returns>
    public bool CanTowerBuild(Vector3 mousePoint)
    {
        bool canBuild = false;
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePoint);

        // 이미 설치된 공간인지 확인
        Vector3Int point = map.WorldToCell(worldPos);
        if (GridObjectMap.ContainsKey(point))
        { 
            canBuild = false;
            return canBuild;
        }
        
        // TowerBuildArea 확인 
        Ray2D ray = new Ray2D(worldPos, Vector2.zero);
        if (Physics2D.Raycast(ray.origin, ray.direction, 100, TowerBuildLayerMask))
        {
            canBuild = true;
        }
        return canBuild;
    }
    public GameObject GetCurrentDragObject(Vector3 mousePoint)
    {
        Ray2D ray = new Ray2D(cam.ScreenToWorldPoint(mousePoint), Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10, CanDragLayerMask);

        return hit == true ? hit.transform.gameObject : null;
    }

    #region 배치된 공간 저장/삭제 메서드들
    public void AddPlacedMapScreenPos(Vector3 mousePos, int id)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
        GridObjectMap.Add(map.WorldToCell(worldPos), id);
    }
    public void AddPlacedMapCell(Vector3Int cellPoint, int id)
    {
        GridObjectMap.Add(cellPoint, id);
    }

    public int RemovePlacedMapScreenPos(Vector3 mousePos)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
        Vector3Int gridPoint = map.WorldToCell(worldPos);
        int id = GridObjectMap[gridPoint];
        GridObjectMap.Remove(gridPoint);
        return id;
    }
    public int RemovePlacedMapWorldPos(Vector3 worldPos)
    {
        Vector3Int gridPoint = map.WorldToCell(worldPos);
        int id = GridObjectMap[gridPoint];
        GridObjectMap.Remove(gridPoint);
        return id;
    }
    #endregion
}
