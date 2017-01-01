using System;
using System.ComponentModel.DataAnnotations;
using ApiQuizGenerator.AppClasses;

namespace ApiQuizGenerator.Models
{
    public class Question
    {           
        [ColumnName("quiz_id")]
        public int Id { get; set; }

        [Required]
        [ColumnName("title")]
        public string Title { get; set; }

        [Required]
        [ColumnName("attributes")]
        public string Attributes { get; set; }

        [Required]
        [ColumnName("quiz_id")]
        public Guid QuizId { get; set; }
    }
}