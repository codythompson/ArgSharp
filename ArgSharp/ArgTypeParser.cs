using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgSharp
{
    public abstract class ArgTypeParser
    {
        public static Dictionary<Type, ArgTypeParser> basicParsers;
        static ArgTypeParser()
        {
            basicParsers = new Dictionary<Type, ArgTypeParser>();
            basicParsers.Add(typeof(string), new stringParser());
            basicParsers.Add(typeof(int), new intParser());
            basicParsers.Add(typeof(double), new doubleParser());
            basicParsers.Add(typeof(bool), new boolParser());
        }
        

        public abstract bool tryConvert(string arg, Type type, out object result);
    }

    public class stringParser : ArgTypeParser
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

    public class intParser : ArgTypeParser
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

    public class doubleParser : ArgTypeParser
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

    public class boolParser : ArgTypeParser
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
