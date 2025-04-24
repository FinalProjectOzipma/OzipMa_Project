using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance 
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<BuildingSystem>();
                if(instance == null )
                {
                    GameObject newObj = new GameObject(nameof(BuildingSystem));
                    instance = newObj.AddComponent<BuildingSystem>();
                }
            }
            return instance;
        }
    }
    private static BuildingSystem instance;
    Tilemap map;
    private Grid grid;
    private Camera cam;
    private LayerMask towerBuildLayerMask;
    private Vector3 lastSelectedPosition;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        map = Util.FindComponent<Tilemap>(Managers.Scene.CurrentScene.CurrentMap, "TowerBuildArea");
        cam = Camera.main;
        towerBuildLayerMask = LayerMask.GetMask("TowerBuildArea");
        Util.Log($"그리드 : {map.transform.parent.parent.gameObject.name}");
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
