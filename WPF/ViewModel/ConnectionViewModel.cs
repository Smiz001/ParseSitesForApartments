using Core.Connections;
using Core.Export;
using Core.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WPF.ViewModel
{
  public class ConnectionViewModel:NotifyClass
  {
    #region Fields
    private ConnetionToSqlServer connection;
    private bool isConnect;
    private ICommand connectCommand;
    private ICommand cancelCommand;
    #endregion

    #region Constructors
    public ConnectionViewModel()
    {
      var import = new ImportConfigDataBase();
      import.Import();
      connection = ConnetionToSqlServer.Default();
    }
    #endregion

    #region Properties
    public ConnetionToSqlServer Connection
    {
      get { return connection; }
    }

    public bool IsConnect
    {
      get
      {
        return isConnect;
      }

      set
      {
        if (isConnect == value) return;
        isConnect = value;
        OnPropertyChanged("IsConnect");
      }
    }
    #endregion

    #region Methods
    public ICommand ConnectCommand
    {
      get
      {
        if (connectCommand == null)
          connectCommand = new RelayCommand(param => Connect(param), true);
        return connectCommand;
      }
    }

    private void Connect(object param)
    {
      var win = param as Window;
      if (win == null) return;
      try
      {
        if (connection.Connect())
        {
          IsConnect = true;
          var export = new ExportConfigDataBase();
          export.Export(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ParseFlat\parseflat.config");
          win.DialogResult = true;
          win.Close();
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
        connection.Disconnect();
      }
    }

    public ICommand CancelCommand
    {
      get
      {
        if (cancelCommand == null)
          cancelCommand = new RelayCommand(param => Cancel(param), true);
        return cancelCommand;
      }
    }

    private void Cancel(object param)
    {
      var win = param as Window;
      if (win == null) return;
      win.DialogResult = false;
      win.Close();
    }
    #endregion
  }
}
