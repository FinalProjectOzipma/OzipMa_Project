using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance {  get; private set; }
    public DragController DragController { get; private set; }

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

    public bool IsTowerBuildArea(Vector3 mousePoint)
    {
        Ray2D ray = new Ray2D(cam.ScreenToWorldPoint(mousePoint), Vector2.zero);
        if (Physics2D.Raycast(ray.origin, ray.direction, 100, TowerBuildLayerMask))
        {
            return true;
        }
        return false;
    }

    public GameObject GetCurrentDragObject(Vector3 mousePoint)
    {
        Ray2D ray = new Ray2D(cam.ScreenToWorldPoint(mousePoint), Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10, CanDragLayerMask);

        return hit == true ? hit.transform.gameObject : null;
    }
}
