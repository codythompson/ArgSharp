using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpParse
{
    public abstract class ArgTypeConverter
    {
        public static Dictionary<Type, ArgTypeConverter> basicConverters;
        static ArgTypeConverter()
        {
            basicConverters = new Dictionary<Type, ArgTypeConverter>();
            basicConverters.Add(typeof(string), new stringConverter());
            basicConverters.Add(typeof(int), new intConverter());
            basicConverters.Add(typeof(double), new doubleConverter());
            basicConverters.Add(typeof(bool), new boolConverter());
        }
        

        public abstract bool tryConvert(string arg, Type type, out object result);
    }

    public class stringConverter : ArgTypeConverter
    {
        public override bool tryConvert(string arg, Type type, out object result)
        {
            result = arg;
            if (type != typeof(string))
            {
                return false;
            }
            return true;
        }
    }

    public class intConverter : ArgTypeConverter
    {
        public override bool tryConvert(string arg, Type type, out object result)
        {
            result = null;
            int middleMan;
            if (type != typeof(int) || !int.TryParse(arg, out middleMan))
            {
                return false;
            }
            result = middleMan;
            return true;
        }
    }

    public class doubleConverter : ArgTypeConverter
    {
        public override bool tryConvert(string arg, Type type, out object result)
        {
            result = null;
            double middleMan;
            if (type != typeof(double) || !double.TryParse(arg, out middleMan))
            {
                return false;
            }
            result = middleMan;
            return true;
        }
    }

    public class boolConverter : ArgTypeConverter
    {
        public override bool tryConvert(string arg, Type type, out object result)
        {
            result = null;
            bool middleMan;
            if (type != typeof(bool) || !bool.TryParse(arg, out middleMan))
            {
                return false;
            }
            result = middleMan;
            return true;
        }
    }
}
