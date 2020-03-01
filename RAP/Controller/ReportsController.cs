using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RAP.Controller
{
    enum Report { Poor, Below, Meeting, Star};
    class ReportsController
    {
        private List<Model.Staff> poor = new List<Model.Staff>(); //List of staff with Poor metric
        private List<Model.Staff> below = new List<Model.Staff>(); //List of staff with Below expectations metric
        private List<Model.Staff> meeting = new List<Model.Staff>(); //List of staff with Meeting Minimum metric
        private List<Model.Staff> star = new List<Model.Staff>(); //List of staff with Star Performer metric
        private Controller.PublicationController pubController = new Controller.PublicationController(); //Publications controller

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

        /////////Methods/////////

        /// <summary>
        /// Adds each staff member to their respective performance list
        /// </summary>
        /// <param name="researchers">List of all researchers</param>
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

        /// <summary>
        /// Create the data object representing the report list
        /// </summary>
        /// <param name="type">Enumeration of specific report</param>
        /// <returns></returns>
        public IDictionary<Model.Researcher, float> Create_Table(Report type)
        {
            IDictionary<Model.Researcher, float> report;
            List<Model.Staff> staff; //variable to hold the staff of certain performance
            
            //select the type of report
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
            
            //Order the list in its appropriate order
            if (type ==Report.Below || type ==Report.Poor)
            {
                var filtered = from Model.Staff s in staff
                               orderby s.Performance() ascending
                               select s;
                report = filtered.ToDictionary(s=>(Model.Researcher)s, s=>s.Performance());
            }
            else
            {
                var filtered = from Model.Staff s in staff
                               orderby s.Performance() descending
                               select s;
                report = filtered.ToDictionary(s => (Model.Researcher)s, s => s.Performance());
            }

            return report;
        }

        /// <summary>
        /// Copies the email addresses of all staff members in the current list into the user's clipboard
        /// </summary>
        /// <param name="type">Enumeration of current report</param>
        public void Generate_Emails_List(Report type)
        {
            string emails = "";
            List<Model.Staff> staff;

            //Order the list in its appropriate order
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

            //Add emails into string
            foreach (Model.Staff i in staff)
            {
                emails += i.Email + " ";
            }

            //copy emails to clipboard
            Clipboard.SetText(emails);
            MessageBox.Show("Emails copied to the clipboard!");
        }
    }
}
