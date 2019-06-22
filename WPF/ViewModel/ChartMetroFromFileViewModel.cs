using Core.Connections;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
    private double squareBefore, squareAfter;
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

    public double SquareBefore
    {
      get => squareBefore;
      set
      {
        if (squareBefore == value) return;
        squareBefore = value;
        OnPropertyChanged("SquareBefore");
      }
    }
    public double SquareAfter
    {
      get => squareAfter;
      set
      {
        if (squareAfter == value) return;
        squareAfter = value;
        OnPropertyChanged("SquareAfter");
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
      var listSelectedRooms = typeRooms.Where(x => x.IsSelected == true).ToList();
      var listSelectedMetro = listMetro.Where(x => x.IsSelected == true).ToList();
      var dictRooms = new Dictionary<string, SortedDictionary<DateTime, double>>();
      foreach (var item in listSelectedRooms)
      {
        dictRooms.Add(item.NameTypeRoom, new SortedDictionary<DateTime, double>());
      }

      foreach (var item in files)
      {
        var fileName = Path.GetFileName(item);
        var pattern = $@"(\d+\.\d+\.\d+)";
        var regex = new Regex(pattern);
        var dateString = regex.Match(fileName).Value;
        if (!string.IsNullOrEmpty(dateString))
        {
          var date = DateTime.Parse(dateString);
          if (date <= selectedDateEnd && date >= selectedDateStart)
          {
            ParseFile(item, dictRooms, listSelectedMetro, date);
          }
        }
      }
      var a = 0;

      var series = new SeriesCollection();
      foreach (var dic in dictRooms)
      {
        ChartValues<double> values = new ChartValues<double>();
        foreach (var item in dic.Value)
        {
          values.Add(item.Value);
        }
        var line = new LineSeries { Title = $"Средняя цена за кв.м для {dic.Key}", Values = values };
        series.Add(line);
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
        fbd.ShowNewFolderButton = false;
        if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
        {
          PathFiles = fbd.SelectedPath;
          files = Directory.GetFiles(PathFiles).ToList();
          files = files.Where(x => x.Contains(".csv")).ToList();
          files.Sort();
        }
      }
    }

    #endregion

    #region Methods
    private void ParseFile(string filePath, Dictionary<string, SortedDictionary<DateTime, double>> typeRooms, List<MetroModel> metros, DateTime date)
    {
      var dict = new Dictionary<string, List<double>>();
      foreach (var item in typeRooms)
      {
        dict.Add(item.Key, new List<double>());
      }
      using (var sr = new StreamReader(filePath))
      {
        string line = sr.ReadLine();
        while ((line = sr.ReadLine()) != null)
        {
          var arr = line.Split(';');
          string typeRoom = arr[5];
          if (typeRooms.ContainsKey(typeRoom))
          {
            var metro = arr[10];
            if (metros.Where(x => x.NameMetro == arr[10]).Count() > 0)
            {
              var square = double.Parse(arr[6]);
              if (square >= SquareBefore && square <= SquareAfter)
              {
                var value = double.Parse(arr[9]);
                dict[typeRoom].Add(value);
              }
            }
          }
        }
      }
      foreach (var item in dict)
      {
        if (item.Value.Count > 0)
        {
          var averDev = CalculateAverageDeviation(item.Value);
          var newList = new List<double>();
          var aver = item.Value.Average();

          if (item.Value.Count == 1)
            newList.Add(item.Value[0]);
          else
          {
            item.Value.ForEach(x =>
            {
              if ((x >= aver - averDev) && (x <= aver + averDev))
                newList.Add(x);
            });
          }
          typeRooms[item.Key].Add(date, newList.Average());
        }
      }
    }

    /// <summary>
    /// Нахождение среднего отклонения в списке
    /// </summary>
    /// <param name="listValue"></param>
    /// <returns></returns>
    private double CalculateAverageDeviation(List<double> listValue)
    {
      if (listValue.Count > 0)
      {
        double sum = 0;
        var aver = listValue.Average();
        var dev = listValue.Count - 1;
        foreach (var item in listValue)
        {
          sum += Math.Pow(item - aver, 2);
        }

        return Math.Round(Math.Sqrt(sum / dev), 2);
      }
      return 0;
    }
    #endregion
  }
}
