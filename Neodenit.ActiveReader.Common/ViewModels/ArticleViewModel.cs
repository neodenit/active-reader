using System.ComponentModel.DataAnnotations;

namespace Neodenit.ActiveReader.Common.ViewModels
{
    public class ArticleViewModel
    {
        public int? ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [PrefixLengthValidation]
        public int PrefixLength { get; set; }
    }
}