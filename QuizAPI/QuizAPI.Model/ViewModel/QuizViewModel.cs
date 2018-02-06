using QuizAPI.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizAPI.Model
{
    /// <summary>
    /// Represents the status of a quiz.
    /// </summary>
    public enum QuizStatus
    {
        New = 1, //Was just created
        In_Progress = 2, //The user started to answer the questions
        Complete = 3 //The user finished the whole quiz
    }

    /// <summary>
    /// Describe a representation of the entities for a view.
    /// This representation is used for showing quizes in a grid.
    /// Here we restrict the information that is shown.
    /// </summary>
    public class Quiz
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int QtdQuestions { get; set; }
        public int QtdAnswers { get; internal set; }
    }

    /// <summary>
    /// Describe a representation of the entities for a view.
    /// It is a specific view of a quiz
    /// </summary>
    public class PublishedQuiz
    {
        public Quiz Quiz { get; set; }
        public List<Question> Questions { get; set; }
    }

    /// <summary>
    /// Describe a representation of the entities for a view.
    /// This class maps a new or a quiz that is being edited.
    /// </summary>
    public class QuizViewModel
    {
        public Quiz Quiz { get; set; }
        public List<string> UserIds { get; set; }
        public EnumQuizAccess Access { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Url { get; set; }
    }

    /// <summary>
    /// Represents the link of User and Quiz. It is used when assigning a quiz to a user.
    /// </summary>
    public class QuizForUser
    {
        public int Id { get; set; }
        public Quiz Quiz { get; set; }
        public QuizStatus Status { get; set; }
    }

    /// <summary>
    /// Represents the answer of users to determined quiz.
    /// </summary>
    public class UserAnswerForQuiz
    {
        /// </summary>
        /// Answer's Id
        /// <summary>
        public int Id { get; set; }

        /// <summary>
        /// User's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Date and time when it was answered
        /// </summary>
        public DateTime? AnsweredAt { get; set; }

        /// <summary>
        /// Identifies if it is still open
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Identify if it was already evaluated.
        /// </summary>
        public bool Evaluated { get; set; }
    }

    /// <summary>
    /// Represents generic entities that can be represented by Id and Name.
    /// </summary>
    public class ListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
