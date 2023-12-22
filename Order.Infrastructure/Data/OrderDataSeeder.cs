using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Data
{
    public static class OrderDataSeeder
    {
        public static string SeedQuery(string Context)
        {
            try {
                if (Context.Equals("OrderContext"))
                {
                    string orderQuery = (@"

                IF OBJECT_ID('Orders', 'U') IS NOT NULL DROP TABLE Orders;

                CREATE TABLE Orders (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    UserName NVARCHAR(MAX),
                    TotalPrice DECIMAL(18,2),
                    FirstName NVARCHAR(MAX),
                    LastName NVARCHAR(MAX),
                    EmailAddress NVARCHAR(MAX),
                    AddressLine NVARCHAR(MAX),
                    Country NVARCHAR(MAX),
                    State NVARCHAR(MAX),
                    ZipCode NVARCHAR(MAX),
                    CardName NVARCHAR(MAX),
                    CardNumber NVARCHAR(MAX),
                    Expiration NVARCHAR(MAX),
                    Cvv NVARCHAR(MAX),
                    PaymentMethod INT,
                    CreatedBy NVARCHAR(MAX),
                    CreatedDate DATETIME,
                    LastModifiedBy NVARCHAR(MAX),
                    LastModifiedDate DATETIME
                );
             
                INSERT INTO Orders (UserName, TotalPrice, FirstName, LastName, EmailAddress, AddressLine, Country, State, ZipCode, CardName, CardNumber, Expiration, Cvv, PaymentMethod, CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                VALUES ('rahul', 750, 'Rahul', 'Sahay', 'rahulsahay@eshop.net', 'Bangalore', 'India', 'KA', '560001', 'Visa', '1234567890123456', '12/25', '123', 1, 'Rahul', GETDATE(), 'Rahul', GETDATE())
                ");

                    return orderQuery;
                }

                return null;
            }
            catch(Exception ex)
            {
                throw;
            }
            }
    }
}
