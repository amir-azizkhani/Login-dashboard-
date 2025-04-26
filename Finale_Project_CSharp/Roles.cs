using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Finale_Project_CSharp
{
    public class Roles
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int UserId { get; set; }
        [Required]
        [Column("Title",TypeName ="varchar")]
        [MaxLength(20)]
        public string UserTitle { get; set; }
        [Required]
        [Column(TypeName = "varchar")]
        [MaxLength(20)]
        [Index("Key-Name",IsUnique =true)]
        public string UserName { get; set; }


        // 1 * chand = userinfo
        public virtual ICollection<Info> UserInfos { get; set; }

        public Roles(){}

        public Roles(int UserId, string UserTitle,string UserName)
        {
            this.UserId = UserId;
            this.UserTitle = UserTitle;
            this.UserName = UserName;
        }
    
    
    
    }
}
