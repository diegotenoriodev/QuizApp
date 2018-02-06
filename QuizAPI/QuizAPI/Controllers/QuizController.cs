using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QuizAPI.Model;
using System.Linq;

namespace QuizAPI.Controllers
{
    [Produces("application/json")]
    [EnableCors("SiteCorsPolicy")]
    [Route("api/Quiz")]
    [Authorize]
    public class QuizController : Controller
    {
        private QuizModel model;
        private QuizModel Model
        {
            get
            {
                if(model == null)
                {
                    model = new QuizModel(HttpContext.User.Claims.First().Value);
                }

                return model;
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult Delete(int id)
        {
            ResultOperation result = Model.Delete(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            return Ok(Model.GetAll());
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult Get(int id)
        {
            Model.Quiz quiz = Model.Get(id);

            if (quiz == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(quiz);
            }
        }
        
        [HttpGet]
        [Route("published/{id:int}")]
        public IActionResult GetPublishedQuiz(int id)
        {
            QuizAPI.Model.PublishedQuiz pubQuiz = Model.GetPublishedQuiz(id);

            if (pubQuiz == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(pubQuiz);
            }
        }

        [HttpGet]
        [Route("finished/{idAnswer:int}")]
        public IActionResult GetFinishedQuiz(int idAnswer)
        {
            QuizAPI.Model.PublishedQuiz pubQuiz = Model.GetFinishedQuiz(idAnswer);

            if (pubQuiz == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(pubQuiz);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]Model.Quiz entity)
        {
            ResultOperation result = Model.Save(entity);

            if (result.Success)
            {
                return Ok(new ResultOperationWithObject(result, entity));
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet]
        [Route("publication/{idquiz:int}")]
        public IActionResult GetPublications(int idquiz)
        {
            return Ok(Model.GetPublicationsForQuiz(idquiz, "api/answerquiz/"));
        }

        [HttpPost]
        [Route("publication")]
        public IActionResult PostPublications([FromBody]QuizViewModel quizModelView)
        {
            return Ok(Model.SavePublicationForQuiz(quizModelView));
        }

        [HttpGet]
        [Route("qtdclosed")]
        public IActionResult GetQtdClosed()
        {
            return Ok(Model.GetQtdClosed());
        }

        [HttpGet]
        [Route("qtdopen")]
        public IActionResult GetQtdOpen()
        {
            return Ok(Model.GetQtdOpen());
        }

        [HttpGet]
        [Route("foruser")]
        public IActionResult GetQuizesForUser()
        {
            return Ok(Model.GetQuizesForUser());
        }

        [HttpGet]
        [Route("forowner/{idQuiz:int}")]
        public IActionResult GetQuizesForOwner(int idQuiz)
        {
            return Ok(Model.GetQuizesForOwner(idQuiz));
        }
    }
}