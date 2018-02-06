using System;
using System.Collections.Generic;
using System.Text;

namespace QuizAPI.Model
{
    /// <summary>
    /// Describe a representation of the entities for a view.
    /// </summary>
    public class AnswerModelViewBase
    {
        public int Id { get; set; }
        public Question Question { get; set; }
    }

    /// <summary>
    /// Describe a representation of the entities for a view.
    /// </summary>
    public class MultipleChoiceAnswerView : AnswerModelViewBase
    {
        public List<int> IdAnswers { get; set; }
    }

    /// <summary>
    /// Describe a representation of the entities for a view.
    /// </summary>
    public class TrueFalseAnswerView : AnswerModelViewBase
    {
        public bool Option { get; set; }
    }

    /// <summary>
    /// Describe a representation of the entities for a view.
    /// </summary>
    public class OpenEndedAnswerView : AnswerModelViewBase
    {
        public string Content { get; set; }
    }
}
