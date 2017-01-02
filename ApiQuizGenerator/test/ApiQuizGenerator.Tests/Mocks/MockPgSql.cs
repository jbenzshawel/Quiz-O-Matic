using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Npgsql;
using ApiQuizGenerator.Models;
using ApiQuizGenerator.DAL;

namespace ApiQuizGenerator.Tests.Mocks
{
    public class MockPgSql : PgSql, IPgSql
    {
        /// <summary>
        /// Number of objects to be mocked for each table / model
        /// </summary>
        private const int NUMBER_MOCK_OBJECTS = 5;        

        # region properties used for mocking tables / sql data       
        private IQueryable<Quiz> _Quizes { get; set; }

        private IQueryable<Answer> _Answers { get; set; }

        private IQueryable<QuizType> _QuizTypes { get; set; }
        
        private IQueryable<Question> _Questions { get; set; }

        private IQueryable<Response> _Responses { get; set; }

        #endregion

        public MockPgSql() 
        {
            _Quizes = MockData.GetMockQueryable<Quiz>(NUMBER_MOCK_OBJECTS); 
            _Answers = MockData.GetMockQueryable<Answer>(NUMBER_MOCK_OBJECTS); 
            _QuizTypes = MockData.GetMockQueryable<QuizType>(NUMBER_MOCK_OBJECTS); 
            _Questions = MockData.GetMockQueryable<Question>(NUMBER_MOCK_OBJECTS); 
            _Responses = MockData.GetMockQueryable<Response>(NUMBER_MOCK_OBJECTS);
        }

        // ToDo: Decide if want to fully mock PgSql or just use Moq framework to mock out DataService
        // If Moq used for DataService Repository logic will not really be tested. Regardless without
        // integration tests PgSql will be hard to test and will still have to use some sort of mock 
        // for sql calls
        public override async Task<int> ExecuteNonQuery(string command, List<NpgsqlParameter> paramz = null)
        {
            Type objType = null;            
            int rowsAffected = 0; 

            try 
            {
                command = command.ToLower();
                if (command.Contains("save"))
                {
                    objType = SaveProcedures.FirstOrDefault(o => ((PgSqlFunction)o.Value).Name.ToLower() == command.ToLower()).Key;                    
                    rowsAffected = _AddMock(objType);
                }
                else if (command.Contains("delete"))
                {
                    objType = DeleteProcedures.FirstOrDefault(o => ((PgSqlFunction)o.Value).Name.ToLower() == command.ToLower()).Key;                 
                }                    
                else 
                {
                    throw new NotSupportedException("procedure type not supported");
                }
            }
            catch (Exception ex) 
            {
                // ToDo: log something about ex
                rowsAffected = -1;
            }

            await Task.Delay(500);

            return rowsAffected;
        }

        # region mock methods dealing with mock tables

        private int _AddMock(Type objType)
        {
            if (objType == typeof (Quiz))
            {
                List<Quiz> quizList = _Quizes.ToList();
                quizList.Add(MockData.GetMockObj<Quiz>());
                _Quizes = quizList.AsQueryable();

                return 1;
            }

            return 0;
        }
        #endregion
    }
}
