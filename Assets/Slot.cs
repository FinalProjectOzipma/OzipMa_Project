using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : UI_Scene, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private enum Images
    {
        Icon,
        StackGageFill,
        Selected
    }

    private enum TextMeshs
    {
        ObjInfo,
        StackText
    }

    private Button button; // 이녀석은 현재 들고 있는 컴포넌트객체니깐 그냥 Get으로 불러드림

    public int Index { get; set; }
    public bool IsActive { get; private set; } = false;

    public IGettable Gettable;

    private GameObject PreviewObj;
    SpriteRenderer previewRenderer;
    private Sprite _sprite;
    private Image _stackGage;

    private void Awake()
    {
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(TextMeshs));
        button = GetComponent<Button>();
        button.onClick.AddListener(SeletToggle);

        GetImage((int)Images.Selected).gameObject.SetActive(false);
    }

    private void SeletToggle()
    {
        GameObject imgObj = GetImage((int)Images.Selected).gameObject;
        imgObj.SetActive(!imgObj.activeSelf);
        IsActive = imgObj.activeInHierarchy;
    }

    // 전체 선택될때 호출해야되는 메서드
    public void OnSelect()
    {
        IsActive = true;
        GetImage((int)Images.Selected).gameObject.SetActive(true);
    }

    public void DisSelect()
    {
        IsActive = false;
        GetImage((int)Images.Selected).gameObject.SetActive(false);
    }


    public void SetData<T>(IGettable gettable) where T : UserObject
    {
        
        Gettable = gettable;
        T obj = gettable.GetClassAddress<T>();
        var status = obj.Status;
        _sprite = obj.Sprite;
        GetImage((int)Images.Icon).sprite = _sprite;
        GetText((int)TextMeshs.ObjInfo).text = $"LV.{status.Level.GetValue()}\r\nEV.{status.Grade.GetValue()}";
        GetImage((int)Images.StackGageFill).fillAmount = status.Stack.GetValue() % status.MaxStack.GetValue();
        GetText((int)TextMeshs.StackText).text = $"{status.Stack.GetValue()}/{status.MaxStack.GetValue()}";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (PreviewObj == null)
        {
            Managers.Resource.Instantiate("BuildingPreview", go => 
            {
                PreviewObj = go;
                go.transform.position = Util.ScreenToWorldPointWithoutZ(eventData.position);
                previewRenderer = PreviewObj.GetComponent<SpriteRenderer>();
            });
        }
        previewRenderer.sprite = _sprite;
    }
    public void OnDrag(PointerEventData eventData)
    {
        PreviewObj.transform.position = Util.ScreenToWorldPointWithoutZ(eventData.position);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Util.Log("떼다");
        Managers.Resource.Destroy(PreviewObj);
        PreviewObj = null;

        // TODO : TowerBrain생성, 그리드에 맞게 배치
        DefaultTable.Tower data = Managers.Data.GetTable<DefaultTable.Tower>(Enums.Sheet.Tower, Index);
        Util.Log($"OnEndDrag : {data.Name}Tower");
        Managers.Resource.Instantiate($"{data.Name}Tower", go =>
        {
            go.transform.position = Util.ScreenToWorldPointWithoutZ(eventData.position);
        });
    }
}
