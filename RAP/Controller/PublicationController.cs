using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RAP.Controller
{
    class PublicationController
    {
        private Model.Researcher currentResearcher; //Currently displayed researcher details
        private List<Model.Publication> currentListCopy; //Copy of the currently displayed researcher list
        private MainWindow Window { get { return Application.Current.MainWindow as MainWindow; } } //Main application window

        public Model.Researcher CurrentResearcher
        {
            get { return currentResearcher; }
        }

        public List<Model.Publication> CurrentListCopy
        {
            get { return currentListCopy; }
            set { currentListCopy = value; }
        }

        /// <summary>
        /// Basic constructor for PublicationController
        /// </summary>
        public PublicationController()
        {
            
        }

        /////////Methods/////////

        /// <summary>
        /// Loads all publications for a given researcher
        /// </summary>
        /// <param name="researcher">The researcher whose publications are being loaded</param>
        /// <returns>A list of basic publications</returns>
        public List<Model.Publication> LoadPublicationsFor(Model.Researcher researcher)
        {
            //set variables for current researcher
            currentResearcher = researcher;

            //return the basic publication details
            return Database.ERDAdapter.fetchBasicPublicationDetails(researcher);
        }

        /// <summary>
        /// Filters the publication list by a given year range
        /// </summary>
        /// <param name="range_min">The lower range of years</param>
        /// <param name="range_max">The upper range of years</param>
        /// <returns>A list of publications filtered by year</returns>
        public List<Model.Publication> filterByYear(int range_min, int range_max)
        {
            var filtered = from Model.Publication p in CurrentListCopy
                           where p.Year.Year >= range_min && p.Year.Year <= range_max
                           select p;
            CurrentListCopy = new List<Model.Publication>(filtered);
            return CurrentListCopy;
        }

        /// <summary>
        /// Loads the full details of a given publication and adds them to the view
        /// </summary>
        /// <param name="publication">The given publication</param>
        public void loadPublicationDetails(Model.Publication publication)
        {
            //get details
            Model.Publication p = Database.ERDAdapter.fetchFullPublicationDetails(publication);

            //load view
            Window.PublicationsDetailsView.SetDetails(p);

        }
    }
}
