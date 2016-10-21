using System.Windows;
using HomeTask.WPF.ViewModels;

namespace HomeTask.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView(IMainViewModel viewModel)
        {
            InitializeComponent();

            base.DataContext = viewModel;
        }
    }
}
