using Core.Connections;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WPF.Model;

namespace WPF.ViewModel
{
  public class ChartMetroWindowViewModel:NotifyClass
  {
    #region Fields
    private List<TypeRoomModel> typeRooms;
    private List<DateTime> datesStart;
    private List<DateTime> datesEnd;
    private DateTime selectedDateStart;
    private DateTime selectedDateEnd;
    private ICommand downoadDataByParametrsCommand;
    private List<string> listLabelsForX;
    private SeriesCollection seriesCollection;
    private List<MetroModel> listMetro;
    #endregion

    #region Constructors
    public ChartMetroWindowViewModel()
    {
      var con = ConnetionToSqlServer.Default();
      typeRooms = new List<TypeRoomModel>();
      string select = @"SELECT DISTINCT [TypeRoom]  
FROM [ParseBulding].[dbo].[AverPriceForTypeRoom]";
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
      var listSelected = typeRooms.Where(x => x.IsSelected == true);
      var listSelectesMetro = listMetro.Where(x => x.IsSelected == true);
      if (listSelected.Count() > 0)
      {
        var series = new SeriesCollection();
        string dopStr = "";
        var cnt = listSelectesMetro.Count();
        if (cnt > 0)
        {
          var arr = listSelectesMetro.ToArray();
          dopStr += "and (";
          for (int i = 0; i < cnt; i++)
          {
            if (i == 0)
              dopStr += $@"Metro = '{arr[i].Guid}'";
            else
              dopStr += $@"or Metro = '{arr[i].Guid}'";
          }
          dopStr += ")";
        }
        foreach (var item in listSelected)
        {
          string select = $@" SELECT [Date]
,AVG([AverPrice])
FROM [ParseBulding].[dbo].[AverPriceForTypeRoomByMetro]
where date between '{selectedDateStart.Year}-{selectedDateStart.Month}-{selectedDateStart.Day}' and '{selectedDateEnd.Year}-{selectedDateEnd.Month}-{selectedDateEnd.Day}' and TypeRoom = '{item.NameTypeRoom}' {dopStr}
Group by [Date]
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
            var line = new LineSeries { Title = item.NameTypeRoom, Values = values };
            series.Add(line);
          }
        }

        SeriesCollection = series;
        var ls = new List<string>();
        foreach (var date in datesStart)
        {
          if (date >= selectedDateStart && date <= selectedDateEnd)
            ls.Add(date.ToShortDateString());
        }
        ListLabelsForX = ls;
      } 
    }
    #endregion
  }
}
