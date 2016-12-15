using System;
using System.ComponentModel.DataAnnotations;
using ApiQuizGenerator.AppClasses;

namespace ApiQuizGenerator.Models 
{
    public class Quiz 
    {    
        [ColumnName("quiz_id")]
        public Guid Id { get; set; }


        [Required]
        [ColumnName("name")]
        public string Name { get; set; }

        [Required]
        [ColumnName("description")]
        public string Description { get; set; }

        [Required]
        [ColumnName("type_id")]
        public int TypeId { get; set; }

        [ColumnName("type")]
        public string Type { get; set; }

        [ColumnName("created")]
        public DateTime Created { get; set; }

        [ColumnName("updated")]
        public DateTime? Updated { get; set; }

        // used for SQL Commands
        internal static string Definition { get { return "Quizes"; } }
    }
}