using System.ComponentModel.DataAnnotations;

namespace EmployeeRegistration.Controllers.Models
{
    public class EmployeeDOT
    {
       
        public int EmpId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string EmpName { get; set; }
        public string EmpAddres { get; set; }
        public string Position { get; set; }
   
    }
}
