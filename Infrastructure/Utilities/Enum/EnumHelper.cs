using System;
using System.ComponentModel;
using System.Reflection;

namespace CourseStudio.Lib.Utilities
{
    public static class EnumHelper
    {
        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }

        public static bool TryParse<T>(string value, out T? result) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("This method is only for Enums");
            }
            if (Enum.TryParse<T>(value, out T tempResult))
            {
                result = tempResult;
                return true;
            }
            result = null;
            return false;
        }
    }
}