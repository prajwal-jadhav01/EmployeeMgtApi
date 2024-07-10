using EmployeeMgtApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMgtApi.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
