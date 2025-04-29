using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager
{
    int _order = 10;

    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    Dictionary<string, UI_Scene> uiSceneList = new Dictionary<string, UI_Scene>();

    public GameObject Root
    {
        get
        {
			GameObject root = GameObject.Find("@UI_Root");
			if (root == null)
				root = new GameObject { name = "@UI_Root" };
            return root;
		}
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    /*public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
	{
		if (string.IsNullOrEmpty(name))
			name = typeof(T).Name;

		GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
		if (parent != null)
			go.transform.SetParent(parent);

		return Util.GetOrAddComponent<T>(go);
	}*/

    //public void ShowSceneUI<T>(string name = null) where T : UI_Scene
    //{
    //    if (string.IsNullOrEmpty(name))
    //        name = typeof(T).Name;

    //    Managers.Resource.Instantiate(name, go => 
    //    {
    //        T sceneUI = Util.GetOrAddComponent<T>(go);
    //        _sceneUI = sceneUI;

    //        go.transform.SetParent(Root.transform);
    //    });
    //}


    public void ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        Managers.Resource.Instantiate(name, go =>
        {
            T popup = Util.GetOrAddComponent<T>(go);
            _popupStack.Push(popup);

            go.transform.SetParent(Root.transform);

        });
    }

    public void ClosePopupUI(UI_Popup popup)
    {
		if (_popupStack.Count == 0)
			return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
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

}
