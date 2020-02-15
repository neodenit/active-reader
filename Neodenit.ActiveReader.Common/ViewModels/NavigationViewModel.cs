using Neodenit.ActiveReader.Common.Attributes;
using Neodenit.ActiveReader.Common.Enums;

namespace Neodenit.ActiveReader.Common.ViewModels
{
    public class NavigationViewModel
    {
        [CheckOwner]
        public int ArticleId { get; set; }

        public NavigationTarget Target { get; set; }
    }
}