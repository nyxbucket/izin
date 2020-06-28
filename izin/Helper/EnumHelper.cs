using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace izin.Helper
{
    public class EnumHelper
    {
        public static IEnumerable<EnumListObj> GetEnumDescWithVal<T>()
        {
            var type = typeof(T);
            var list = from string enm in Enum.GetNames(typeof(T))
                       select new EnumListObj
                       {
                           Id = Convert.ToInt32((T)Enum.Parse(typeof(T), enm)),
                           Ad = ((DescriptionAttribute)type.GetField(enm).GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault()).Description
                       };

            return list;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }

    public class EnumListObj
    {
        public int Id { get; set; }
        public string Ad { get; set; }
    }
}