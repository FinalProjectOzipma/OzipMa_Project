using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragController : MonoBehaviour
{
    private BuildingSystem buildingSystem;
    private Vector3 eventPosition;
    private GameObject dragObject;
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
            eventPosition = Input.mousePosition;
            dragObject = buildingSystem.GetCurrentDragObject(eventPosition).transform.root.gameObject; 
            if (dragObject != null)
            {
                dragObject.transform.position = buildingSystem.UpdatePosition(eventPosition);
                isDragging = true;
                Util.Log($"DragController : {dragObject.transform.position.x}, {dragObject.transform.position.y} 드래그");
            }
        }

        // 드래그 Update
        if(isDragging)
        {
            dragObject.transform.position = buildingSystem.UpdatePosition(Input.mousePosition);
        }

        // 드래그 End
        if(Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            if(buildingSystem.IsTowerBuildArea(Input.mousePosition))
            {
                dragObject.transform.position = buildingSystem.UpdatePosition(Input.mousePosition);
                return;
            }

            dragObject.transform.position = buildingSystem.UpdatePosition(eventPosition);
            dragObject = null;
        }
    }
}
