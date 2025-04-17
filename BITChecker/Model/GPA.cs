using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.Model
{
    public class GPA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime DateCalculated { get; set; } = DateTime.Now;

        // One GPA to many scores
        public List<PerSubjectScore> SubjectScores { get; set; } = new();

        [Required]
        public decimal GPACalculated { get; set; }

        [Required]
        public string Status { get; set; } = "Calculating...";
    }
}
