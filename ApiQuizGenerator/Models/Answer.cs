namespace ApiQuizGenerator.Models
{
    public class Answer
    {    
        public int Id { get; set; }

        public string Content { get; set; }

        public string Identifier { get; set; }

        public string Attributes { get; set; }

        public int QuestionId { get; set; }

         // used for SQL Commands
        internal static string Definition { get { return "Answers"; } }
        
    }
}