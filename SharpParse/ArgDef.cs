using System;
using System.Collections.Generic;

namespace SharpParse
{
    public class ArgDef
    {
        // options
        public List<string> argLabels;
        public char[] labelPrefixes = new char [] {'-'};
        public int argCount = 1;
        public bool argCountIsRemainderOfArgs = false;
        public Type type = typeof(string);
        public object defaultValue;
        public string helpMessage = "";
        public int minAllowedInstances = 0;
        public int maxAllowedInstances = 1;
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
            this.typeParsers = typeParsers;
            if (!typeParsers.ContainsKey(type))
            {
                throw new Exception(); // todo custom exception
            }

            if (name == null)
            {
                name = getNameFromArgLabels();
                if (name == null)
                {
                    throw new Exception(); // todo custom exception
                }
            }
            instanceCount = 0;
            errorMessages.Clear();
            if (isOrderedArg())
            {
                argCount = 1;
                minAllowedInstances = 1;
                maxAllowedInstances = 1;
                // TODO throw an excpetion here instead of fixing the values
            }
        }

        public virtual bool consume(VirtualArray<string> vArgs)
        {
            if (!isConsumeable(vArgs))
            {
                return false;
            }

            if (argCountIsRemainderOfArgs)
            {
                vArgs.moveStart(vArgs.endIndexExclusive);
            }
            vArgs.moveStartBy(argCount);
            return true;
        }

        public virtual void parseFinish()
        {
            if (instanceCount < minAllowedInstances)
            {
                if (minAllowedInstances == 1)
                {
                    errorMessages.Add(string.Format("The '{0}' argument is required."));
                }
                else
                {
                    errorMessages.Add(string.Format("The '{0}' argument must be provided at least {1} times.", name, minAllowedInstances));
                }
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
                throw new ArgumentException("vArgs must have a length of at least 1."); // TODO custom argument
            }

            if (!isOrderedArg() && !labelMatch(vArgs[0]))
            {
                return false; // this isn't the arg we're looking for
            }

            if (++instanceCount > maxAllowedInstances)
            {
                errorMessages.Add(string.Format("Encountered the option '{0}' too many times. (only allowed {1} time(s))", name, maxAllowedInstances));
                return false;
            }

            if (!argCountIsRemainderOfArgs && vArgs.length < argCount)
            {
                errorMessages.Add(string.Format("The option '{0}' expects {1} following arguments, only {2} were encountered.", name, argCount, vArgs.length - 1));
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
                    int lastIx = argCount - 1;
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
    }
}
