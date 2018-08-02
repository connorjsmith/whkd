using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using whkd;
using whkd.Grammar;

namespace whkd.Tests
{
    [TestClass]
    public class TestWhkdGrammarVisitor
    {
        [TestMethod]
        public void TestParsesSingleChords()
        {
            string[] chordKeys = { "a", "b", "c" };
            string command = "dummy command";
            string basicRuleText = String.Join(';', chordKeys) + "\n\t" + command;

            var testVisitor = new WhkdChordBindingVisitor();

            var grammar = new WhkdGrammar(basicRuleText);
            for (int i = 0; i < chordKeys.Length; ++i)
            {
                List<KeyChord> chords = testVisitor.VisitChord(grammar.bindings[0].chord()[i]);
                Assert.AreEqual(1, chords.Count);
                Assert.AreEqual(KeyChord.TerminalKeyCodeFromName(chordKeys[i]), chords.First().terminalKeyCode);
            }
        }

        [TestMethod]
        public void TestParsesModifierChords()
        {
            string[] chordKeys = { "Alt+a", "Ctrl+SHIFT+b" };
            string command = "dummy command";
            string basicRuleText = String.Join(';', chordKeys) + "\n\t" + command;

            var testVisitor = new WhkdChordBindingVisitor();

            var grammar = new WhkdGrammar(basicRuleText);
            List<KeyChord> firstChordSet = testVisitor.VisitChord(grammar.bindings[0].chord()[0]);
            Assert.AreEqual(1, firstChordSet.Count);
            KeyChord firstChord = firstChordSet.First();
            Assert.AreEqual(KeyChord.TerminalKeyCodeFromName("a"), firstChord.terminalKeyCode);
            Assert.AreEqual(1, firstChord.modifiers.Count);
            Assert.IsTrue(firstChord.modifiers.Contains(Modifier.Alt));

            List<KeyChord> secondChordSet = testVisitor.VisitChord(grammar.bindings[0].chord()[1]);
            Assert.AreEqual(1, secondChordSet.Count);
            KeyChord secondChord = secondChordSet.First();
            Assert.AreEqual(KeyChord.TerminalKeyCodeFromName("b"), secondChord.terminalKeyCode);
            Assert.AreEqual(2, secondChord.modifiers.Count);
            Assert.IsTrue(secondChord.modifiers.Contains(Modifier.Ctrl));
            Assert.IsTrue(secondChord.modifiers.Contains(Modifier.Shift));
        }
    }
}
