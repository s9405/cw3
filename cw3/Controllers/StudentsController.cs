using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using cw3.Services;
using cw3.Requests;
using cw3.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using cw3.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentDbService _dbService;

        public IConfiguration Configuration;

        public StudentsController(IStudentDbService dbService, IConfiguration configuration)
        {
            Configuration = configuration;
            _dbService = dbService;
        }
        [HttpGet]
        [Authorize(Roles = "employee")]
        public IActionResult GetStudents()
        {
            var list = new List<Student>();
            list.AddRange
                (_dbService.GetStudents());
            return Ok(list);
        }
 
        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Kowalski");
            }
            else if (id == 2)
            {
                return Ok("Maleski");
            }

            return NotFound("Nie znaleziono studenta");
        }

        [HttpPut("{id}")]
        public IActionResult Put()
        {
            return StatusCode(200, "Aktualizacja dokonczona");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete()
        {
            return StatusCode(200, "Usuwanie ukonczone");
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {

            Student student = _dbService.GetStudent(request.Login);


            if (!request.Haslo.Equals(student.Password))
            {
                return Unauthorized("Incorrect password !");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, student.IndexNumber),
                new Claim(ClaimTypes.Name, student.FirstName),
                new Claim(ClaimTypes.Surname, student.LastName),
                new Claim(ClaimTypes.Role, "employee"),
                new Claim(ClaimTypes.Role, "admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Gakko",
                audience: "Student",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid()
            });
        }
    }
}