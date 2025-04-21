using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager 
{
    
    public void Initialize()
    {
        MyUnit unit = new MyUnit();
        unit.Init(1, null);
        Managers.Player.Inventory.Add<MyUnit>(unit);
        Managers.Wave.StartWave(0);
    }
}
