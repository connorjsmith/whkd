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

        [TestMethod]
        public void TestParsesChoiceChords()
        {
            string[] chordKeys = { "Alt+{a,b,c}", "Ctrl+{SHIFT,Alt}+b", "Ctrl+{shift,alt}+{a,b}" };
            string command = "dummy command";
            string basicRuleText = String.Join(';', chordKeys) + "\n\t" + command;

            var testVisitor = new WhkdChordBindingVisitor();

            var grammar = new WhkdGrammar(basicRuleText);
            List<KeyChord> firstChordSet = testVisitor.VisitChord(grammar.bindings[0].chord()[0]);
            Assert.AreEqual(3, firstChordSet.Count);
            KeyChord firstChordFirstSet = firstChordSet[0];
            Assert.AreEqual(KeyChord.TerminalKeyCodeFromName("a"), firstChordFirstSet.terminalKeyCode);
            Assert.AreEqual(1, firstChordFirstSet.modifiers.Count);
            Assert.IsTrue(firstChordFirstSet.modifiers.Contains(Modifier.Alt));

            KeyChord secondChordFirstSet = firstChordSet[1];
            Assert.AreEqual(KeyChord.TerminalKeyCodeFromName("b"), secondChordFirstSet.terminalKeyCode);
            Assert.AreEqual(1, secondChordFirstSet.modifiers.Count);
            Assert.IsTrue(secondChordFirstSet.modifiers.Contains(Modifier.Alt));

            KeyChord thirdChordFirstSet = firstChordSet[2];
            Assert.AreEqual(KeyChord.TerminalKeyCodeFromName("c"), thirdChordFirstSet.terminalKeyCode);
            Assert.AreEqual(1, thirdChordFirstSet.modifiers.Count);
            Assert.IsTrue(thirdChordFirstSet.modifiers.Contains(Modifier.Alt));



            List<KeyChord> secondChordSet = testVisitor.VisitChord(grammar.bindings[0].chord()[1]);
            Assert.AreEqual(2, secondChordSet.Count);
            KeyChord firstChordSecondSet = secondChordSet[0];
            Assert.AreEqual(KeyChord.TerminalKeyCodeFromName("b"), firstChordSecondSet.terminalKeyCode);
            Assert.AreEqual(2, firstChordSecondSet.modifiers.Count);
            Assert.IsTrue(firstChordSecondSet.modifiers.Contains(Modifier.Ctrl));
            Assert.IsTrue(firstChordSecondSet.modifiers.Contains(Modifier.Shift));

            KeyChord secondChordSecondSet = secondChordSet[1];
            Assert.AreEqual(KeyChord.TerminalKeyCodeFromName("b"), secondChordSecondSet.terminalKeyCode);
            Assert.AreEqual(2, secondChordSecondSet.modifiers.Count);
            Assert.IsTrue(secondChordSecondSet.modifiers.Contains(Modifier.Ctrl));
            Assert.IsTrue(secondChordSecondSet.modifiers.Contains(Modifier.Alt));



            List<KeyChord> thirdChordSet = testVisitor.VisitChord(grammar.bindings[0].chord()[2]);
            Assert.AreEqual(4, thirdChordSet.Count);
            KeyChord firstChordThirdSet = thirdChordSet[0];
            Assert.AreEqual(KeyChord.TerminalKeyCodeFromName("a"), firstChordThirdSet.terminalKeyCode);
            Assert.AreEqual(2, firstChordThirdSet.modifiers.Count);
            Assert.IsTrue(firstChordThirdSet.modifiers.Contains(Modifier.Ctrl));
            Assert.IsTrue(firstChordThirdSet.modifiers.Contains(Modifier.Shift));

            KeyChord secondChordThirdSet = thirdChordSet[1];
            Assert.AreEqual(KeyChord.TerminalKeyCodeFromName("b"), secondChordThirdSet.terminalKeyCode);
            Assert.AreEqual(2, secondChordThirdSet.modifiers.Count);
            Assert.IsTrue(secondChordThirdSet.modifiers.Contains(Modifier.Ctrl));
            Assert.IsTrue(secondChordThirdSet.modifiers.Contains(Modifier.Shift));

            KeyChord thirdChordThirdSet = thirdChordSet[2];
            Assert.AreEqual(KeyChord.TerminalKeyCodeFromName("a"), thirdChordThirdSet.terminalKeyCode);
            Assert.AreEqual(2, thirdChordThirdSet.modifiers.Count);
            Assert.IsTrue(thirdChordThirdSet.modifiers.Contains(Modifier.Ctrl));
            Assert.IsTrue(thirdChordThirdSet.modifiers.Contains(Modifier.Alt));

            KeyChord fourthChordThirdSet = thirdChordSet[3];
            Assert.AreEqual(KeyChord.TerminalKeyCodeFromName("b"), fourthChordThirdSet.terminalKeyCode);
            Assert.AreEqual(2, fourthChordThirdSet.modifiers.Count);
            Assert.IsTrue(fourthChordThirdSet.modifiers.Contains(Modifier.Alt));
            Assert.IsTrue(fourthChordThirdSet.modifiers.Contains(Modifier.Ctrl));

        }
    }
}
