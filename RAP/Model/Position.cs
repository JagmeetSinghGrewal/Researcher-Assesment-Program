using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP.Model
{

    public enum EmploymentLevel { Student, A, B, C, D, E}
    public class Position
    {
        //variables
        private EmploymentLevel level;
        private DateTime start;
        private DateTime end;

        public EmploymentLevel Level
        {
            get { return level; }
            set { level = value; }
        }

        public DateTime Start
        {
            get { return start.Date; }
            set { start = value; }
        }

        public DateTime End
        {
            get { return end.Date; }
            set { end = value; }
        }

        public string Title
        {
            get { return ToTitle(Level); }
        }

        public string ToTitle(EmploymentLevel l)
        {
            string job = "N/A";
            //get the employment level
            switch (l)
            {
                case EmploymentLevel.Student:
                    job = "Student";
                    break;
                case EmploymentLevel.A:
                    job = "Postdoc";
                    break;
                case EmploymentLevel.B:
                    job = "Lecturer";
                    break;
                case EmploymentLevel.C:
                    job = "Senior Lecturer";
                    break;
                case EmploymentLevel.D:
                    job = "Associate Professor";
                    break;
                case EmploymentLevel.E:
                    job = "Professor";
                    break;
            }

            return job;
        }

            
    }
}
