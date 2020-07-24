using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAP.Model
{
    public class Researcher
    {
        //variables
        private int id;
        private string givenName;
        private string familyName;
        private string type;
        private string title;
        private string school;
        private string campus;
        private string email;
        private Uri photo;
        private List<Position> positions;
        private List<Publication> publications;
        private IDictionary<string, int> cumulativeCount = new Dictionary<string, int>();

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string GivenName{
            get {   return givenName;   }
            set {   givenName = value;  }
        }

        public string FamilyName
        {
            get { return familyName; }
            set { familyName = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string School
        {
            get { return school; }
            set { school = value; }
        }

        public string Campus
        {
            get { return campus; }
            set { campus = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public Uri Photo
        {
            get { return photo; }
            set { photo = value; }
        }

        public List<Position> Positions
        {
            get { return positions; }
            set { positions = value; }
        }

        public List<Publication> Publications
        {
            get { return publications; }
            set { publications = value; }
        }

        public IDictionary<string, int> CumulativeCount
        {
            get { return cumulativeCount; }
        }

        /////////Methods/////////
        public Position GetCurrentJob()
        {
            foreach (Position i in Positions)
            {
                if(i.End == default(DateTime))
                {
                    return i;
                }
            }
            return null;
        }

        public string CurrentJobTitle()
        {
            return GetCurrentJob().Title; 
        }

        public DateTime CurrentJobStart()
        {
            return GetCurrentJob().Start; 
        }

        public Position GetEarliestJob()
        {
            Position current = Positions[0];

            foreach (Position i in Positions)
            {
                //If the start date of current is earlier than the current element
                if (i.Start.CompareTo(current.Start) < 0)
                {
                    current = i;
                }
            }
            return current;
        }

        public float Tenure()
        {

            //check if end date is set
            float tenure;
            Position firstJob = GetEarliestJob();
            Console.WriteLine();
            if (DateTime.Compare(GetCurrentJob().End, default(DateTime)) != 0)
            {

                tenure = (float)((GetCurrentJob().End - firstJob.Start).Days/365f);
            }
            else
            {
                tenure = (float)((DateTime.Now - firstJob.Start).Days/365f);
            }
            //calc tenure (current date if end date is not set)
            return (float)Math.Round(tenure, 2);//Check whether end is set, 
        }

        public int PublicationsCount()
        {
            int count = 0;
            foreach (Publication i in publications)
            {
                count++;
            }
            return count;
        }

        public int PublicationsCount3Year()
        {
            int count = 0;
            foreach (Publication i in publications)
            {
                if (i.Year.Year <= (DateTime.Now.Year - 1) && i.Year.Year >= (DateTime.Now.Year - 3))
                {
                    count++;
                }
            }
            return count;
        }

        public void calPubPerYear()
        {
            int cur = GetEarliestJob().Start.Year;
            int prev = -1;
            while(cur <= DateTime.Now.Year)
            {
                cumulativeCount[cur.ToString()] = publications.Where(s => s != null && s.Year.Year == cur).Count();
                if (prev == -1)
                {
                    cumulativeCount[cur.ToString()] = publications.Where(s => s != null && s.Year.Year == cur).Count();
                } else
                {
                    cumulativeCount[cur.ToString()] = publications.Where(s => s != null && s.Year.Year == cur).Count() + cumulativeCount[prev.ToString()];
                }
                prev = cur;
                cur++;
            }
        }

        public override string ToString()
        {
            return FamilyName + ", " + GivenName+" ("+Title+")";
        }
    }
}