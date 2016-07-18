using System;

namespace SmallDahuaLib
{
    public class FieldInfo
    {
        public FieldAttribute Field { get; private set; }
        public Object Value { get; private set; }
        public System.Reflection.MemberInfo MemberInfo { get; private set; }

        private FieldInfo()
        {
        }

        public static FieldInfo FromPropertyInfo(System.Reflection.PropertyInfo pi)
        {
            FieldInfo fi = new FieldInfo();
            fi.Field = (FieldAttribute)Attribute.GetCustomAttribute(pi, typeof(FieldAttribute));
            fi.Value = null;
            fi.MemberInfo = pi;
            return fi;
        }

        public static FieldInfo FromFieldInfo(System.Reflection.FieldInfo fieldInfo)
        {
            FieldInfo fi = new FieldInfo();
            fi.Field = (FieldAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(FieldAttribute));
            fi.Value = null;
            fi.MemberInfo = fieldInfo;
            return fi;
        }

        public static FieldInfo FromPropertyInfo(System.Reflection.PropertyInfo pi, Object obj)
        {
            FieldInfo fi = new FieldInfo();
            fi.Field = (FieldAttribute)Attribute.GetCustomAttribute(pi, typeof(FieldAttribute));
            fi.Value = pi.GetValue(obj);
            fi.MemberInfo = pi;
            return fi;
        }

        public static FieldInfo FromFieldInfo(System.Reflection.FieldInfo fieldInfo, Object obj)
        {
            FieldInfo fi = new FieldInfo();
            fi.Field = (FieldAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(FieldAttribute));
            fi.Value = fieldInfo.GetValue(obj);
            fi.MemberInfo = fieldInfo;
            return fi;
        }
    }
}
