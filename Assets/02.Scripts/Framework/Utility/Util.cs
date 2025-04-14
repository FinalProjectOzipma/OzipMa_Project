using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
		if (component == null)
            component = go.AddComponent<T>();
        return component;
	}

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
		}
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static T StringToEnum<T>(string value, bool ignoreCase = true) where T : struct, System.Enum
    {
        if(System.Enum.TryParse<T>(value, ignoreCase, out var result))
        {
            return result;
        }
        else
        {
            throw new System.ArgumentException($"'{value}'는 '{typeof(T)}' 타입으로서 유효하지 않습니다.");
        }
    }

    public static List<T> StringListToEnumList<T>(List<string> values, bool ignoreCase = true) where T : struct, System.Enum
    {
        List<T> results = new List<T>();
        foreach(string value in values)
        {
            if (System.Enum.TryParse<T>(value, ignoreCase, out var result))
            {
                results.Add(result);
            }
            else
            {
                throw new System.ArgumentException($"'{value}'는 '{typeof(T)}' 타입으로서 유효하지 않습니다.");
            }
        }
        return results;
    }
}
