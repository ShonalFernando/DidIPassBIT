using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.Model
{
    class SubjectScore
    {
        public Guid Guid { get; set; }  //Ref

        // Subject Information
        public int SubjectID { get; set; }
        public string SubjectCode { get; set; } = null!;
        public string SubjectName { get; set; } = null!;
        public bool isEnhancement { get; set; }

        // Sem & Year
        public int Semester { get; set; }
        public int Year { get; set; }

        public decimal Credit { get; set; }

        public decimal Grade { get; set; }

        public decimal Weight { get; set; } // Grade x Credit
    }
}
