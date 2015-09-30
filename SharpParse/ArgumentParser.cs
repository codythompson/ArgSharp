using System;
using System.Collections.Generic;

namespace SharpParse
{
    public class ArgumentParser
    {
        // options
        public bool printUsageOnInvalidArgs = true;
        public bool printErrorMessageOnInvalidArgs = true;
        //
        
        private List<ArgDef> labeledArgDefs;
        private List<ArgDef> orderedArgDefs;
        public Dictionary<Type, ArgTypeParser> typeParsers;

        public ArgumentParser()
        {
            labeledArgDefs = new List<ArgDef>();
            orderedArgDefs = new List<ArgDef>();
            typeParsers = new Dictionary<Type, ArgTypeParser>();
            foreach (KeyValuePair<Type, ArgTypeParser> kvp in ArgTypeParser.basicParsers)
            {
                typeParsers.Add(kvp.Key, kvp.Value);
            }
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
            // setup stuff
            ParsedArgs pArgs = new ParsedArgs();
            int orderedArgDefIx = 0;
            initArgDefs();
            VirtualArray<string> vArgs = new VirtualArray<string>(args);
            //

            
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
                        Console.WriteLine(string.Format("[TEMP DEBUG] LArg '{0}' was consumed", def.name));
                    }
                    //

                    if (def.errorOccured())
                    {
                        pArgs.addErrorMessages(def.getErrorMessages());
                        vArgs.moveStart(vArgs.endIndexExclusive); // this exits the outer while loop
                    }
                }
                if (!argConsumed)
                {
                    if (orderedArgDefIx >= orderedArgDefs.Count)
                    {
                        pArgs.addErrorMessage("Encountered more args than expected.");
                        break;
                    }

                    ArgDef oDef = orderedArgDefs[orderedArgDefIx];
                    argConsumed = oDef.consume(vArgs);
                    orderedArgDefIx++;

                    if (oDef.errorOccured())
                    {
                        pArgs.addErrorMessages(oDef.getErrorMessages());
                        vArgs.moveStart(vArgs.endIndexExclusive); // this exits the outer while loop
                    }
                    else if (!argConsumed)
                    {

                        pArgs.addErrorMessage(string.Format("Unable to use arg '{0}'", vArgs[0]));
                        vArgs.moveStart(vArgs.endIndexExclusive); // this exits the outer while loop
                    }

                    // test crap
                    else
                    {
                        Console.WriteLine(string.Format("[Temp Debug] OArg '{0}' was consumed", oDef.name));
                    }
                    //
                }
            } // end while

            if (pArgs.errorOccured())
            {
                if (printErrorMessageOnInvalidArgs)
                {
                    foreach (string message in pArgs.getErrorMessages())
                    {
                        Console.WriteLine(message);
                    }
                }
                // TODO print usage
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
                def.parseInit(typeParsers);
            }
            foreach (ArgDef def in orderedArgDefs)
            {
                def.parseInit(typeParsers);
            }
        }

        private void finishArgDefs()
        {
            foreach (ArgDef def in labeledArgDefs)
            {
                def.parseFinish();
            }
            foreach (ArgDef def in orderedArgDefs)
            {
                def.parseFinish();
            }
        }
    }
}
