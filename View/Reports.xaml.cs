using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace RAP.View
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : Window
    {
        private Controller.ReportsController controller;
        private Controller.ResearcherController con;
        
        /// <summary>
        /// Basic constructor for Reports window
        /// </summary>
        public Reports()
        {
            InitializeComponent();
            //Get the current researcher list and researcher conrtoller instance
            ObjectDataProvider researcherlist = (ObjectDataProvider)FindResource("researcherlist");
            con = (Controller.ResearcherController)researcherlist.ObjectInstance;

            //Create reports
            controller = new Controller.ReportsController();
            controller.Generate(con.Reseachers);
            Reports_Poor.ItemsSource = controller.Create_Table(Controller.Report.Poor);
            Reports_Below.ItemsSource = controller.Create_Table(Controller.Report.Below);
            Reports_Min.ItemsSource = controller.Create_Table(Controller.Report.Meeting);
            Reports_Star.ItemsSource = controller.Create_Table(Controller.Report.Star);
        }

        //////////Methods//////////////

        /// <summary>
        /// Gets the list of emails for the researchers in the currently displayed report
        /// </summary>
        private void Generate_Emails(object sender, RoutedEventArgs e)
        {
            int type = Tab_Control_Reports.SelectedIndex;
            switch (type)
            {
                case 0:
                    controller.Generate_Emails_List(Controller.Report.Poor);
                    break;
                case 1:
                    controller.Generate_Emails_List(Controller.Report.Below);
                    break;
                case 2:
                    controller.Generate_Emails_List(Controller.Report.Meeting);
                    break;
                default:
                    controller.Generate_Emails_List(Controller.Report.Star);
                    break;
            }
        }
    }
}
