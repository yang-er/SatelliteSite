﻿using Antlr4.Grammar.Java;
using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Plag.Frontend.Java
{
    public class Language : ILanguage
    {
        public IReadOnlyCollection<string> Suffixes { get; } = new[] { ".JAVA" };
        
        public string Name => "Java9";

        public string ShortName => "java9";

        public int MinimalTokenMatch => 12;

        public bool SupportsColumns => true;

        public bool IsPreformated => true;

        public Func<Structure, IJava9Listener> ListenerFactory { get; }

        public bool UsesIndex => true;

        public int CountOfTokens => (int)TokenConstants.NUM_DIFF_TOKENS;

        public Language() : this(s => new JplagListener(s))
        {
        }

        public Language(Func<Structure, IJava9Listener> factory)
        {
            ListenerFactory = factory;
        }

        public Structure Parse(string fileName, Func<Stream> streamFactory)
        {
            var structure = new Structure();
            var outputWriter = new StringWriter(structure.OtherInfo);
            var errorWriter = new StringWriter(structure.ErrorInfo);
            var lexer = new Java9Lexer(CharStreams.fromStream(streamFactory()), outputWriter, errorWriter);
            var parser = new Java9Parser(new BufferedTokenStream(lexer), outputWriter, errorWriter);
            var listener = ListenerFactory(structure);
            parser.AddErrorListener(structure);
            parser.AddParseListener(listener);
            var root = parser.CompilationUnit();
            parser.ErrorListeners.Clear();
            parser.ParseListeners.Clear();
            return structure;
        }

        public string TypeName(int type) => Token.TypeToString((TokenConstants)type);
    }
}
