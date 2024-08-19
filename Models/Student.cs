using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        public int ClassStreamId { get; set; }

        [ForeignKey("ClassStreamId")]
        public virtual ClassStream ClassStream { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    public class Class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class ClassStream
    {
        [Key]
        public int ClassStreamId { get; set; }

        [Required]
        [StringLength(50)]
        public string StreamName { get; set; }

        [Required]
        public int ClassId { get; set; }

        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }

    public class StudentByClassStreamViewModel
    {
        public ClassStream ClassStream { get; set; }
        public List<Student> Students { get; set; }
    }
}