using cw3.Models;
using cw3.Requests;
using cw3.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {

        public IEnumerable<Student> GetStudents()
        {
            var tmp = new List<Student>();

          
            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s9405;Integrated Security=True"))
            using (var command = new SqlCommand())

            {
                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                command.CommandText = "select * from Student";
           
                
                var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.BirthDate = (DateTime)dr["BirthDate"];
                    tmp.Add(st);


                }
            }
            
    
            return tmp;
        }
        public Student GetStudent(string Index)
        {
            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s9405;Integrated Security=true"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                command.CommandText = "SELECT IndexNumber, FirstName, LastName, Password  FROM student where indexnumber = @index";
                command.Parameters.AddWithValue("index", Index);
                var dr = command.ExecuteReader();
                if (!dr.Read())
                {
                    
                    throw new ArgumentException("Brak studentow o powyzszym id");
                }

                Student student = new Student();

                student.IndexNumber = dr["IndexNumber"].ToString();
                student.FirstName = dr["FirstName"].ToString();
                student.LastName = dr["LastName"].ToString();
                student.Password = dr["Password"].ToString();
                dr.Close();

                return student;
            }
        }
        [HttpPost]
        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest student)
        {

            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s9405;Integrated Security=true"))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                DateTime date = DateTime.Now;
                DateTime BD = DateTime.Parse(student.BirthDate);

                command.CommandText = "select IdStudy from studies where name=@name";
                command.Parameters.AddWithValue("firstname", student.FirstName);
                command.Parameters.AddWithValue("lastname", student.LastName);
                command.Parameters.AddWithValue("birthdate", BD);
                command.Parameters.AddWithValue("name", student.Studies);
                command.Parameters.AddWithValue("indexnumber", student.IndexNumber);
                command.Parameters.AddWithValue("date", date);

                int idStudy;

                var dr = command.ExecuteReader();
                if (!dr.HasRows)
                {
                    transaction.Rollback();
                    throw new ArgumentException("Brak powyższych studiów");
                }
                dr.Read();
                idStudy = dr.GetInt32(0);
                dr.Close();

                command.Parameters.AddWithValue("idstudy", idStudy);

                command.CommandText = "SELECT enrollment.semester, studies.name FROM enrollment, studies WHERE enrollment.idstudy = studies.idstudy AND enrollment.semester = 1 AND studies.name = @name";
                dr = command.ExecuteReader();

                if (!dr.HasRows)
                {
                    command.CommandText = "INSERT INTO enrollment(semester,idstudy,startdate) values (1,@idstudy,@date)";
                    command.ExecuteNonQuery();
                }
                dr.Close();
                command.CommandText = "SELECT indexnumber FROM student WHERE indexnumber = @indexnumber";
                dr = command.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Close();
                    transaction.Rollback();
                    throw new ArgumentException("student juz istnieje");
                }
                dr.Close();
                command.CommandText = "select idenrollment from enrollment where idstudy = @idstudy and semester = 1";
                dr = command.ExecuteReader();
                dr.Read();
                int idenrollment = dr.GetInt32(0);
                dr.Close();
                command.Parameters.AddWithValue("idenrollment", idenrollment);
                
                command.CommandText = "insert into student(indexnumber,firstname,lastname,birthdate,idenrollment) values(@indexnumber,@firstname,@lastname,@birthdate,@idenrollment)";

                dr = command.ExecuteReader();
                dr.Close();

                transaction.Commit();
                command.Parameters.Clear();
                connection.Close();
                return new EnrollStudentResponse
                {
                    StartDate = date,
                    IndexNumber = student.IndexNumber,
                    LastName = student.LastName
                };
            }
        }
        public PromoteStudentResponse PromoteStudent(PromoteStudentRequest promotion)
        {

            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s9405;Integrated Security=true"))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                Console.WriteLine(promotion.Studies);

                command.Parameters.AddWithValue("name", promotion.Studies);
                command.Parameters.AddWithValue("semester", promotion.Semester);
                command.CommandText = "exec promocja @name,@semester";
                command.ExecuteNonQuery();

                command.CommandText = "SELECT * FROM enrollment, studies WHERE enrollment.idstudy = studies.idstudy AND studies.name = @name AND enrollment.semester = @semester+1";

                var dr = command.ExecuteReader();


                if (!dr.HasRows)
                {
                    dr.Close();
                    transaction.Rollback();
                    throw new Exception("no promoted students");
                }

                dr.Read();
                PromoteStudentResponse response = new PromoteStudentResponse
                {
                    IdEnrollment = int.Parse(dr["IdEnrollment"].ToString()),
                    Semester = int.Parse(dr["Semester"].ToString()),
                    IdStudy = int.Parse(dr["IdStudy"].ToString()),
                    StartDate = DateTime.Parse(dr["StartDate"].ToString())

                };

                dr.Close();
                transaction.Commit();

                return response;
            }
        }
    }
}
