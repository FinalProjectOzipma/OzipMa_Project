using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    private BuildingSystem buildingSystem;
    private Vector3 eventPosition;
    private GameObject dragObject;
    private SpriteRenderer spriteRenderer;
    private bool isDragging = false;


    private void Start()
    {
        buildingSystem = GetComponent<BuildingSystem>();
    }

    private void Update()
    {
        // 드래그 Begin
        if(Input.GetMouseButton(0) && !isDragging)
        {
            BeginDrag();
        }

        // 드래그 Update
        if(isDragging)
        {
            Drag();
        }

        // 드래그 End
        if(isDragging && Input.GetMouseButtonUp(0))
        {
            EndDrag();
        }
    }

    public void BeginDrag()
    {
        eventPosition = Input.mousePosition;
        GameObject go = buildingSystem.GetCurrentDragObject(eventPosition);
        if (go == null) return;
        dragObject = go.transform.root.gameObject;
        spriteRenderer = Util.FindComponent<SpriteRenderer>(go, "MainSprite");
        if (dragObject != null)
        {
            dragObject.transform.position = buildingSystem.UpdatePosition(eventPosition);
            isDragging = true;
            Util.Log($"DragController : {dragObject.transform.position.x}, {dragObject.transform.position.y} 드래그");
        }
    }

    public void Drag()
    {
        dragObject.transform.position = buildingSystem.UpdatePosition(Input.mousePosition);
        if (buildingSystem.IsTowerBuildArea(Input.mousePosition))
        {
            spriteRenderer.color = Color.green;
            return;
        }
        spriteRenderer.color = Color.red;
    }

    public void EndDrag()
    {
        isDragging = false;
        spriteRenderer.color = Color.white;

        if (buildingSystem.IsTowerBuildArea(Input.mousePosition))
        {
            dragObject.transform.position = buildingSystem.UpdatePosition(Input.mousePosition);
            dragObject = null;
            return;
        }

        dragObject.transform.position = buildingSystem.UpdatePosition(eventPosition);
        dragObject = null;
    }
}
