using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGettable
{
    T GetClassAddress<T>() where T : UserObject;
}
