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

        [TestMethod]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown if the value of argCount is less than zero.")]
        public void parseInitNegativeArgCountException()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.argCount = -1;
            testDef.parseInit(ArgTypeParser.basicParsers);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown as no type parser exists for the type FakeUnitTestType.")]
        public void parseInitNoTypeParserException()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "test";
            testDef.type = typeof(FakeUnitTestType);
            testDef.parseInit(ArgTypeParser.basicParsers);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown when no label or name is provided before pareInit is called.")]
        public void parseInitNoNameException()
        {
            ArgDef testDef = new ArgDef();
            testDef.parseInit(ArgTypeParser.basicParsers);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown when an ordered arg has a non zero argCount.")]
        public void parseInitOrderedArgCount1Exception()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "test";
            testDef.argCount = 1;
            testDef.parseInit(ArgTypeParser.basicParsers);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown when an ordered arg has a non zero argCount.")]
        public void parseInitOrderedArgCountNeg1Exception()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "test";
            testDef.argCount = -1;
            testDef.parseInit(ArgTypeParser.basicParsers);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown when an ordered arg has a non zero argCount.")]
        public void parseInitOrderedArgCountMaxIntException()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "test";
            testDef.argCount = int.MaxValue;
            testDef.parseInit(ArgTypeParser.basicParsers);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown when an ordered arg has a non zero argCount.")]
        public void parseInitOrderedArgCountMinIntException()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "test";
            testDef.argCount = int.MinValue;
            testDef.parseInit(ArgTypeParser.basicParsers);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown when a labeled arg with argCount equal to zero has a type of than bool or string (string is considered default and will be changed to bool in this case).")]
        public void parseInitOrderedArgCount0NonBoolException()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.type = typeof(double);
            testDef.parseInit(ArgTypeParser.basicParsers);
        }
    }

    public class FakeUnitTestType {}
}
