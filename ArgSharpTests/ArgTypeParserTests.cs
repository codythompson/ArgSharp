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

        [TestMethod]
        [TestCategory("ArgTypeParserTests")]
        public void intParserTest()
        {
            IntParser parser = new IntParser();
            object parseResult;
            Assert.IsTrue(parser.tryConvert("-7979", out parseResult), "[IntParser][tryConvert] Expected a successful parse.");
            Assert.AreEqual(-7979, (int)parseResult, "[IntParser][tryConvert] Unexpected parse output.");
            Assert.IsFalse(parser.tryConvert(null, out parseResult), "[IntParser][tryConvert] Expected an unsuccessful parse.");
            Assert.IsNull(parseResult, "[IntParser][tryConvert] Expected the output to be null after an unsuccessful parse.");
            Assert.IsFalse(parser.tryConvert("lkajslkdfjasdf908ulkjlk", out parseResult), "[IntParser][tryConvert] Expected an unsuccessful parse.");
            Assert.IsNull(parseResult, "[IntParser][tryConvert] Expected the output to be null after an unsuccessful parse.");
        }

        [TestMethod]
        [TestCategory("ArgTypeParserTests")]
        public void doubleParserTest()
        {
            DoubleParser parser = new DoubleParser();
            object parseResult;
            Assert.IsTrue(parser.tryConvert("-7979.98", out parseResult), "[DoubleParser][tryConvert] Expected a successful parse.");
            Assert.AreEqual(-7979.98, (double)parseResult, "[DoubleParser][tryConvert] Unexpected parse output.");
            Assert.IsTrue(parser.tryConvert("89", out parseResult), "[DoubleParser][tryConvert] Expected a successful parse.");
            Assert.AreEqual(89, (double)parseResult, "[DoubleParser][tryConvert] Unexpected parse output.");
            Assert.IsFalse(parser.tryConvert(null, out parseResult), "[DoubleParser][tryConvert] Expected an unsuccessful parse.");
            Assert.IsNull(parseResult, "[DoubleParser][tryConvert] Expected the output to be null after an unsuccessful parse.");
            Assert.IsFalse(parser.tryConvert("lkajslkdfjasdf908ulkjlk", out parseResult), "[DoubleParser][tryConvert] Expected an unsuccessful parse.");
            Assert.IsNull(parseResult, "[DoubleParser][tryConvert] Expected the output to be null after an unsuccessful parse.");
        }

        [TestMethod]
        [TestCategory("ArgTypeParserTests")]
        public void boolParserTest()
        {
            BoolParser parser = new BoolParser();
            object parseResult;
            Assert.IsTrue(parser.tryConvert("true", out parseResult), "[BoolParser][tryConvert] Expected a successful parse.");
            Assert.IsTrue((bool)parseResult, "[BoolParser][tryConvert] Unexpected parse output.");
            Assert.IsTrue(parser.tryConvert("True", out parseResult), "[BoolParser][tryConvert] Expected a successful parse.");
            Assert.IsTrue((bool)parseResult, "[BoolParser][tryConvert] Unexpected parse output.");
            Assert.IsTrue(parser.tryConvert("TRUE", out parseResult), "[BoolParser][tryConvert] Expected a successful parse.");
            Assert.IsTrue((bool)parseResult, "[BoolParser][tryConvert] Unexpected parse output.");
            Assert.IsTrue(parser.tryConvert("false", out parseResult), "[BoolParser][tryConvert] Expected a successful parse.");
            Assert.IsFalse((bool)parseResult, "[BoolParser][tryConvert] Unexpected parse output.");
            Assert.IsTrue(parser.tryConvert("False", out parseResult), "[BoolParser][tryConvert] Expected a successful parse.");
            Assert.IsFalse((bool)parseResult, "[BoolParser][tryConvert] Unexpected parse output.");
            Assert.IsTrue(parser.tryConvert("FALSE", out parseResult), "[BoolParser][tryConvert] Expected a successful parse.");
            Assert.IsFalse((bool)parseResult, "[BoolParser][tryConvert] Unexpected parse output.");
            Assert.IsFalse(parser.tryConvert(null, out parseResult), "[BoolParser][tryConvert] Expected an unsuccessful parse.");
            Assert.IsNull(parseResult, "[BoolParser][tryConvert] Expected the output to be null after an unsuccessful parse.");
            Assert.IsFalse(parser.tryConvert("lkjlkj", out parseResult), "[BoolParser][tryConvert] Expected an unsuccessful parse.");
            Assert.IsNull(parseResult, "[BoolParser][tryConvert] Expected the output to be null after an unsuccessful parse.");
            Assert.IsFalse(parser.tryConvert("", out parseResult), "[BoolParser][tryConvert] Expected an unsuccessful parse.");
            Assert.IsNull(parseResult, "[BoolParser][tryConvert] Expected the output to be null after an unsuccessful parse.");
        }
    }
}