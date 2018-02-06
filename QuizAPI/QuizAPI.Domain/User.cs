using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace QuizAPI.Domain
{
    public class UserQuiz : IdentityUser
    {
        public UserQuiz()
        {
            Registration = DateTime.Now;
        }

        public DateTime Registration { get; set; }

        public List<Quiz> Quizes { get; set; }
    }
}
