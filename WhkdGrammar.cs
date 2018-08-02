using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using whkd.Grammar;
using static whkd.Grammar.whkdParser;

namespace whkd
{
    public class WhkdGrammar
    {
        public BindingContext[] bindings;
        public WhkdGrammar(StreamReader configFileStream)
        {
            var inputStream = new AntlrInputStream(configFileStream);
            bindings = GetBindingsFromStream(inputStream);
        }

        public WhkdGrammar(string ruleText)
        {
            var inputStream = new AntlrInputStream(ruleText);
            bindings = GetBindingsFromStream(inputStream);
        }

        private BindingContext[] GetBindingsFromStream(AntlrInputStream inputStream)
        {
            var lexer = new whkdLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new whkdParser(commonTokenStream);
            return parser.profile().binding();
        }
    }

    // Return a list of list of KeyChords
    public class WhkdChordBindingVisitor : whkdParserBaseVisitor<List<KeyChord>>
    {

        public override List<KeyChord> VisitBinding([NotNull] BindingContext context)
        {
            var result = new List<KeyChord>();
            // TODO: call VisitChord on each chord of this binding, combining them and ensuring each chord has a terminal key below
            /*
            foreach (KeyChord chord in chords)
            {
                if (!chord.terminalKeyCode.HasValue)
                {
                    throw new MissingTerminalKeyException("Every chord must have a terminal key");
                }
            }
            */
            return result;
        }

        public override List<KeyChord> VisitChord([NotNull] ChordContext context)
        {
            // handle regular keys first
            // keys and choice keys can be done in any order because a
            // chord involves all of these elements being pressed simulateneously
            KeyChord staticChord = new KeyChord();
            foreach (KeyContext staticKey in context.key())
            {
                if (staticKey.Modifier() != null)
                {
                    Modifier mod = KeyChord.ModifierFromTerminalNode(staticKey.Modifier());
                    staticChord.modifiers.Add(mod);
                }
                else if (staticKey.TerminalKey() != null)
                {
                    if (staticChord.terminalKeyCode.HasValue)
                    {
                        throw new MultipleNonModifiersSingleChordException("Cannot assign two non-modifier keys to a single chord");
                    }
                    staticChord.terminalKeyCode = KeyChord.TerminalKeyCodeFromNode(staticKey.TerminalKey());
                }
            }

            var chords = new List<KeyChord> { staticChord };
            foreach (ChoiceKeyListContext choiceKeyList in context.choiceKeyList())
            {
                var swapChords = new List<KeyChord>();
                List<KeyChord> choiceChords = VisitChoiceKeyList(choiceKeyList);
                foreach (KeyChord existingChord in chords)
                {
                    foreach (KeyChord newChord in choiceChords)
                    {
                        if (existingChord.terminalKeyCode.HasValue && newChord.terminalKeyCode.HasValue)
                        {
                            throw new MultipleNonModifiersSingleChordException("Cannot assign two non-modifier keys to a single chord");
                        }
                        uint? mergedKeyCode = existingChord.terminalKeyCode ?? newChord.terminalKeyCode;
                        var mergedModifiers = new HashSet<Modifier>(existingChord.modifiers.Union(newChord.modifiers));
                        swapChords.Add(new KeyChord(mergedKeyCode, mergedModifiers));
                    }
                }
                chords.Clear();
                chords = swapChords;
            }

            return chords;
        }

        public override List<KeyChord> VisitChoiceKeyList([NotNull] ChoiceKeyListContext context)
        {
            var result = new List<KeyChord>();
            foreach (ChordContext child in context.chord())
            {
                if (child.choiceKeyList().Length > 0)
                {
                    throw new NestedChoiceChordsException("Cannot nest choice lists");
                }
                result.AddRange(VisitChord(child));
            }
            return result;
        }

    }

    public class MultipleNonModifiersSingleChordException : Exception
    {
        public MultipleNonModifiersSingleChordException(string message) : base(message)
        {
        }
    }

    public class NestedChoiceChordsException : Exception
    {
        public NestedChoiceChordsException(string message) : base(message)
        {
        }
    }

    public class MissingTerminalKeyException : Exception
    {
        public MissingTerminalKeyException(string message) : base(message)
        {
        }
    }
}
