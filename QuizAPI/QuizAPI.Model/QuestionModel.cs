using QuizAPI.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QuizAPI.Model
{
    /// <summary>
    /// This class is responsible for managing questions.
    /// </summary>
    public class QuestionModel : BaseModel<Question>
    {
        private string currentUserId;

        /// <summary>
        /// QuestionModel's constructor
        /// </summary>
        /// <param name="userId">Requires a authenticated user</param>
        public QuestionModel(string userId)
        {
            this.currentUserId = userId;
        }

        /// <summary>
        /// Returns the options for a multiplechoice question.
        /// Verifies if the answer exists and if the user has permission to see.
        /// </summary>
        /// <param name="idAnswer"></param>
        /// <param name="idQuestion"></param>
        /// <returns></returns>
        public List<ListItem> GetListOptionsForAnswer(int idAnswer, int idQuestion)
        {
            using (IRepository repository = Repository.Repository.CreateRepository())
            {
                var answer = repository.GetAnswer(idAnswer);

                if (answer != null && !answer.IsOpen && answer.Quiz.Creator.Id == currentUserId)
                {
                    return GetMultipleChoiceFromQuestion(repository, answer, idQuestion);
                }
            }

            return null;
        }

        /// <summary>
        /// Builds the return for GetListOptionsForAnswer
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="answer"></param>
        /// <param name="idQuestion"></param>
        /// <returns></returns>
        private List<ListItem> GetMultipleChoiceFromQuestion(IRepository repository, Domain.Answer answer, int idQuestion)
        {
            List<ListItem> returnedList = new List<ListItem>();

            if (answer.Quiz.Questions.Any(r => r.Id == idQuestion))
            {
                var options = repository.GetMultipleChoiceOptions(idQuestion);

                if (options != null && options.Count > 0)
                {
                    options.ForEach(r =>
                    {
                        returnedList.Add(new ListItem()
                        {
                            Id = r.Id,
                            Name = r.Content
                        });
                    });
                }
            }

            return returnedList;
        }

        /// <summary>
        /// Returns a list of options, but considering the user that is taking the quiz.
        /// </summary>
        /// <param name="idAnswer"></param>
        /// <param name="idQuestion"></param>
        /// <returns></returns>
        public List<ListItem> GetListOptions(int idAnswer, int idQuestion)
        {
            using (IRepository repository = Repository.Repository.CreateRepository())
            {
                var answer = repository.GetAnswer(idAnswer);

                if (answer != null)
                {
                    if (answer.User.Id == currentUserId || answer.Quiz.Creator.Id == currentUserId)
                    {
                        return GetMultipleChoiceFromQuestion(repository, answer, idQuestion);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get the generic options for a question.
        /// It is used for questions that are multiple choice, open ended, or true false.
        /// It also verifies authorization.
        /// </summary>
        /// <param name="idQuestion"></param>
        /// <returns></returns>
        public IList GetQuestionOptions(int idQuestion)
        {
            using (IRepository repository = Repository.Repository.CreateRepository())
            {
                Domain.Question question = repository.GetQuestion(idQuestion);

                if (question != null)
                {
                    if (question.Quiz.Creator == repository.GetUser(this.currentUserId))
                    {
                        return question.Options;
                    }
                }
            }

            return new List<Domain.BaseQuestionOptionType>();
        }

        /// <summary>
        /// Saves the options for a question.
        /// </summary>
        /// <param name="idQuestion"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public ResultOperation SaveQuestionOptions(int idQuestion, Domain.MultipleChoice[] option)
        {
            try
            {
                using (IRepository repository = Repository.Repository.CreateRepository())
                {
                    Domain.Question question = repository.GetQuestion(idQuestion);

                    if (question != null)
                    {
                        if (question.Quiz.Creator == repository.GetUser(this.currentUserId))
                        {
                            question.Options.Clear();

                            foreach (var item in option)
                            {
                                question.Options.Add(item);
                            }

                            repository.SaveQuestion(question);
                        }
                    }
                    else
                    {
                        messages.Add("Question was not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                messages.Add(DefaultErrorMessage());
            }

            return messages;
        }

        /// <summary>
        /// Responsible for removing a option from a question.
        /// Option can be TrueFalse, multiple choice or open ended.
        /// </summary>
        /// <param name="idQuestion"></param>
        /// <param name="idOption"></param>
        /// <returns></returns>
        public ResultOperation DeleteQuestionOption(int idQuestion, int idOption)
        {
            try
            {
                using (IRepository repository = Repository.Repository.CreateRepository())
                {
                    Domain.Question question = repository.GetQuestion(idQuestion);

                    if (question != null && question.Options.Any(r => r.Id == idOption))
                    {
                        if (question.Quiz.Creator == repository.GetUser(this.currentUserId))
                        {
                            question.Options.Remove(question.Options.FirstOrDefault(r => r.Id == idOption));
                            repository.SaveQuestion(question);
                        }
                    }
                    else
                    {
                        messages.Add("Question was not found.");
                    }

                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                messages.Add(DefaultErrorMessage());
            }

            return messages;
        }

        /// <summary>
        /// Responsible for removing the question itself.
        /// It also verifies authorization.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override ResultOperation Delete(int id)
        {
            try
            {
                using (IRepository repository = Repository.Repository.CreateRepository())
                {
                    Domain.Question question = repository.GetQuestion(id);

                    if (question != null)
                    {
                        if (question.Quiz.Creator == repository.GetUser(this.currentUserId))
                        {
                            repository.DeleteQuestion(question);
                            return true;
                        }
                    }
                    else
                    {
                        messages.Add("Question was not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                messages.Add(DefaultErrorMessage());
            }

            return messages;
        }

        /// <summary>
        /// Verifies authorization and returns the required question.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Question Get(int id)
        {
            using (IRepository repository = Repository.Repository.CreateRepository())
            {
                Domain.Question question = repository.GetQuestion(id);

                if (question != null)
                {
                    if (question.Quiz.Creator == repository.GetUser(this.currentUserId))
                    {
                        return new Question()
                        {
                            Description = question.Description,
                            Id = question.Id,
                            ImageURL = question.ImageURL,
                            QuestionType = question.QuestionType
                        };
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get all questions for one specific quiz.
        /// Returns only if the user is the creator of this quiz.
        /// The view returned here is for edition mode.
        /// </summary>
        /// <param name="idQuiz"></param>
        /// <returns></returns>
        public List<Question> GetAll(int idQuiz)
        {
            using (IRepository repository = Repository.Repository.CreateRepository())
            {
                Domain.Quiz quiz = repository.GetQuiz(idQuiz);

                if (quiz != null)
                {
                    if (quiz.Creator == repository.GetUser(this.currentUserId))
                    {
                        if (quiz.Questions != null)
                        {
                            return quiz.Questions.Select(
                                r => new Question()
                                {
                                    Id = r.Id,
                                    Description = r.Description,
                                    ImageURL = r.ImageURL,
                                    QuestionType = r.QuestionType,
                                    IdQuiz = quiz.Id,
                                    Options = null
                                }).ToList();
                        }
                    }
                }
            }

            return new List<Question>();
        }

        /// <summary>
        /// If the user is the creator, it saves the question.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override ResultOperation Save(Question entity)
        {
            try
            {
                using (IRepository repository = Repository.Repository.CreateRepository())
                {
                    Domain.Quiz quiz = repository.GetQuiz(entity.IdQuiz);

                    if (quiz != null)
                    {
                        if (quiz.Creator == repository.GetUser(this.currentUserId))
                        {
                            Domain.Question question = new Domain.Question()
                            {
                                Id = entity.Id,
                                Description = entity.Description,
                                ImageURL = entity.ImageURL,
                                QuestionType = entity.QuestionType,
                                Options = entity.GetOptions()
                            };

                            if (entity.Id == 0)
                            {
                                quiz.Questions.Add(question);
                            }

                            repository.SaveQuestion(question);

                            entity.Id = question.Id;

                            return true;
                        }
                    }
                    else
                    {
                        messages.Add("quiz was not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                messages.Add(DefaultErrorMessage());
            }

            return messages;
        }
    }
}
