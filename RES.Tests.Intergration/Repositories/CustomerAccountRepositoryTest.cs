using AutoBogus;
using Bogus;
using RES.API;
using RES.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace RES.Tests.Intergration.Repositories
{
    [Collection("Series")]
    public class CustomerAccountRepositoryTest : IClassFixture<DbFixture<Startup>>
    {
        private readonly DbFixture<Startup> _fixture;
        public CustomerAccountRepositoryTest(DbFixture<Startup> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldGetCustomerAccountDetails()
        {
            using (new TransactionScope(TransactionScopeOption.Required, System.TimeSpan.MaxValue, TransactionScopeAsyncFlowOption.Enabled))
            {
                var CustomerAccountDetailsRule = new Faker<CustomerAccountDetails>()
                .RuleFor(c => c.customerAccountNo,5678910)
                .RuleFor(c => c.customerInitialBalance, 8000)
                .RuleFor(c => c.customerPin, f => 1235)
                .RuleFor(c => c.customerOverdruftBalance, 100)
                .RuleFor(c => c.customerName, "Phatlane")
                .RuleFor(c => c.customerCurrentBalance, 8000);

                var customerAccountDetails = CustomerAccountDetailsRule.Generate();

                //Act
                 await _fixture.CustomerAccountDetailsHelper.AddCustomerAccountDetails(customerAccountDetails);
                 var expectedCustomer = await _fixture.CustomerAccountRepository.GetCustomerAccountDetails(customerAccountDetails.customerAccountNo, customerAccountDetails.customerPin);
                //Assert
                Assert.NotNull(expectedCustomer);
                Assert.Equal(expectedCustomer.customerName, customerAccountDetails.customerName);
            }
        }

        [Fact]
        public async Task ShouldUpdateCustomerAccountDetails()
        {
            using (new TransactionScope(TransactionScopeOption.Required, System.TimeSpan.MaxValue, TransactionScopeAsyncFlowOption.Enabled))
            {
                var CustomerAccountDetailsRule = new Faker<CustomerAccountDetails>()
                  .RuleFor(c => c.customerAccountNo, 5678910)
                  .RuleFor(c => c.customerInitialBalance, 8000)
                  .RuleFor(c => c.customerPin, f => 1235)
                  .RuleFor(c => c.customerName, "Phatlane")
                  .RuleFor(c => c.customerOverdruftBalance, 100)
                  .RuleFor(c => c.customerCurrentBalance, 600);

                var customerAccountDetails = CustomerAccountDetailsRule.Generate();

                //Act
                await _fixture.CustomerAccountDetailsHelper.AddCustomerAccountDetails(customerAccountDetails);
                customerAccountDetails.customerCurrentBalance = 600;
                await _fixture.CustomerAccountRepository.UpdateCustomerAccountDetails(customerAccountDetails);
                var expectedCustomer = await _fixture.CustomerAccountDetailsHelper.GetCustomerAccountDetails(customerAccountDetails.customerAccountNo, customerAccountDetails.customerPin);

                //Assert
                Assert.NotNull(expectedCustomer);
                Assert.Equal(expectedCustomer.customerName, customerAccountDetails.customerName);
                Assert.Equal(expectedCustomer.customerCurrentBalance, customerAccountDetails.customerCurrentBalance);
            }
        }

        
    }
}
