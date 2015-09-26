using System;
using SharpParse;

namespace SharpParseExampleProj
{
    class Program
    {
        static void Main(string[] args)
        {
            ArgDef optionA = new ArgDef();
            optionA.argLabels.Add("-a");
            optionA.argLabels.Add("-optiona");

            ArgDef orderedA = new ArgDef();
            orderedA.name = "orderedA";

            ArgumentParser argParser = new ArgumentParser();
            argParser.addArgDef(optionA);
            argParser.addArgDef(orderedA);

            ParsedArgs pArgs;
            try
            {
                pArgs = argParser.parseArgs(args);
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("Caught Not Implemented Exception");
            }
        }
    }
}
