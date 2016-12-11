using System;
using Microsoft.EntityFrameworkCore;

namespace ApiQuizGenerator.Models 
{
    public class Quiz 
    {    
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TypeId { get; set; }

        public string Type { get; set; }

        // used for SQL Commands
        internal string Definition { get { return "Quizes"; } }
    
    }


}