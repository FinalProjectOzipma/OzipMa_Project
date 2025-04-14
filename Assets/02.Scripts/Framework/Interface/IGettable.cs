using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGettable
{
    public Type Type { get; set; }
    public T GetClassAddress<T>() where T : UserObject;
}
