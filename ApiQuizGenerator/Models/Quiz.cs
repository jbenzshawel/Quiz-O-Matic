using System;
using Microsoft.EntityFrameworkCore;

namespace ApiQuizGenerator.Models 
{
    public class Quiz 
    {    
        public string Definition { get { return "Quizes"; } }
        
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TypeId { get; set; }

        public string Type { get; set; }
    }


}