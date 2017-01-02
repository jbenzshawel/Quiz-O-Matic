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

       /// <summary>
       /// Overload to make unit testing easier. May also add PgSql to DI services 
       /// </summary>
       /// <param name="_pgSql"></param>
       public DataService(IPgSql _pgSql) 
       {
           this.Quizes = new Repository<Quiz>(_pgSql); 
           this.QuizTypes = new Repository<QuizType>(_pgSql);
           this.Questions = new Repository<Question>(_pgSql);
           this.Answers = new Repository<Answer>(_pgSql);
           this.Responses = new Repository<Response>(_pgSql); 
       }
    }
}
