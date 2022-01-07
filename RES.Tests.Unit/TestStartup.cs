using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RES.Domain;
using RES.Domain.Services;
using RES.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RES.Tests.Unit
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

            _server = new TestServer
             (
              new WebHostBuilder()
              .UseStartup<TStartup>()
              .ConfigureAppConfiguration((context, config) =>
              {
                  config
                  .AddJsonFile("appsettings.json").AddJsonFile("appsettings.Development.json")
                  .AddEnvironmentVariables();
              })
              .ConfigureTestServices(services =>
              {
                  ConfigureServices(services);
              })
             );
            Configuration = _server.Host.Services.GetService<IConfiguration>();
            Client = _server.CreateClient();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICustomerAccountServices, CustomerAccountServices>();
            services.AddTransient(serviceProvider => CustomerAccountRepository.Object);
            services.AddControllers();
            services.AddHttpClient();
        }
    }
}
