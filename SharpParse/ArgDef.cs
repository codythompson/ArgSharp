using System;
using System.Collections.Generic;

namespace SharpParse
{
    public class ArgDef
    {
        public List<string> argLabels;
        public char[] labelPrefixes = new char [] {'-'};
        public int argValCount = 1;
        public Type type = typeof(string);
        public object defaultValue;
        public string helpMessage = "";
        public string name;

        public ArgDef()
        {
            argLabels = new List<string>();
        }

        public virtual void init()
        {
            if (name == null)
            {
                name = getNameFromArgLabels();
            }
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
