using System;
using ArgSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArgSharpTests
{
    [TestClass]
    public class ArgTypeParserTests
    {
        [TestMethod]
        [TestCategory("ArgTypeParserTests")]
        public void stringParserTest()
        {
            StringParser parser = new StringParser();
            object parseResult;
            Assert.IsTrue(parser.tryConvert("!@#!@#98798okok", out parseResult), "[StringParser][tryConvert] Expected a successful parse.");
            Assert.AreEqual("!@#!@#98798okok", (string)parseResult, "[StringParser][tryConvert] Unexpected parse output.");
            Assert.IsFalse(parser.tryConvert(null, out parseResult), "[StringParser][tryConvert] Expected an unsuccessful parse.");
            Assert.IsNull(parseResult, "[StringParser][tryConvert] Expected the output to be null after an unsuccessful parse.");
        }
    }
}