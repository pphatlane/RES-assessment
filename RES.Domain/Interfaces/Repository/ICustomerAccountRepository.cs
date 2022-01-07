using RES.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RES.Domain
{
    public interface ICustomerAccountRepository
    {
        Task<CustomerAccountDetails> GetCustomerAccountDetails(long customerAcount, int customerPin);
        Task UpdateCustomerAccountDetails(CustomerAccountDetails customerAccountDetails);
    }
}
