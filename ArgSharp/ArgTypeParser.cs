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
            basicParsers.Add(typeof(string), new StringParser());
            basicParsers.Add(typeof(int), new IntParser());
            basicParsers.Add(typeof(double), new DoubleParser());
            basicParsers.Add(typeof(bool), new BoolParser());
        }
        

        public abstract bool tryConvert(string arg, out object result);
    }

    public class StringParser : ArgTypeParser
    {
        public override bool tryConvert(string arg, out object result)
        {
            result = arg;
            return true;
        }
    }

    public class IntParser : ArgTypeParser
    {
        public override bool tryConvert(string arg, out object result)
        {
            result = null;
            int middleMan;
            if (!int.TryParse(arg, out middleMan))
            {
                return false;
            }
            result = middleMan;
            return true;
        }
    }

    public class DoubleParser : ArgTypeParser
    {
        public override bool tryConvert(string arg, out object result)
        {
            result = null;
            double middleMan;
            if (!double.TryParse(arg, out middleMan))
            {
                return false;
            }
            result = middleMan;
            return true;
        }
    }

    public class BoolParser : ArgTypeParser
    {
        public override bool tryConvert(string arg, out object result)
        {
            result = null;
            bool middleMan;
            if (!bool.TryParse(arg, out middleMan))
            {
                return false;
            }
            result = middleMan;
            return true;
        }
    }
}
