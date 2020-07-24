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
    public enum Options { All, Student, A, B, C, D, E }
    /// <summary>
    /// Interaction logic for ResearcherListView.xaml
    /// </summary>
    public partial class ResearcherListView : UserControl
    {
        private Controller.ResearcherController controller;

        /// <summary>
        /// <para>Basic constructor for ResearcherListView</para>
        /// <para>Gets current controller instance</para>
        /// </summary>
        public ResearcherListView()
        {
            InitializeComponent();

            //get researcher and controller instance
            ObjectDataProvider researcherlist = (ObjectDataProvider)FindResource("researcherlist");
            controller = (Controller.ResearcherController)researcherlist.ObjectInstance;

            //ListBox initialisation
            ResearcherListView_DisplayBox.SelectionMode = SelectionMode.Single;
        }

        //////////Methods//////////////

        //Parses any strings into enums
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
        
        /// <summary>
        /// Users selects a level to filter the list with
        /// </summary>
        private void ResearcherListView_Sort_Box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Reset researhcer list selction
            ResearcherListView_DisplayBox.UnselectAll();

            //Get the original list of researchers
            controller.CurrentListCopy = controller.Reseachers;

            //If the user has selected to filter by a specific level (Not all researchers), filter by that level
            if (ParseEnum<Options>(e.AddedItems[0].ToString()) != Options.All)
            {
                controller.CurrentListCopy = controller.FilterByLevel(ParseEnum<Model.EmploymentLevel>(e.AddedItems[0].ToString()));
            }

            //If the user has already filtered by a name, filter the new list with that as well
            if (ResearcherListView_Filter_Box.Text != "" && ResearcherListView_Filter_Box.Text != " ")
            {
                controller.CurrentListCopy = controller.FilterByName(ResearcherListView_Filter_Box.Text);
            }

            //Display the filtered list
            controller.ViewableResearchers = new ObservableCollection<Model.Researcher>(controller.CurrentListCopy);
            ResearcherListView_DisplayBox.ItemsSource = controller.ViewableResearchers;
        }
        
        /// <summary>
        /// User selects a researcher from the list
        /// </summary>
        private void ResearcherListView_DisplayBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
           
            //Display the details of the selected researcher
            if (ResearcherListView_DisplayBox.SelectedIndex != -1)
            {
                Model.Researcher researcher = ResearcherListView_DisplayBox.SelectedItem as Model.Researcher;
                controller.LoadResearcherDetails(researcher);
            }
        }

        /// <summary>
        /// User inputs text into the name filter box
        /// </summary>
        private void ResearcherListView_Filter_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Reset current list
            controller.CurrentListCopy = controller.Reseachers;

            //if current name filter is empty, set displayed list to list filtered by any selected level
            if (ResearcherListView_Filter_Box.Text == "" || ResearcherListView_Filter_Box.Text == " ")
            {
                //If a level is selected
                if (ResearcherListView_Sort_Box.SelectedIndex != -1 && ParseEnum<Options>(ResearcherListView_Sort_Box.SelectedIndex.ToString()) != Options.All)
                {
                    //Filter current list by that level
                    controller.CurrentListCopy = controller.FilterByLevel(ParseEnum<Model.EmploymentLevel>(ResearcherListView_Sort_Box.SelectedValue.ToString()));
                }

                //Set displayed list to current filtered list
                controller.ViewableResearchers = new ObservableCollection<Model.Researcher>(controller.CurrentListCopy);
                ResearcherListView_DisplayBox.ItemsSource = controller.ViewableResearchers;
            }
            else //If a name filter has been entered, filter by the current select level, then by that name
            {
                //Filter the current list by the entered name, then by level if one is entered
                controller.CurrentListCopy = controller.FilterByName(ResearcherListView_Filter_Box.Text);
                if (ResearcherListView_Sort_Box.SelectedIndex != -1 && ParseEnum<Options>(ResearcherListView_Sort_Box.SelectedIndex.ToString()) != Options.All)
                {
                    controller.CurrentListCopy = controller.FilterByLevel(ParseEnum<Model.EmploymentLevel>(ResearcherListView_Sort_Box.SelectedValue.ToString()));
                }

                //Set displayed list to current filtered list
                controller.ViewableResearchers = new ObservableCollection<Model.Researcher>(controller.CurrentListCopy);
                ResearcherListView_DisplayBox.ItemsSource = controller.ViewableResearchers;
            }
        }
    }
}
