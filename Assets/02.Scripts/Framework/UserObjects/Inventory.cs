using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    Dictionary<string, List<IGettable>> inventory = new Dictionary<string, List<IGettable>>();
    Dictionary<Enums.RankType, List<IGettable>> units = new Dictionary<Enums.RankType, List<IGettable>>();

    public void Add<T>(T gettable) where T : IGettable
    {
        if(inventory.ContainsKey(nameof(T)) == false)
        {
            inventory.Add(nameof(T), new List<IGettable>());
        }

        inventory[nameof(T)].Add(gettable);

        if(typeof(T) == typeof(MyUnit))
        {
            MyUnit unit = gettable.GetClassAddress<MyUnit>();
            units[unit.RankType].Add(gettable);
        }
    }

    public List<T> GetList<T>() where T : IGettable
    {
        if(inventory.TryGetValue(nameof(T), out var list))
        {
            return list as List<T>;
        }

        return null;
    }
}
