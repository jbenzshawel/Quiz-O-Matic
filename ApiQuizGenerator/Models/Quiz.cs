using System;
using System.ComponentModel.DataAnnotations;

namespace ApiQuizGenerator.Models 
{
    public class Quiz 
    {    
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int TypeId { get; set; }

        public string Type { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        // used for SQL Commands
        internal static string Definition { get { return "Quizes"; } }
    
    }
}