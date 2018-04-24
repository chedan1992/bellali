using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QINGUO.Common
{
   public class ReferanceAttribute
    {
        public static EntityAttribute ReferanceEntityAttribute(MemberInfo t)
        {
            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(t);
            return attrs.OfType<EntityAttribute>().FirstOrDefault();
        }

        public static PropertyAttribute ReferancePropertyAttribute(MemberInfo t)
        {
            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(t);
            return attrs.OfType<PropertyAttribute>().FirstOrDefault();  
        }
    }
}
