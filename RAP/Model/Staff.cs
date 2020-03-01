using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP.Model
{
    class Staff : Researcher
    {
        //variables
        private List<Student> supervisions;

        public List<Student> Supervisions
        {
            get { return supervisions; }
            set { supervisions = value;  }
        }

        /////////Methods/////////

        //adds a single student to the staff member's supervision list
        public void AddSupervision(Student student)
        {
            Supervisions.Add(student);
        }
        
        //average publications over three years
        public float ThreeYearAverage()
        {
            return PublicationsCount3Year() / 3f;
        }

        //performance value calculation (ThreeYearAverage / expected number of puclications)
        public float Performance()
        {
            //get employment level
            float p = 1f;
            EmploymentLevel pos = GetCurrentJob().Level;

            switch(pos)
            {
                case EmploymentLevel.A:
                    p = 0.5f;
                    break;
                case EmploymentLevel.B:
                    p = 1f;
                    break;
                case EmploymentLevel.C:
                    p = 2;
                    break;
                case EmploymentLevel.D:
                    p = 3.2f;
                    break;
                case EmploymentLevel.E:
                    p = 4f;
                    break;
            }

            //calc performance
            return (float)Math.Round(100f * (ThreeYearAverage() / p), 1);
        }
    }
}
