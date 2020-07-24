using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RAP.Controller
{
    class ResearcherController
    {
        //Variables
        public static List<Model.Researcher> researchers; //Basic researcher list
        public ObservableCollection<Model.Researcher> ViewableResearchers; //
        public PublicationController pubController; //Publications controllor
        private MainWindow Window { get { return Application.Current.MainWindow as MainWindow; } } //CUrrent main window
        private List<Model.Researcher> currentListCopy; //Copy of basic researcher list for filtering

        //Properties
        public List<Model.Researcher> Reseachers
        {
            get { return researchers; }
            set { researchers = value; }
        }

        public ObservableCollection<Model.Researcher> GetViewableList()
        {
            return ViewableResearchers;
        }

        public List<Model.Researcher> CurrentListCopy
        {
            get { return currentListCopy; }
            set { currentListCopy = value; }
        }

        /// <summary>
        /// <para>Basic ResearchController constructor</para>
        /// <para>Gets the basic researcher list and adds it to the view</para>
        /// </summary>
        public ResearcherController()
        {
            researchers = Database.ERDAdapter.FetchBasicResearcherDetails();
            CurrentListCopy = researchers;
            ViewableResearchers = new ObservableCollection<Model.Researcher>(researchers as List<Model.Researcher>);
            pubController = new Controller.PublicationController();
        }

        /// <summary>
        /// Filters the researcher list by an employment level
        /// </summary>
        /// <param name="l">Employment level being filtered for</param>
        /// <returns>Filtered list</returns>
        public List<Model.Researcher> FilterByLevel(Model.EmploymentLevel l)
        {
            var filtered = from Model.Researcher r in CurrentListCopy
                           where r.GetCurrentJob().Level == l
                           orderby r.FamilyName ascending, r.GivenName ascending
                           select r;
            CurrentListCopy = new List<Model.Researcher>(filtered);
            return CurrentListCopy;
        }

        /// <summary>
        /// Filters the research list by a name
        /// </summary>
        /// <param name="inputName">Text being filtered for</param>
        /// <returns>Filtered list</returns>
        public List<Model.Researcher> FilterByName(string inputName)
        {
            var filtered = from Model.Researcher r in CurrentListCopy
                           where r.GivenName.ToLower().Contains(inputName.ToLower()) || r.FamilyName.ToLower().Contains(inputName.ToLower())
                           select r;
            CurrentListCopy = new List<Model.Researcher>(filtered);
            return CurrentListCopy;
        }

        /// <summary>
        /// Loads full researcher details
        /// </summary>
        /// <param name="researcher">A researcher object with basic details</param>
        public void LoadResearcherDetails(Model.Researcher researcher)
        {
            //Get full details
            Model.Researcher details = Database.ERDAdapter.FetchFullResearcherDetails(researcher.ID, Reseachers);

            //Get publications
            details.Publications = pubController.LoadPublicationsFor(details);
            pubController.CurrentListCopy = details.Publications;

            //Adds details to view
            Window.ResearcherDetailView.SetDetails(details);
            Window.ResearcherDetailView.ActivatePubList();
            Window.CumulativeCountView.loadCumulativeCount(details);
        }
    }
}
