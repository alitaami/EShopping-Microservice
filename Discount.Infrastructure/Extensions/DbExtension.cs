using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;

public class DbExtension
{
    private readonly string _connectionString;

    public DbExtension(IConfiguration configuration)
    {
        _connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
    }

    public async Task RecreateCouponTableAsync()
    {
        using IDbConnection db = new NpgsqlConnection(_connectionString);

        try
        {
            // Check if the table exists
            if (await TableExistsAsync(db, "Coupon"))
            {
                // Drop the table if it exists
                await DropCouponTableAsync(db);
            }

            // Create the table
            await CreateCouponTableAsync(db);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            // Optionally, you can throw the exception to be handled by the caller or log it for further analysis.
            throw;
        }
    }

    private async Task<bool> TableExistsAsync(IDbConnection db, string tableName)
    {
        var result = await db.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM information_schema.tables WHERE table_name = @TableName",
            new { TableName = tableName.ToLower() }
        );
        return result > 0;
    }

    private async Task DropCouponTableAsync(IDbConnection db)
    {
        var dropTableSql = "DROP TABLE Coupon";
        await db.ExecuteAsync(dropTableSql);
        Console.WriteLine("Coupon table dropped successfully.");
    }

    private async Task CreateCouponTableAsync(IDbConnection db)
    {
        var createTableSql = @"
        CREATE TABLE Coupon (
            Id SERIAL PRIMARY KEY,
            ProductName VARCHAR(500) NOT NULL,
            Description TEXT,
            Amount INT NOT NULL
        )";
        await db.ExecuteAsync(createTableSql);

        var command = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Adidas Quick Force Indoor Badminton Shoes', 'Shoe Discount', 500);";
        await db.ExecuteAsync(command);

        var command1 = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Yonex VCORE Pro 100 A Tennis Racquet (270gm, Strung)', 'Racquet Discount', 700);";
        await db.ExecuteAsync(command1);

        Console.WriteLine("Coupon table created successfully.");
    }
}
