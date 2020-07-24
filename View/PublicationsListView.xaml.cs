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
    /// Interaction logic for PublicationsListView.xaml
    /// </summary>
    public partial class PublicationsListView : UserControl
    {
        private Controller.ResearcherController researcherController;

        /// <summary>
        /// <para>Basic Constructor for PublicationListView</para>
        /// <para>Gets the current instance of researchController</para>
        /// </summary>
        public PublicationsListView()
        {
            InitializeComponent();
            //get researcher and controller instance
            ObjectDataProvider researcherlist = (ObjectDataProvider)FindResource("researcherlist");
            researcherController = (Controller.ResearcherController)researcherlist.ObjectInstance;
        }

        //////////Methods//////////////

        /// <summary>
        /// Users enters a range of years to filter the publication list by
        /// </summary>
        private void PublicationsListView_Button_Range_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Reset list
                researcherController.pubController.CurrentListCopy = researcherController.pubController.CurrentResearcher.Publications;

                //Get range values
                int range_min = Int32.Parse(PublicationsListView_Range_From_Input.Text);
                int range_max = Int32.Parse(PublicationsListView_Range_To_Input.Text);

                //check range values are valid
                if (range_max - range_min >= 0)
                {
                    //Call filter function and Set displayed list to filtered list
                    PublicationsListView_Display_Box.ItemsSource = new ObservableCollection<Model.Publication>(researcherController.pubController.filterByYear(range_min, range_max));
                }
                else
                {
                    //Display error message
                    throw new FormatException();
                }
            }
            catch(FormatException)
            {
                MessageBox.Show("Please enter a valid publication year range", "Publication Range Error");
            }
        }

        /// <summary>
        /// User resets filter by year
        /// </summary>
        private void PublicationsListView_Button_Range_Reset_Click(object sender, RoutedEventArgs e)
        {
            PublicationsListView_Range_From_Input.Text = "0000";
            PublicationsListView_Range_To_Input.Text = "0000";
            researcherController.pubController.CurrentListCopy = researcherController.pubController.CurrentResearcher.Publications;
            PublicationsListView_Display_Box.ItemsSource = new ObservableCollection<Model.Publication>(researcherController.pubController.CurrentResearcher.Publications);
        }

        /// <summary>
        /// User inverts the publication list
        /// </summary>
        private void PublicationsListView_Button_Range_Invert_Click(object sender, RoutedEventArgs e)
        {
            //invert list and display it
            researcherController.pubController.CurrentListCopy.Reverse();
            PublicationsListView_Display_Box.ItemsSource = new ObservableCollection<Model.Publication>(researcherController.pubController.CurrentListCopy);
        }

        /// <summary>
        /// User selects a publication from the list
        /// </summary>
        private void PublicationsListView_Display_Box_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            //If a publication has been selected
            if (PublicationsListView_Display_Box.SelectedIndex != -1)
            {
                //load details in controller
                Model.Publication publication = PublicationsListView_Display_Box.SelectedItem as Model.Publication;
                researcherController.pubController.loadPublicationDetails(publication);
            }
        }

        /// <summary>
        /// Activates inputs of researcher details view
        /// </summary>
        public void ActivateInputs()
        {
            PublicationsListView_Button_Invert.IsEnabled = true;
            PublicationsListView_Range_From_Input.IsEnabled = true;
            PublicationsListView_Range_To_Input.IsEnabled = true;
            PublicationsListView_Button_Range.IsEnabled = true;
            PublicationsListView_Button_Range_Reset.IsEnabled = true;
        }
    }
}
