using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;

namespace Finale_Project_CSharp
{
    public class Student : Info
    {
        public string FieldOfStudy { get; set; }
        [Key]
        public int StudentCode { get; set; }
        public static int StudentNumber { get; set; } = 403;
        public int TuitionOfEachTerm { get; set; }
        public static int RandomTuition { get; set; } = 0;
        

        // 1 * chand = Course
        public virtual ICollection<Course> Courses { get; set; }

        public Student() { }

        public Student(string FieldOfStudy, string Name, string LastName, string Phonenumber, string Password, Roles Role, int age) :base(Name,LastName,Phonenumber,Password, Role, age)
        {
            int StudentNumber = GetMaxStudentCodeFromDb();
            StudentCode = ++StudentNumber;
            this.FieldOfStudy = FieldOfStudy;
            Courses = new List<Course>();
            Random random = new Random();
            RandomTuition = random.Next(3000,10001);
            TuitionOfEachTerm = RandomTuition;
        }

        private int GetMaxStudentCodeFromDb()
        {
            int maxCode = 0;
            using (SqlConnection connectionForMaxNum = new SqlConnection ("ProjectConnStr"))
            {
                string query = "Select Max (StudentCode) from Infoes";
                SqlCommand commandMax = new SqlCommand(query, connectionForMaxNum);
                connectionForMaxNum.Open();
                object resault = commandMax.ExecuteScalar();
                if (resault != null && resault != DBNull.Value)
                {
                    maxCode = Convert.ToInt32(resault);
                }
            }
            return maxCode;
        }
    }
    
}
