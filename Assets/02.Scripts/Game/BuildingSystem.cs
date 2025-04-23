using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem
{
    Tilemap map;
    private Grid grid;
    private Camera camera;
    private LayerMask towerBuildLayerMask;
    private Vector3 lastSelectedPosition;

    public BuildingSystem(Tilemap currentGrid)
    {
        Util.Log($"그리드 : {currentGrid.gameObject.name}");
        map = currentGrid;
        camera = Camera.main;
        towerBuildLayerMask = LayerMask.GetMask("TowerBuildArea");
    }

    public Vector3 UpdatePosition(Vector2 eventPosition)
    {
        Vector3Int cell = map.WorldToCell(eventPosition);
        Util.Log($"좌표 : {cell.x}, {cell.y}");
        
        return map.GetCellCenterWorld(cell);
    }

    public bool IsTowerBuildArea(Vector3 mousePos)
    {
        Ray2D ray = new Ray2D(mousePos, Vector2.zero);
        if (Physics2D.Raycast(ray.origin, ray.direction, 100, towerBuildLayerMask))
        {
            return true;
        }
        return false;
    }
}
