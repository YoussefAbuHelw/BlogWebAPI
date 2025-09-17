using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }// PK
        [Required(ErrorMessage = "Title Is Required")]
        [StringLength(50, MinimumLength = 3)]
        //[MaxLength(50, ErrorMessage ="Max Length is 50 characters"), MinLength(3)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content Is Required")]
        [MaxLength(300), MinLength(10)]
        public string Content { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relations
        // 1. belongs to one category
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // 2 .one user create post
        public int UserID { get; set; }
        public User User { get; set; }


        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
