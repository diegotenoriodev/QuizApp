using Newtonsoft.Json;
using QuizAPI.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QuizAPI.Model
{
    /// <summary>
    /// Describe a representation of the entities for a view.
    /// </summary>
    public class Question
    {
        public int Id { get; set; }

        public int IdQuiz { get; set; }

        public string Description { get; set; }

        public string ImageURL { get; set; }

        public Domain.QuestionType QuestionType { get; set; }

        /// <summary>
        /// This property was created to overcome the problems of serialization of objects
        /// that have the same base class.
        /// </summary>
        public List<object> Options { get; set; }

        /// <summary>
        /// This function gets the returned json and casts into objects, depending on the property
        /// QuestionType.
        /// </summary>
        /// <returns></returns>
        public List<Domain.BaseQuestionOptionType> GetOptions()
        {
            List<Domain.BaseQuestionOptionType> returnItems = new List<Domain.BaseQuestionOptionType>();

            foreach (var item in Options)
            {
                switch (this.QuestionType)
                {
                    case Domain.QuestionType.TrueFalse_Question:
                        returnItems.Add(JsonConvert.DeserializeObject<Domain.TrueFalseOption>(item.ToString()));
                        break;
                    case Domain.QuestionType.Multiple_Choice:
                        returnItems.Add(JsonConvert.DeserializeObject<Domain.MultipleChoice>(item.ToString()));
                        break;
                    case Domain.QuestionType.Image_Chooser:
                        returnItems.Add(JsonConvert.DeserializeObject<Domain.ImageChoice>(item.ToString()));
                        break;
                    default:
                        break;
                }
            }

            return returnItems;
        }
    }

}
