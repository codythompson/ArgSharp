using System;
using System.Collections.Generic;

namespace ArgSharp
{
    public class ArgDef
    {
        // options
        public List<string> argLabels;
        public char[] labelPrefixes = new char [] {'-'};
        public int argCount = 0;
        public bool argCountIsRemainderOfArgs = false;
        public bool createArrayForArgCount1 = false;
        public Type type = typeof(string);
        public object defaultValue;
        public string helpMessage = "";
        //public int minAllowedInstances = 0;
        //public int maxAllowedInstances = 1;
        public bool required = false;
        public string name;
        // end options

        private int instanceCount;
        private List<string> errorMessages;
        private Dictionary<Type, ArgTypeParser> typeParsers;

        public ArgDef()
        {
            argLabels = new List<string>();
            errorMessages = new List<string>();
        }

        public virtual void parseInit(Dictionary<Type, ArgTypeParser> typeParsers)
        {
            instanceCount = 0;
            errorMessages.Clear();

            this.typeParsers = typeParsers;

            if (argCount < 0)
            {
                throw new ArgDefBadOptionsException("ArgDef.argCount must be non negative.");
            }

            if (!typeParsers.ContainsKey(type))
            {
                throw new ArgDefBadOptionsException(string.Format("Can't use type '{0}', no matching ArgTypeParser found.", type));
            }

            if (name == null)
            {
                name = getNameFromArgLabels();
                if (name == null)
                {
                    throw new ArgDefBadOptionsException(string.Format("At least one argLabel or a name must be provided."));
                }
            }
            if (isOrderedArg())
            {
                if (argCount > 0)
                {
                    throw new ArgDefBadOptionsException(string.Format("An ordered arg must have an argCount of 0."));
                }
                required = true;
            }
            else if (argCount == 0 && type != typeof(bool))
            {
                if (type == typeof(string))
                {
                    type = typeof(bool);
                }
                else if (type != typeof(bool))
                {
                    throw new ArgDefBadOptionsException(string.Format("Only type 'bool' is allowed for 0 argCount non-ordered args (args without any argLabels)"));
                }
            }
        }

        public virtual bool consume(VirtualArray<string> vArgs, ParsedArgs pArgs)
        {
            if (!isConsumeable(vArgs))
            {
                return false;
            }

            pArgs.add(name, getValue(vArgs));

            if (argCountIsRemainderOfArgs)
            {
                vArgs.moveStart(vArgs.endIndexExclusive);
            }
            else
            {
                vArgs.moveStartBy(argCount + 1);
            }
            return true;
        }

        public virtual void parseFinish(ParsedArgs pArgs)
        {
            if (instanceCount < 1 && required)
            {
                errorMessages.Add(string.Format("The '{0}' argument is required."));
                return;
            }
            if (instanceCount == 0)
            {
                pArgs.add(name, defaultValue);
            }
        }

        /*
         * 
         */
        public virtual bool isOrderedArg()
        {
            return argLabels.Count == 0;
        }

        public virtual bool labelMatch(string arg)
        {
            foreach (string label in argLabels)
            {
                if (arg == label)
                {
                    return true;
                }
            }
            return false;
        }

        public bool errorOccured()
        {
            return errorMessages.Count > 0;
        }

        public List<string> getErrorMessages()
        {
            return errorMessages;
        }

        public string getUsageString(bool appendHelpString)
        {
            // TODO better use of [] for multi label options
            string usage;
            if (isOrderedArg())
            {
                usage = name;
            }
            else
            {
                usage = argLabels[0];
                for (int i = 1; i < argLabels.Count; i++)
                {
                    usage += " | " + argLabels[i];
                }
            }
            for (int i = 0; i < argCount; i++)
            {
                usage += string.Format(" {0}_val_{1}", name, i + 1);
            }
            if (!isOrderedArg())
            {
                if (required)
                {
                    usage = string.Format("<{0}>", usage);
                }
                else
                {
                    usage = string.Format("[{0}]", usage);
                }
            }

            if (appendHelpString)
            {
                usage += "\n" + helpMessage;
            }

            return usage;
        }

        /*
         * helpers
         */
        private string getNameFromArgLabels()
        {
            if (argLabels.Count < 1)
            {
                return null;
            }

            int lastMax = 0;
            int lastMaxI = -1;
            for (int i = 0; i < argLabels.Count; i++)
            {
                string trimmedLabel = argLabels[i].Trim(labelPrefixes);
                if (trimmedLabel.Length > lastMax) {
                    lastMax = trimmedLabel.Length;
                    lastMaxI = i;
                }
            }

            return argLabels[lastMaxI].Trim(labelPrefixes);
        }

        private bool isConsumeable(VirtualArray<string> vArgs)
        {
            if (vArgs.length <= 0)
            {
                throw new ArgDefException(string.Format("vArgs must have a length of at least 1. If you encounter this exception, something went wrong in ArgumentParser.parseArgs."));
            }

            if (!isOrderedArg() && !labelMatch(vArgs[0]))
            {
                return false; // this isn't the arg we're looking for
            }

            if (++instanceCount > 1)
            {
                errorMessages.Add(string.Format("Encountered the option '{0}' more than once.", name));
                return false;
            }

            if (!argCountIsRemainderOfArgs && vArgs.length < argCount + 1)
            {
                errorMessages.Add(string.Format("The option '{0}' expects {1} following arguments, only {2} were encountered.", name, argCount, vArgs.length - 1));
                return false;
            }

            if (type != typeof(string))
            {
                object dummyObj;
                if (isOrderedArg())
                {
                    if (!typeParsers[type].tryConvert(vArgs[0], type, out dummyObj))
                    {
                        errorMessages.Add(string.Format("The '{0}' argument expects an argument of type '{1}', unable to parse '{2}'.", name, type, vArgs[0]));
                        return false;
                    }
                }
                else
                {
                    int lastIx = argCount;
                    if (argCountIsRemainderOfArgs)
                    {
                        lastIx = vArgs.length - 1;
                    }
                    for (int i = 1; i <= lastIx; i++)
                    {
                        if (!typeParsers[type].tryConvert(vArgs[i], type, out dummyObj))
                        {
                            errorMessages.Add(string.Format("The '{0}' argument expects an argument of type '{1}', unable to parse '{2}'.", name, type, vArgs[i]));
                            return false;
                        }
                    }
                }

            }

            return true;
        }

        private object getValue(VirtualArray<string> vArgs)
        {
            if (argCount == 0 && type == typeof(bool))
            {
                if (instanceCount > 0)
                {
                    return true;
                }
            }

            if (argCount == 0 && !argCountIsRemainderOfArgs)
            {
                object val;
                typeParsers[type].tryConvert(vArgs[0], type, out val);
                return val;
            }

            int lastIx = argCount;
            if (argCountIsRemainderOfArgs)
            {
                lastIx = vArgs.length - 1;
            }
            object[] vals = new object[lastIx];
            for (int i = 1; i <= lastIx; i++)
            {
                object val;
                typeParsers[type].tryConvert(vArgs[i], type, out val);
                vals[i - 1] = val;
            }
            if (!createArrayForArgCount1 && vals.Length == 1)
            {
                return vals[0];
            }
            return vals;
        }
    }

    public class ArgDefException : ArgSharpException
    {
        public ArgDefException(string message) : base(message) {}
    }

    public class ArgDefBadOptionsException : ArgDefException
    {
        public ArgDefBadOptionsException(string message) : base(message) {}
    }
}
