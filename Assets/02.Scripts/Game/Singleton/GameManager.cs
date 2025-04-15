using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager 
{
    public Inventory inven = new Inventory();
    public void Initialize()
    {
        for(int i = 0; i < 3; i++)
        {
            Tower tower = new Tower();
            Managers.Resource.LoadAssetAsync<Sprite>("SprSquare", (sprite) => { tower.Init(20, sprite); });
            inven.Add<Tower>(tower);
        }
    }
}
