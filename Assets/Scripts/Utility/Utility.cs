using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Utility
{
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
}