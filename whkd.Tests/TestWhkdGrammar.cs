using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using whkd;
using whkd.Grammar;

namespace whkd.Tests
{
    [TestClass]
    public class TestWhkdGrammar
    {
        [TestMethod]
        public void TestBasicRule()
        {
            string[] chords = { "a", "b", "c" };
            string command = "dummy command";
            string basicRuleText = String.Join(';', chords) + "\n\t" + command;

            var grammar = new WhkdGrammar(basicRuleText);
            Assert.AreEqual(1, grammar.bindings.Length);

            whkdParser.BindingContext bindingCxt = grammar.bindings[0];

            string[] parsedChords = ExtractParsedChords(bindingCxt);
            Assert.AreEqual(chords.Length, parsedChords.Length);

            Assert.AreEqual(1, bindingCxt.commandPiece().Length);
            string parsedCommand = bindingCxt.commandPiece()[0].GetText();
            Assert.AreEqual(command, parsedCommand);

            for (int i = 0; i < chords.Length; i++)
            {
                Assert.AreEqual(chords[i], parsedChords[i]);
            }
        }

        [TestMethod]
        public void TestModifierChord()
        {
            string chord = "Alt+A";
            string command = "dummy command";
            string modifierChordRule = chord + "\n\t" + command;

            var grammar = new WhkdGrammar(modifierChordRule);
            Assert.AreEqual(1, grammar.bindings.Length);

            whkdParser.BindingContext bindingCxt = grammar.bindings[0];
            string[] parsedChords = ExtractParsedChords(bindingCxt);
            Assert.AreEqual(1, parsedChords.Length);
            string parsedCommand = bindingCxt.commandPiece()[0].GetText();
            Assert.AreEqual(command, parsedCommand);

            whkdParser.KeyContext[] keys = bindingCxt.chords().chord()[0].key();
            var modifiers = keys.Select(k => k.Modifier()).Where(m => m != null).ToList();
            var terminalKeys = keys.Select(k => k.TerminalKey()).Where(t => t != null).ToList();
            Assert.AreEqual(1, modifiers.Count());
            Assert.AreEqual("Alt", modifiers[0].GetText());
            Assert.AreEqual(1, terminalKeys.Count());
            Assert.AreEqual("A", terminalKeys[0].GetText());
        }

        #region Helper Functions
        private static string[] ExtractParsedChords(whkdParser.BindingContext bindingCxt)
        {
            return bindingCxt.chords().chord().Select(c => c.GetText()).ToArray();
        }
        #endregion Helper Functions
    }
}
