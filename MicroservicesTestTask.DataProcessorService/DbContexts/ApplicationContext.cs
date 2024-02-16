using MicroservicesTestTask.DataProcessorService.Models;

using Microsoft.EntityFrameworkCore;

namespace MicroservicesTestTask.DataProcessorService.DbContexts
{
    public class ApplicationContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Module> Modules { get; set; }

        public ApplicationContext(string connectionString)
        {
            _connectionString = connectionString;
            _ = Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite(_connectionString);
    }
}
