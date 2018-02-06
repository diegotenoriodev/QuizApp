using System;
using System.Collections.Generic;
using System.Text;

namespace QuizAPI.Domain
{
    public enum EnumQuizAccess
    {
        Public = 1,
        Private = 2
    }

    public class Quiz
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Question> Questions { get; set; }
        public UserQuiz Creator { get; set; }
        public bool IsPublished { get; set; }

        public EnumQuizAccess Access { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Url { get; set; }

        public List<Answer> Answers { get; set; }
    }
}
