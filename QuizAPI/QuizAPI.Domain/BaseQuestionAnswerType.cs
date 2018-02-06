using Newtonsoft.Json;
using System.Collections.Generic;

namespace QuizAPI.Domain
{
    public abstract class BaseQuestionOptionType
    {
        public int Id { get; set; }

        [JsonIgnore]
        public Question Question { get; set; }

        public abstract void Merge(BaseQuestionOptionType newValues);
    }
    
    public class TrueFalseOption : BaseQuestionOptionType
    {
        public bool Answer { get; set; }
        public override void Merge(BaseQuestionOptionType newValues)
        {
            this.Answer = (newValues as TrueFalseOption).Answer;
        }
    }

    public class MultipleChoice : BaseQuestionOptionType
    {
        public int Order { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }
        
        public override void Merge(BaseQuestionOptionType newValues)
        {
            var answer = (newValues as MultipleChoice);

            this.Order = answer.Order;
            this.Content = answer.Content;
            this.IsCorrect = answer.IsCorrect;
        }
    }
    
    public class ImageChoice : BaseQuestionOptionType
    {
        public int Order { get; set; }
        public string ImageURL { get; set; }
        public bool IsCorrect { get; set; }

        public override void Merge(BaseQuestionOptionType newValues)
        {
            var answer = (newValues as ImageChoice);

            this.Order = answer.Order;
            this.ImageURL = answer.ImageURL;
            this.IsCorrect = answer.IsCorrect;
        }
    }
}
