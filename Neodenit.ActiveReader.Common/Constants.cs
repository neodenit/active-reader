﻿namespace Neodenit.ActiveReader.Common
{
    public static class Constants
    {
        public const string PrefixDelimiter = " ";

        public const char LineBreak = '\n';
        public const string Ellipsis = "...";

        public const int StartingPosition = 1;

        public static readonly char[] SentenceBreaks = new[] { '.', '!', '?', '\n' };
    }
}
