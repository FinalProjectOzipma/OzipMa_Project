using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
	protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    protected Sequence uiSeq; // UI시퀀스

	public virtual void Init()
    {

    }

	public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
	{
		UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

		switch (type)
		{
			case Define.UIEvent.Click:
				evt.OnClickHandler -= action;
				evt.OnClickHandler += action;
				break;
			case Define.UIEvent.Drag:
				evt.OnDragHandler -= action;
				evt.OnDragHandler += action;
				break;
		}
	}

#if UNITY_EDITOR
    #region Editor
    private void OnValidate()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            return;
        }

        var fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (field.GetCustomAttribute<SerializeField>() == null)
            {
                continue;
            }

            if (field.FieldType.IsSubclassOf(typeof(Component)))
            {
                field.SetValue(this, FindComponent(field.FieldType, field.Name));
                continue;
            }

            if (field.FieldType != typeof(GameObject))
            {
                continue;
            }

            var component = FindComponent(typeof(Transform), field.Name);
            if (component == null)
            {
                continue;
            }

            field.SetValue(this, component.gameObject);
        }
    }

    private Component FindComponent(Type type, string name)
    {
        var components = GetComponentsInChildren(type, true);
        foreach (var component in components)
        {
            if (component.name == name)
            {
                return component;
            }
        }

        return null;
    }
    #endregion
#endif
}
