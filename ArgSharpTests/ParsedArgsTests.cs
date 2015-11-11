using System;
using System.Collections.Generic;
using ArgSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArgSharpTests
{
    [TestClass]
    public class ParsedArgsTests
    {
        [TestMethod]
        [TestCategory("ParsedArgs")]
        public void addErrorMessageTest()
        {
            ParsedArgs args = new ParsedArgs();
            args.addErrorMessage("Test Error Message");
            Assert.AreEqual<string>("Test Error Message", args.getErrorMessages()[0], "[ParsedArgs][addErrorMessage] Unexpected error message encountered.");
        }

        [TestMethod]
        [TestCategory("ParsedArgs")]
        public void addErrorMessagesTest()
        {
            ParsedArgs args = new ParsedArgs();
            args.addErrorMessages(new List<string> {"Test Error Message 1", "Test Error Message 2"});
            Assert.AreEqual<string>("Test Error Message 1", args.getErrorMessages()[0], "[ParsedArgs][addErrorMessages] Unexpected error message encountered.");
            Assert.AreEqual<string>("Test Error Message 2", args.getErrorMessages()[1], "[ParsedArgs][addErrorMessages] Unexpected error message encountered.");
        }

        [TestMethod]
        [TestCategory("ParsedArgs")]
        public void errorOccuredTest()
        {
            ParsedArgs args = new ParsedArgs();
            Assert.IsFalse(args.errorOccured(), "[ParsedArgs][errorOccured] Expected false.");
            args.addErrorMessage("Test message");
            Assert.IsTrue(args.errorOccured(), "[ParsedArgs][errorOccured] Expected true.");
        }

        [TestMethod]
        [TestCategory("ParsedArgs")]
        public void containsKeyTest()
        {
            ParsedArgs args = new ParsedArgs();
            Assert.IsFalse(args.containsKey("test"), "[ParsedArgs][containsKey] Expected false.");
            args.add("test", "test");
            Assert.IsTrue(args.containsKey("test"), "[ParsedArgs][containsKey] Expected true.");
        }

        [TestMethod]
        [TestCategory("ParsedArgs")]
        [ExpectedException(typeof(ParsedArgsKeyNotFoundException), "[ParsedArgs][getValue] Expected a ParsedArgsKeyNotFoundException to be thrown")]
        public void getValueTestKeyNotFound()
        {
            ParsedArgs args = new ParsedArgs();
            args.add("not this", "blah blah");
            args.getValue<string>("yes this please");
        }

        [TestMethod]
        [TestCategory("ParsedArgs")]
        [ExpectedException(typeof(ParsedArgsWrongTypeException), "[ParsedArgs][getValue] Expected a ParsedArgsWrongTypeException to be thrown")]
        public void getValueTestWrongType()
        {
            ParsedArgs args = new ParsedArgs();
            args.add("a key", "whaaat, this isn't an int");
            args.getValue<int>("a key");
        }

        [TestMethod]
        [TestCategory("ParsedArgs")]
        public void getStringTest()
        {
            ParsedArgs args = new ParsedArgs();
            args.add("blah", "blah blah");
            args.add("mmk", "");
            Assert.AreEqual("blah blah", args.getString("blah"), "[ParsedArgs][getString] Encountered unexpected return value.");
            Assert.AreEqual("", args.getString("mmk"), "[ParsedArgs][getString] Encountered unexpected return value.");
        }

        [TestMethod]
        [TestCategory("ParsedArgs")]
        public void geIntTest()
        {
            ParsedArgs args = new ParsedArgs();
            args.add("blah", 2123123);
            args.add("mmk", 0);
            Assert.AreEqual(2123123, args.getInt("blah"), "[ParsedArgs][getInt] Encountered unexpected return value.");
            Assert.AreEqual(0, args.getInt("mmk"), "[ParsedArgs][getInt] Encountered unexpected return value.");
        }

        [TestMethod]
        [TestCategory("ParsedArgs")]
        public void getDoubleTest()
        {
            ParsedArgs args = new ParsedArgs();
            args.add("blah", 21.23123);
            args.add("mmk", 0.0);
            Assert.AreEqual(21.23123, args.getDouble("blah"), "[ParsedArgs][getDouble] Encountered unexpected return value.");
            Assert.AreEqual(0.0, args.getDouble("mmk"), "[ParsedArgs][getDouble] Encountered unexpected return value.");
        }

        [TestMethod]
        [TestCategory("ParsedArgs")]
        [ExpectedException(typeof(ParsedArgsWrongTypeException), "[ParsedArgs][getArray] Expected a Wrong Type exception when accessing a string array using int as the type parameter.")]
        public void getArrayTestWrongType()
        {
            ParsedArgs args = new ParsedArgs();
            args.add("crazy", new string[] {"wut", "ok"});
            args.getArray<int>("crazy");
        }

        [TestMethod]
        [TestCategory("ParsedArgs")]
        [ExpectedException(typeof(ParsedArgsKeyNotFoundException), "[ParsedArgs][getArray] Expected a key not found exception.")]
        public void getArrayTestKeyNotFound()
        {
            ParsedArgs args = new ParsedArgs();
            args.getArray<int>("nooope");
        }

        private void AssertAreEqual<T>(T[] expected, T[] actual, string message)
        {
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual<T>(expected[i], actual[i], message);
            }
        }

        [TestMethod]
        [TestCategory("ParsedArgs")]
        public void getArrayTest()
        {
            ParsedArgs args = new ParsedArgs();
            args.add("crazy", new string[] { "wut", "ok" });
            args.add("ummm yeah", new int[] { -234, 23, 0});
            args.add("if you say so", new double[] { -234.542, 23.333333, 0.0 });

            AssertAreEqual<string>(new string[] { "wut", "ok" }, args.getArray<string>("crazy"), "[ParsedArgs][getArray] Encountered unexpected return value.");
            AssertAreEqual<int>(new int[] { -234, 23, 0}, args.getArray<int>("ummm yeah"), "[ParsedArgs][getArray] Encountered unexpected return value.");
            AssertAreEqual<double>(new double[] { -234.542, 23.333333, 0.0 }, args.getArray<double>("if you say so"), "[ParsedArgs][getArray] Encountered unexpected return value.");
        }

        [TestMethod]
        [TestCategory("ParsedArgs")]
        public void getArrayTestBizzaro()
        {
            ParsedArgs args = new ParsedArgs();
            args.add("weirdy weirdy 1", new object[] { "wut", "ok" });
            args.add("weirdy weirdy 2", new object[] { -234, 23, 0 });
            args.add("weirdy weirdy 3", new object[] { -234.542, 23.333333, 0.0 });
            AssertAreEqual<string>(new string[] { "wut", "ok" }, args.getArray<string>("weirdy weirdy 1"), "[ParsedArgs][getArray] Encountered unexpected return value.");
            AssertAreEqual<int>(new int[] { -234, 23, 0 }, args.getArray<int>("weirdy weirdy 2"), "[ParsedArgs][getArray] Encountered unexpected return value.");
            AssertAreEqual<double>(new double[] { -234.542, 23.333333, 0.0 }, args.getArray<double>("weirdy weirdy 3"), "[ParsedArgs][getArray] Encountered unexpected return value.");
        }
    }
}