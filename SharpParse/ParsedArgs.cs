using System;
using System.Collections.Generic;

namespace SharpParse
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
            object[] vals = getValue<object[]>(key);
            T[] pVals = new T[vals.Length];
            int i = 0;
            foreach (object val in vals)
            {
                if (!(val is T))
                {
                    throw new ParsedArgsWrongTypeException(key, typeof(T)); // TODO make this list specific
                }
                pVals[i] = (T)val;
                i++;
            }
            return pVals;
        }
    }

    public class ParsedArgsException : SharpParseException 
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
}
