using Core.Connections;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF.ViewModel
{
  public class ChartWindowViewModel : CoreViewModel
  {
    #region Fields
    private List<string> typeRooms;
    private List<DateTime> datesStart;
    private List<DateTime> datesEnd;
    private string selectedTypeRoom;
    private DateTime selectedDateStart;
    private DateTime selectedDateEnd;
    private ICommand downoadDataByParametrsCommand;
    private List<string> listLabelsForX;
    private SeriesCollection seriesCollection;
    private double maxValueY=100;
    private double minValueY=10;
    #endregion

    #region Constructors
    public ChartWindowViewModel()
    {
      var con = ConnetionToSqlServer.Default();
      typeRooms = new List<string>();
      string select = @"SELECT DISTINCT [TypeRoom]  
FROM [ParseBulding].[dbo].[AverPriceForTypeRoom]";
      var reader = con.ExecuteReader(select);
      if (reader != null)
      {
        while (reader.Read())
        {
          typeRooms.Add(reader.GetString(0));
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
      selectedTypeRoom = typeRooms.First();
      selectedDateStart = datesStart.First();
      selectedDateEnd = datesEnd.Last();
    }
    #endregion

    #region Properties
    public List<string> TypeRooms { get => typeRooms; }
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
    public string SelectedTypeRoom
    {
      get => selectedTypeRoom;
      set
      {
        if (selectedTypeRoom == value) return;
        selectedTypeRoom = value;
        OnPropertyChanged("SelectedTypeRoom");
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

    public double MaxValueY
    {
      get => maxValueY;
      set
      {
        if (maxValueY == value) return;
        maxValueY = value;
        OnPropertyChanged("MaxValueY");
      }
    }
    public double MinValueY
    {
      get => minValueY;
      set
      {
        if (minValueY == value) return;
        minValueY = value;
        OnPropertyChanged("MinValueY");
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

      string select = $@"SELECT [Date]
          ,[AverPrice]
      FROM [ParseBulding].[dbo].[AverPriceForTypeRoom]
      where date between '{selectedDateStart.Year}-{selectedDateStart.Month}-{selectedDateStart.Day}' and '{selectedDateEnd.Year}-{selectedDateEnd.Month}-{selectedDateEnd.Day}' and TypeRoom = '{selectedTypeRoom}'
      order by Date";
      var con = ConnetionToSqlServer.Default();
      var reader = con.ExecuteReader(select);
      if (reader != null)
      {
        ChartValues<double> values = new ChartValues<double>();
        while (reader.Read())
        {
          values.Add(reader.GetDouble(1));
        }
        reader.Close();
        SeriesCollection = new SeriesCollection
        {
          new LineSeries
          {
              Title = selectedTypeRoom,
              Values = values
          }
        };
        var aver = values.Average();
        MaxValueY = aver + 10000;
        MinValueY = aver - 10000;
      }

      var ls = new List<string>();
      foreach (var item in datesStart)
      {
        if (item >= selectedDateStart && item <= selectedDateEnd)
          ls.Add(item.ToShortDateString());
      }
      ListLabelsForX = ls;
    }
    #endregion
  }
}
