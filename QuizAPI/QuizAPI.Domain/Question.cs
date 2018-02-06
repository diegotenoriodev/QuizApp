using System.Collections.Generic;

namespace QuizAPI.Domain
{
    public enum QuestionType
    {
        YesNo_Question,
        TrueFalse_Question,
        Multiple_Choice,    
        Open_Ended,
        Image_Chooser//, Rank_Order, Rating_Scale, Semantic_Differential_Scale, Staple_Scale,Constant_Sum
    }

    public class Question
    {

        public int Id { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public QuestionType QuestionType { get; set; }
        public Quiz Quiz { get; set; }
        public List<BaseQuestionOptionType> Options{ get; set; }
    }
}
