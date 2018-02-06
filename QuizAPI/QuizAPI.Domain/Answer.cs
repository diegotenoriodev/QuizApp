using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizAPI.Domain
{
    public class Answer
    {
        public int Id { get; set; }
        public DateTime? AnsweredAt { get; set; }

        public UserQuiz User { get; set; }
        public Quiz Quiz { get; set; }

        public List<AnswerQuestionOption> Answers { get; set; }

        public bool IsOpen { get; set; }
        public bool Evaluated { get; set; }
    }

    public abstract class AnswerQuestionOption
    {
        public int Id { get; set; }
        public Question Question { get; set; }
        public Answer Answer { get; set; }
    }

    public class MultipleChoiceAnswer : AnswerQuestionOption
    {
        public List<Choice> MultipleChoice_Choices { get; set; }
    }

    public class Choice
    {
        public int Id { get; set; }
        public int IdChoice { get; set; }
    }

    public class TrueFalseAnswer : AnswerQuestionOption
    {
        public bool Choice { get; set; }
    }

    public enum YesNo
    {
        No = 0,
        Yes = 1
    }

    public class YesNoAnswer : AnswerQuestionOption
    {
        public YesNo Response { get; set; }
    }

    public class OpenEnded : AnswerQuestionOption
    {
        public string Response { get; set; }
    }
}
