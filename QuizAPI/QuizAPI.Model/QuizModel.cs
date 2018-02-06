using QuizAPI.Domain;
using QuizAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizAPI.Model
{
    /// <summary>
    /// Responsible for managing business rules for a quiz.
    /// </summary>
    public class QuizModel : BaseModel<Quiz>
    {
        private string currentUserId;

        /// <summary>
        /// Only authenticated users can make access of this class.
        /// </summary>
        /// <param name="userId"></param>
        public QuizModel(string userId)
        {
            this.currentUserId = userId;
        }

        /// <summary>
        /// Delete a quiz if CanDeleteQuiz returns true.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override ResultOperation Delete(int id)
        {
            try
            {
                using (IRepository repository = Repository.Repository.CreateRepository())
                {
                    Domain.Quiz quiz = repository.GetQuiz(id);

                    if (CanDeleteQuiz(repository, quiz))
                    {
                        repository.DeleteQuiz(quiz);
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
        /// Identifies if a quiz can be removed, considering
        /// The logged user;
        /// If there are any questions;
        /// Or if there are answers for this quiz.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="quiz"></param>
        /// <returns></returns>
        private bool CanDeleteQuiz(IRepository repository, Domain.Quiz quiz)
        {
            if (quiz != null && quiz.Creator == repository.GetUser(this.currentUserId))
            {
                if (quiz.Questions.Any())
                {
                    messages.Add("There are questions for this quiz. Please delete the questions before.");
                }
                else if (repository.GetAnswers(quiz).Any())
                {
                    messages.Add("This quiz has answers, it cannot be removed.");
                }
            }
            else
            {
                messages.Add("Item was not found.");
            }

            return !messages.Any();
        }

        /// <summary>
        /// Get information for publishing quiz
        /// </summary>
        /// <param name="idAnswer"></param>
        /// <returns></returns>
        public PublishedQuiz GetPublishedQuiz(int idAnswer)
        {
            using (IRepository repository = Repository.Repository.CreateRepository())
            {
                Domain.Answer answer = repository.GetAnswer(idAnswer);

                if (answer != null && answer.User.Id == currentUserId)
                {
                    return CreatePublishedQuiz(answer.Quiz);
                }
            }

            return null;
        }

        /// <summary>
        /// Get information for quiz/answer for showing in finished/report routine.
        /// </summary>
        /// <param name="idAnswer"></param>
        /// <returns></returns>
        public PublishedQuiz GetFinishedQuiz(int idAnswer)
        {
            using (IRepository repository = Repository.Repository.CreateRepository())
            {
                Domain.Answer answer = repository.GetAnswer(idAnswer);

                if (answer != null && answer.Quiz.Creator.Id == currentUserId)
                {
                    return CreatePublishedQuiz(answer.Quiz);
                }
            }

            return null;
        }

        /// <summary>
        /// Creates a PublishedQuiz view model for a quiz.
        /// </summary>
        /// <param name="quiz"></param>
        /// <returns></returns>
        private PublishedQuiz CreatePublishedQuiz(Domain.Quiz quiz)
        {
            if (quiz != null)
            {
                return new PublishedQuiz()
                {
                    Quiz = new Quiz()
                    {
                        Id = quiz.Id,
                        Name = quiz.Name,
                        Description = quiz.Description
                    },
                    Questions = quiz.Questions.Select(r => new Question()
                    {
                        Id = r.Id,
                        Description = r.Description,
                        IdQuiz = quiz.Id,
                        ImageURL = r.ImageURL,
                        QuestionType = r.QuestionType
                    }).ToList()
                };
            }

            return null;
        }

        /// <summary>
        /// Returns a quiz based on its Id and its creator.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Quiz Get(int id)
        {
            using (IRepository repository = Repository.Repository.CreateRepository())
            {
                Domain.Quiz quiz = repository.GetQuiz(id);

                if (quiz != null && quiz.Creator == repository.GetUser(this.currentUserId))
                {
                    return new Quiz() { Id = quiz.Id, Name = quiz.Name, Description = quiz.Description };
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the list of publications/answers for a specific quiz.
        /// </summary>
        /// <param name="idquiz"></param>
        /// <param name="urlBase"></param>
        /// <returns></returns>
        public QuizViewModel GetPublicationsForQuiz(int idquiz, string urlBase)
        {
            using (IRepository repository = Repository.Repository.CreateRepository())
            {
                Domain.Quiz quiz = repository.GetQuiz(idquiz);

                QuizViewModel returnedQuiz = null;

                if (quiz != null && quiz.Creator == repository.GetUser(this.currentUserId))
                {
                    returnedQuiz = new QuizViewModel()
                    {
                        Access = quiz.Access,
                        ExpirationDate = quiz.ExpirationDate,
                        UserIds = new List<string>(),
                        Url = quiz.Url ?? $"{urlBase}/{quiz.Id}/{quiz.Name}",
                        Quiz = new Quiz()
                        {
                            Description = quiz.Description,
                            Id = quiz.Id,
                            Name = quiz.Name
                        }
                    };

                    if (returnedQuiz.Access == Domain.EnumQuizAccess.Private)
                    {
                        returnedQuiz.UserIds.AddRange(repository.GetUsersIds(quiz));
                    }
                }

                return returnedQuiz;
            }

        }

        /// <summary>
        /// Return the quizes that a user has to answer, or answered.
        /// </summary>
        /// <returns></returns>
        public List<QuizForUser> GetQuizesForUser()
        {
            var returnList = new List<QuizForUser>();

            using (IRepository repository = Repository.Repository.CreateRepository())
            {
                List<Domain.Answer> answers = repository.GetAnswers(repository.GetUser(this.currentUserId));

                if (answers != null)
                {
                    foreach (var item in answers)
                    {
                        returnList.Add(CreateNewQuizForUser(item));
                    }
                }
            }

            return returnList;
        }

        /// <summary>
        /// Returns the list of answers for a quiz that the user created.
        /// </summary>
        /// <param name="idQuiz"></param>
        /// <returns></returns>
        public List<UserAnswerForQuiz> GetQuizesForOwner(int idQuiz)
        {
            var returnList = new List<UserAnswerForQuiz>();

            if(idQuiz != 0)
            {
                using (IRepository repository = Repository.Repository.CreateRepository())
                {
                    Domain.Quiz quiz = repository.GetQuiz(idQuiz);

                    if (quiz != null && quiz.Creator.Id == currentUserId)
                    {
                        List<Domain.Answer> answers = repository.GetAnswers(quiz);

                        if (answers != null)
                        {
                            foreach (var answer in answers)
                            {
                                returnList.Add(new UserAnswerForQuiz()
                                {
                                    Id = answer.Id,
                                    AnsweredAt = answer.AnsweredAt,
                                    Evaluated = answer.Evaluated,
                                    IsOpen = answer.IsOpen,
                                    Name = answer.User.UserName
                                });
                            }
                        }
                    }
                }
            }

            return returnList;
        }

        private QuizForUser CreateNewQuizForUser(Answer item)
        {
            QuizForUser quizForUser = new QuizForUser()
            {
                Id = item.Id,
                Quiz = new Quiz()
                {
                    Description = item.Quiz.Description,
                    Id = item.Quiz.Id,
                    Name = item.Quiz.Name
                }
            };

            if (item.AnsweredAt.HasValue)
            {
                if (item.IsOpen && !item.Evaluated)
                {
                    quizForUser.Status = QuizStatus.In_Progress;
                }
                else
                {
                    quizForUser.Status = QuizStatus.Complete;
                }
            }
            else
            {
                quizForUser.Status = QuizStatus.New; //The user have not started yet
            }

            return quizForUser;
        }

        public int GetQtdOpen()
        {
            using (IRepository repository = Repository.Repository.CreateRepository())
            {
                return repository.GetOpenQuizesForUser(repository.GetUser(this.currentUserId));
            }
        }

        public int GetQtdClosed()
        {
            using (IRepository repository = Repository.Repository.CreateRepository())
            {
                return repository.GetClosedQuizesFromUser(repository.GetUser(this.currentUserId));
            }
        }

        public ResultOperation SavePublicationForQuiz(QuizViewModel quizModelView)
        {
            try
            {
                using (IRepository repository = Repository.Repository.CreateRepository())
                {
                    Domain.Quiz quiz = repository.GetQuizWithAnswers(quizModelView.Quiz.Id);

                    if (quiz != null && quiz.Creator == repository.GetUser(this.currentUserId))
                    {
                        quiz.IsPublished = true;
                        quiz.Access = quizModelView.Access;
                        quiz.ExpirationDate = quizModelView.ExpirationDate.Value;
                        quiz.Url = quizModelView.Url;

                        if (quizModelView.UserIds != null)
                        {
                            // new users to be added
                            var newUsers = quizModelView.UserIds.Where(
                                                r => !quiz.Answers.Any(answer => answer.User.Id == r)).ToList();

                            quiz.Answers.RemoveAll(r => !quizModelView.UserIds.Contains(r.User.Id));

                            // Adding new items to the list
                            foreach (var item in newUsers)
                            {
                                quiz.Answers.Add(new Domain.Answer()
                                {
                                    IsOpen = true,
                                    User = repository.GetUser(item)
                                });
                            }
                            repository.SaveQuiz(quiz);

                            return true;
                        }
                    }
                    else
                    {
                        messages.Add("Quiz was not found.");
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

        public List<Quiz> GetAll()
        {
            using (IRepository repository = Repository.Repository.CreateRepository())
            {
                var quizes = repository.GetQuizesFromUser(this.currentUserId);

                if (quizes != null)
                {
                    return quizes.Select(r => new Quiz() {
                        Id = r.Id,
                        Name = r.Name,
                        Description = r.Description,
                        QtdQuestions = r.Questions.Count,
                        QtdAnswers = r.Answers.Count}).ToList();
                }

            }
            return null;
        }

        private bool VerifyFields(Quiz entity)
        {
            if (string.IsNullOrEmpty(entity.Name))
            {
                messages.Add("Please inform the name");
            }
            if (string.IsNullOrEmpty(entity.Description))
            {
                messages.Add("Please inform the description");
            }

            return messages.Count == 0;
        }

        public override ResultOperation Save(Quiz entity)
        {
            try
            {
                using (IRepository repository = Repository.Repository.CreateRepository())
                {
                    if (VerifyFields(entity))
                    {
                        Domain.Quiz quiz = null;

                        if (entity.Id != 0)
                        {
                            quiz = repository.GetQuiz(entity.Id);
                            if (quiz == null)
                            {
                                return "Quiz was not found.";
                            }
                            else if (quiz.Creator != repository.GetUser(this.currentUserId))
                            {
                                return "Quiz was not found.";
                            }
                        }
                        else
                        {
                            quiz = new Domain.Quiz() { Creator = repository.GetUser(this.currentUserId) };
                        }

                        quiz.Name = entity.Name;
                        quiz.Description = entity.Description;

                        repository.SaveQuiz(quiz);

                        entity.Id = quiz.Id;

                        return true;
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
