using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    Dictionary<string, List<IGettable>> inventory = new Dictionary<string, List<IGettable>>();
    Dictionary<Enums.RankType, List<IGettable>> units = new Dictionary<Enums.RankType, List<IGettable>>();

    public Inventory()
    {
        for(int i = 0; i < (int)Enums.RankType.Count; i++)
        {
            units.Add((Enums.RankType)i, new List<IGettable>());
        }
    }

    public void Add<T>(T gettable) where T : UserObject, IGettable
    {
        if(inventory.ContainsKey(typeof(T).Name) == false)
        {
            inventory.Add(typeof(T).Name, new List<IGettable>());
        }

        inventory[typeof(T).Name].Add(gettable);

        if(typeof(T) == typeof(MyUnit))
        {
            MyUnit unit = gettable.GetClassAddress<MyUnit>();
            units[unit.RankType].Add(gettable);
        }
    }

    public List<IGettable> GetList<T>() where T : IGettable
    {
        if(inventory.TryGetValue(typeof(T).Name, out var list))
        {
            return list;
        }

        return null;
    }
}
