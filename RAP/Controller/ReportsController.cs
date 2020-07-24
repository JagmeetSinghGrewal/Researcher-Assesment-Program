using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP.Controller
{
    enum Report { Poor, Below, Meeting, Star};
    class ReportsController
    {
        private List<Model.Staff> poor = new List<Model.Staff>();
        private List<Model.Staff> below = new List<Model.Staff>();
        private List<Model.Staff> meeting = new List<Model.Staff>();
        private List<Model.Staff> star = new List<Model.Staff>();
        private Controller.PublicationController pubController = new Controller.PublicationController();

        public List<Model.Staff> Poor
        {
            get { return poor; }
        }

        public List<Model.Staff> Below
        {
            get { return below; }
        }

        public List<Model.Staff> Meeting
        {
            get { return meeting; }
        }

        public List<Model.Staff> Star
        {
            get { return star; }
        }

        public void Generate(List<Model.Researcher> researchers)
        {

            foreach (Model.Researcher a in researchers)
            {
                if (a.Type=="Staff")
                {
                    Model.Staff i = (Model.Staff)a;
                    i.Publications = new List<Model.Publication>(pubController.LoadPublicationsFor(i));
                    float performance = i.Performance();
                    if (performance <= 70f)
                    {
                        poor.Add(i);
                    }
                    else if (performance > 70f && performance < 110f)
                    {
                        below.Add(i);
                    }
                    else if (performance >= 100f && performance < 200f)
                    {
                        meeting.Add(i);
                    }
                    else if (performance >= 200f)
                    {
                        star.Add(i);
                    }
                }
            }

        }

        public IDictionary<Model.Researcher, float> Create_Table(Report type)
        {
            IDictionary<Model.Researcher, float> report;
            List<Model.Staff> staff;
            
            switch (type)
            {
                case Report.Poor:
                    staff = Poor;
                    
                    break;
                case Report.Below:
                    staff = Below;
                    
                    break;
                case Report.Meeting:
                    staff = Meeting;
                    break;
                default:
                    staff = Star;
                    break;
            }
            
            if (type ==Report.Below || type ==Report.Poor)
            {
                var filtered = from Model.Staff s in staff
                               orderby s.Performance() ascending
                               select s;
                report = filtered.ToDictionary(s=>(Model.Researcher)s, s=>s.Performance());
            } else
            {
                var filtered = from Model.Staff s in staff
                               orderby s.Performance() descending
                               select s;
                report = filtered.ToDictionary(s => (Model.Researcher)s, s => s.Performance());
            }
            return report;
        }

        public List<string> Generate_Emails_List(Report type)
        {

            return null;
        }
    }
}
