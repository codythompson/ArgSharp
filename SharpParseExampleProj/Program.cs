using System;
using ArgSharp;

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
            optionB.argCount = 2;
            optionB.type = typeof(int);

            ArgDef orderedA = new ArgDef();
            orderedA.name = "orderedA";

            ArgDef orderedLast = new ArgDef();
            orderedLast.name = "last";

            ArgumentParser argParser = new ArgumentParser("testprog");
            argParser.addArgDef(optionA);
            argParser.addArgDef(optionB);
            argParser.addArgDef(orderedA);
            argParser.addArgDef(orderedLast);

            ParsedArgs pArgs = null;
            try
            {
                pArgs = argParser.parseArgs(args);
            }
            catch (Exception e)
            {
                object message = e.Message;
                if (e.Message == null) {
                    message = e;
                }
                Console.WriteLine("[Temp debug] Caught Exception '{0}'", message);
                Console.WriteLine("\n[Temp debug] Stack trace:\n");
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("\n[Temp debug] end stack trace.\n");
            }

            Console.WriteLine("\n[Temp debug] ParsedArgs:\n");
            Console.WriteLine(pArgs);
            Console.WriteLine("\n[Temp debug] end ParsedArgs:\n");

            Console.ReadKey();
        }
    }
}
