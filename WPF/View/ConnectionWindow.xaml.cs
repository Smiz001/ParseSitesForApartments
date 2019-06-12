using System.Windows;
using WPF.ViewModel;

namespace WPF.View
{
  /// <summary>
  /// Interaction logic for ConnectionWindow.xaml
  /// </summary>
  public partial class ConnectionWindow : Window
  {
    private ConnectionViewModel vm = new ConnectionViewModel();
    public ConnectionWindow()
    {
      InitializeComponent();
      DataContext = vm;
    }
  }
}
