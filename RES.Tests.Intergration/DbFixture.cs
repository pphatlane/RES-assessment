using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RES.Data;
using RES.Domain;
using RES.Tests.Intergration.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RES.Tests.Intergration
{
    public class DbFixture<TStartup> where TStartup : class
    {
        public ICustomerAccountRepository CustomerAccountRepository { get; }
        public CustomerAccountDetailsHelper CustomerAccountDetailsHelper { get; }

        private readonly TestServer _server;

        public DbFixture()
        {

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

            CustomerAccountRepository = _server.Host.Services.GetService<ICustomerAccountRepository>();
            CustomerAccountDetailsHelper = _server.Host.Services.GetService<CustomerAccountDetailsHelper>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpClient();
            services.AddScoped<ICustomerAccountRepository, CustomerAccountRepository>();
            services.AddTransient<CustomerAccountDetailsHelper>();

        }
    }
}
