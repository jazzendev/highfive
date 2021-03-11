using System;

namespace HighFive.Core.Utility
{
    public class DescriptionAttribute : Attribute
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //public DescriptionAttribute(string name)
        //{
        //    Name = name;
        //}
        //public DescriptionAttribute(string name, string description)
        //{
        //    Name = name;
        //    Description = description;
        //}
        public DescriptionAttribute(string name = null, string description = null, string code = null)
        {
            Name = name;
            Description = description;
            Code = code;
        }
    }

    public static class EnumHelper
    {
        public static string Name(this Enum value)
        {
            // variables  
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // return  
            return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Name;
        }

        public static string Description(this Enum value)
        {
            // variables  
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // return  
            return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }

        public static string Code(this Enum value)
        {
            // variables  
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // return  
            return attributes.Length == 0 ? string.Empty : ((DescriptionAttribute)attributes[0]).Name;
        }

    }
}
