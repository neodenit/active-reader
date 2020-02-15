using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Neodenit.ActiveReader.Common.Interfaces;

namespace Neodenit.ActiveReader.Common.Attributes
{
    public class CheckOwnerAttribute : ValidationAttribute
    {
        private readonly ValidationResult FailedValidationResult = new ValidationResult("Unauthorized");

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var articleService = validationContext.GetService(typeof(IArticlesService)) as IArticlesService;
            var httpContextAccessor = validationContext.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;

            var articleId = (int?)value;

            if (articleId == null)
            {
                return ValidationResult.Success;
            }

            var article = articleService.Get(articleId.Value);

            if (article == null)
            {
                return ValidationResult.Success;
            }

            return article.Owner == httpContextAccessor.HttpContext.User.Identity.Name
                ? ValidationResult.Success
                : FailedValidationResult;
        }
    }
}
