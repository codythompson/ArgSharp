using System;
using System.Collections;
using System.Collections.Generic;

namespace ArgSharp
{
    public class ParsedArgs
    {
        public List<string> errorMessages;

        Dictionary<string, object> args;

        public ParsedArgs()
        {
            errorMessages = new List<string>();
            args = new Dictionary<string, object>();
        }

        public void addErrorMessage(string message)
        {
            errorMessages.Add(message);
        }
        
        public void addErrorMessages(List<string> messages)
        {
            errorMessages.AddRange(messages);
        }

        public List<string> getErrorMessages()
        {
            return errorMessages;
        }

        public bool errorOccured()
        {
            return errorMessages.Count > 0;
        }

        public void add(string key, object obj)
        {
            args.Add(key, obj);
        }

        public bool containsKey(string key)
        {
            return args.ContainsKey(key);
        }

        public T getValue<T>(string key)
        {
            if (!args.ContainsKey(key))
            {
                throw new ParsedArgsKeyNotFoundException(key);
            }

            object val = args[key];

            if (!(val is T))
            {
                throw new ParsedArgsWrongTypeException(key, typeof(T));
            }

            return (T)args[key];
        }

        public string getString(string key)
        {
            return getValue<string>(key);
        }

        public int getInt(string key)
        {
            return getValue<int>(key);
        }

        public double getDouble(string key)
        {
            return getValue<double>(key);
        }

        public T[] getArray<T>(string key)
        {
            if (!args.ContainsKey(key))
            {
                throw new ParsedArgsKeyNotFoundException(key);
            }
            object preCastVal = args[key];
            Type valType = preCastVal.GetType();

            if (!valType.IsArray)
            {
                throw new ParsedArgsValueNotArrayException(key);
            }

            Type valEleType = valType.GetElementType();
            if (valEleType != typeof(object) && valEleType != typeof(T))
            {
                throw new ParsedArgsWrongTypeException(key, typeof(T[]));
            }

            if (valEleType != typeof(object))
            {
                return (T[])preCastVal;
            }

            object[] vals = getValue<object[]>(key);
            T[] pVals = new T[vals.Length];
            int i = 0;
            foreach (object val in vals)
            {
                if (!(val is T))
                {
                    throw new ParsedArgsWrongTypeException(key, typeof(T[]));
                }
                pVals[i] = (T)val;
                i++;
            }
            return pVals;
        }

        public override string ToString()
        {
            string str = "'ParsedArgs': {\n";
            int i = 0;
            foreach (KeyValuePair<string, object> kvp in args)
            {
                string sep = ",";
                string valQuotes = "'";
                object val = kvp.Value;
                object type = kvp.Value.GetType();
                if (val is object[])
                {
                    valQuotes = "";
                    object[] arrVal = (object[])val;
                    string strVal = "[\n";
                    if (arrVal.Length > 0)
                    {
                        type = arrVal[0].GetType().ToString() + "[]";
                    }
                    for (int j = 0; j < arrVal.Length; j++)
                    {
                        string subSep = ",";
                        if (j == arrVal.Length - 1)
                        {
                            subSep = "";
                        }
                        strVal += string.Format("    '{0}'{1}\n", arrVal[j], subSep);
                    }
                    strVal += "  ]\n";
                    val = strVal;
                }
                if (i == args.Count - 1)
                {
                    sep = "";
                }
                str += string.Format("'{0}': {{'type': '{1}', 'value': {4}{2}{4}}}{3}\n", kvp.Key, type, val, sep, valQuotes);
            }
            str += "}";
            return str;
        }
    }

    public class ParsedArgsException : ArgSharpException 
    {
        public ParsedArgsException(string message) : base(message) {}
    }
    public class ParsedArgsKeyNotFoundException : ParsedArgsException
    {
        public ParsedArgsKeyNotFoundException(string key) : base(string.Format("The key '{0}' was not found.", key)) {}
    }
    public class ParsedArgsWrongTypeException : ParsedArgsException
    {
        public ParsedArgsWrongTypeException(string key, Type type) : base(string.Format("The value associated with key '{0}' is not a {1}.", key, type)) { }
    }
    public class ParsedArgsValueNotArrayException : ParsedArgsException
    {
        public ParsedArgsValueNotArrayException(string key) : base(string.Format("The value associated with key '{0}' is not an array.", key)) {}
    }
}
