﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Responses
{
    public class PromoteStudentResponse
    {
        public int IdEnrollment { get; set; }
        public int IdStudy { get; set; }

        public int Semester { get; set; }
        public DateTime StartDate { get; set; }
    }
}
