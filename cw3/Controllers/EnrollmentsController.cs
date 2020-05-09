﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using cw3.Services;
using cw3.Requests;
using cw3.Responses;

namespace cw3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _service;

        public EnrollmentsController(IStudentDbService dbService)
        {
            _service = dbService;
        }

        [HttpPost]
        public IActionResult Enroll(EnrollStudentRequest student)
        {
            var response = _service.EnrollStudent(student);
            if (response != null)
            {
                return Ok(response);
            }
            return BadRequest();
        }
      
    }
}