using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.CLI
{
    public class ArgumentBuilder
    {
        public ArgumentAttribute Argument
        {
            get;
            set;
        }

        public System.Reflection.PropertyInfo Property
        {
            get;
            set;
        }

        public object GetValue(string data)
        {
            if (Argument.Required && string.IsNullOrEmpty(data))
                return null;
            if (!string.IsNullOrEmpty(Argument.Regex))
                if (!System.Text.RegularExpressions.Regex.IsMatch(data, Argument.Regex))
                    return null;
            try
            {
                if (Property.PropertyType.IsEnum)
                    return new EnumConvert().Cast(data, Property.PropertyType);
                return Convert.ChangeType(data, Property.PropertyType);
            }
            catch (Exception e_)
            {
                return null;
            }
        }
        public void SetValue(object data, object target)
        {
            Property.SetValue(target, data, null);
        }
    }
}
