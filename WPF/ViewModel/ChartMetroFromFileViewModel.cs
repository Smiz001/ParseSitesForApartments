using Core.Connections;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using WPF.Model;

namespace WPF.ViewModel
{
  public class ChartMetroFromFileViewModel : NotifyClass
  {
    #region Fields
    private List<TypeRoomModel> typeRooms;
    private List<DateTime> datesStart;
    private List<DateTime> datesEnd;
    private List<MetroModel> listMetro;
    private DateTime selectedDateStart;
    private DateTime selectedDateEnd;
    private ICommand downoadDataByParametrsCommand;
    private List<string> listLabelsForX;
    private SeriesCollection seriesCollection;
    private string pathFiles = string.Empty;
    private ICommand selectFolderFileCommand;
    private List<string> files;
    #endregion

    #region Constructors
    public ChartMetroFromFileViewModel()
    {
      var con = ConnetionToSqlServer.Default();
      typeRooms = new List<TypeRoomModel>();
      string select = @"SELECT DISTINCT [TypeRoom]  
FROM [ParseBulding].[dbo].[AverPriceForTypeRoom]
order by TypeRoom";
      var reader = con.ExecuteReader(select);
      if (reader != null)
      {
        while (reader.Read())
        {
          typeRooms.Add(new TypeRoomModel { NameTypeRoom = reader.GetString(0) });
        }
        reader.Close();
      }

      select = @"SELECT DISTINCT Date  
FROM [ParseBulding].[dbo].[AverPriceForTypeRoom]
order by Date";
      reader = con.ExecuteReader(select);
      if (reader != null)
      {
        datesStart = new List<DateTime>();
        while (reader.Read())
        {
          datesStart.Add(reader.GetDateTime(0));
        }
        datesEnd = new List<DateTime>(datesStart);
        reader.Close();
      }
      selectedDateStart = datesStart.First();
      selectedDateEnd = datesEnd.Last();

      select = @"SELECT [Id]
      ,[Name]
  FROM [ParseBulding].[dbo].[Metro]
  order by Name";
      reader = con.ExecuteReader(select);
      if (reader != null)
      {
        listMetro = new List<MetroModel>();
        while (reader.Read())
        {
          listMetro.Add(new MetroModel { Guid = reader.GetGuid(0), NameMetro = reader.GetString(1) });
        }
        reader.Close();
      }
    }
    #endregion

    #region Properties
    public List<TypeRoomModel> TypeRooms { get => typeRooms; }
    public List<DateTime> DateStart { get => datesStart; }
    public List<DateTime> DateEnd { get => datesEnd; }
    public SeriesCollection SeriesCollection
    {
      get => seriesCollection;
      set
      {
        if (seriesCollection == value) return;
        seriesCollection = value;
        OnPropertyChanged("SeriesCollection");
      }
    }
    public List<MetroModel> ListMetro { get => listMetro; }
    public List<string> ListLabelsForX
    {
      get => listLabelsForX;
      set
      {
        if (listLabelsForX == value) return;
        listLabelsForX = value;
        OnPropertyChanged("ListLabelsForX");
      }
    }
    public DateTime SelectedDateStart
    {
      get => selectedDateStart;
      set
      {
        if (selectedDateStart == value) return;
        selectedDateStart = value;
        OnPropertyChanged("SelectedDateStart");
      }
    }
    public DateTime SelectedDateEnd
    {
      get => selectedDateEnd;
      set
      {
        if (selectedDateEnd == value) return;
        selectedDateEnd = value;
        OnPropertyChanged("SelectedDateEnd");
      }
    }
    public string PathFiles
    {
      get => pathFiles;
      set
      {
        if (pathFiles == value) return;
        pathFiles = value;
        OnPropertyChanged("PathFiles");
      }
    }
    #endregion

    #region Methods
    #endregion

    #region Commands

    public ICommand DownoadDataByParametrsCommand
    {
      get
      {
        if (downoadDataByParametrsCommand == null)
          downoadDataByParametrsCommand = new RelayCommand(() => DownoadDataByParametrs());
        return downoadDataByParametrsCommand;
      }
    }


    private void DownoadDataByParametrs()
    {

    }

    public ICommand SelectFolderFileCommand
    {
      get
      {
        if (selectFolderFileCommand == null)
          selectFolderFileCommand = new RelayCommand(() => SelectFolderFile());
        return selectFolderFileCommand;
      }
    }

    private void SelectFolderFile()
    {
      using (var fbd = new FolderBrowserDialog())
      {
        if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
        {
          files = Directory.GetFiles(fbd.SelectedPath).ToList();
        }
      }
    }

    #endregion
  }
}
