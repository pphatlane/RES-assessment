using Bogus;
using Moq;
using Newtonsoft.Json;
using RES.API;
using RES.Domain.Models;
using RES.Tests.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RES.Test.Unit.Controllers
{
    [Collection("Fixture collection")]
    public class CustomerAccountDetailsController
    {
        private readonly TestStartup<Startup> _fixture;
        private readonly HttpClient _client;

        public CustomerAccountDetailsController(TestStartup<Startup> fixture)
        {
            _fixture = fixture;
            _client = _fixture.Client;
        }

        [Fact]
        public async Task ShouldValidatingCustomerAccount()
        {
            //Arrange

            var CustomerAccountDetailsRule = new Faker<CustomerAccountDetails>()
            .RuleFor(c => c.customerAccountNo, 5678910)
            .RuleFor(c => c.customerInitialBalance, 8000)
            .RuleFor(c => c.customerPin, f => 1235)
            .RuleFor(c => c.customerName, "Phatlane")
            .RuleFor(c => c.customerCurrentBalance, 600);

            var customerAccountDetails = CustomerAccountDetailsRule.Generate();

            _fixture.CustomerAccountRepository.Setup(c => c.GetCustomerAccountDetails(
             It.IsAny<long>(), It.IsAny<int>())).Returns(Task.FromResult(customerAccountDetails));

            //Act
            var response = await _client.GetAsync($"/CustomerAccountDetails/{customerAccountDetails.customerAccountNo}/{customerAccountDetails.customerPin}/Validate");

            var isValid = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(isValid);
            _fixture.CustomerAccountRepository.VerifyAll();

        }

        [Fact]
        public async Task ShouldCheckCustomerBalance()
        {
            //Arrange

            var CustomerAccountDetailsRule = new Faker<CustomerAccountDetails>()
            .RuleFor(c => c.customerAccountNo, 5678910)
            .RuleFor(c => c.customerInitialBalance, 8000)
            .RuleFor(c => c.customerPin, f => 1235)
            .RuleFor(c => c.customerName, "Phatlane")
            .RuleFor(c => c.customerCurrentBalance, 600);

            var customerAccountDetails = CustomerAccountDetailsRule.Generate();

            _fixture.CustomerAccountRepository.Setup(c => c.GetCustomerAccountDetails(
             It.IsAny<long>(), It.IsAny<int>())).Returns(Task.FromResult(customerAccountDetails));

            //Act
            var response = await _client.GetAsync($"/CustomerAccountDetails/{customerAccountDetails.customerAccountNo}/{customerAccountDetails.customerPin}/CheckBalance");

            var experctedBalance = JsonConvert.DeserializeObject<decimal>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(customerAccountDetails.customerCurrentBalance, experctedBalance);
            _fixture.CustomerAccountRepository.VerifyAll();

        }


        [Fact]
        public async Task ShouldCheckWithdrawAndUpdateBalance()
        {
            //Arrange

            var CustomerAccountDetailsRule = new Faker<CustomerAccountDetails>()
            .RuleFor(c => c.customerAccountNo, 5678910)
            .RuleFor(c => c.customerInitialBalance, 8000)
            .RuleFor(c => c.customerOverdruftBalance, 100)
            .RuleFor(c => c.customerPin, f => 1235)
            .RuleFor(c => c.customerName, "Phatlane")
            .RuleFor(c => c.customerCurrentBalance, 500);

            var customerAccountDetails = CustomerAccountDetailsRule.Generate();

            _fixture.CustomerAccountRepository.Setup(c => c.GetCustomerAccountDetails(
             It.IsAny<long>(), It.IsAny<int>())).Returns(Task.FromResult(customerAccountDetails));

            _fixture.CustomerAccountRepository.Setup(c => c.UpdateCustomerAccountDetails(
            It.IsAny<CustomerAccountDetails>())).Returns(Task.CompletedTask);
            //Act
            var response = await _client.GetAsync($"/CustomerAccountDetails/{customerAccountDetails.customerAccountNo}/{customerAccountDetails.customerPin}/{600}");
            var experctedMsg = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("successfully", experctedMsg);
            _fixture.CustomerAccountRepository.VerifyAll();

        }

        [Fact]
        public async Task ShouldReturnFundsErr()
        {
            //Arrange

            var CustomerAccountDetailsRule = new Faker<CustomerAccountDetails>()
            .RuleFor(c => c.customerAccountNo, 5678910)
            .RuleFor(c => c.customerInitialBalance, 8000)
            .RuleFor(c => c.customerOverdruftBalance, 100)
            .RuleFor(c => c.customerPin, f => 1235)
            .RuleFor(c => c.customerName, "Phatlane")
            .RuleFor(c => c.customerCurrentBalance, 500);

            var customerAccountDetails = CustomerAccountDetailsRule.Generate();

            _fixture.CustomerAccountRepository.Setup(c => c.GetCustomerAccountDetails(
             It.IsAny<long>(), It.IsAny<int>())).Returns(Task.FromResult(customerAccountDetails));

            //Act
            var response = await _client.GetAsync($"/CustomerAccountDetails/{customerAccountDetails.customerAccountNo}/{customerAccountDetails.customerPin}/{800}");
            var experctedMsg = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Insufficient funds", experctedMsg);
            _fixture.CustomerAccountRepository.VerifyAll();
        }


        [Fact]
        public async Task ShouldReturnAuthErr()
        {
            //Arrange

            var CustomerAccountDetailsRule = new Faker<CustomerAccountDetails>()
            .RuleFor(c => c.customerAccountNo, 12345678)
            .RuleFor(c => c.customerInitialBalance, 8000)
            .RuleFor(c => c.customerOverdruftBalance, 100)
            .RuleFor(c => c.customerPin, f => 1234)
            .RuleFor(c => c.customerName, "Phatlane")
            .RuleFor(c => c.customerCurrentBalance, 500);

            var customerAccountDetails = CustomerAccountDetailsRule.Generate();

            _fixture.CustomerAccountRepository.Setup(c => c.GetCustomerAccountDetails(
             It.IsAny<long>(), It.IsAny<int>())).Returns(Task.FromResult(customerAccountDetails));

            //Act
            var response = await _client.GetAsync($"/CustomerAccountDetails/{customerAccountDetails.customerAccountNo}/{1235}/{600}");
            var experctedMsg = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Invalid user", experctedMsg);
            _fixture.CustomerAccountRepository.VerifyAll();

        }


    }
}
