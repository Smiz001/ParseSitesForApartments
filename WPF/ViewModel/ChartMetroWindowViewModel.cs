﻿using Core.Connections;
using Core.MainClasses;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace WPF.ViewModel
{
  public class ChartMetroWindowViewModel:NotifyClass
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
    private double maxValueY = 100;
    private double minValueY = 10;
    private List<Metro> listMetro;
    private Metro selectedMetro;
    #endregion

    #region Constructors
    public ChartMetroWindowViewModel()
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

      select = @"SELECT [Id]
      ,[Name]
  FROM [ParseBulding].[dbo].[Metro]
  order by Name";
      reader = con.ExecuteReader(select);
      if (reader != null)
      {
        listMetro = new List<Metro>();
        while (reader.Read())
        {
          listMetro.Add(new Metro { Id = reader.GetGuid(0), Name = reader.GetString(1) });
        }
        reader.Close();
        selectedMetro = listMetro.First();
      }
    }

    #endregion

    #region Properties    public List<string> TypeRooms { get => typeRooms; }
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
    public List<Metro> ListMetro { get => listMetro; }
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
    public Metro SelectedMetro
    {
      get => selectedMetro;
      set
      {
        if (selectedMetro == value) return;
        selectedMetro = value;
        OnPropertyChanged("SelectedMetro");
      }
    }
    #endregion

    #region Methods
    #endregion
  }
}
