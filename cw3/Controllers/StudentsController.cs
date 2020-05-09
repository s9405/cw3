using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw3.DAL;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using cw3.Services;

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentDbService _dbService;

        public StudentsController(IStudentDbService dbService)
        {
            _dbService = dbService;
        }
        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
   
            return Ok();
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

        [HttpPost]

        public IActionResult CreateStudent(Student student)
        {
            //... add to database
            //.. generating index number
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
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
    }
}