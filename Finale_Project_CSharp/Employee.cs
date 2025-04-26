using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finale_Project_CSharp
{
    public class Employee:Info
    {
        public string DepartmentName { get; set; }
        public float Salary { get; set; }

        public Employee(){ }

        public Employee(string DepartmentName, float Salary, string Name, string LastName, string Phonenumber, string Password, Roles Role, int age) : base(Name, LastName, Phonenumber, Password, Role, age)
        {
            this.DepartmentName = DepartmentName;
            this.Salary = Salary;
        }
    }
}
