﻿using System;
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
        [TestCategory("ArgDef")]
        public void parseInitTestLabelName()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.AddRange(new string[] {"-t", "-test1", "-test22", "-test0"});
            testDef.parseInit(ArgTypeParser.basicParsers);

            Assert.AreEqual<string>("test22", testDef.name, "[ArgDef][parseInit] Wrong name chose from labels.");
        }

        [TestMethod]
        [TestCategory("ArgDef")]
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
        [TestCategory("ArgDef")]
        public void parseInitTestRequired()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "atest";
            testDef.parseInit(ArgTypeParser.basicParsers);

            Assert.IsTrue(testDef.required, "[ArgDef][parseInit] Ordered args should be required.");

            testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.parseInit(ArgTypeParser.basicParsers);

            Assert.IsFalse(testDef.required, "[ArgDef][parseInit] Labeled args should not be required unless required was explicitly set.");

            testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.required = true;

            Assert.IsTrue(testDef.required, "[ArgDef][parseInit] Labeled args should be required when required is explicitly set.");
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown if the value of argCount is less than zero.")]
        public void parseInitNegativeArgCountException()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.argCount = -1;
            testDef.parseInit(ArgTypeParser.basicParsers);
        }


        [TestMethod]
        [TestCategory("ArgDef")]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown as no type parser exists for the type FakeUnitTestType.")]
        public void parseInitNoTypeParserException()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "test";
            testDef.type = typeof(FakeUnitTestType);
            testDef.parseInit(ArgTypeParser.basicParsers);
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown when no label or name is provided before pareInit is called.")]
        public void parseInitNoNameException()
        {
            ArgDef testDef = new ArgDef();
            testDef.parseInit(ArgTypeParser.basicParsers);
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown when an ordered arg has a non zero argCount.")]
        public void parseInitOrderedArgCount1Exception()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "test";
            testDef.argCount = 1;
            testDef.parseInit(ArgTypeParser.basicParsers);
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown when an ordered arg has a non zero argCount.")]
        public void parseInitOrderedArgCountNeg1Exception()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "test";
            testDef.argCount = -1;
            testDef.parseInit(ArgTypeParser.basicParsers);
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown when an ordered arg has a non zero argCount.")]
        public void parseInitOrderedArgCountMaxIntException()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "test";
            testDef.argCount = int.MaxValue;
            testDef.parseInit(ArgTypeParser.basicParsers);
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown when an ordered arg has a non zero argCount.")]
        public void parseInitOrderedArgCountMinIntException()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "test";
            testDef.argCount = int.MinValue;
            testDef.parseInit(ArgTypeParser.basicParsers);
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        [ExpectedException(typeof(ArgDefBadOptionsException), "[ArgDef][parseInit] An exception should be thrown when a labeled arg with argCount equal to zero has a type of than bool or string (string is considered default and will be changed to bool in this case).")]
        public void parseInitOrderedArgCount0NonBoolException()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.type = typeof(double);
            testDef.parseInit(ArgTypeParser.basicParsers);
        }

        /*
         * The following test the private isConsumeable method via consume
         */
        [TestMethod]
        [TestCategory("ArgDef")]
        [ExpectedException(typeof(ArgDefException), "[ArgDef][consume] An exception should be thrown when the length of vArgs is less than 1")]
        public void consumeTestNoVArgs()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "test";
            testDef.parseInit(ArgTypeParser.basicParsers);
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[]{}, 0, 0);
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
        }
        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestFalseOnNoLabelMatch()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-test");
            testDef.parseInit(ArgTypeParser.basicParsers);
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] {"-nottest"}, 0, 1);
            ParsedArgs pArgs = new ParsedArgs();
            bool result = testDef.consume(vArgs, pArgs);
            Assert.IsFalse(result, "[ArgDef][consume] consume should return false when no label matches the input");
        }
        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestMultipleOptionEncounters()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.AddRange(new string[]{"-test", "-t"});
            testDef.parseInit(ArgTypeParser.basicParsers);
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] {"-test", "-t"}, 0, 2);
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            bool result2 = testDef.consume(vArgs, pArgs);
            bool errors = testDef.errorOccured();
            Assert.IsFalse(result2, "[ArgDef][consume] consume should return false when a label is encountered twice.");
            Assert.IsTrue(errors, "[ArgDef][consume] consume should generate an error message when a label is encountered twice.");
        }
        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestWrongNumberOfFollowingArgs()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-test");
            testDef.argCount = 2;
            testDef.parseInit(ArgTypeParser.basicParsers);
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { "-test", "a" }, 0, 2);
            ParsedArgs pArgs = new ParsedArgs();
            bool result = testDef.consume(vArgs, pArgs);
            bool errors = testDef.errorOccured();
            Assert.IsFalse(result, "[ArgDef][consume] consume should return false when there are less remaining args than argCount.");
            Assert.IsTrue(errors, "[ArgDef][consume] consume should generate an error message when there are less remaining args than argCount.");
        }
        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestLabelEncounteredWrongType()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-test");
            testDef.argCount = 1;
            testDef.type = typeof(int);
            testDef.parseInit(ArgTypeParser.basicParsers);
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { "-test", "a" }, 0, 2);
            ParsedArgs pArgs = new ParsedArgs();
            bool result = testDef.consume(vArgs, pArgs);
            bool errors = testDef.errorOccured();
            Assert.IsFalse(result, "[ArgDef][consume] consume should return false when the args provided don't match the arg def's type.");
            Assert.IsTrue(errors, "[ArgDef][consume] consume should generate an error when the args provided don't match the arg def's type.");
        }
        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestOrderedEncounteredWrongType()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "test";
            testDef.type = typeof(int);
            testDef.parseInit(ArgTypeParser.basicParsers);
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { "a" }, 0, 1);
            ParsedArgs pArgs = new ParsedArgs();
            bool result = testDef.consume(vArgs, pArgs);
            bool errors = testDef.errorOccured();
            Assert.IsFalse(result, "[ArgDef][consume] consume should return false when the args provided don't match the arg def's type.");
            Assert.IsTrue(errors, "[ArgDef][consume] consume should generate an error when the args provided don't match the arg def's type.");
        }

        /*
         * The following test the private getValue method via consume
         */
        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestFlag()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.parseInit(ArgTypeParser.basicParsers);
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { "-t" });
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            Assert.IsTrue(pArgs.containsKey("t"), "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
            Assert.IsTrue(pArgs.getValue<bool>("t"), "[ArgDef][consume] consume should set the appropriate value for the given arg name when encountered.");
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestLabeledBool()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.type = typeof(bool);
            testDef.argCount = 1;
            testDef.parseInit(ArgTypeParser.basicParsers);
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { "-t", "false" });
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            Assert.IsTrue(pArgs.containsKey("t"), "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
            Assert.IsFalse(pArgs.getValue<bool>("t"), "[ArgDef][consume] consume should set the appropriate value for the given arg name when encountered.");
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestLabeledBoolAsArray()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.type = typeof(bool);
            testDef.argCount = 1;
            testDef.createArrayForArgCount1 = true;
            testDef.parseInit(ArgTypeParser.basicParsers);
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { "-t", "false" });
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            Assert.IsTrue(pArgs.containsKey("t"), "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
            bool[] result = pArgs.getArray<bool>("t");
            Assert.AreEqual<int>(1, result.Length, "[ArgDef][consume] consume should create an array of size 1 when argCount is 1 and createArrayForArgCount1 is true");
            Assert.IsFalse(result[0], "[ArgDef][consume] consume should set the appropriate value for the given arg name when encountered.");
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestLabeledInt()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.type = typeof(int);
            testDef.argCount = 1;
            testDef.parseInit(ArgTypeParser.basicParsers);
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { "-t", "33" });
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            Assert.IsTrue(pArgs.containsKey("t"), "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
            Assert.AreEqual<int>(33, pArgs.getValue<int>("t"), "[ArgDef][consume] consume should set the appropriate value for the given arg name when encountered.");
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestLabeledDouble()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.type = typeof(double);
            testDef.argCount = 1;
            testDef.parseInit(ArgTypeParser.basicParsers);
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { "-t", "33.3" });
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            Assert.IsTrue(pArgs.containsKey("t"), "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
            Assert.AreEqual(33.3, pArgs.getValue<double>("t"), 0.01, "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestOrderedInt()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "test";
            testDef.type = typeof(int);
            testDef.parseInit(ArgTypeParser.basicParsers);
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { "33" });
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            Assert.IsTrue(pArgs.containsKey("test"), "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
            Assert.AreEqual<int>(33, pArgs.getValue<int>("test"), "[ArgDef][consume] consume should set the appropriate value for the given arg name when encountered.");
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestOrderedDouble()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "test";
            testDef.type = typeof(double);
            testDef.parseInit(ArgTypeParser.basicParsers);
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { "33.63" });
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            Assert.IsTrue(pArgs.containsKey("test"), "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
            Assert.AreEqual(33.63, pArgs.getValue<double>("test"), 0.01, "[ArgDef][consume] consume should set the appropriate value for the given arg name when encountered.");
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestListOfInt()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.type = typeof(int);
            testDef.argCount = 3;
            testDef.parseInit(ArgTypeParser.basicParsers);
            int[] testVals = new int[]{-234, 0, 234234};
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { "-t", testVals[0].ToString(), testVals[1].ToString(), testVals[2].ToString() });
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            Assert.IsTrue(pArgs.containsKey("t"), "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
            int[] result = pArgs.getArray<int>("t");
            int i = 0;
            foreach (int val in result)
            {
                Assert.AreEqual<int>(testVals[i], val, "[ArgDef][consume] consume resulted in an array with unexpected values.");
                i++;
            }
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestListOfDouble()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.type = typeof(double);
            testDef.argCount = 3;
            testDef.parseInit(ArgTypeParser.basicParsers);
            double[] testVals = new double[] { -234.556, 0, 234234.234 };
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { "-t", testVals[0].ToString(), testVals[1].ToString(), testVals[2].ToString() });
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            Assert.IsTrue(pArgs.containsKey("t"), "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
            double[] result = pArgs.getArray<double>("t");
            int i = 0;
            foreach (double val in result)
            {
                Assert.AreEqual<double>(testVals[i], val, "[ArgDef][consume] consume resulted in an array with unexpected values.");
                i++;
            }
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestListOfBool()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.type = typeof(bool);
            testDef.argCount = 2;
            testDef.parseInit(ArgTypeParser.basicParsers);
            bool[] testVals = new bool[] { false, true };
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { "-t", testVals[0].ToString(), testVals[1].ToString() });
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            Assert.IsTrue(pArgs.containsKey("t"), "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
            bool[] result = pArgs.getArray<bool>("t");
            int i = 0;
            foreach (bool val in result)
            {
                Assert.AreEqual<bool>(testVals[i], val, "[ArgDef][consume] consume resulted in an array with unexpected values.");
                i++;
            }
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestListOfString()
        {
            ArgDef testDef = new ArgDef();
            testDef.argLabels.Add("-t");
            testDef.argCount = 5;
            testDef.parseInit(ArgTypeParser.basicParsers);
            string[] testVals = new string[] { "ok", "", "what ", "yeyeah", "turn down" };
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { "-t", testVals[0].ToString(), testVals[1].ToString(), testVals[2].ToString(), testVals[3].ToString(), testVals[4].ToString() });
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            Assert.IsTrue(pArgs.containsKey("t"), "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
            string[] result = pArgs.getArray<string>("t");
            int i = 0;
            foreach (string val in result)
            {
                Assert.AreEqual<string>(testVals[i], val, "[ArgDef][consume] consume resulted in an array with unexpected values.");
                i++;
            }
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestRemainingDouble()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "t";
            testDef.type = typeof(double);
            testDef.argCountIsRemainderOfArgs = true;
            testDef.parseInit(ArgTypeParser.basicParsers);
            double[] testVals = new double[] { -234.556, 0, 234234.234 };
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { testVals[0].ToString(), testVals[1].ToString(), testVals[2].ToString() });
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            Assert.IsTrue(pArgs.containsKey("t"), "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
            double[] result = pArgs.getArray<double>("t");
            int i = 0;
            foreach (double val in result)
            {
                Assert.AreEqual<double>(testVals[i], val, "[ArgDef][consume] consume resulted in an array with unexpected values.");
                i++;
            }
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestRemainingInt()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "t";
            testDef.type = typeof(int);
            testDef.argCountIsRemainderOfArgs = true;
            testDef.parseInit(ArgTypeParser.basicParsers);
            int[] testVals = new int[] { -234, 0, 234234};
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { testVals[0].ToString(), testVals[1].ToString(), testVals[2].ToString() });
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            Assert.IsTrue(pArgs.containsKey("t"), "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
            int[] result = pArgs.getArray<int>("t");
            int i = 0;
            foreach (int val in result)
            {
                Assert.AreEqual<int>(testVals[i], val, "[ArgDef][consume] consume resulted in an array with unexpected values.");
                i++;
            }
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestRemainingBool()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "t";
            testDef.type = typeof(bool);
            testDef.argCountIsRemainderOfArgs = true;
            testDef.parseInit(ArgTypeParser.basicParsers);
            bool[] testVals = new bool[] { true, false, true };
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { testVals[0].ToString(), testVals[1].ToString(), testVals[2].ToString() });
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            Assert.IsTrue(pArgs.containsKey("t"), "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
            bool[] result = pArgs.getArray<bool>("t");
            int i = 0;
            foreach (bool val in result)
            {
                Assert.AreEqual<bool>(testVals[i], val, "[ArgDef][consume] consume resulted in an array with unexpected values.");
                i++;
            }
        }

        [TestMethod]
        [TestCategory("ArgDef")]
        public void consumeTestRemainingString()
        {
            ArgDef testDef = new ArgDef();
            testDef.name = "t";
            testDef.argCountIsRemainderOfArgs = true;
            testDef.parseInit(ArgTypeParser.basicParsers);
            string[] testVals = new string[] { "what", "the", "french toast" };
            VirtualArray<string> vArgs = new VirtualArray<string>(new string[] { testVals[0].ToString(), testVals[1].ToString(), testVals[2].ToString() });
            ParsedArgs pArgs = new ParsedArgs();
            testDef.consume(vArgs, pArgs);
            Assert.IsTrue(pArgs.containsKey("t"), "[ArgDef][consume] consume should add a value to the passed in ParsedArgs when the appropriate args are given");
            string[] result = pArgs.getArray<string>("t");
            int i = 0;
            foreach (string val in result)
            {
                Assert.AreEqual<string>(testVals[i], val, "[ArgDef][consume] consume resulted in an array with unexpected values.");
                i++;
            }
        }
    }

    public class FakeUnitTestType {}
}
