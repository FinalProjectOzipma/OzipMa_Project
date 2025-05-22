using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    int _order = 10;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    Dictionary<string, UI_Scene> uiSceneList = new Dictionary<string, UI_Scene>();
    Dictionary<string, UI_Popup> uiPopupList = new Dictionary<string, UI_Popup>();

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
            {
                root = new GameObject { name = "@UI_Root" };
            }
            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        go.transform.SetParent(Root.transform);

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
    }

    /// <summary>
    /// 알림 UI 띄우기
    /// </summary>
    /// <param name="msg">알림 메세지</param>
    /// <param name="isGreen">true : 초록색 알림, false : 빨간색 알림</param>
    public void Notify(string msg, bool isGreen = true)
    {
        Managers.Resource.Instantiate("NotificationUI", obj =>
        {
            NotificationUI ui = obj.GetComponent<NotificationUI>();
            ui.SetMessage(msg, isGreen);
        });
    }

    public void NotifyDequeue(GameObject go)
    {
        Managers.Resource.Destroy(go, true);
        return;
    }

    public void ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        if (uiPopupList.TryGetValue(name, out var result))
        {
            _popupStack.Push(result);
            result.gameObject.SetActive(true);
        }
        else
        {
            Managers.Resource.Instantiate(name, go =>
            {
                T popup = Util.GetOrAddComponent<T>(go);
                uiPopupList.Add(name, popup);
                _popupStack.Push(popup);
                //go.transform.SetParent(Root.transform);
            });
        }
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup)
        {
            Util.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
        _order--;
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }


    /// <summary>
    /// 추가된 UI씬들을 UI 매니저에서 관리
    /// </summary>
    public void SetSceneList<T>(T uiScene) where T : UI_Scene
    {
        string key = typeof(T).ToString();


        if (!uiSceneList.ContainsKey(key))
        {
            uiSceneList.Add(key, uiScene);
        }
        else
        {
            uiSceneList[key] = uiScene;
        }
    }


    /// <summary>
    /// 추가된 UI씬 가져오기
    /// </summary>
    public T GetScene<T>() where T : UI_Scene
    {
        string key = typeof(T).ToString();

        if (uiSceneList.TryGetValue(key, out UI_Scene scene))
        {
            return scene as T;
        }

        return null;
    }


    public T GetPopup<T>() where T : UI_Popup
    {
        if (_popupStack.Count == 0) return null;

        T popUp = _popupStack.Peek() as T;
        return popUp;
    }


}
