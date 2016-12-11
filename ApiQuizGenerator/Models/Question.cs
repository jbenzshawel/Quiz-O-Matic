using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiQuizGenerator.Models 
{
    public class Question
    {   
        public int Id { get; set; }

        public string Title { get; set; }

        public string Attributes { get; set; }

        public Guid QuizId { get; set; }
        
        // used for SQL Commands
        internal static string Definition { get { return "Questions"; } }
    }
}