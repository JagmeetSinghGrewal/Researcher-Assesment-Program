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

namespace RAP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        //private Controller.ResearcherController controller;
        public MainWindow()
        {
            InitializeComponent();

            

        }

        private void CumulativeCountView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ResearcherDetailView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Generate_Reports(object sender, RoutedEventArgs e)
        {
            View.Reports reports = new View.Reports();
            reports.Show();
        }
    }
}