using ApiQuizGenerator.DAL;

namespace ApiQuizGenerator.Tests.Mocks
{
    public class MockDAL
    {
        public IPgSql MockPgSql { get; set; }

        public DataService DataService { get; set; }

        public MockDAL() 
        {
            MockPgSql = new MockPgSql();
            DataService = new DataService(MockPgSql);
        }
    }
}