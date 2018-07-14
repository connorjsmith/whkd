using System;
using System.Runtime.Serialization;

namespace whkd
{
    public class MergeError : Exception
    {
        private MergeError(string message) : base(message)
        {
        }

        public static MergeError Create(string leftCommand, string rightCommand)
        {
            return new MergeError(String.Format("Conflicting command assignments for same node.\nFirst Assignment:\n\t{0}\nSecond Assignment\n\t{1}", leftCommand, rightCommand));
        }
    }
}