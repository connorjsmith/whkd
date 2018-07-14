using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using whkd.Grammar;

namespace whkd
{
    public class WhkdGrammar
    {
        public whkdParser.BindingContext[] bindings;
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

        private whkdParser.BindingContext[] GetBindingsFromStream(AntlrInputStream inputStream)
        {
            var lexer = new whkdLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new whkdParser(commonTokenStream);
            return parser.profile().binding();
        }
    }
}
