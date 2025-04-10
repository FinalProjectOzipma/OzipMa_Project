using GoogleSheet.Core.Type;
using GoogleSheet.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomType<T>
{
    public List<T> Values;
    public CustomType()
    {
        Values = new List<T>();
    }
}

[Type(typeof(CustomType<Enums.EAtkType>), new string[] { "List<EAtkType>" })]
public class EAtkTypeReader : GoogleSheet.Type.IType
{
    public object DefaultValue => null;

    public object Read(string value)
    {
        // Cry,Jelly,Work,입니다
        var values = value.Split(',');
        CustomType<Enums.EAtkType> custom = new CustomType<Enums.EAtkType>();
        List<Enums.EAtkType> list = custom.Values;
        foreach (var v in values)
        {
            list.Add((Enums.EAtkType)System.Enum.Parse(typeof(Enums.EAtkType), v));
        }
        return custom;
    }

    public string Write(object value)
    {
        return "";
    }
}