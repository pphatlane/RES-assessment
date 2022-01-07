using Dapper;
using Microsoft.Extensions.Configuration;
using RES.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RES.Tests.Intergration.Helpers
{
    public class CustomerAccountDetailsHelper
    {
        private readonly IConfiguration _configuration;

        public CustomerAccountDetailsHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task AddCustomerAccountDetails(CustomerAccountDetails customerAccountDetails)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ResDb")))
            {

                var sql = @"insert into CustomerAccountDetails
	                            (customerPin,
	                             customerName,
	                             customerAccountNo,
	                             customerInitialBalance,
                                 customerCurrentBalance,
                                 customerOverdruftBalance)

	                            Values( @customerPin, 
                                        @customerName, 
                                        @customerAccountNo, 
                                        @customerInitialBalance,
                                        @customerCurrentBalance,
                                        @customerOverdruftBalance
                                    )  ";

                var results = await connection.ExecuteAsync(sql,
                          new
                          {
                              customerPin = customerAccountDetails.customerPin,
                              customerName = customerAccountDetails.customerName,
                              customerAccountNo = customerAccountDetails.customerAccountNo,
                              customerInitialBalance = customerAccountDetails.customerInitialBalance,
                              customerCurrentBalance = customerAccountDetails.customerCurrentBalance,
                              customerOverdruftBalance = customerAccountDetails.customerOverdruftBalance
                          });
            }
        }

        public async Task<CustomerAccountDetails> GetCustomerAccountDetails(long customerAcount, int customerPin)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ResDb")))
            {
                var sql = @"Select customerAccountNo, customerPin, customerName, customerCurrentBalance,CHAR(0163)  
                            From CustomerAccountDetails Where customerPin = @customerPin 
                            And customerAccountNo = @customerAcount";

                var rtn = await connection.QueryFirstAsync<CustomerAccountDetails>(sql, new { customerPin, customerAcount });
                return rtn;
            }
        }
    }
}
