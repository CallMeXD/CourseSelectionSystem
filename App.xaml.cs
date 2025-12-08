using System.Configuration;
using System.Data;
using System.Windows;
using CourseSelectionSystem.Views;

namespace CourseSelectionSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 应用程序从 LoginView 启动
            var loginView = new LoginView();
            loginView.Show();
        }
    }

}
