using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neodenit.ActiveReader.Common.Attributes;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.ViewModels;

namespace Neodenit.ActiveReader.Web.Angular.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionsService questionsService;

        public QuestionsController(IQuestionsService questionsService)
        {
            this.questionsService = questionsService;
        }

        [ValidateModel]
        [HttpGet("article/{articleId}/position/{position}")]
        public async Task<ActionResult<QuestionViewModel>> Get([CheckOwner]int articleId, int position)
        {
            QuestionViewModel question = await questionsService.GetQuestionAsync(articleId, position);
            return Ok(question);
        }
    }
}
