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
            Assert.AreEqual(0, testArray.length, "[VirtualArray][length] Unexpected length, expected length 0");

            testArray = makeVirtArray(1);
            Assert.AreNotEqual(0, testArray.length, "[VirtualArray][length] Unexpected length, did not expect length 0");

            testArray.moveStartBy(1);
            Assert.AreEqual(0, testArray.length, "[VirtualArray][length] Unexpected length, expected length 0");

            testArray = makeVirtArray(2);
            Assert.AreNotEqual(0, testArray.length, "[VirtualArray][length] Unexpected length, did not expect length 0");

            testArray.moveStartBy(2);
            Assert.AreEqual(0, testArray.length, "[VirtualArray][length] Unexpected length, expected length 0");
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        public void legthTest1()
        {
            VirtualArray<object> testArray = makeVirtArray(1);
            Assert.AreEqual(1, testArray.length, "[VirtualArray][length] Unexpected length, expected length 1");

            testArray = makeVirtArray(2);
            Assert.AreNotEqual(1, testArray.length, "[VirtualArray][length] Unexpected length, did not expect length 1");

            testArray.moveStartBy(1);
            Assert.AreEqual(1, testArray.length, "[VirtualArray][length] Unexpected length, expected length 1");
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        public void legthTest2()
        {
            VirtualArray<object> testArray = makeVirtArray(2);
            Assert.AreEqual(2, testArray.length, "[VirtualArray][length] Unexpected length, expected length 2");

            testArray.moveStartBy(1);
            Assert.AreNotEqual(2, testArray.length, "[VirtualArray][length] Unexpected length, did not expect length 2");
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        public void resizeTest()
        {
            VirtualArray<object> testArray = makeVirtArray(2);
            testArray.resize(1, 2);
            Assert.AreEqual(1, testArray.length, "[VirtualArray][resize] Expected length to be 1");
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        [ExpectedException(typeof(IndexOutOfRangeException), "[VirtualArray][resize] An Exception should be thrown when the start index is greater than the end index.")]
        public void resizeTestGreaterThanEnd()
        {
            VirtualArray<object> testArray = makeVirtArray(3);
            testArray.resize(2, 1);
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        [ExpectedException(typeof(IndexOutOfRangeException), "[VirtualArray][resize] An Exception should be thrown when the start index is less than 0.")]
        public void resizeTestLessThan0()
        {
            VirtualArray<object> testArray = makeVirtArray(3);
            testArray.resize(-2, -1);
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        [ExpectedException(typeof(IndexOutOfRangeException), "[VirtualArray][resize] An Exception should be thrown when the end index is greater than the length of the base array.")]
        public void resizeTestGreaterThanBase()
        {
            VirtualArray<object> testArray = makeVirtArray(3);
            testArray.resize(0, 4);
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        public void isInBoundsTest()
        {
            VirtualArray<object> testArray = makeVirtArray(3);
            Assert.IsFalse(testArray.isInBounds(-1), "[VirtualArray][isInBounds] Should return false for values less than 0");
            Assert.IsFalse(testArray.isInBounds(-10), "[VirtualArray][isInBounds] Should return false for values less than 0");
            Assert.IsFalse(testArray.isInBounds(3), "[VirtualArray][isInBounds] Should return false for values greater than or equal to the length of the array");
            Assert.IsFalse(testArray.isInBounds(4), "[VirtualArray][isInBounds] Should return false for values greater than or equal to the length of the array");
            Assert.IsFalse(testArray.isInBounds(400), "[VirtualArray][isInBounds] Should return false for values greater than or equal to the length of the array");
            Assert.IsTrue(testArray.isInBounds(0), "[VirtualArray][isInBounds] Should return true for values greater than or equal to 0 and less than the length of the array");
            Assert.IsTrue(testArray.isInBounds(1), "[VirtualArray][isInBounds] Should return true for values greater than or equal to 0 and less than the length of the array");
            Assert.IsTrue(testArray.isInBounds(2), "[VirtualArray][isInBounds] Should return true for values greater than or equal to 0 and less than the length of the array");
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        public void getTest()
        {
            int[] ints = new int[3] { 0, 1, 2 };
            VirtualArray<int> testArray = new VirtualArray<int>(ints);
            Assert.AreEqual(0, testArray[0], string.Format("[VirtualArray][get] expected {0}, received {1}", 0, testArray[0]));
            Assert.AreEqual(1, testArray[1], string.Format("[VirtualArray][get] expected {0}, received {1}", 1, testArray[1]));
            Assert.AreEqual(2, testArray[2], string.Format("[VirtualArray][get] expected {0}, received {1}", 2, testArray[2]));

            testArray.moveStartBy(1);
            Assert.AreEqual(1, testArray[0], string.Format("[VirtualArray][get] expected {0}, received {1}", 1, testArray[0]));
            Assert.AreEqual(2, testArray[1], string.Format("[VirtualArray][get] expected {0}, received {1}", 2, testArray[1]));

            testArray.moveStartBy(1);
            Assert.AreEqual(2, testArray[0], string.Format("[VirtualArray][get] expected {0}, received {1}", 2, testArray[0]));
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        [ExpectedException(typeof (IndexOutOfRangeException), "[VirtualArray][get] an IndexOutOfRange exception should be thrown when the index is not in range of the current state of the Array")]
        public void getTestOutOfBoundsPositive()
        {
            VirtualArray<object> testArray = makeVirtArray(2);
            testArray.moveEndBy(-1);
            object obj = testArray[1];
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        [ExpectedException(typeof(IndexOutOfRangeException), "[VirtualArray][get] an IndexOutOfRange exception should be thrown when the index is not in range of the current state of the Array")]
        public void getTestOutOfBoundsNegative()
        {
            VirtualArray<object> testArray = makeVirtArray(2);
            testArray.moveStartBy(1);
            object obj = testArray[-1];
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        public void setTest()
        {
            int[] ints = new int[3] { 0, 1, 2 };
            VirtualArray<int> testArray = new VirtualArray<int>(ints);

            testArray[0] = -77;
            testArray[1] = -88;
            testArray[2] = -99;

            Assert.AreEqual(-77, testArray[0], string.Format("[VirtualArray][set] expected {0}, received {1}", -77, testArray[0]));
            Assert.AreEqual(-88, testArray[1], string.Format("[VirtualArray][set] expected {0}, received {1}", -88, testArray[1]));
            Assert.AreEqual(-99, testArray[2], string.Format("[VirtualArray][set] expected {0}, received {1}", -99, testArray[2]));

            testArray.moveStartBy(1);
            testArray[0] += 88;
            Assert.AreEqual(0, testArray[0], string.Format("[VirtualArray][set] expected {0}, received {1}", 0, testArray[0]));
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        [ExpectedException(typeof(IndexOutOfRangeException), "[VirtualArray][set] an IndexOutOfRange exception should be thrown when the index is not in range of the current state of the Array")]
        public void setTestOutOfBoundsPositive()
        {
            VirtualArray<object> testArray = makeVirtArray(2);
            testArray.moveEndBy(-1);
            testArray[1] = null;
        }

        [TestMethod]
        [TestCategory("VirtualArray")]
        [ExpectedException(typeof(IndexOutOfRangeException), "[VirtualArray][set] an IndexOutOfRange exception should be thrown when the index is not in range of the current state of the Array")]
        public void setTestOutOfBoundsNegative()
        {
            VirtualArray<object> testArray = makeVirtArray(2);
            testArray.moveStartBy(1);
            testArray[-1] = null;
        }
    }
}