using QuizAPI.Domain;
using QuizAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizAPI.Model
{
    /// <summary>
    /// The responsibility of this class is to manage Answer entity.
    /// </summary>
    public class AnswerModel : BaseModel
    {
        private string userId;

        /// <summary>
        /// Constructor for AnswerModel
        /// </summary>
        /// <param name="userid">It requires an authenticated user to make use of this class.</param>
        public AnswerModel(string userid)
        {
            this.userId = userid;
        }

        /// <summary>
        /// Overload of SaveAnswer for Open Ended answers.
        /// It will verify if the user is the owner and if the already answer exists.
        /// Create a new answer in case of it is required, or save the values in the
        /// existing answer.
        /// </summary>
        /// <param name="answer">Answer to be persisted</param>
        /// <returns></returns>
        public ResultOperation SaveAnswer(OpenEndedAnswerView answer)
        {
            //It calls the method SaveAnswer that has a default routine for saving, and
            //gives two delegates, the first to be executed in case of Inclusion and the 
            //second in case of Update.
            return this.SaveAnswer(answer,
              repository =>
              {
                  return new Domain.OpenEnded()
                  {
                      Response = answer.Content
                  };
              },
              (repository, updatedItem) =>
              {
                  (updatedItem as Domain.OpenEnded).Response = answer.Content;

                  return true;
              });
        }

        /// <summary>
        /// Overload of SaveAnswer in a true or false request.
        /// It will verify if the user is the owner and if the already answer exists.
        /// Create a new answer in case of it is required, or save the values in the
        /// existing answer.
        /// </summary>
        /// <param name="answer">Answer to be persisted</param>
        /// <returns></returns>
        public ResultOperation SaveAnswer(TrueFalseAnswerView answer)
        {
            //It calls the method SaveAnswer that has a default routine for saving, and
            //gives two delegates, the first to be executed in case of Inclusion and the 
            //second in case of Update.
            return this.SaveAnswer(answer,
              repository =>
              {
                  return new Domain.TrueFalseAnswer()
                  {
                      Choice = answer.Option
                  };
              },
              (repository, updatedItem) =>
              {
                  (updatedItem as Domain.TrueFalseAnswer).Choice = answer.Option;

                  return true;
              });
        }

        /// <summary>
        /// Overload of SaveAnswer for a multiple choice question.
        /// It will verify if the user is the owner and if the already answer exists.
        /// Create a new answer in case of it is required, or save the values in the
        /// existing answer.
        /// </summary>
        /// <param name="answer">Answer to be persisted</param>
        /// <returns></returns>
        public ResultOperation SaveAnswer(MultipleChoiceAnswerView answer)
        {
            //It calls the method SaveAnswer that has a default routine for saving, and
            //gives two delegates, the first to be executed in case of Inclusion and the 
            //second in case of Update.
            return this.SaveAnswer(answer,
                repository =>
                {
                    var newAnswer = new Domain.MultipleChoiceAnswer() { MultipleChoice_Choices = new List<Choice>() };

                    for (int i = 0; i < answer.IdAnswers.Count; i++)
                    {
                        newAnswer.MultipleChoice_Choices.Add(new Choice()
                        {
                            // ensure that the choice exists
                            IdChoice = repository.GetMultipleChoiceOption(answer.IdAnswers[i]).Id
                        });
                    }

                    return newAnswer;
                },
                (repository, updatedItem) =>
                {
                    //in case of update
                    Domain.MultipleChoiceAnswer multipleChoiceAnswer = repository.GetMultipleChoiceAnswer(updatedItem.Id);

                    var deleted = multipleChoiceAnswer.MultipleChoice_Choices.Where(
                                        r => !answer.IdAnswers.Contains(r.IdChoice)
                                    ).ToList();

                    foreach (var item in answer.IdAnswers.Where(r =>
                                            !multipleChoiceAnswer.MultipleChoice_Choices.Any(choice => choice.IdChoice == r)))
                    {
                        multipleChoiceAnswer.MultipleChoice_Choices.Add
                        (
                            new Choice()
                            {
                                // ensure that the choice exists
                                IdChoice = repository.GetMultipleChoiceOption(item).Id
                            }
                        );
                    }

                    if (deleted != null && deleted.Any())
                    {
                        deleted.ForEach(r => multipleChoiceAnswer.MultipleChoice_Choices.Remove(r));
                    }

                    return true;
                });
        }

        /// <summary>
        /// This method currently sets the Evaluated property for a given answer to true.
        /// Before making any change, it identifies the creator for this quiz and verifies 
        /// if the creator is the user that is logged in.
        /// </summary>
        /// <param name="idAnswer"></param>
        /// <returns></returns>
        public ResultOperation EvaluateAnswer(int idAnswer)
        {
            try
            {
                using (IRepository repository = Repository.Repository.CreateRepository())
                {
                    var answer = repository.GetAnswer(idAnswer);

                    if (answer.Quiz.Creator.Id == userId && !answer.Evaluated)
                    {
                        answer.IsOpen = false;
                        answer.Evaluated = true;
                        repository.SaveAnswer(answer);

                        return true;
                    }
                    else
                    {
                        messages.Add("Quiz was already evaluated");
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                messages.Add(ex.Message);
            }

            return messages;
        }

        /// <summary>
        /// This method is responsible for marking a answer as Closed (IsOpen = false).
        /// Before making any change, it verifies if the user logged in is the user that
        /// is marked to answer this quiz.
        /// </summary>
        /// <param name="idAnswer"></param>
        /// <returns></returns>
        public ResultOperation FinishAnswer(int idAnswer)
        {
            try
            {
                using (IRepository repository = Repository.Repository.CreateRepository())
                {
                    var answer = repository.GetAnswer(idAnswer);

                    if (answer.User.Id == userId && !answer.Evaluated)
                    {
                        answer.IsOpen = false;
                        answer.AnsweredAt = DateTime.Now;
                        repository.SaveAnswer(answer);
                        return true;
                    }
                    else
                    {
                        messages.Add("Quiz was already finished");
                    }
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
                messages.Add(ex.Message);
            }

            return messages;
        }

        /// <summary>
        /// Gets a answer for a quiz/question for the logged user.
        /// </summary>
        /// <param name="idAnswer"></param>
        /// <param name="idQuestion"></param>
        /// <returns></returns>
        public AnswerModelViewBase GetAnswer(int idAnswer, int idQuestion)
        {
            using (IRepository repository = Repository.Repository.CreateRepository())
            {
                Domain.Answer answer = repository.GetAnswer(idAnswer);

                if (answer != null)
                {
                    if (answer.User.Id == userId || answer.Quiz.Creator.Id == userId)
                    {
                        return CreateInfoAnswer(repository, answer, idQuestion);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Return the answer for a question. But it returns for the creator of this quiz.
        /// </summary>
        /// <param name="idAnswer"></param>
        /// <param name="idQuestion"></param>
        /// <returns></returns>
        public AnswerModelViewBase GetAnswerReport(int idAnswer, int idQuestion)
        {
            using (IRepository repository = Repository.Repository.CreateRepository())
            {
                Domain.Answer answer = repository.GetAnswer(idAnswer);

                if (answer != null)
                {
                    if (answer.Quiz.Creator.Id == userId || answer.User.Id == userId)
                    {
                        return CreateInfoAnswer(repository, answer, idQuestion);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Auxiliar method for getting the persisted answer for a quiz application and determined question.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="answer"></param>
        /// <param name="idQuestion"></param>
        /// <returns></returns>
        private AnswerModelViewBase CreateInfoAnswer(IRepository repository,
                            Answer answer, int idQuestion)
        {
            AnswerQuestionOption infoAnswer = repository.GetAnswerQuestionOptionByQuestioId(answer.Id, idQuestion);

            if (infoAnswer != null)
            {
                AnswerModelViewBase returnAnswer = CreateAnswerModelView(repository, answer, idQuestion, infoAnswer);

                if (returnAnswer != null)
                {
                    returnAnswer.Id = infoAnswer.Id;
                    return returnAnswer;
                }
            }

            return null;
        }

        /// <summary>
        /// Create the specific AnswerView based on the question type.
        /// Can return a TrueFalseAnswerview, MultipleChoiceAnswerView or OpenEndedAnswerView
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="answer"></param>
        /// <param name="idQuestion"></param>
        /// <param name="infoAnswer"></param>
        /// <returns></returns>
        private AnswerModelViewBase CreateAnswerModelView(IRepository repository,
                            Answer answer, int idQuestion, Domain.AnswerQuestionOption infoAnswer)
        {
            AnswerModelViewBase returnAnswer = null;

            switch (repository.GetQuestion(idQuestion).QuestionType)
            {
                case QuestionType.TrueFalse_Question:
                    returnAnswer = new TrueFalseAnswerView() { Option = (infoAnswer as TrueFalseAnswer).Choice };
                    break;
                case QuestionType.Multiple_Choice:
                    returnAnswer = new MultipleChoiceAnswerView() { IdAnswers = new List<int>() };

                    foreach (var item in repository.GetMultipleChoiceAnswer(answer.Id, idQuestion).MultipleChoice_Choices)
                    {
                        (returnAnswer as MultipleChoiceAnswerView).IdAnswers.Add(item.IdChoice);
                    }

                    break;
                case QuestionType.Open_Ended:
                    returnAnswer = new OpenEndedAnswerView() { Content = (infoAnswer as OpenEnded).Response };
                    break;
            }

            return returnAnswer;
        }

        /// <summary>
        /// Implements the default behavior for saving an answer, such as checking
        /// the ownership of that quiz, identifying the parameters and etc.
        /// At the end of the execution, this method will delegate the operation to
        /// one of the functions sent as parameter, depending on the results encontered here.
        /// </summary>
        /// <param name="answer">The answer to be persisted</param>
        /// <param name="insert">The instructions for including a new value</param>
        /// <param name="update">The instructions for updating a item that is already in the database</param>
        /// <returns></returns>
        protected ResultOperation SaveAnswer(AnswerModelViewBase answer,
                                            Func<IRepository, AnswerQuestionOption> insert,
                                            Func<IRepository, AnswerQuestionOption, bool> update)
        {
            try
            {
                // Disposing repository to avoid overloading the data context
                using (IRepository repository = Repository.Repository.CreateRepository())
                {
                    // Loading the quiz from the database
                    Domain.Quiz quiz = repository.GetQuiz(answer.Question.IdQuiz);

                    // Loading the answer for that qu iz and for that user
                    Domain.Answer savedAnswer = repository.GetAnswer(quiz.Id, userId);

                    if (savedAnswer.Id == answer.Id)
                    {
                        // If there is no answer, it might mean that someone is trying to send bad data.
                        // We also will allow only changes to quizes that are open and have not been evaluated
                        if (savedAnswer != null && savedAnswer.IsOpen && !savedAnswer.Evaluated)
                        {
                            // Making sure that the question received belongs to that quiz
                            var question = quiz.Questions.FirstOrDefault(r => r.Id == answer.Question.Id);
                            if (question != null)
                            {
                                SaveAnswer(savedAnswer, answer, repository, question, insert, update);
                            }
                        }
                        else
                        {
                            messages.Add("There is no quiz active for this question!");
                        }
                    }
                    else
                    {
                        messages.Add("There is no quiz active for this question!");
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
        /// Executes the default behavior for saving an aswer.
        /// </summary>
        /// <param name="savedAnswer"></param>
        /// <param name="answer"></param>
        /// <param name="repository"></param>
        /// <param name="question"></param>
        /// <param name="insert"></param>
        /// <param name="update"></param>
        private void SaveAnswer(Answer savedAnswer, AnswerModelViewBase answer, IRepository repository, Domain.Question question, Func<IRepository, AnswerQuestionOption> insert, Func<IRepository, AnswerQuestionOption, bool> update)
        {
            // If it came until here, we know that it is likely to be a valid request
            savedAnswer.AnsweredAt = DateTime.Now; //Changing to know the last time the user answered

            // Loading the last option that the user selected
            AnswerQuestionOption answerOption = null;

            if (savedAnswer.Answers != null)
            {
                answerOption = savedAnswer.Answers.FirstOrDefault(r => r.Question.Id == answer.Question.Id);
            }
            else
            {
                savedAnswer.Answers = new List<AnswerQuestionOption>();
            }

            // If it is null, means that the user have not answered the question yet
            if (answerOption == null)
            {
                answerOption = insert(repository);
                answerOption.Question = question;
                savedAnswer.Answers.Add(answerOption);
            }
            else
            {
                update(repository, answerOption);
            }

            repository.SaveChanges();
        }
    }
}
