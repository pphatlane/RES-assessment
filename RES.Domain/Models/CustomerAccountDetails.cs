using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RES.Domain.Models
{
    public class CustomerAccountDetails
    {
        public int customerPin { get; set; }
        public string customerName { get; set; }
        public long customerAccountNo { get; set; }
        public decimal customerCurrentBalance { get; set; }
        public decimal customerInitialBalance { get; set; }
        public decimal customerOverdruftBalance { get; set; }

        public string customerCurrency { get; set; }


    }
}
