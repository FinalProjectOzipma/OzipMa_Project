using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Core
{
    public FloatBase Health = new();
    public FloatBase MaxHealth = new();

    public Core()
    {
        Health.SetValue(100.0f);
        MaxHealth.SetValue(100.0f);
    }

}
