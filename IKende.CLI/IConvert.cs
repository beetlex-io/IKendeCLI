using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKende.CLI
{
    public interface IConvert
    {
        object Cast(string value, Type type);
    }
    public class EnumConvert : IConvert
    {
        public object Cast(string value, Type type)
        {
            return Enum.Parse(type, value);
        }
    }
}
