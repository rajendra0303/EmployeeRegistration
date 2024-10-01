using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EmployeeRegistration.Controllers.Models
{
    public class EmpMigration : DbContext
    {
        public EmpMigration(DbContextOptions<EmpMigration> options): base(options) 
        
        {
            
        }
        public DbSet<Employee> Employees { get; set;}
    }
}
