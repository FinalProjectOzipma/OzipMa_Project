using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem Instance { get; private set; }
    public DragController DragController { get; private set; }
    public Dictionary<Vector3Int, int> GridObjectMap { get; private set; } = new();

    public int CurrentTowerCount { get => GridObjectMap.Count; }
    public int MaxTowerCount = 2;

    public event Action<string, bool> OnTowerCountChanged;

    [SerializeField] private LayerMask CanDragLayerMask;
    [SerializeField] private LayerMask TowerBuildLayerMask;
    private Tilemap map;
    private Camera cam;
    private MapHandler mapHandler;
    private List<DefaultTable.Stage> stage;


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
        cam = Camera.main;
        map = Util.FindComponent<Tilemap>(Managers.Scene.CurrentScene.CurrentMap, "TowerBuildArea");
        mapHandler = map?.transform.root.GetComponent<MapHandler>();
        DragController = GetComponent<DragController>();
        stage = Util.TableConverter<DefaultTable.Stage>(Managers.Data.Datas[Enums.Sheet.Stage]);

        // 설치 개수 메세지 UI 구독
        OnTowerCountChanged -= Managers.UI.Notify;
        OnTowerCountChanged += Managers.UI.Notify;

        // 타워 최대 개수
        MaxTowerCount = stage[Managers.Player.CurrentKey].TowerAmont;
    }

    public void UpdateTowerCount()
    {
        int newValue = stage[Managers.Player.CurrentKey].TowerAmont;
        if(MaxTowerCount != newValue)
        {
            MaxTowerCount = newValue;
            Managers.UI.Notify($"타워 제한: {MaxTowerCount}개", true);
        }
    }

    #region BuildingSystem 에서 직접해주는 행위들
    /// <summary>
    /// 배치되어 시작하는 타워들 설치
    /// </summary>
    /// <param name="gridObjectMap">배치 위치/ID 데이터</param>
    public void BuildingInit(Dictionary<Vector3Int, int> gridObjectMap = null)
    {
        if (gridObjectMap != null && gridObjectMap.Count > 0)
        {
            // Load된 데이터로 세팅된 타워로 배치해야 함. 
            foreach (Vector3Int point in gridObjectMap.Keys)
            {
                int primaryKey = gridObjectMap[point];
                Tower towerInfo = Managers.Player.Inventory.GetItem<Tower>(primaryKey);

                string towerName = $"{towerInfo.Name}Tower";
                Managers.Resource.Instantiate(towerName, go =>
                {
                    go.transform.position = map.GetCellCenterWorld(point);
                    AddPlacedMapCell(point, primaryKey, false); // 이때 배치 가능 구역 표시가 켜짐
                    HideBuildHighlight(); // 시작할 때니까 배치 가능 구역 끄기
                    go.GetComponent<TowerControlBase>().TakeRoot(primaryKey, towerName, towerInfo);

                    Managers.Wave.CurTowerDict.TryAdd(point, towerInfo); // 애널리틱스 사용하기 위해서 추가한것
                });
            }
        }
    }


    /// <summary>
    /// 배치 가능 구역 초기화, 보여주기
    /// </summary>
    public void RefreshBuildHighlight()
    {
        HideBuildHighlight();
        ShowBuildHighlight();
    }

    /// <summary>
    /// 배치 가능한 구역들을 보여주는 오브젝트 표시하기 
    /// </summary>
    public void ShowBuildHighlight(bool isGreen = true)
    {
        if (mapHandler == null)
        {
            Util.LogWarning("mapHandler가 null인 상태로 진행되고 있습니다. map과 mapHandler를 잘 찾고있는지 확인해보세요.");
            return;
        }

        foreach (Vector3Int point in mapHandler.BuildHighlightList)
        {
            if (GridObjectMap.ContainsKey(point) == false)
            {
                // 빌드 가능한 구역 표시
                mapHandler.ShowBuildHighlight(map.GetCellCenterWorld(point), isGreen);
            }
        }
    }

    /// <summary>
    /// 배치 가능한 구역들을 보여주는 오브젝트 모두 끄기
    /// </summary>
    public void HideBuildHighlight()
    {
        if (mapHandler == null)
        {
            Util.LogWarning("mapHandler가 null인 상태로 진행되고 있습니다. map과 mapHandler를 잘 찾고있는지 확인해보세요.");
            return;
        }

        mapHandler.HideAllHighlights();
    }
    #endregion

    #region BuildingSystem 제공 기능들
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
    /// Cell Point를 World 좌표로 변환
    /// </summary>
    /// <param name="point">셀 좌표</param>
    /// <returns>변환된 월드 좌표</returns>
    public Vector3 CellToWorldPos(Vector3Int point)
    {
        return map.GetCellCenterWorld(point);
    }

    // 오버로드
    public Vector3 UpdatePosition(Vector2 eventPosition, out Vector3Int result)
    {
        result = map.WorldToCell(cam.ScreenToWorldPoint(eventPosition));
        return map.GetCellCenterWorld(result);
    }

    /// <summary>
    /// 타워 배치 가능 위치인지 확인
    /// </summary>
    /// <param name="mousePoint">터치 포인트</param>
    /// <returns>배치 가능 여부</returns>
    public bool CanTowerBuildArea(Vector3 mousePoint)
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

    public bool IsTowerCountFull(bool onMessage = true)
    {
        if (CurrentTowerCount + 1 > MaxTowerCount)
        {
            if (onMessage)
                OnTowerCountChanged?.Invoke($"타워 배치 제한 {GridObjectMap.Count + 1} / {MaxTowerCount}", false);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 특정 포인트에 위치한 드래그 가능한 오브젝트 반환
    /// </summary>
    /// <param name="mousePoint">오브젝트를 가져올 위치(Screen좌표값)</param>
    /// <returns>특정 포인트에 위치한 드래그 가능한 오브젝트</returns>
    public GameObject GetCurrentDragObject(Vector3 mousePoint)
    {
        Ray2D ray = new Ray2D(cam.ScreenToWorldPoint(mousePoint), Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10, CanDragLayerMask);

        return hit == true ? hit.transform.gameObject : null;
    }
    #endregion

    #region 배치된 공간 저장/삭제 메서드들
    public void AddPlacedMapScreenPos(Vector3 mousePos, int id, bool nofity = true)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
        AddPlacedMapCell(map.WorldToCell(worldPos), id, nofity);
    }
    public void AddPlacedMapCell(Vector3Int cellPoint, int id, bool nofity = true) // Add는 이 함수를 꼭 지나감.
    {
        GridObjectMap.Add(cellPoint, id);

        RefreshBuildHighlight();

        if (nofity)
            OnTowerCountChanged?.Invoke($"타워 배치 {GridObjectMap.Count} / {MaxTowerCount}", true);
    }

    public int RemovePlacedMapScreenPos(Vector3 mousePos, bool nofity = true)
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
        return RemovePlacedMapWorldPos(worldPos, nofity);
    }
    public int RemovePlacedMapWorldPos(Vector3 worldPos, bool nofity = true) // Remove는 이함수를 꼭 지나감.
    {
        Vector3Int gridPoint = map.WorldToCell(worldPos);
        int id = GridObjectMap[gridPoint];
        GridObjectMap.Remove(gridPoint);
        Managers.Wave.CurTowerDict.Remove(gridPoint); // 애널리틱스 사용하기 위해서 추가해준것

        if (nofity)
            OnTowerCountChanged?.Invoke($"타워 배치 {GridObjectMap.Count} / {MaxTowerCount}", true);

        return id;
    }
    #endregion

    public MapHandler GetCurMapHandler()
    {
        return mapHandler;
    }
}
