using Microsoft.EntityFrameworkCore;
using QuizAPI.Domain;
using System.Collections.Generic;
using System.Linq;

namespace QuizAPI.Repository
{
    public class Repository : IRepository
    {
        public static string ConnectionString { get; set; }
        private IQuizDbContext DB;

        private Repository()
        {
            DB = new QuizDbContext(ConnectionString);
        }

        public static IRepository CreateRepository()
        {
            return new Repository();
        }

        public void DeleteAnswer(Answer answer)
        {
            DB.Answers.Remove(answer);
            DB.SaveChanges();
        }

        public void DeleteQuestion(Question question)
        {
            DB.Questions.Remove(question);
            DB.SaveChanges();
        }

        public void DeleteQuiz(Quiz quiz)
        {
            DB.Quizes.Remove(quiz);
            DB.SaveChanges();
        }

        public void DeleteUser(UserQuiz user)
        {
            DB.Users.Remove(user);
            DB.SaveChanges();
        }

        public Answer GetAnswer(int id)
        {
            return DB.Answers.Include("Quiz.Questions").Include("Quiz.Creator").Include("User").FirstOrDefault(r => r.Id == id);
        }

        public List<Answer> GetAnswers(Quiz quiz)
        {
            return DB.Answers.Where(r => r.Quiz == quiz).ToList();
        }

        public List<Answer> GetAnswers(UserQuiz user)
        {
            return DB.Answers.Include("Quiz").Where(r => r.User == user).ToList();
        }

        public Quiz GetQuiz(int id)
        {
            return DB.Quizes.Include("Answers.User").Include("Questions.Options").Include("Creator").FirstOrDefault(r => r.Id == id);
        }

        public Quiz GetQuizWithAnswers(int id)
        {
            return DB.Quizes.Include("Answers.User").Include("Creator").FirstOrDefault(r => r.Id == id);
        }

        public List<Quiz> GetQuizes(string searchValue = null)
        {
            var query = from r in DB.Quizes select r;

            if (searchValue != null)
            {
                foreach (var item in searchValue.Split(' '))
                {
                    query = from r in query
                            where r.Name.Contains(item)
                            || r.Description.Contains(item)
                            select r;
                }
            }

            return query.ToList();
        }


        public List<Quiz> GetQuizesFromUser(string userId)
        {
            var query = from r in DB.Quizes.Include("Questions").Include("Answers")
                        where r.Creator.Id == userId
                        select r;

            return query.ToList();
        }
        public UserQuiz GetUser(string id)
        {
            return DB.Users.FirstOrDefault(r => r.Id == id);
        }

        public UserQuiz GetUserByUsername(string username)
        {
            return DB.Users.FirstOrDefault(r => r.Email == username);
        }

        public UserQuiz GetUserByEmail(string email)
        {
            return DB.Users.FirstOrDefault(r => r.Email == email);
        }

        public List<UserQuiz> GetUsers(string searchValue = null)
        {
            var query = from r in DB.Users select r;

            if (searchValue != null)
            {
                foreach (var item in searchValue.Split(' '))
                {
                    query = from r in query
                            where r.Email.Contains(item)
                            select r;
                }
            }

            return query.ToList();
        }

        public void SaveAnswer(Answer answer)
        {
            if (answer.Id == 0)
            {
                DB.Answers.Add(answer);
            }
            else
            {
                var oldAnswer = DB.Answers.FirstOrDefault(r => r.Id == answer.Id);

                oldAnswer.AnsweredAt = answer.AnsweredAt;
                oldAnswer.Answers = answer.Answers;
                oldAnswer.Quiz = answer.Quiz;
                oldAnswer.User = answer.User;
                oldAnswer.IsOpen = answer.IsOpen;
                oldAnswer.Evaluated = answer.Evaluated;
            }

            DB.SaveChanges();
        }

