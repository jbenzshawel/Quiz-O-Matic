using System;
using System.ComponentModel.DataAnnotations;

namespace ApiQuizGenerator.Models
{
    public class Question
    {   
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Attributes { get; set; }

        [Required]
        public Guid QuizId { get; set; }
        
        // used for SQL Commands
        internal static string Definition { get { return "Questions"; } }
    }
}