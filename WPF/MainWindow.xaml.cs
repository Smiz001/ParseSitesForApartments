using System.Windows;
using WPF.View;

namespace WPF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      ConnectionWindow conectionView = new ConnectionWindow();
      if (conectionView.ShowDialog() == false)
        this.Close();

      InitializeComponent();
      DataContext = new MainWindowViewModel();
    }
  }
}
