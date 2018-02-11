using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace QuizAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Answer")]
    [Authorize]
    [EnableCors("SiteCorsPolicy")]
    public class AnswerController : Controller
    {
        private Model.AnswerModel model;

        private Model.AnswerModel Model
        {
            get
            {
                if (this.model == null)
                {
                    this.model = new QuizAPI.Model.AnswerModel(HttpContext.User.Claims.First().Value);
                }

                return this.model;
            }
        }

        [HttpPost]
        [Route("multiplechoice")]
        public IActionResult PostAnswer([FromBody]Model.MultipleChoiceAnswerView answer)
        {
            return Ok(Model.SaveAnswer(answer));
        }

        [HttpPost]
        [Route("openended")]
        public IActionResult PostAnswer([FromBody]Model.OpenEndedAnswerView answer)
        {
            return Ok(Model.SaveAnswer(answer));
        }

        [HttpPost]
        [Route("truefalse")]
        public IActionResult PostAnswer([FromBody]Model.TrueFalseAnswerView answer)
        {
            return Ok(Model.SaveAnswer(answer));
        }

        [HttpGet]
        [Route("{idAnswer:int}/{idQuestion:int}")]
        public IActionResult Get(int idAnswer, int idQuestion)
        {
            return Ok(Model.GetAnswer(idAnswer, idQuestion));
        }

        [HttpGet]
        [Route("report/{idAnswer:int}/{idQuestion:int}")]
        public IActionResult GetAnswerReport(int idAnswer, int idQuestion)
        {
            return Ok(Model.GetAnswerReport(idAnswer, idQuestion));
        }

        [HttpPost]
        [Route("finish")]
        public IActionResult PostFinishAnswer([FromBody] int idAnswer)
        {
            return Ok(Model.FinishAnswer(idAnswer));
        }

        [HttpPost]
        [Route("evaluate")]
        public IActionResult PostEvaluateAnswer([FromBody] int idAnswer)
        {
            return Ok(Model.EvaluateAnswer(idAnswer));
        }
    }
}