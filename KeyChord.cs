using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace whkd
{
    public enum Modifiers
    {
        Shift,
        Alt,
        Ctrl,
        Windows,
        Meta,
        Hyper
    }
    public class KeyChord : IEquatable<KeyChord>
    {
        public ICollection<Modifiers> modifiers = new HashSet<Modifiers>();
        public uint terminalKeyCode;

        #region Operators
        public override bool Equals(object obj)
        {
            return obj is KeyChord && Equals((KeyChord)obj);
        }

        public bool Equals(KeyChord other)
        {
            return terminalKeyCode == other.terminalKeyCode
                && !modifiers.Except(other.modifiers).Any()
                && !other.modifiers.Except(modifiers).Any();
        }

        public override int GetHashCode()
        {
            var hashCode = 1806219545;
            hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<Modifiers>>.Default.GetHashCode(modifiers);
            hashCode = hashCode * -1521134295 + terminalKeyCode.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(KeyChord chord1, KeyChord chord2)
        {
            return chord1.Equals(chord2);
        }

        public static bool operator !=(KeyChord chord1, KeyChord chord2)
        {
            return !(chord1 == chord2);
        }
        #endregion Operators
    }
}
