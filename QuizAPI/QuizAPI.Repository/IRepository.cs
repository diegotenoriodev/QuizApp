using QuizAPI.Domain;
using System;
using System.Collections.Generic;

namespace QuizAPI.Repository
{
    public interface IRepository : IDisposable
    {
        void SaveUser(UserQuiz user);
        void DeleteUser(UserQuiz user);
        List<UserQuiz> GetUsers(string searchValue = null);
        UserQuiz GetUser(string id);
        UserQuiz GetUserByUsername(string username);
        UserQuiz GetUserByEmail(string email);

        void SaveQuiz(Quiz quiz);
        void DeleteQuiz(Quiz quiz);
        List<Quiz> GetQuizes(string searchValue = null);
        List<Quiz> GetQuizesFromUser(string userId);
        Quiz GetQuiz(int id);
        Quiz GetQuizWithAnswers(int id);

        void SaveQuestion(Question question);
        void DeleteQuestion(Question question);
        Question GetQuestion(int id);

        void SaveAnswer(Answer answer);
        void DeleteAnswer(Answer answer);
        Answer GetAnswer(int id);
        List<Answer> GetAnswers(Quiz quiz);
        List<Answer> GetAnswers(UserQuiz user);

        List<Question> GetQuestions();
        List<string> GetUsersIds(Quiz quiz, bool? answered = null);
        int GetOpenQuizesForUser(UserQuiz userQuiz);
        int GetClosedQuizesFromUser(UserQuiz userQuiz);

        MultipleChoice GetMultipleChoiceOption(int id);

        void SaveChanges();
        Answer GetAnswer(int idQuiz, string userId);
        List<MultipleChoice> GetMultipleChoiceOptions(int idQuestion);
        MultipleChoiceAnswer GetMultipleChoiceAnswer(int idAnswer, int idQuestion);
        MultipleChoiceAnswer GetMultipleChoiceAnswer(int id);
        AnswerQuestionOption GetAnswerQuestionOptionByQuestioId(int idAnswer, int idQuestion);
    }
}
