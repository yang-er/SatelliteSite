﻿using Antlr4.Runtime;
using System.Collections;
using System.Collections.Generic;

namespace Plag
{
    public class SubmissionString : ISubmissionFile
    {
        public SubmissionString(string fileName, string content)
        {
            Path = fileName;
            Content = content;
        }

        public IEnumerator<ISubmissionFile> GetEnumerator()
        {
            yield return this;
        }

        public string Path { get; }

        public string Content { get; }

        public bool IsLeaf => true;

        public ICharStream Open() => new AntlrInputStream(Content) { name = Path };
    }
}