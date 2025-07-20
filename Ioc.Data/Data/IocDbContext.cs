using Ioc.Core;
using Ioc.Core.DbModel;
using Ioc.Core.DbModel.Models;
using Ioc.Core.DbModel.Models.Quiz;
using Ioc.Core.DbModel.Models.SiteInfo;
using Ioc.Core.DbModel.SqlLoadModel;
using Ioc.Core.DbModel.Validation;
using Ioc.Core.Helper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.CompilerServices;

namespace Ioc.Data.Data
{
    public class IocDbContext : IdentityDbContext<ApplicationUser>
    {
        public IocDbContext(DbContextOptions<IocDbContext>? options)
        : base(options)
        {
        }

        public DbSet<Category>? Category { get; set; }
        public DbSet<SubCategory>? SubCategory { get; set; }
        public DbSet<User>? User { get; set; }
        public DbSet<UserProfile>? UserProfile { get; set; }
        public DbSet<CommonGroup>? CommonGroup { get; set; }
        public DbSet<Customer>? Customers { get; set; }
        public DbSet<Setting>? Setting { get; set; }
        //public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<QuizSetup> QuizSetups { get; set; }
        public DbSet<QuestionSetup> QuestionSetup { get; set; }
        public DbSet<AnswerSetup> AnswerSetup { get; set; }
        public DbSet<QuizDescription> QuizDescription { get; set; }
        public DbSet<CompleteUserDet> CompleteUserDetails { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<SalesPerson> SalesPerson { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<BankDetails> BankDetails { get; set; }
        public DbSet<InvoiceNew> InvoiceNew { get; set; }
        public DbSet<RandomGenHelp> RandomGenHelp { get; set; }
        public DbSet<ValidationRule> ValidationRule { get; set; }
        public DbSet<ImportProduct> ImportProducts { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }

        public string GetConnectionString() { return this.Database.GetDbConnection().ConnectionString; }


        public async Task<int> ExecuteNonQueryAsync(string query, CommandType commandType, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
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

        public async Task<object> ExecuteScalarAsync(string query, CommandType commandType, params SqlParameter[] parameters)
        {
            try
            {
                using (var connection = new SqlConnection(GetConnectionString()))
                {
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.CommandType = commandType;
                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        await connection.OpenAsync();
                        return await command.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //ex.ManualDBLog("DBContext", "ExecuteScalarAsync");
            }
            return null;
        }

        public async Task<DataTable> ExecuteQueryAsync(string query, CommandType commandType, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
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

        public async Task<List<T>> ExecuteQueryAsync<T>(string query, CommandType commandType, Func<IDataReader, T> map, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
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

        public async Task<T> ExecuteSingleAsync<T>(string query, CommandType commandType, Func<IDataReader, T> map, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.DateTimeZoneConvertion();

            /*modelBuilder.Entity<QuizSetup>()
            .HasMany(qs => qs.Questions)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuestionSetup>()
                .HasMany(q => q.Answers)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);*/

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>().Navigation(e => e.CompanyAddress).AutoInclude();
            modelBuilder.Entity<InvoiceNew>().Navigation(e => e.Company).AutoInclude();
            modelBuilder.Entity<InvoiceNew>().Navigation(e => e.Salesperson).AutoInclude();


            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            //builder.Entity<ApplicationUser>().Navigation(e => e.IdentityRole.Where(x=>x.Id== e.IdentityRole.U)).AutoInclude();
        }

        /*public new IDbSet<TEntity> Set<TEntity>() where TEntity : PublicBaseEntity
        {
            return (IDbSet<TEntity>)base.Set<TEntity>();
        }*/

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseAutoInclude();
        }


        public void UpdateDb()
        {
            /*using (var context = new IocDbContext())
            {
                context.Database.ExecuteSqlRaw(sqlScript);
            }
            */

            // Get the current directory where the application is running
            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var projectDirectory = Directory.GetParent(currentDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;
            // Combine it with the relative path to your SQL files
            var relativePath = @"Ioc.Data/_Database/MainDB/ObjectList";
            var fullPath = Path.Combine(projectDirectory, relativePath); // Get all SQL files in the directory

            var sqlFiles = Directory.GetFiles(fullPath, "*.sql");


            //var sqlFiles = Directory.GetFiles("D:/GIT/MainRepo/AgentWebApp01/Ioc.Data/_Database/MainDB/ObjectList", "*.sql");
            var AddList = File.ReadAllText("D:/GIT/MainRepo/AgentWebApp01/Ioc.Data/_Database/MainDB/NeedToAdd.sql");
            var deleteList = File.ReadAllText("D:/GIT/MainRepo/AgentWebApp01/Ioc.Data/_Database/MainDB/NeedToDelete.sql");

            var sqlScripts = new List<string>();
            var sqlAddList = new List<string>();
            var sqlDeleteList = new List<string>();
            sqlAddList.AddRange(AddList.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim()).ToList());
            sqlDeleteList.AddRange(deleteList.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(line => line.Trim()).ToList());
            foreach (var file in sqlFiles)
            {
                if (!sqlAddList.Contains(Path.GetFileNameWithoutExtension(file)))
                    continue;
                var script = File.ReadAllText(file);
                sqlScripts.Add(script);
            }

            var scriptA = @"IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_GetInvoice]') AND type in (N'P', N'PC'))
                            DROP PROCEDURE [dbo].[USP_GetInvoice]
                            GO
                            CREATE PROCEDURE USP_GetInvoice
                            AS
                            BEGIN
                                SET NOCOUNT ON;
                                SELECT * FROM InvoiceNew;
                            END
                                GO";

            this.Database.ExecuteSqlRaw(scriptA);


            foreach (var item in sqlScripts)
            {
                try
                {
                    this.Database.ExecuteSqlRaw("{0}", item);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
           
        }
    }
}
