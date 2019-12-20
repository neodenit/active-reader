using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Neodenit.ActiveReader.Common.Interfaces;
using Neodenit.ActiveReader.Common.Models;

namespace ActiveReader.Web.Controllers.API
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionsService questionsService;

        public QuestionsController(IQuestionsService questionsService)
        {
            this.questionsService = questionsService;
        }

        [HttpGet]
        [Route("{articleID}/{lastAnswerPosition}")]
        public async Task<ActionResult<QuestionViewModel>> Get(int articleID, int lastAnswerPosition)
        {
            var question = await questionsService.GetQuestionAsync(articleID, lastAnswerPosition);
            return Ok(question);
        }
    }
}
