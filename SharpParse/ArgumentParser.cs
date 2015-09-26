using System;
using System.Collections.Generic;

namespace SharpParse
{
    public class ArgumentParser
    {
        // options
        public bool printUsageOnArgDefRuleFail = true;
        public bool printErrorMessageOnArgDefRuleFail = true;
        //

        public List<ArgDef> argDefs;

        public ArgumentParser()
        {
            argDefs = new List<ArgDef>();
        }

        public virtual void addArgDef(ArgDef argDef)
        {
            argDefs.Add(argDef);
        }

        public virtual ParsedArgs parseArgs(string[] args)
        {
            for(int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                foreach (ArgDef def in argDefs)
                {
                    if (def.labelMatch(arg))
                    {
                        Console.WriteLine(string.Format("Matched arg {0}", def.name));
                    }
                }
            }

            // throw new NotImplementedException();
            return null;
        }
    }
}
