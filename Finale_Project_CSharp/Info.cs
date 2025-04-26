using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Finale_Project_CSharp
{
    public abstract class Info
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar")]
        [MaxLength(15)]
        public string Name { get; set; }
        [Column(TypeName = "varchar")]
        [MaxLength(15)]
        public string LastName { get; set; }
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(11)]
        public string Phonenumber { get; set; }
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(12)]
        public string Password { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime Registerdate { get; set; }
        [Required]
        public Roles Role { get; set; }
        public bool IsActive { get; set; }

        // 1 * 1 = active infos

        public Info() { }


        public Info(int Id, string Name, string LastName, string Phonenumber, string Password, Roles Role,int age)
        {
            this.Id = Id;
            this.Name = Name;
            this.LastName = LastName;
            this.Phonenumber = Phonenumber;
            this.Password = Password;
            this.Role = Role;
            Birthdate = DateTime.Now.AddDays(-age);
            Registerdate = DateTime.Now;
            IsActive = true;
        }
        public Info(string Name, string LastName, string Phonenumber, string Password, Roles Role, int age)
        {
            this.Name = Name;
            this.LastName = LastName;
            this.Phonenumber = Phonenumber;
            this.Password = Password;
            this.Role = Role;
            Birthdate = DateTime.Now.AddDays(-age);
            Registerdate = DateTime.Now;
            IsActive = true;
        }
    }

}
