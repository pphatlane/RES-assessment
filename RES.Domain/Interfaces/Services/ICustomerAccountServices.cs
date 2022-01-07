using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RES.Domain.Services
{
    public interface ICustomerAccountServices
    {
        Task<bool> ValidatingCustomerAccount(long customerAcount, int customerPin);
        Task<string> CheckBalance(long customerAcount, int customerPin);
        Task<string> CashWithdraws(long customerAcount, int customerPin, decimal amount);
    }
}
