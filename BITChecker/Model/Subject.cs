using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.Model
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Primary key

        [Required]
        public string Code { get; set; } = null!;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Semester { get; set; }

        [Required]
        public int Credit { get; set; }

        [Required]
        public int GPACredit { get; set; }

        [Required]
        public char CourseType { get; set; }

        // One Subject to many PerSubjectScores
        public List<PerSubjectScore> PerSubjectScores { get; set; } = new();
    }
}
