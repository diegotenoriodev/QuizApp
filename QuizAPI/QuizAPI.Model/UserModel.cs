using System.Collections.Generic;
using System.Linq;

namespace QuizAPI.Model
{
    public class UserQuizPublish
    {
        public string IdUser { get; set; }
        public string Username { get; set; }
        public int IdQuiz { get; set; }
        public bool IsMarked { get; set; }
        public bool HasAnswered { get; set; }
    }

    /// <summary>
    /// Responsible for managing some information about users.
    /// </summary>
    public class UserModel
    {
        public List<UserQuizPublish> GetListOfUsersForPublication(int idquiz, string userId)
        {
            using (Repository.IRepository repository = QuizAPI.Repository.Repository.CreateRepository())
            {
                Domain.Quiz quiz = repository.GetQuiz(idquiz);

                if (quiz.Creator.Id == userId)
                {
                    List<UserQuizPublish> returnList = new List<UserQuizPublish>();

                    if (quiz != null)
                    {
                        //List of users that have been marked to answer the quiz
                        List<string> usersMarkedToAnswer = repository.GetUsersIds(quiz);

                        //List of users that have an answer in the database for that quiz
                        List<string> usersThatAnswered = repository.GetUsersIds(quiz, true);

                        List<Domain.UserQuiz> users = repository.GetUsers();

                        foreach (var item in users)
                        {
                            if (item.Id != userId)
                            {
                                returnList.Add(new UserQuizPublish()
                                {
                                    IdQuiz = idquiz,
                                    IdUser = item.Id,
                                    IsMarked = usersMarkedToAnswer.Any(r => r == item.Id),
                                    Username = item.UserName,
                                    HasAnswered = usersThatAnswered.Any(r => r == item.Id)
                                });
                            }
                        }
                    }

                    return returnList;
                }
            }

            return null;
        }
    }
}