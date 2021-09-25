using System;
using System.ComponentModel;
using System.Reflection;

namespace GcodeInterpreter
{
    public static class EnumExtensions
    {
        public static string GetName(this Enum value) =>
            Enum.GetName(value.GetType(), value) ?? string.Empty;

        public static string GetDescription(this Enum value)
        {
            FieldInfo? fieldInfo = GetFieldInfo(value);
            var attribute = (DescriptionAttribute?)fieldInfo?.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute?.Description ?? string.Empty;
        }

        private static FieldInfo? GetFieldInfo(Enum value) =>
            value.GetType().GetField(value.ToString());
    }
}