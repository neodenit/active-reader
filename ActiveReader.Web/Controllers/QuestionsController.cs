using ActiveReader.Interfaces;
using ActiveReader.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        [Route("api/Questions/{articleID}/{position}")]
        public QuestionViewModel Get(int articleID, int position)
        {
            var minPosition = questionsService.GetPosition(position);

            return new QuestionViewModel {
                ArticleID = articleID,
                Position = minPosition,
                StartingWords = questionsService.GetStartingWords(articleID, minPosition),
             };
        }

        // POST: api/Question
        public QuestionViewModel Post(QuestionViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
