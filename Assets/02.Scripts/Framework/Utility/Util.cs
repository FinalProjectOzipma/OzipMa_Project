using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
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
            foreach (T component in go.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static T StringToEnum<T>(string value, bool ignoreCase = true) where T : struct, System.Enum
    {
        if (System.Enum.TryParse<T>(value, ignoreCase, out var result))
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
        foreach (string value in values)
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

    public static T FindComponent<T>(GameObject gameObject, string name) where T : Component
    {
        var components = gameObject.GetComponentsInChildren<T>(true);
        foreach (var component in components)
        {
            if (component.name.Equals(name))
            {
                return component;
            }
        }
        Debug.LogWarning($"Failed to FindComponent<{typeof(T).Name}>({gameObject.name}, {name})");
        return null;
    }

    public static T[] FindComponents<T>(GameObject gameObject, Type enumType) where T : Component
    {
        var components = gameObject.GetComponentsInChildren<T>(true);
        var children = new Dictionary<string, T>(components.Length);
        foreach (var component in components)
        {
            string key = component.name;
            children.TryAdd(key, component);
        }

        var names = Enum.GetNames(enumType);
        return names.Where(children.ContainsKey).Select(name => children[name]).ToArray();
    }

    public static DG.Tweening.Sequence RecyclableSequence()
    {
        return DOTween.Sequence().Pause().SetAutoKill(false);
    }

    public static void SetPositionX(Transform transform, float x)
    {
        Vector3 position = transform.position;
        position.x = x;
        transform.position = position;
    }

    public static void SetPositionY(Transform transform, float y)
    {
        Vector3 position = transform.position;
        position.y = y;
        transform.position = position;
    }

    public static void SetPositionZ(Transform transform, float z)
    {
        Vector3 position = transform.position;
        position.z = z;
        transform.position = position;
    }

    public static List<T> TableConverter<T>(List<GoogleSheet.ITable> origins) where T : GoogleSheet.ITable
    {
        List<T> res = new List<T>();
        foreach (GoogleSheet.ITable item in origins)
        {
            res.Add((T)item);
        }
        return res;
    }

    public static Vector3 ScreenToWorldPointWithoutZ(Vector2 screenPoint)
    {
        Vector3 res = Camera.main.ScreenToWorldPoint(screenPoint);
        res.z = 0;
        return res;
    }

    public static bool TryStringToVector3Int(string str, out Vector3Int result)
    {
        result = Vector3Int.zero;

        if (string.IsNullOrWhiteSpace(str))
            return false;

        string[] trimmed = str.Trim('(', ')').Split(',');

        if (trimmed.Length != 3)
            return false;

        bool successX = int.TryParse(trimmed[0].Trim(), out int x);
        bool successY = int.TryParse(trimmed[1].Trim(), out int y);
        bool successZ = int.TryParse(trimmed[2].Trim(), out int z);

        if (successX && successY && successZ)
        {
            result = new Vector3Int(x, y, z);
            return true;
        }

        return false;
    }

    // 디버그를 위한
    public static void Log(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }

    public static void LogWarning(string message)
    {
#if UNITY_EDITOR
        Debug.LogWarning(message);
#endif
    }
    public static void LogError(string message)
    {
#if UNITY_EDITOR
        Debug.LogError(message);
#endif
    }


    /// <summary>
    /// long => String
    /// </summary>
    public static string FormatNumber(long number)
    {
        if (number >= 1_000_000_000)
            return (number / 1_000_000_000f).ToString("0.0") + "B";
        else if (number >= 1_000_000)
            return (number / 1_000_000f).ToString("0.0") + "M";
        else if (number >= 1_000)
            return (number / 1_000f).ToString("0.0") + "K";
        else
            return number.ToString();
    }

    public static float GetAngle(Vector2 start, Vector2 end)
    {
        Vector2 v2 = end - start;
        return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
    }

    public static int ConvertLayer(int LayerNumber)
    {
        return 1 << LayerNumber;
    }
}
