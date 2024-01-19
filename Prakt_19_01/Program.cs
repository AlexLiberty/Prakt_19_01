using Microsoft.Data.SqlClient;
using System.Transactions;

string connectionString = "Server=LIBERTY; Database=Cars; Trusted_Connection=True; TrustServerCertificate=True; Integrated Security=True;";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    connection.Open();

    SqlTransaction transaction = connection.BeginTransaction();
    SqlCommand command = connection.CreateCommand();
    command.Connection = connection;
    command.Transaction = transaction;

    try
        {
            command.CommandText = "INSERT INTO Customers (FirstName, LastName) VALUES ('John', 'Smith')";
            command.ExecuteNonQuery();

            transaction.Save("point1");

            command.CommandText = "INSERT INTO Customers (FirstName, LastName) VALUES ('Jeck', 'Richer')"; 
            command.ExecuteNonQuery();

            Console.WriteLine("Транзакція завершена успішно.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка транзакції: " + ex.Message);
            transaction.Rollback("point1");
        }

    finally
    {
        transaction.Commit();
        connection.Close();
    }
}