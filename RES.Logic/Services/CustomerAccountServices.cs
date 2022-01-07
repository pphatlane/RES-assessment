using RES.Domain;
using RES.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RES.Logic.Services
{
    public class CustomerAccountServices : ICustomerAccountServices
    {
        private readonly ICustomerAccountRepository _customerAccountRepository;
        public CustomerAccountServices(ICustomerAccountRepository customerAccountRepository)
        {
            _customerAccountRepository = customerAccountRepository;
        }
        public  async Task<string> CashWithdraws(long customerAcount, int customerPin, decimal amount)
        {
            if ( await ValidatingCustomerAccount(customerAcount, customerPin))
            {
                var customerInfo = await _customerAccountRepository.GetCustomerAccountDetails(customerAcount, customerPin);
                if (customerInfo.customerCurrentBalance < amount)
                {
                    if (customerInfo.customerCurrentBalance + customerInfo.customerOverdruftBalance < amount)
                    {
                        return "Insufficient funds";

                    }
                    else {

                        customerInfo.customerCurrentBalance = 0;
                        customerInfo.customerOverdruftBalance = 0;
                        await _customerAccountRepository.UpdateCustomerAccountDetails(customerInfo);
                        return "successfully";
                    }
                }
                else
                {
                    customerInfo.customerCurrentBalance = customerInfo.customerInitialBalance - amount;
                    await _customerAccountRepository.UpdateCustomerAccountDetails(customerInfo);
                    return "successfully";
                }
            }
            else
            {
                return "Invalid user";
            }
        }

        public async Task<string> CheckBalance(long customerAcount, int customerPin)
        {
            if (await ValidatingCustomerAccount(customerAcount, customerPin))
            {
                var customerInfo = await _customerAccountRepository.GetCustomerAccountDetails(customerAcount, customerPin);
                return   customerInfo.customerCurrentBalance.ToString();
            }
            else
            {
                return "Invalid user";
            }
        }

        public async Task<bool> ValidatingCustomerAccount(long customerAcount, int customerPin)
        {
           var isValid = await _customerAccountRepository.GetCustomerAccountDetails(customerAcount, customerPin);
            if (isValid != null) {
                return isValid.customerPin == customerPin && isValid.customerAccountNo == customerAcount;
            }
            else
            {
                return false;
            }
        }
    }
}
