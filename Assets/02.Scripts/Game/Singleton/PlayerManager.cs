using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager 
{
    public int Money { get; set; }
    public Inventory Inventory { get; set; } = new Inventory();

    public void Initialize()
    {
        // 처음 시작할때 선언
        Inventory = new Inventory();
        // 저장된게 있으면 선언
        // Inventory = 가져오는거
    }
}
