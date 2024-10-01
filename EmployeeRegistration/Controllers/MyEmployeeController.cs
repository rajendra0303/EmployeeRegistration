using EmployeeRegistration.Controllers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeRegistration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyEmployeeController : Controller
    {
        private readonly EmpMigration _context;
        private readonly IConfiguration _configuration;
        public MyEmployeeController(EmpMigration conaxt, IConfiguration configuration)
        {

            _context = conaxt;
            _configuration = configuration;

        }
        [HttpPost]
        [Route("EmployeeRegitration")]

        public IActionResult EmpRegistration(EmployeeDOT employeeDOT)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emp = _context.Employees.FirstOrDefault(x => x.UserName == employeeDOT.UserName);
            if (emp == null)
            {
                _context.Employees.Add(new Employee

                {
                    UserName = employeeDOT.UserName,


                    Password = employeeDOT.Password,

                    EmpName = employeeDOT.EmpName,

                    EmpAddres = employeeDOT.EmpAddres,

                    Position = employeeDOT.Position,

                });

                _context.SaveChanges();

                return Ok("Successfully Register");
            }
            else
            {
                return BadRequest("Already Exisst");
            }
        }

        [HttpPost]
        [Route("EmpLogin")]
        public IActionResult EmpLogin(EmpLoginDOT empLoginDOT)
        {
            var emp = _context.Employees.FirstOrDefault(x => x.UserName == empLoginDOT.UserName && x.Password == empLoginDOT.Password);

            if (emp != null)
            {
                var Claims = new[]
                {
        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("EmpId", emp.EmpId.ToString()),
        // Avoid storing sensitive info like passwords in JWT claims
        new Claim("EmpName", emp.EmpName),
        new Claim("Position", emp.Position) // Optional additional information
    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var SignIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],   // Corrected: Removed space
                    _configuration["Jwt:Audience"], // Make sure this key exists in appsettings.json
                    Claims,
                    expires: DateTime.UtcNow.AddMonths(2), // Usually, tokens last for a shorter period, like hours or days
                    signingCredentials: SignIn
                );

                string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new { Token = tokenValue, Employee = emp });
            }

            return Unauthorized();

        }
        [Authorize]
        [HttpGet]
        [Route("GetEmployee{id}")]
        public IActionResult Get(int id)
        {
            var emp = _context.Employees.FirstOrDefault(x => x.EmpId == id);
            if (emp != null)
            {
                return Ok(emp); //200 ok

            }
            else
            {
                return NotFound(); //404 not found
            }

        }

        [HttpGet]
        [Route("GetEmp")]
        public IActionResult Get()
        {
            return Ok(_context.Employees.ToList());
        }

        [HttpPost]
        [Route("EmpAllRecords")]
        public IActionResult GetAll()
        {
            return Ok(_context.Employees.ToList());

        }


        [HttpPut]
        [Route("EmpUpdate")]

        public IActionResult EmpUpdate(int id, EmployeeDOT employeeDOT)
        {
            var emp = _context.Employees.Find(id);
            if (emp == null)
            {
                return NotFound("Not Found Emp");
            }

            emp.UserName = employeeDOT.UserName;


            emp.Password = employeeDOT.Password;

            emp.EmpName = employeeDOT.EmpName;

            emp.EmpAddres = employeeDOT.EmpAddres;

            emp.Position = employeeDOT.Position;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok("Employee Record Update successfully");
            return NoContent();

        }

        [HttpDelete]

        [Route("EmpDelete")]

        public IActionResult EmpDelete(int id)

        {

            var Emp = _context.Employees.Find(id);

            if (Emp == null)
            {

                return NoContent();

            }

            _context.Remove(Emp);
            _context.SaveChanges();
            return Ok("Delete Successfuly Employee Record");




        }



    }
}
