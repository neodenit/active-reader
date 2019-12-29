using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{articleId}/{lastAnswerPosition}")]
        public async Task<ActionResult<QuestionViewModel>> Get(int articleId, int lastAnswerPosition)
        {
            QuestionViewModel question = await questionsService.GetQuestionAsync(articleId, lastAnswerPosition);
            return Ok(question);
        }
    }
}
