using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.Model
{
    public class Subject
    {
        public int Id { get; set; }

        public string SubjectCode { get; set; } = "ITXXXX";

        public string SubjectName { get; set; } = null!;

        public int Semester { get; set; }
        public int Year { get; set; }

        public int Credits { get; set; }
        public int GPACredits { get; set; }
    }
}
