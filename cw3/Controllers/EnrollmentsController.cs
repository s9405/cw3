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
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _service;
        public IConfiguration Configuration { get; set; }

        public EnrollmentsController(IStudentDbService dbService, IConfiguration configuration)
        {
            _service = dbService;
            Configuration = Configuration;
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult Enroll(EnrollStudentRequest student)
        {
            var response = _service.EnrollStudent(student);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest();
        }
/*
        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {

            Student student = _service.GetStudent(request.Login);


            if (!request.Haslo.Equals(student.Password))
            {
                return Unauthorized("Incorrect password !");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, student.IndexNumber),
                new Claim(ClaimTypes.Name, student.FirstName),
                new Claim(ClaimTypes.Surname, student.LastName),
                new Claim(ClaimTypes.Role, "employee") 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Gakko",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid()
            });
        }*/

    }
}