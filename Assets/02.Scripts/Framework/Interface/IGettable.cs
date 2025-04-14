using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGettable
{
    public T GetClassAddress<T>() where T : UserObject;
}
