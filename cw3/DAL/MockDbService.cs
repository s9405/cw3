using cw3.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students;

        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student{IdStudent=1, FirstName="Jan", LastName="Kowalski" },
                new Student{IdStudent=2, FirstName="Anna", LastName="Malewski" },
                new Student{IdStudent=1, FirstName="Andrzej", LastName="Andrzejewicz" }
            };

        }

        public IEnumerable<Student> GetStudents( string indexNumber)
        {
            var tmp = new List<Student>();
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s9405;Integrated Security=True"))
            using (var com = new SqlCommand())
       
            {
                string query = "select IdEnrolment from Student where IndexNumber = " + indexNumber + ";";
                string id = "1";
                com.Connection = con;
                //com.CommandText = query;
                com.CommandText = "select * from Students where IndexNumber=@id";
                com.Parameters.AddWithValue("id", id);
                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.BirthDate = dr["BirthDate"].ToString();
                    st.IdEnrollment = dr["IdEnrollment"].ToString();
                    tmp.Add(st);
                    

                }
            }
            _students = tmp;
            return _students;
        }
    }
}
