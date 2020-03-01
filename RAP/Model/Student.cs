using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP.Model
{
    class Student : Researcher
    {
        //variables
        private string degree;
        private int supervisor;
        private string supervisorName;

        public string Degree
        {
            get { return degree; }
            set { degree = value; }
        }

        public int Supervisor
        {
            get { return supervisor; }
            set { supervisor = value; }
        }

        public string SupervisorName
        {
            get { return supervisorName; }
            set { supervisorName = value;  }
        }
    }
}
