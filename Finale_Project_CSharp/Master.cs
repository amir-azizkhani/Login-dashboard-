using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finale_Project_CSharp
{
    public class Master : Info
    {
        public string FieldOfStudy { get; set; }
        public float Salary { get; set; }

        // 1 * chand = course
        [ForeignKey("Id")]
        public virtual IEnumerable<Course> Courses { get; set; }

        public Master() { }

        public Master(string FieldOfStudy, float Salary, string Name, string LastName, string Phonenumber, string Password, Roles Role, int age) : base(Name, LastName, Phonenumber, Password, Role, age)
        {
            this.FieldOfStudy = FieldOfStudy;
            this.Salary = Salary;
        }


    }
}
