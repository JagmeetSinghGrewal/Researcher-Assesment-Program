using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RAP.View
{
    /// <summary>
    /// Interaction logic for CumulativeCountView.xaml
    /// </summary>
    public partial class CumulativeCountView : UserControl
    {
        private Controller.ResearcherController researcherController;
        /// <summary>
        /// <para>Basic Constructor for Culmulative Count</para>
        /// <para>Gets the current researcher controller</para>
        /// </summary>
        public CumulativeCountView()
        {
            InitializeComponent();
            //get researcher and controller instance
            ObjectDataProvider researcherlist = (ObjectDataProvider)FindResource("researcherlist");
            researcherController = (Controller.ResearcherController)researcherlist.ObjectInstance;
        }

        //////////Methods//////////////

        /// <summary>
        /// Loads the culmulative publication counts per year for a given researcher
        /// </summary>
        /// <param name="researcher">The researhcer the counts are being calculated for</param>
        public void LoadCumulativeCount(Model.Researcher researcher)
        {
            //Calculate the counts
            researcher.calPubPerYear();

            //Get the count for each year and create a list out of those value
            List<KeyValuePair<string, int>> data = researcher.CumulativeCount.ToList();

            //Add list to view
            CumulativeCountView_Display_Data.ItemsSource = data;
        }

    }
}
