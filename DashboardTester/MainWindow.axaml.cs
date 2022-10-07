using Avalonia.Controls;
using GenericDashboard;

namespace DashboardTester
{
    public partial class MainWindow : Window
    {

        private DashboardManager _dashboardManager;
        
        public MainWindow()
        {
            InitializeComponent();
            
            _dashboardManager = new DashboardManager();

            this.FindControl<ContentControl>("CcControl").Content = _dashboardManager.GetDashboardManager;
        }
    }
}