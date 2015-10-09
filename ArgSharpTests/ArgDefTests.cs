using System;
using ArgSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArgSharpTests
{
    [TestClass]
    public class ArgDefTests
    {
        /*
         * The following test the parseInit method
         */
        [TestMethod]
        public void parseInitTestLabelName()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.AddRange(new string[] {"-t", "-test1", "-test22", "-test0"});
            testDef.parseInit(ArgTypeParser.basicParsers);

            Assert.AreEqual<string>("test22", testDef.name, "[ArgDef][parseInit] Wrong name chose from labels.");
        }

        [TestMethod]
        public void parseInitTestType()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.AddRange(new string[] { "-t", "-test1", "-test22", "-test0" });
            testDef.parseInit(ArgTypeParser.basicParsers);

            Assert.AreEqual<Type>(typeof(bool), testDef.type, "[ArgDef][parseInit] Expecting type bool for a labeled arg of argCount 0.");

            testDef = new ArgDef();
            testDef.argLabels.AddRange(new string[] { "-t", "-test1", "-test22", "-test0" });
            testDef.argCount++;
            testDef.parseInit(ArgTypeParser.basicParsers);

            Assert.AreEqual<Type>(typeof(string), testDef.type, "[ArgDef][parseInit] Expecting type string as the type for a labeled arg of argCount > 0 with no specified type.");

            testDef = new ArgDef();
            testDef.argLabels.AddRange(new string[] { "-t", "-test1", "-test22", "-test0" });
            testDef.argCount++;
            testDef.type = typeof(double);
            testDef.parseInit(ArgTypeParser.basicParsers);

            Assert.AreEqual(typeof(double), testDef.type, "[ArgDef][parseInit] Type was set to double, but the type after parseInit was not double.");
        }

        [TestMethod]
        public void parseInitTestRequired()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "atest";
            testDef.parseInit(ArgTypeParser.basicParsers);

            Assert.AreEqual<bool>(true, testDef.required, "[ArgDef][parseInit] Ordered args should be required.");

            testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.parseInit(ArgTypeParser.basicParsers);

            Assert.AreEqual<bool>(false, testDef.required, "[ArgDef][parseInit] Labeled args should not be required unless required was explicitly set.");

            testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.required = true;

            Assert.AreEqual<bool>(true, testDef.required, "[ArgDef][parseInit] Labeled args should be required when required is explicitly set.");
        }
    }
}
