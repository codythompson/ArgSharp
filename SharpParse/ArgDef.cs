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
        public int maxAllowedInstances = 1;
        public string name;
        // end options

        private int instanceCount;
        private List<string> errorMessages;

        public ArgDef()
        {
            argLabels = new List<string>();
            errorMessages = new List<string>();
        }

        public virtual void parseInit()
        {
            // if argLabels is empty then this is an ordered arg
            // if this is an ordered arg then:
            //   maxAllowedInstances must = 1

            if (name == null)
            {
                name = getNameFromArgLabels();
            }
            instanceCount = 0;
            errorMessages.Clear();
        }

        public virtual bool consume(VirtualArray<string> vArgs)
        {
            if (!isOrderedArg() && !labelMatch(vArgs[0]))
            {
                return false; // this isn't the arg we're looking for
            }

            if (++instanceCount > maxAllowedInstances)
            {
                errorMessages.Add(string.Format("Encountered the option '{0}' too many times. (only allowed {1} time(s))", name, maxAllowedInstances));
                return false;
            }

            if (argCountIsRemainderOfArgs)
            {
                vArgs.moveStart(vArgs.endIndexExclusive);
            }
            vArgs.moveStartBy(argCount);
            return true;
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
                throw new Exception(); // TODO use a custom exception
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
    }
}
