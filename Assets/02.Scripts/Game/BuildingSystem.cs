using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance {  get; private set; }
    public DragController DragController { get; private set; }

    [SerializeField] private LayerMask CanDragLayerMask;
    [SerializeField] private LayerMask TowerBuildLayerMask;
    private HashSet<Vector3Int> placedMap = new();
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
        //Util.Log($"CanTowerBuild : {point.x}, {point.y}");
        if (placedMap.Contains(point))
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
    public void AddPlacedMap(Vector3 mousePos)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
        placedMap.Add(map.WorldToCell(worldPos));
    }

    public void RemovePlacedMapScreenPos(Vector3 mousePos)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
        placedMap.Remove(map.WorldToCell(worldPos));
    }
    public void RemovePlacedMapWorldPos(Vector3 worldPos)
    {
        placedMap.Remove(map.WorldToCell(worldPos));
    }
    #endregion
}
