using System.Windows.Controls;
using CourseSelectionSystem.Services;
using CourseSelectionSystem.ViewModels.Admin;

namespace CourseSelectionSystem.Views.Admin
{
    public partial class AdminOfferingView : UserControl
    {
        public AdminOfferingView(OfferingService offeringService)
        {
            InitializeComponent();
            this.DataContext = new AdminOfferingViewModel(offeringService);
        }
    }
}
