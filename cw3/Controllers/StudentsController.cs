using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cw3.DAL;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }
        [HttpGet("{id}")]
        public IActionResult GetStudents(int id)
        {
   
            return Ok(_dbService.GetStudents(id.ToString()));
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