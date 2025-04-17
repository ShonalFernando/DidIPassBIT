using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.Model
{
    public class PerSubjectScore
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int SubjectId { get; set; }  // FK to Subject
        public Subject Subject { get; set; } = null!;

        [Required]
        public int GPAId { get; set; }      // FK to GPA
        public GPA GPA { get; set; } = null!;

        public decimal PointValue { get; set; }

        public decimal QaulityPoint { get; set; } // Point Value x Credits
    }
}
