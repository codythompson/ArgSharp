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

        private List<ArgDef> labeledArgDefs;
        private List<ArgDef> orderedArgDefs;
        private int orderedArgDefIx;

        public ArgumentParser()
        {
            labeledArgDefs = new List<ArgDef>();
            orderedArgDefs = new List<ArgDef>();
        }

        public virtual void addArgDef(ArgDef argDef)
        {
            if (argDef.isOrderedArg())
            {
                orderedArgDefs.Add(argDef);
            }
            else
            {
                labeledArgDefs.Add(argDef);
            }
        }

        public virtual ParsedArgs parseArgs(string[] args)
        {
            orderedArgDefIx = 0;

            initArgDefs();
            VirtualArray<string> vArgs = new VirtualArray<string>(args);
            while (vArgs.length > 0)
            {
                bool argConsumed = false;
                for (int i = 0; !argConsumed && i < labeledArgDefs.Count; i++)
                {
                    ArgDef def = labeledArgDefs[i];
                    argConsumed = def.consume(vArgs);

                    // test crap
                    if (argConsumed)
                    {
                        Console.WriteLine(string.Format("LArg '{0}' was consumed", def.name));
                    }
                    //

                    if (def.errorOccured())
                    {
                        Console.WriteLine("Parse error occurred - largs"); // TODO usage + error messages
                        vArgs.moveStart(vArgs.endIndexExclusive); // this exits the outer while loop
                    }
                }
                if (!argConsumed)
                {
                    if (orderedArgDefIx >= orderedArgDefs.Count)
                    {
                        Console.WriteLine(string.Format("Unable to find def for arg '{0}'", vArgs[0]));
                        break;
                    }

                    ArgDef oDef = orderedArgDefs[orderedArgDefIx];
                    argConsumed = oDef.consume(vArgs);
                    orderedArgDefIx++;

                    if (!argConsumed)
                    {
                        Console.WriteLine("Parse error occurred - oargs"); // TODO usage + error messages
                        vArgs.moveStart(vArgs.endIndexExclusive); // this exits the outer while loop
                    }

                    // test crap
                    else
                    {
                        Console.WriteLine(string.Format("OArg '{0}' was consumed", oDef.name));
                    }
                    //
                }
            }

            throw new NotImplementedException();
        }

        /*
         * helpers
         */
        private void initArgDefs()
        {
            foreach (ArgDef def in labeledArgDefs)
            {
                def.parseInit();
            }
            foreach (ArgDef def in orderedArgDefs)
            {
                def.parseInit();
            }
        }
    }
}
