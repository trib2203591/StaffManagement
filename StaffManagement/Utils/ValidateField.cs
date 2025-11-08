namespace StaffManagement.Utils;

using System;
using System.Reflection;
using System.Text.RegularExpressions;
using StaffManagement.Models;

public class ValidateField
{
    public ValidateField()
    {
    }
    public static bool CheckEmail(Staff staff)
    {
        Type StaffType = staff.GetType();
        PropertyInfo propertyInfo = StaffType.GetProperty("Email");
        if (propertyInfo != null)
        {
            CheckAttribute customAttribute =
                propertyInfo.GetCustomAttribute<CheckAttribute>();
            if (customAttribute != null)
            {
                object value =
                    propertyInfo.GetValue(staff);
                string stringValue = value?.ToString() ?? string.Empty;
                Regex validateEmailRegex =
                    new Regex(customAttribute.RE);
                return validateEmailRegex.IsMatch(stringValue.ToString());
            }
        }
        return true;
    }

    public static bool CheckPhone(Staff staff)
    {
        Type StaffType = staff.GetType();
        PropertyInfo propertyInfo = StaffType.GetProperty("PhoneNumber");
        if (propertyInfo != null)
        {
            CheckAttribute customAttribute =
                propertyInfo.GetCustomAttribute<CheckAttribute>();
            if (customAttribute != null)
            {
                object value =
                    propertyInfo.GetValue(staff);
                string stringValue = value?.ToString() ?? string.Empty;
                Regex validatePhoneRegex =
                    new Regex(customAttribute.RE);
                return validatePhoneRegex.IsMatch(stringValue.ToString());
            }
        }
        return true;
    }
}