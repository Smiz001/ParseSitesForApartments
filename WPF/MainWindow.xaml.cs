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

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      comboBox.SelectedIndex = 0;
      comboBox1.SelectedIndex = 0;
      comboBox2.SelectedIndex = 0;
    }
  }
}
