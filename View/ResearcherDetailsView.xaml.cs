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
    /// Interaction logic for ResearcherDetailsView.xaml
    /// </summary>
    public partial class ResearcherDetailsView : UserControl
    {
        /// <summary>
        /// Basic constructor for ResearcherDetailsView
        /// </summary>
        public ResearcherDetailsView()
        {
            InitializeComponent();
        }

        private MainWindow Window { get { return Application.Current.MainWindow as MainWindow; } } //Current Window

        //////////Methods//////////////

        /// <summary>
        /// Adds the details of a given researcher to the view
        /// </summary>
        /// <param name="researcher">The researhcer details being added to the screen</param>
        public void SetDetails(Model.Researcher researcher)
        {
            //add data to text boxes
            ResearcherDetailsView_Name_Value.Text = researcher.GivenName + " " + researcher.FamilyName;
            ResearcherDetailsView_Title_Value.Text = researcher.Title;
            ResearcherDetailsView_Unit_Value.Text = researcher.School;
            ResearcherDetailsView_Campus_Value.Text = researcher.Campus;
            ResearcherDetailsView_Email_Value.Text = researcher.Email;
            ResearcherDetailsView_CurrentJob_Value.Text = researcher.CurrentJobTitle();
            ResearcherDetailsView_ComInst_Value.Text = researcher.GetEarliestJob().Start.ToString("dd/MM/yyyy");
            ResearcherDetailsView_ComPos_Value.Text = researcher.GetCurrentJob().Start.ToString("dd/MM/yyyy");
            ResearcherDetailsView_Tenure_Value.Text = researcher.Tenure().ToString() + " years";
            ResearcherDetailsView_Publications_Value.Text = researcher.PublicationsCount().ToString();
            ResearcherDetailsView_ProfileImage.Source = new BitmapImage(researcher.Photo);
            PublicationsListView.PublicationsListView_Display_Box.ItemsSource = new ObservableCollection<Model.Publication>(researcher.Publications);
            

            //Staff only details
            if (researcher.Type == "Staff")
            {
                int count = 0; //Supervised students count
                Model.Staff staffCast = (Model.Staff)researcher; //Current researcher object

                //Get count of supervised students
                foreach (Model.Student i in staffCast.Supervisions)
                {
                    count++;
                }

                ResearcherDetailsView_3YAv_Value.Text = staffCast.ThreeYearAverage().ToString();
                ResearcherDetailsView_Supervisions_Value.Text = count.ToString();
                Window.SupervisionsListView.SupervisionsListView_Display_Box.ItemsSource = new ObservableCollection<Model.Researcher>(staffCast.Supervisions as List<Model.Student>);
                ResearcherDetailsView_Performance_Value.Text = staffCast.Performance().ToString() + "%";
                ResearcherDetailsView_PrevPos_Data.ItemsSource = researcher.Positions;

                //Reset any set student details
                ResearcherDetailsView_Degree_Value.Text = "";
                ResearcherDetailsView_Supervisor_Value.Text = "";
            }
            //Students only details
            else
            {
                Model.Student stundetCast = (Model.Student)researcher; //Current researcher object

                ResearcherDetailsView_Degree_Value.Text = stundetCast.Degree;
                ResearcherDetailsView_Supervisor_Value.Text = stundetCast.SupervisorName;

                //Reset any set staff details
                ResearcherDetailsView_3YAv_Value.Text = "";
                ResearcherDetailsView_Supervisions_Value.Text = "";
                Window.SupervisionsListView.SupervisionsListView_Display_Box.ItemsSource = new ObservableCollection<Model.Researcher>();
                ResearcherDetailsView_Performance_Value.Text = "";
                ResearcherDetailsView_PrevPos_Data.ItemsSource = null;

            }
        }

        /// <summary>
        /// Activate the inputs of researcher details view
        /// </summary>
        public void ActivatePubList()
        {
            PublicationsListView.ActivateInputs();
        }
    }
}
