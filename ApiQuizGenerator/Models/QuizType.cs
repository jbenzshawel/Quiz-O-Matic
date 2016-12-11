using System;
using Microsoft.EntityFrameworkCore;

namespace ApiQuizGenerator.Models 
{
    public class QuizType
    {   
        public int Id { get; set; }

        public string Type { get; set; }

        internal string Definition { get { return "QuizType"; } }
    }
}