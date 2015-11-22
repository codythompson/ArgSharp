using System;
using ArgSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArgSharpTests
{
    [TestClass]
    public class SystemWideTests
    {
        public static ArgDef buildOptionalString(string label)
        {
            ArgDef ad = new ArgDef();
            ad.argLabels.Add(label);
            return ad;
        }

        [TestMethod]
        [TestCategory("SystemWide")]
        public void noArgsTest()
        {
            ArgumentParser argParser = new ArgumentParser("test_prog");
            ArgDef testArg = buildOptionalString("-t");
            argParser.addArgDef(testArg);
            ParsedArgs pArgs = argParser.parseArgs(new string[] {});
            Assert.AreEqual(0, pArgs.count, "Expected the parsed args to have a count of 0");

            testArg.useDefaultIfNull = true;
            pArgs = argParser.parseArgs(new string[] { });
            Assert.AreEqual(1, pArgs.count, "Expected the parsed args to have a count of 1");
        }
    }
}