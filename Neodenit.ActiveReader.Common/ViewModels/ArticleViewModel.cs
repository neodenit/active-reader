using System.ComponentModel.DataAnnotations;
using Neodenit.ActiveReader.Common.Attributes;
using Neodenit.ActiveReader.Common.Enums;

namespace Neodenit.ActiveReader.Common.ViewModels
{
    public class ArticleViewModel
    {
        [CheckOwner]
        public int? ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public string Owner { get; set; }

        [PrefixLengthValidation]
        public int PrefixLength { get; set; }

        public int AnswerLength { get; set; }

        [MaxChoicesValidation]
        public int MaxChoices { get; set; }

        public bool IgnoreCase { get; set; }

        public bool IgnorePunctuation { get; set; }

        public ArticleState State { get; set; }
    }
}