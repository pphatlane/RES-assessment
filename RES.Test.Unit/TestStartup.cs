using Moq;
using System.Net.Http;

namespace RES.Test.Unit
{
    public class TestStartup<TStartup> where TStartup : class
    {
        public MockRepository MockRepository { get; }
        public Mock<ICustomerAccountRepository> CustomerAccountRepository { get; }

        private readonly TestServer _server;
        public HttpClient Client { get; }
        public IConfiguration Configuration { get; }

        public TestStartup()
        {
            MockRepository = new MockRepository(MockBehavior.Default);
            CustomerAccountRepository = MockRepository.Create<ICustomerAccountRepository>();
        }
    }
}
