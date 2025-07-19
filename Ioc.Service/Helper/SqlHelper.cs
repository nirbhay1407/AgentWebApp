using Microsoft.Data.SqlClient;
using System.Data;

namespace Ioc.Service.Helper
{

    public static class SqlHelper
    {
        private static string? _connectionString;

        public static void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static async Task<int> ExecuteNonQueryAsync(string query, CommandType commandType, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task<object> ExecuteScalarAsync(string query, CommandType commandType, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    await connection.OpenAsync();
                    return await command.ExecuteScalarAsync();
                }
            }
        }

        public static async Task<DataTable> ExecuteQueryAsync(string query, CommandType commandType, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        await Task.Run(() => adapter.Fill(dataTable));
                        return dataTable;
                    }
                }
            }
        }

        public static async Task<List<T>> ExecuteQueryAsync<T>(string query, CommandType commandType, Func<IDataReader, T> map, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var results = new List<T>();
                        while (await reader.ReadAsync())
                        {
                            results.Add(map(reader));
                        }
                        return results;
                    }
                }
            }
        }

        public static async Task<T> ExecuteSingleAsync<T>(string query, CommandType commandType, Func<IDataReader, T> map, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = commandType;
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return map(reader);
                        }
                        else
                        {
                            return default;
                        }
                    }
                }
            }
        }
    }
}