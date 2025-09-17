using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        //public Guid Id { get; set; }


        [Required(ErrorMessage = "UserName is Required")]
        [StringLength(100, MinimumLength = 3)]
        public string UserName { get; set; }



        [Required(ErrorMessage = "Email is Required")]
        [StringLength(100), EmailAddress]
        //[RegularExpression("/^[^ ] + @/")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is Required")]
        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }



        // Relations
        public ICollection<Post> Posts { get; set; } = new List<Post>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
