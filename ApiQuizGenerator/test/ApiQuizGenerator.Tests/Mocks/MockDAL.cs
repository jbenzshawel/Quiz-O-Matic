using ApiQuizGenerator.DAL;

namespace ApiQuizGenerator.Tests.Mocks
{
    public class MockDAL
    {
        public IPgSql MockPgSql { get; set; }

        public IDataService DataService { get; set; }

        // ToDo: Move mock tables to MockDAL class then use Moq Framework for 
        // mocking PgSql sql calls that return mock data from mock tables
        public MockDAL() 
        {
            MockPgSql = new MockPgSql();
            DataService = new DataService(MockPgSql);
        }
    }
}
