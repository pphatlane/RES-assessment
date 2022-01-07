using Microsoft.AspNetCore.Mvc;
using RES.Domain.Services;
using System.Threading.Tasks;

namespace RES.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerAccountDetailsController : ControllerBase
    {
        private readonly ICustomerAccountServices _customerAccountServices;

        public CustomerAccountDetailsController(ICustomerAccountServices customerAccountServices)
        {
            _customerAccountServices = customerAccountServices;
        }

        [HttpGet("{customerAccountNo}/{customerPin}/Validate")]
        public async Task<ActionResult<bool>> ValidatingCustomerAccount(long customerAccountNo, int customerPin)
        {
            var isValid = await _customerAccountServices.ValidatingCustomerAccount(customerAccountNo, customerPin);
            return Ok(isValid);
        }

        [HttpGet("{customerAccountNo}/{customerPin}/CheckBalance")]
        public async Task<ActionResult<string>> CheckBalance(long customerAccountNo, int customerPin)
        {
            var balance = await _customerAccountServices.CheckBalance(customerAccountNo, customerPin);
            return Ok(balance);
        }

        [HttpGet("{customerAccountNo}/{customerPin}/{amount}")]
        public async Task<ActionResult<string>> CashWithdraws(long customerAccountNo, int customerPin, decimal amount)
        {
           var rtn = await _customerAccountServices.CashWithdraws(customerAccountNo, customerPin, amount);
            return Ok(rtn);
        }

    }
}
