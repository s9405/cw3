using cw3.Models;
using cw3.Requests;
using cw3.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Services
{
    public interface IStudentDbService
    {
        EnrollStudentResponse EnrollStudent(EnrollStudentRequest student);
        PromoteStudentResponse PromoteStudent(PromoteStudentRequest request);
        Student GetStudent(String index);
        public IEnumerable<Student> GetStudents();

    }
}
