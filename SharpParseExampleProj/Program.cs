using System;
using SharpParse;

namespace SharpParseExampleProj
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("args received: ");
            foreach (string arg in args)
            {
                Console.WriteLine(arg);
            }
            Console.WriteLine("------------");

            ArgDef optionA = new ArgDef();
            optionA.argLabels.Add("-a");
            optionA.argLabels.Add("-optiona");

            ArgDef optionB = new ArgDef();
            optionB.argLabels.Add("-y");
            optionB.argCount = 3;
            optionB.type = typeof(int);

            ArgDef orderedA = new ArgDef();
            orderedA.name = "orderedA";

            ArgumentParser argParser = new ArgumentParser("testprog");
            argParser.addArgDef(optionA);
            argParser.addArgDef(optionB);
            argParser.addArgDef(orderedA);

            ParsedArgs pArgs;
            try
            {
                pArgs = argParser.parseArgs(args);
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("[Temp debug] Caught Not Implemented Exception");
            }

            Console.ReadKey();
        }
    }
}
