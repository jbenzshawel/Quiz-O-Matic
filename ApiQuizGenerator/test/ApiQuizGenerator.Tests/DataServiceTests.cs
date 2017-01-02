using Xunit;
using ApiQuizGenerator.DAL;
using ApiQuizGenerator.Tests.Mocks;

namespace ApiQuizGenerator.Tests
{
    public class DataServiceTests
    {
        private MockDAL _MockDAL { get; set; }

        

        public DataServiceTests() 
        {
            _MockDAL = new MockDAL();
        }
    }
}
