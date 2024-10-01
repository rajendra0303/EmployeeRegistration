using System.ComponentModel.DataAnnotations;

namespace EmployeeRegistration.Controllers.Models
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string EmpName { get; set; }
        public string EmpAddres { get; set; }
        public string Position { get; set; }
        
        public int IsActive { get; set; } = 1;
        public DateTime createOnDate { get; set; } = DateTime.Now;
    
    
    }
}
