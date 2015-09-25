using System;
using System.Collections.Generic;

namespace SharpParse
{
    public class ArgDef
    {
        public List<string> argLabels;
        public int argValCount = 1;
        public Type type = typeof(string);
        public object defaultValue;
        public string helpMessage = "";
        public string name;

        public ArgDef()
        {
            argLabels = new List<string>();
        }

        public void init()
        {
            if (name == null)
            {
                name = getNameFromArgLabels();
            }
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
                string trimmedLabel = argLabels[i].Trim('-');
                if (trimmedLabel.Length > lastMax) {
                    lastMax = trimmedLabel.Length;
                    lastMaxI = i;
                }
            }

            return argLabels[lastMaxI].Trim('-');
        }
    }
}
