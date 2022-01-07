using Dapper;
using Microsoft.Extensions.Configuration;
using RES.Domain;
using RES.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RES.Data
{
    public class CustomerAccountRepository: ICustomerAccountRepository
    {
        private readonly IConfiguration _configuration;
        public CustomerAccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<CustomerAccountDetails> GetCustomerAccountDetails(long customerAcount, int customerPin)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ResDb")))
            {
                var sql = @"Select customerAccountNo, customerPin, customerName, customerInitialBalance, customerCurrentBalance, CHAR(0163)  as 'customerCurrency' 
                            From CustomerAccountDetails Where customerPin = @customerPin 
                            And customerAccountNo = @customerAcount";

                var rtn = await connection.QueryFirstAsync<CustomerAccountDetails>(sql, new { customerPin, customerAcount });
                return rtn;
            }
        }

        public async Task UpdateCustomerAccountDetails(CustomerAccountDetails customerAccountDetails)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ResDb")))
            {
                var sql = @"update CustomerAccountDetails 
                            set customerCurrentBalance = @customerCurrentBalance, 
                            customerOverdruftBalance = @customerOverdruftBalance
                            Where customerPin = @customerPin And customerAccountNo = @customerAccountNo";
                await connection.ExecuteAsync(sql,
                    new
                    {
                        customerCurrentBalance = customerAccountDetails.customerCurrentBalance,
                        customerOverdruftBalance = customerAccountDetails.customerOverdruftBalance,
                        customerPin = customerAccountDetails.customerPin,
                        customerAccountNo = customerAccountDetails.customerAccountNo
                    });
            }
        }
    }
}