        public void SaveQuestion(Question question)
        {
            if (question.Id != 0)
            {
                Question oldQuestion = GetQuestion(question.Id);

                oldQuestion.Description = question.Description;
                oldQuestion.ImageURL = question.ImageURL;
                oldQuestion.QuestionType = question.QuestionType;

                var oldOptions = oldQuestion.Options;
                var newOptions = question.Options;

                // Removing
                oldQuestion.Options.RemoveAll(o => !newOptions.Any(n => o.Id == n.Id));

                foreach (var item in newOptions)
                {
                    if (item.Id == 0) //new option included in this post
                    {
                        oldQuestion.Options.Add(item);
                    }
                    else
                    {
                        oldQuestion.Options.FirstOrDefault(r => r.Id == item.Id).Merge(item);
                    }
                }
            }
            else
            {
                DB.Questions.Add(question);
            }

            DB.SaveChanges();
        }

        public void SaveQuiz(Quiz quiz)
        {
            if (quiz.Id != 0)
            {
                Quiz oldQuiz = GetQuiz(quiz.Id);

                oldQuiz.Name = quiz.Name;
                oldQuiz.Description = quiz.Description;

                oldQuiz.Answers = quiz.Answers;

                oldQuiz.IsPublished = quiz.IsPublished;
                oldQuiz.Access = quiz.Access;
                oldQuiz.ExpirationDate = quiz.ExpirationDate;
                oldQuiz.Url = quiz.Url;
            }
            else
            {
                DB.Quizes.Add(quiz);
            }
            DB.SaveChanges();
        }

        public void SaveUser(UserQuiz user)
        {
        }

        public Question GetQuestion(int id)
        {
            return DB.Questions.Include("Options").Include("Quiz.Creator").FirstOrDefault(r => r.Id == id);
        }

        public List<Question> GetQuestions()
        {
            return DB.Questions.ToList();
        }

        public List<string> GetUsersIds(Quiz quiz, bool? answered = null)
        {
            var query = from r in DB.Answers.Include("User")
                        where r.Quiz == quiz
                        select r;

            if (answered.HasValue)
            {
                query = query.Where(r => r.IsOpen == !answered.Value);
            }

            return query.Select(r => r.User.Id).ToList();
        }

        public int GetOpenQuizesForUser(UserQuiz userQuiz)
        {
            var query = from r in DB.Answers
                        where r.User == userQuiz
                        && r.IsOpen
                        && !r.Evaluated
                        select r;

            return query.Count();
        }

        public int GetClosedQuizesFromUser(UserQuiz userQuiz)
        {
            var query = from r in DB.Answers
                        where r.Quiz.Creator == userQuiz
                        && !r.IsOpen
                        && !r.Evaluated
                        select r;

            return query.Count();
        }

        public void Dispose()
        {
            this.DB.Dispose();
        }

        public void SaveChanges()
        {
            DB.SaveChanges();
        }

        public MultipleChoice GetMultipleChoiceOption(int id)
        {
            return DB.MultipleChoices.FirstOrDefault(r => r.Id == id);
        }

        public List<MultipleChoice> GetMultipleChoiceOptions(int idQuestion)
        {
            var query = from r in DB.MultipleChoices
                        where r.Question.Id == idQuestion
                        select r;

            return query.ToList();
        }

        public MultipleChoiceAnswer GetMultipleChoiceAnswer(int idAnswer, int idQuestion)
        {
            var query = from r in DB.MultipleChoiceAnswers.Include("MultipleChoice_Choices")
                        where r.Question.Id == idQuestion 
                        && r.Answer.Id == idAnswer
                        select r;

            return query.FirstOrDefault();
        }

        public MultipleChoiceAnswer GetMultipleChoiceAnswer(int id)
        {
            var query = from r in DB.MultipleChoiceAnswers.Include("MultipleChoice_Choices")
                        where r.Id == id
                        select r;

            return query.FirstOrDefault();
        }

        public Answer GetAnswer(int idQuiz, string userId)
        {
            return DB.Answers
                .Include("Answers.Question")
                .Include("Quiz.Questions")
                .FirstOrDefault(r => r.User.Id == userId && r.Quiz.Id == idQuiz);
        }

        public AnswerQuestionOption GetAnswerQuestionOptionByQuestioId(int idAnswer, int idQuestion)
        {
            var query = from answer in DB.Answers
                        where answer.Id == idAnswer
                        select answer.Answers.FirstOrDefault(r => r.Question.Id == idQuestion);

            return query.FirstOrDefault();

        }
    }
}
