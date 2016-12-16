using System.ComponentModel.DataAnnotations;
using ApiQuizGenerator.AppClasses;


namespace ApiQuizGenerator.Models
{
    public class Answer
    {    
        [ColumnName("answer_id")]
        public int Id { get; set; }

        [Required]
        [ColumnName("content")]
        public string Content { get; set; }

        [Required]
        [ColumnName("identifier")]
        public string Identifier { get; set; }

        [ColumnName("attributes")]
        public string Attributes { get; set; }

        [Required]
        [ColumnName("question_id")]
        public int QuestionId { get; set; }
    }
}