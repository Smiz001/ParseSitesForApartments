using Core.Connections;
using Core.Enum;
using Core.MainClasses;
using Core.Sites;
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
    private ICommand callChartWithMetroByFilesWindowCommand;
    private List<District> listDistricts = new List<District>();
    private List<Metro> listMetros = new List<Metro>();
    #endregion

    #region Constructors
    public MainWindowViewModel()
    {
      parseSites = new List<string> {"Все сайты", "ELMS", "BKN", "BN"};
      typeRooms = new List<string> { "Все", "Студии", "1 ком. кв.","2 ком. кв.","3 ком. кв.","4 ком. кв.","Более 4 ком. кв."};
      typeSale = new List<string> { "Продажа", "Сдача" };

      var connection = ConnetionToSqlServer.Default();

      string select = "SELECT [ID],[Name] FROM [ParseBulding].[dbo].[District]";
      var reader = connection.ExecuteReader(select);
      if (reader != null)
      {
        while (reader.Read())
        {
          listDistricts.Add(new District { Id = reader.GetGuid(0), Name = reader.GetString(1) });
        }
        reader.Close();
        foreach (var district in listDistricts)
        {
          select = $@"SELECT [Id]
              ,[Name]
              ,[XCoor]
              ,[YCoor]
              ,[IdRegion]
            FROM[ParseBulding].[dbo].[Metro]
            where IdRegion = '{district.Id}'";
          reader = connection.ExecuteReader(select);
          while (reader.Read())
          {
            var metro = new Metro
            {
              Id = reader.GetGuid(0),
              Name = reader.GetString(1),
              XCoor = (float)reader.GetDouble(2),
              YCoor = (float)reader.GetDouble(3)
            };
            district.Metros.Add(metro);
            listMetros.Add(metro);
          }
          reader.Close();
        }
      }
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
      BaseParse parser = null;
      switch (selectedSites)
      {
        case "Все сайты":
          parser = new AllSites(listDistricts, listMetros, null);
          break;
        case "ELMS":
          parser = new ELMS(listDistricts, listMetros, null);
          break;
        case "BN":
          parser = new BN(listDistricts, listMetros, null);
          break;
        case "BKN":
          parser = new BKN(listDistricts, listMetros, null);
          break;
        //case "":
        //  parser = new Avito(listDistricts, listMetros, listProxy);
        //  break;
      }
      parser.Filename = selectedPath;

      switch (selectedTypeSale)
      {
        case "Продажа":
          parser.TypeParseFlat = TypeParseFlat.Sale;
          break;
        case "Сдача":
          parser.TypeParseFlat = TypeParseFlat.Rent;
          break;
      }

      switch (selectedTypeRooms)
      {
        case "Все":
          parser.ParsingAll();
          break;
        case "Студии":
          parser.ParsingStudii();
          break;
        case "1 ком. кв.":
          parser.ParsingOne();
          break;
        case "2 ком. кв.":
          parser.ParsingTwo();
          break;
        case "3 ком. кв.":
          parser.ParsingThree();
          break;
        case "4 ком. кв.":
          parser.ParsingFour();
          break;
        case "Более 4 ком. кв.":
          parser.ParsingMoreFour();
          break;
      }
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

    public ICommand CallChartWithMetroByFilesWindowCommand
    {
      get
      {
        if (callChartWithMetroByFilesWindowCommand == null)
          callChartWithMetroByFilesWindowCommand = new RelayCommand(() => CallChartWithMetroByFilesWindow());
        return callChartWithMetroByFilesWindowCommand;
      }
    }

    private void CallChartWithMetroByFilesWindow()
    {
      var vm = new ChartMetroFromFileViewModel();
      var view = new ChartMetroFromFileWindow();
      view.DataContext = vm;
      view.ShowDialog();
    }
    #endregion
  }
}
