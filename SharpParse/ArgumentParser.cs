using System;
using System.Collections.Generic;

namespace SharpParse
{
    public class ArgumentParser
    {
        public List<ArgDef> argDefs;

        public ArgumentParser()
        {
            argDefs = new List<ArgDef>();
        }

        public virtual void addArgDef(ArgDef argDef)
        {
            argDefs.Add(argDef);
        }

        public virtual ParsedArgs parseArgs(string[] args, bool printUsageOnFail = true, bool printErrorMessageOnFail = true, bool exitOnFail = true)
        {
            throw new NotImplementedException("parseArgs not implemented yet.");
        }
    }
}
