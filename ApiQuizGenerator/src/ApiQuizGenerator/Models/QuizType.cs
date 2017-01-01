using ApiQuizGenerator.AppClasses;
using System.ComponentModel.DataAnnotations;

namespace ApiQuizGenerator.Models
{
    public class QuizType
    {   
        [ColumnName("type_id")]
        public int Id { get; set; }

        [Required]
        [ColumnName("type")]
        public string Type { get; set; }
    }
}