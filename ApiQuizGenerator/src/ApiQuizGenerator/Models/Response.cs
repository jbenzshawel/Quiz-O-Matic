using System;
using System.ComponentModel.DataAnnotations;
using ApiQuizGenerator.AppClasses;

namespace ApiQuizGenerator.Models
{
    public class Response
    {
        [ColumnName("response_id")]
        public int Id { get; set; }

        [Required]
        [ColumnName("quiz_id")]
        public Guid QuizId { get; set; }

        [Required]
        [ColumnName("question_id")]
        public int QuestionId { get; set; }

        [Required]
        [ColumnName("value")]
        public string Value { get; set; }

        [ColumnName("created")]
        public DateTime Created { get; set; }
    }
}