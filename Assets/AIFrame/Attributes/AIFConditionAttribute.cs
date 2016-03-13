using System;
using UnityEngine;
using System.Collections;

public class AIFConditionAttribute : Attribute
{
    public string Name;
    public Type conditionType;

    public AIFConditionAttribute(string name,Type condtype)
    {
        Name = name;
        conditionType = condtype;
    }

}
