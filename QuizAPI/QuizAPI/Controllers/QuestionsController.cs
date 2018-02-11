using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuizAPI.Model;
using System.Collections;
using System.Linq;

namespace QuizAPI.Controllers
{
    [Produces("application/json")]
    [EnableCors("SiteCorsPolicy")]
    [Route("api/Questions")]
    [Authorize]
    public class QuestionsController : Controller
    {
        private QuestionModel model;
        private QuestionModel Model
        {
            get
            {
                if (model == null)
                {
                    model = new QuestionModel(HttpContext.User.Claims.First().Value);
                }

                return model;
            }
        }

        [HttpGet]
        [Route("typelist")]
        public IActionResult GetTypeList()
        {
            return Ok(
                new ArrayList() {
                    new { Id = Domain.QuestionType.Multiple_Choice, Name = "Multiple Choice" },
                    new { Id = Domain.QuestionType.TrueFalse_Question, Name = "True or False" },
                    new { Id = Domain.QuestionType.Open_Ended, Name = "Open Ended" }
                }
            );
        }

        [HttpPost]
        public IActionResult Post([FromBody]Question question)
        {
            if (question == null)
            {
                return BadRequest();
            }

            ResultOperation result = Model.Save(question);

            return Ok(new ResultOperationWithObject(result, question));
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult Get(int id)
        {
            return Ok(Model.GetAll(id));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult Delete(int id)
        {
            return Ok(Model.Delete(id));
        }

        public IActionResult Get()
        {
            return NotFound();
        }

        public IActionResult Get(string search)
        {
            return NotFound();
        }

        [HttpGet]
        [Route("options/{idQuestion:int}")]
        public IActionResult GetOptions(int idQuestion)
        {
            return Ok(Model.GetQuestionOptions(idQuestion));
        }

        [HttpGet]
        [Route("listoptions/{idAnswer:int}/{idQuestion:int}")]
        public IActionResult GetListOptions(int idAnswer, int idQuestion)
        {
            return Ok(Model.GetListOptions(idAnswer, idQuestion));
        }
        [HttpGet]
        [Route("listoptionsanswer/{idAnswer:int}/{idQuestion:int}")]
        public IActionResult GetListOptionsAnswer(int idAnswer, int idQuestion)
        {
            return Ok(Model.GetListOptionsForAnswer(idAnswer, idQuestion));
        }

        [HttpPost]
        [Route("options/{idQuestion:int}")]
        public IActionResult PostOptions([FromRoute]int idQuestion, [FromBody]Domain.MultipleChoice[] answer)
        {
            if (answer != null)
            {
                return Ok(Model.SaveQuestionOptions(idQuestion, answer));
            }

            return NotFound();
        }

        [HttpDelete]
        [Route("options/{idQuestion:int}/{idAnswer:int}")]
        public IActionResult DeleteOptions(int idQuestion, int IdAnswer)
        {
            return Ok(Model.DeleteQuestionOption(idQuestion, IdAnswer));
        }
    }
}