using GoogleSheet.Core.Type;
using GoogleSheet.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UGS(typeof(EAtkType))]
public enum EAtkType
{
    Cry,
    Jelly,
    Work,
    입니다,
}

public class CustomType
{
    public List<EAtkType> Values;
    public CustomType()
    {
        Values = new List<EAtkType>();
    }
}

[Type(typeof(CustomType), new string[] { "EnumList", "CustomType", "CustomTest" })]
public class CustomTest : GoogleSheet.Type.IType
{
    public object DefaultValue => null;

    public object Read(string value)
    {
        // Cry,Jelly,Work,입니다
        var values = value.Split(',');
        CustomType custom = new CustomType();
        List<EAtkType> list = custom.Values;
        foreach(var v in values)
        {
            list.Add((EAtkType)System.Enum.Parse(typeof(EAtkType), v));
        }
        return custom;
    }

    public string Write(object value)
    {
        return "";
    }
}