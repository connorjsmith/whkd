using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using whkd;

namespace whkd.Tests
{
    [TestClass]
    public class TestMergeError
    {
        private string command1 = "distinctive_command_name_123";
        private string command2 = "distinctive_command_name_321";
        [TestMethod]
        public void TestErrorMessage()
        {
            var mergeException = whkd.MergeError.Create(command1, command2);
            Assert.IsInstanceOfType(mergeException, typeof(System.Exception));
            try
            {
                throw mergeException;
            }
            catch (Exception e)
            {
                // Exception message is somewhat distinct and mentions the commands
                Assert.IsTrue(e.Message.Contains(command1) && e.Message.Contains(command2));
            }
        }
    }
}
