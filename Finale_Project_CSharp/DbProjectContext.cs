using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Finale_Project_CSharp
{
    class DbProjectContext : DbContext
    {
        public DbProjectContext(string name) : base(name) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Master> Masters { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Roles> Roles { get; set; }


    }
}
