﻿using System.ComponentModel;

namespace WPF
{
  public class CoreViewModel : INotifyPropertyChanged
  {
    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
  }
}