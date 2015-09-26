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
            throw new NotImplementedException();
        }
    }
}
