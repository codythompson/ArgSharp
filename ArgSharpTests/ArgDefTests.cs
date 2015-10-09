using System;
using ArgSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArgSharpTests
{
    [TestClass]
    public class ArgDefTests
    {
        [TestMethod]
        public void parseInitTestLabelName()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.AddRange(new string[] {"-t", "-test1", "-test22", "-test0"});
            testDef.parseInit(ArgTypeParser.basicParsers);

            Assert.AreEqual<string>("test22", testDef.name, "Wrong name chose from labels.");
        }

        [TestMethod]
        public void parseInitTestType()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.AddRange(new string[] { "-t", "-test1", "-test22", "-test0" });
            testDef.parseInit(ArgTypeParser.basicParsers);

            Assert.AreEqual<Type>(typeof(bool), testDef.type, "Expecting type bool for a labeled arg of argCount 0.");

            testDef = new ArgDef();
            testDef.argLabels.AddRange(new string[] { "-t", "-test1", "-test22", "-test0" });
            testDef.argCount++;
            testDef.parseInit(ArgTypeParser.basicParsers);

            Assert.AreEqual<Type>(typeof(string), testDef.type, "Expecting type string as the type for a labeled arg of argCount > 0 with no specified type.");

            testDef = new ArgDef();
            testDef.argLabels.AddRange(new string[] { "-t", "-test1", "-test22", "-test0" });
            testDef.argCount++;
            testDef.type = typeof(double);
            testDef.parseInit(ArgTypeParser.basicParsers);

            Assert.AreEqual(typeof(double), testDef.type, "Type was set to double, but the type after parseInit was not double.");
        }
    }
}
