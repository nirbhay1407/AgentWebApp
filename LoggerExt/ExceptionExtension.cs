using Microsoft.Data.SqlClient;

public static class ExceptionExtensions
{
    private static string? _connectionString;

    public static void SetConnectionString(string connectionString)
    {
        _connectionString = connectionString;

    }
    public static void ManualDBLog(this Exception ex, string? className, string? methodName)
    {
        /*using (var connection = new SqlConnection(_connectionString))
        {
            var command = new SqlCommand("INSERT INTO ExceptionLogs (ExceptionMessage, ExceptionType, StackTrace, Source, ClassName, MethodName, TargetSite) VALUES (@ExceptionMessage, @ExceptionType, @StackTrace, @Source," +
                " @ClassName, @MethodName, @TargetSite)", connection);
            command.Parameters.AddWithValue("@ExceptionMessage", ex.Message);
            command.Parameters.AddWithValue("@ExceptionType", ex.GetType().ToString());
            command.Parameters.AddWithValue("@StackTrace", ex.StackTrace ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Source", ex.Source ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ClassName", className ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@MethodName", methodName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@TargetSite", ex.TargetSite?.ToString() ?? (object)DBNull.Value);

            connection.Open();
            command.ExecuteNonQuery();
        }*/
    }
}
