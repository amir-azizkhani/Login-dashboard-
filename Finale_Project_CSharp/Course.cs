using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Finale_Project_CSharp
{
    public class Course
    {
        #region Properties
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(30)]
        public string CourseName { get; set; }
        [Required]
        public int CourseUnit { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsActive { get; set; }
        #endregion


        #region Connections
        // 1 * chand = student
        public virtual ICollection<Student> Students { get; set; }
        // 1 * 1 = master
        public virtual Master Master { get; set; }
        #endregion


        public Course() { }
        public Course(int Id, string CourseName, int CourseUnit, Master Master)
        {
            this.Id = Id;
            this.CourseName = CourseName;
            this.CourseUnit = CourseUnit;
            this.Master = Master;
            this.RegisterDate = DateTime.Now;
            this.IsActive = true;
        }
        public Course(string CourseName, int CourseUnit, Master Master)
        {
            this.CourseName = CourseName;
            this.CourseUnit = CourseUnit;
            this.Master = Master;
            this.RegisterDate = DateTime.Now;
            this.IsActive = true;
        }
    }
}
