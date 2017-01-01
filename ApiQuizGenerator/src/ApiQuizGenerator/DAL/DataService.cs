using ApiQuizGenerator.Models;

namespace ApiQuizGenerator.DAL
{
    public interface IDataService 
    {
        IRepository<Quiz> Quizes { get; set; }

        IRepository<QuizType> QuizTypes { get; set; }

        IRepository<Question> Questions { get; set; }
        
        IRepository<Answer> Answers { get; set; }

        IRepository<Response> Responses { get; set; }        
    }

    public class DataService : IDataService
    {
       public IRepository<Quiz> Quizes { get; set;}

       public IRepository<QuizType> QuizTypes { get; set; }

       public IRepository<Question> Questions { get; set; }
       
       public IRepository<Answer> Answers { get; set; }

       public IRepository<Response> Responses { get; set; }

       public DataService() 
       {
           this.Quizes = new Repository<Quiz>(); 
           this.QuizTypes = new Repository<QuizType>();
           this.Questions = new Repository<Question>();
           this.Answers = new Repository<Answer>();
           this.Responses = new Repository<Response>(); 
       }
    }
}
