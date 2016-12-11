using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiQuizGenerator.Models 
{
    public class Answer
    {    
        public const string Definition = "Answers"; 
        
        public int Id { get; set; }

        public string Content { get; set; }

        public string Identifier { get; set; }

        public string Attributes { get; set; }

        public int QuestionId { get; set; }
    }
}