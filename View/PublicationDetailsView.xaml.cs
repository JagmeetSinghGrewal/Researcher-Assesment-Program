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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RAP.View
{
    /// <summary>
    /// Interaction logic for PublicationDetailsView.xaml
    /// </summary>
    public partial class PublicationDetailsView : UserControl
    {
        /// <summary>
        /// Basic Constructor for PublicationDetailsView
        /// </summary>
        public PublicationDetailsView()
        {
            InitializeComponent();
        }

        //////////Methods//////////////

        /// <summary>
        /// Sets the details of a given publication on the view
        /// </summary>
        /// <param name="publication">The publication details being displayed</param>
        public void SetDetails(Model.Publication publication)
        {
            PublicationDetailsView_DOI_Value.Text = publication.DOI;
            PublicationDetailsView_Title_Value.Text = publication.Title;
            PublicationDetailsView_Authors_Value.Text = publication.Authors;
            PublicationDetailsView_PublicationYear_Value.Text = publication.Year.Year.ToString();
            PublicationDetailsView_Type_Value.Text = publication.Type.ToString();
            PublicationDetailsView_CiteAs_Value.Text = publication.CiteAs;
            PublicationDetailsView_AvailabilityDate_Value.Text = publication.Available.ToString("dd/MM/yyyy");
            PublicationDetailsView_Age_Value.Text = publication.Age().ToString() + " days";
        }
    }
}
