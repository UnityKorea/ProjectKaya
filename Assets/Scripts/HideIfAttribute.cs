using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
public class HideIfAttribute : PropertyAttribute
{
    public string conditionalFieldName;
    public bool reverseConditional = false;

    public HideIfAttribute(string fieldName)
    {
        this.conditionalFieldName = fieldName;
        this.reverseConditional = false;
    }

    public HideIfAttribute(string fieldName, bool reverse)
    {
        this.conditionalFieldName = fieldName;
        this.reverseConditional = reverse;
    }
}
