using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.Helper
{
    internal class GradeToDecimalConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string grade)
            {
                return grade switch
                {
                    "A+" => 4.0m,
                    "A" => 4.0m,
                    "A-" => 3.7m,
                    "B+" => 3.3m,
                    "B" => 3.0m,
                    "B-" => 2.7m,
                    "C+" => 2.3m,
                    "C" => 2.0m,
                    "C-" => 1.7m,
                    "D+" => 1.5m,
                    "D" => 1.0m,
                    "E" => 0.0m,
                    "Pass" => 0.00m,
                    "Fail" => 0.00m,
                    _ => 0.0m // Default if unknown
                };
            }

            return 0.0m;
        }
    }
}
