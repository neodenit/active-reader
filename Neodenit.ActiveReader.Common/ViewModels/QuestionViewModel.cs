﻿using System.Collections.Generic;
using Neodenit.ActiveReader.Common.Attributes;

namespace Neodenit.ActiveReader.Common.ViewModels
{
    public class QuestionViewModel
    {
        [CheckOwner]
        public int ArticleId { get; set; }

        public int AnswerPosition { get; set; }

        public int LastPosition { get; set; }

        public int Progress { get; set; }

        public string StartingText { get; set; }

        public string NewText { get; set; }

        public IEnumerable<string> Choices { get; set; }

        public string CorrectAnswer { get; set; }
    }
}