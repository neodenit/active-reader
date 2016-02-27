using ActiveReader.Interfaces;
using ActiveReader.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ActiveReader.Web.Controllers
{
    public class QuestionsController : ApiController
    {
        private readonly IQuestionsService questionsService;

        public QuestionsController(IQuestionsService questionsService)
        {
            this.questionsService = questionsService;
        }

        // GET: api/Question/5/10
        [Route("api/Questions/{articleID}/{lastAnswerPosition}")]
        [ResponseType(typeof(IQuestionViewModel))]
        public async Task<IHttpActionResult> Get(int articleID, int lastAnswerPosition)
        {
            var question = await questionsService.GetQuestionAsync(articleID, lastAnswerPosition);
            return Ok(question);
        }
    }
}
