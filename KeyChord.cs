using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Antlr4.Runtime.Tree;

namespace whkd
{
    public enum Modifier
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
        public ICollection<Modifier> modifiers = new HashSet<Modifier>();
        public uint? terminalKeyCode;

        public KeyChord(ITerminalNode terminalNode, IEnumerable<Modifier> mods)
        {
            foreach(Modifier mod in mods)
            {
                modifiers.Add(mod);
            }
            terminalKeyCode = TerminalKeyCodeFromNode(terminalNode);
        }
        public KeyChord(uint? keyCode)
        {
            terminalKeyCode = keyCode;
        }
        public KeyChord(uint? keyCode, IEnumerable<Modifier> mods)
        {
            foreach(Modifier mod in mods)
            {
                modifiers.Add(mod);
            }
            terminalKeyCode = keyCode;
        }

        public KeyChord(IEnumerable<Modifier> mods)
        {
            foreach (Modifier mod in mods)
            {
                modifiers.Add(mod);
            }
        }

        public KeyChord()
        {
        }

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
            hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<Modifier>>.Default.GetHashCode(modifiers);
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

        #region Conversion Helpers
        public static Dictionary<string, Modifier> ModifierFromName = new Dictionary<string, Modifier>
        {
            {"alt", Modifier.Alt },
            {"ctrl", Modifier.Ctrl },
            {"control", Modifier.Ctrl },
            {"hyper", Modifier.Hyper },
            {"hyp", Modifier.Hyper },
            {"meta", Modifier.Meta },
            {"shift", Modifier.Shift },
            {"win", Modifier.Windows },
            {"windows", Modifier.Windows },
        };
        public static uint TerminalKeyCodeFromNode(ITerminalNode terminalNode)
        {
            return (uint) (VkKeyScan(terminalNode.GetText()[0]) & 0x00FF);
        }

        public static uint TerminalKeyCodeFromName(string name)
        {
            return (uint) (VkKeyScan(name[0]) & 0x00FF);
        }
        public static Modifier ModifierFromTerminalNode(ITerminalNode terminalNode)
        {
            string text = terminalNode.GetText().ToLower();
            return ModifierFromName[text];
        }
        #endregion

        #region Win32 Helpers
        [DllImport("user32.dll")] private static extern short VkKeyScan(char ch);
        #endregion Win32 Helpers
    }
}
