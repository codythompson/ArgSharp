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
    }
}