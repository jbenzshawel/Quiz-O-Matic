using ApiQuizGenerator.Models;

namespace ApiQuizGenerator.DAL
{
    public interface IDataService 
    {
        Repository<Quiz> Quizes { get; set; }

        Repository<QuizType> QuizTypes { get; set; }

        Repository<Question> Questions { get; set; }
        
    }

    public class DataService : IDataService
    {
       public Repository<Quiz> Quizes { get; set;}

       public Repository<QuizType> QuizTypes { get; set; }

       public Repository<Question> Questions { get; set; }
       public DataService() 
       {
           this.Quizes = new Repository<Quiz>(); 
           this.QuizTypes = new Repository<QuizType>();
           this.Questions = new Repository<Question>();
       }
    }
}
