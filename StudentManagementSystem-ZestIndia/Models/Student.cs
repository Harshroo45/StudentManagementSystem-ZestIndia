using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem_ZestIndia.Models
{

    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [Range(1, 120)]
        public int Age { get; set; }

        [Required]
        [StringLength(100)]
        public string Course { get; set; } = null!;

        [Required]
        public DateTime CreatedDate { get; set; }
    }
}



