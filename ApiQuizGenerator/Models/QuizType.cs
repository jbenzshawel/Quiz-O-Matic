using System;
using Microsoft.EntityFrameworkCore;

namespace ApiQuizGenerator.Models 
{
    public class QuizType
    {    
        public const string Definition = "QuizType"; 
        
        public int Id { get; set; }

        public string Type { get; set; }
    }
}