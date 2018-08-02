using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using whkd;

namespace whkd.Tests
{
    [TestClass]
    public class TestKeyChord
    {
        [TestMethod]
        public void TestEquality()
        {
            var k1 = new KeyChord(1);
            Assert.AreEqual(k1, k1);
            var k2 = new KeyChord(1);
            Assert.AreEqual(k1, k2);
            k2.modifiers.Add(Modifier.Alt);
            k1.modifiers.Add(Modifier.Alt);
            Assert.AreEqual(k1, k2);
        }

        [TestMethod]
        public void TestInequality()
        {
            var k1 = new KeyChord(1);
            var k2 = new KeyChord(1);
            k2.modifiers.Add(Modifier.Alt);
            Assert.AreNotEqual(k1, k2);
            var k3 = new KeyChord(2);
            Assert.AreNotEqual(k1, k3);
            Assert.AreNotEqual(k2, k3);
        }
    }
}
