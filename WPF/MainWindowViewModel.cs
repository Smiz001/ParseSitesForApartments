using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using WPF.View;
using WPF.ViewModel;

namespace WPF
{
  public class MainWindowViewModel: NotifyClass
  {
    #region Fields
    private ICommand selectPathForSaveFileCommand;
    private string selectedPath;
    private List<string> parseSites;
    private List<string> typeRooms;
    private List<string> typeSale;
    private string selectedSites;
    private string selectedTypeRooms;
    private string selectedTypeSale;
    private ICommand executeParseCommand;
    private ICommand callChartWindowCommand;
    private ICommand callChartWithMetroWindowCommand;
    #endregion

    #region Constructors
    public MainWindowViewModel()
    {
      parseSites = new List<string> {"Все сайты", "ELMS", "BKN", "BN"};
      typeRooms = new List<string> { "Все", "Студии", "1 ком. кв.","2 ком. кв.","3 ком. кв.","4 ком. кв.","Более 4 ком. кв."};
      typeSale = new List<string> { "Продажа", "Сдача" };
    }
    #endregion

    #region Properties
    public string SelectedPath
    {
      get => selectedPath;
      set
      {
        if (selectedPath == value) return;
        selectedPath = value;
        OnPropertyChanged("SelectedPath");
      }
    }
    public string SelectedSites
    {
      get => selectedSites;
      set
      {
        if (selectedSites == value) return;
        selectedSites = value;
        OnPropertyChanged("SelectedSites");
      }
    }
    public string SelectedTypeRooms
    {
      get => selectedTypeRooms;
      set
      {
        if (selectedTypeRooms == value) return;
        selectedTypeRooms = value;
        OnPropertyChanged("SelectedTypeRooms");
      }
    }

    public string SelectedTypeSale
    {
      get => selectedTypeSale;
      set
      {
        if (selectedTypeSale == value) return;
        selectedTypeSale = value;
        OnPropertyChanged("SelectedTypeSale");
      }
    }
    public List<string> ParseSites { get => parseSites; }

    public List<string> TypeRooms { get => typeRooms; }

    public List<string> TypeSale { get => typeSale; }
    #endregion

    #region Methods

    #endregion

    #region Commands
    public ICommand SelectPathForSaveFileCommand
    {
      get
      {
        if (selectPathForSaveFileCommand == null)
          selectPathForSaveFileCommand = new RelayCommand(() => SelectPathForSaveFile());
        return selectPathForSaveFileCommand;
      }
    }

    private void SelectPathForSaveFile()
    {
      var save = new SaveFileDialog();
      save.Filter = "csv files (*.csv)|*.csv";
      save.FilterIndex = 1;
      save.RestoreDirectory = true;
      save.FileName = $@"{SelectedSites}-{SelectedTypeRooms}-{SelectedTypeSale}-{DateTime.Now.ToShortDateString()}";
      if(save.ShowDialog() == true)
      {
        SelectedPath = save.FileName;
      }
    }

    public ICommand ExecuteParseCommand
    {
      get
      {
        if (executeParseCommand == null)
          executeParseCommand = new RelayCommand(() => ExecuteParse());
        return executeParseCommand;
      }
    }

    private void ExecuteParse()
    {
     
    }

    public ICommand CallChartWindowCommand
    {
      get
      {
        if (callChartWindowCommand == null)
          callChartWindowCommand = new RelayCommand(() => CallChartWindow());
        return callChartWindowCommand;
      }
    }

    private void CallChartWindow()
    {
      var vm = new ChartWindowViewModel();
      var view = new ChartWindow();
      view.DataContext = vm;
      view.ShowDialog();
    }
    public ICommand CallChartWithMetroWindowCommand
    {
      get
      {
        if (callChartWithMetroWindowCommand == null)
          callChartWithMetroWindowCommand = new RelayCommand(() => CallChartWithMetroWindow());
        return callChartWithMetroWindowCommand;
      }
    }

    private void CallChartWithMetroWindow()
    {
      var vm = new ChartMetroWindowViewModel();
      var view = new ChartMetroWindow();
      view.DataContext = vm;
      view.ShowDialog();
    }
    #endregion
  }
}
