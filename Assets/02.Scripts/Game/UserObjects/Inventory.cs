using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory
{
    Dictionary<string, IGettable> userObject = new Dictionary<string, IGettable>();
    Dictionary<string, List<IGettable>> inventory = new Dictionary<string, List<IGettable>>();
    Dictionary<RankType, List<IGettable>> units = new Dictionary<RankType, List<IGettable>>();


    public Inventory()
    {
        for(int i = 0; i < (int)RankType.Count; i++)
        {
            units.Add((RankType)i, new List<IGettable>());
        }
    }

    public void Add<T>(T gettable) where T : UserObject, IGettable
    {
        // T 타입 딕셔너리에 존재한다면 
        if (inventory.ContainsKey(typeof(T).Name))
        {
            foreach(IGettable ge in inventory[typeof(T).Name])
            {
                UserObject uo = ge as UserObject;
                UserObject input = gettable as UserObject;
                //이미 인벤토리에 존재하는것이라면.
                if (uo.Name == input.Name)
                {
                    uo.Status.Stack.AddValue(1);
                    uo.UpGrade();
                    return;
                }
            }
        }
        //T 타입 딕셔너리에 존재하지않는다면
        else
        {
            inventory.Add(typeof(T).Name, new List<IGettable>());
        }

        inventory[typeof(T).Name].Add(gettable);
        userObject[$"{typeof(T).Name}{gettable.PrimaryKey}"] = gettable;

        if(typeof(T) == typeof(MyUnit))
        {
            MyUnit unit = gettable.GetClassAddress<MyUnit>();
            units[unit.RankType].Add(gettable);
        }
    }

    public void ClearList<T>() where T : IGettable
    {
        Type tType = typeof(T);
        if (inventory.ContainsKey(tType.Name) == false) return;
        inventory[tType.Name].Clear();
    }

    public List<IGettable> GetList<T>() where T : IGettable
    {
        if(inventory.TryGetValue(typeof(T).Name, out var list))
        {
            return list;
        }

        return null;
    }

    public T GetItem<T>(int key) where T : UserObject
    {
        if (userObject.TryGetValue($"{typeof(T).Name}{key}", out var value)) return value as T;

        Util.LogWarning("해당 되는 키에 오브젝트를 가져오질 못했습니다.");
        return null;
    }
}