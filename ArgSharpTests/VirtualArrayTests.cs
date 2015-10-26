using System;
using ArgSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArgSharpTests
{
    [TestClass]
    public class VirtualArrayTests
    {
        public static VirtualArray<object> makeVirtArray(int baseArrayLength)
        {
            object[] objArr = new object[baseArrayLength];
            return new VirtualArray<object>(objArr);
        }


        // TODO length tests that make use of moveEndBy
        [TestMethod]
        [TestCategory("VirtualArray")]
        public void legthTest0()
        {
            VirtualArray<object> testArray = makeVirtArray(0);
            Assert.AreEqual(0, testArray.length, "Unexpected length, expected length 0");

            testArray = makeVirtArray(1);
            Assert.AreNotEqual(0, testArray.length, "Unexpected length, did not expect length 0");

            testArray.moveStartBy(1);
            Assert.AreEqual(0, testArray.length, "Unexpected length, expected length 0");

            testArray = makeVirtArray(2);
            Assert.AreNotEqual(0, testArray.length, "Unexpected length, did not expect length 0");

            testArray.moveStartBy(2);
            Assert.AreEqual(0, testArray.length, "Unexpected length, expected length 0");
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        public void legthTest1()
        {
            VirtualArray<object> testArray = makeVirtArray(1);
            Assert.AreEqual(1, testArray.length, "Unexpected length, expected length 1");

            testArray = makeVirtArray(2);
            Assert.AreNotEqual(1, testArray.length, "Unexpected length, did not expect length 1");

            testArray.moveStartBy(1);
            Assert.AreEqual(1, testArray.length, "Unexpected length, expected length 1");
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        public void legthTest2()
        {
            VirtualArray<object> testArray = makeVirtArray(2);
            Assert.AreEqual(2, testArray.length, "Unexpected length, expected length 2");

            testArray.moveStartBy(1);
            Assert.AreNotEqual(2, testArray.length, "Unexpected length, did not expect length 2");
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        public void resizeTest()
        {
            VirtualArray<object> testArray = makeVirtArray(2);
            testArray.resize(1, 2);
            Assert.AreEqual(1, testArray.length, "Expected length to be 1");
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void resizeTestGreaterThanEnd()
        {
            VirtualArray<object> testArray = makeVirtArray(3);
            testArray.resize(2, 1);
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void resizeTestLessThan0()
        {
            VirtualArray<object> testArray = makeVirtArray(3);
            testArray.resize(-2, -1);
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void resizeTestGreaterThanBase()
        {
            VirtualArray<object> testArray = makeVirtArray(3);
            testArray.resize(0, 4);
        }
    }
}