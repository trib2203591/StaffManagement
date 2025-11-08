namespace StaffManagement.Utils;

using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
    AllowMultiple = true)]
public class CheckAttribute : Attribute
{
    public string RE { get; set; }
    public CheckAttribute(string re)
    {
        this.RE = re;
    }
}
