using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Media;

namespace Core.UI
{
  public partial class MainChartsForm : Form
  {
    public MainChartsForm()
    {
      InitializeComponent();
      FillComboBoxes();
    }

    public void FillComboBoxes()
    {
      var con = Connections.ConnetionToSqlServer.Default();

      string select = @"SELECT DISTINCT [TypeRoom]  
FROM [ParseBulding].[dbo].[AverPriceForTypeRoom]";
      var reader = con.ExecuteReader(select);
      if (reader != null)
      {
        var list = new List<string>();
        while (reader.Read())
        {
          list.Add(reader.GetString(0));
        }
        cmbTypeRoom.DataSource = list;
        reader.Close();
      }

      select = @"SELECT DISTINCT Date  
FROM [ParseBulding].[dbo].[AverPriceForTypeRoom]
order by Date";
      reader = con.ExecuteReader(select);
      if (reader != null)
      {
        var list = new List<DateTime>();
        while (reader.Read())
        {
          list.Add(reader.GetDateTime(0));
        }
        cmbStartDate.DataSource = list;
        cmbEndDate.DataSource = new List<DateTime>(list);
        cmbEndDate.SelectedIndex = cmbEndDate.Items.Count - 1;
        reader.Close();
      }
    }

    private void BtnDownload_Click(object sender, EventArgs e)
    {
      //    var start = (DateTime)cmbStartDate.SelectedItem;
      //    var end = (DateTime)cmbEndDate.SelectedItem;

      //    string select = $@"SELECT [Date]
      //    ,[AverPrice]
      //FROM [ParseBulding].[dbo].[AverPriceForTypeRoom]
      //where date between '{start.Year}-{start.Month}-{start.Day}' and '{end.Year}-{end.Month}-{end.Day}' and TypeRoom = '{cmbTypeRoom.SelectedItem.ToString()}'
      //order by Date";
      //    var con = Connections.ConnetionToSqlServer.Default();
      //    var reader = con.ExecuteReader(select);
      //    if (reader != null)
      //    {
      //      ChartValues<double> points = new ChartValues<double>();
      //      ChartValues<ObservablePoint> List1Points = new ChartValues<ObservablePoint>();
      //      var listDates = new List<string>();
      //      var listPoints = new List<Point>();
      //      while (reader.Read())
      //      {
      //        listPoints.Add(new Point { Date = reader.GetDateTime(0), AverSum = reader.GetDouble(1) });
      //      }
      //      reader.Close();
      //      foreach (var item in listPoints)
      //      {
      //        points.Add(item.AverSum);
      //        listDates.Add(item.Date.ToShortDateString());
      //      }
      //      SeriesCollection seriesViews = new SeriesCollection { new LineSeries { Title = cmbTypeRoom.SelectedItem.ToString(), Values = points } };

      //      mainChart.AxisX.Add(new Axis
      //      {
      //        Title = "Дата",
      //        //Labels = new List<string> { listDates[0], listDates[listDates.Count/2], listDates[listDates.Count-1] }
      //      });
      //      mainChart.AxisY.Add(new Axis
      //      {
      //        Title = "Средняя цена за кв. м.",
      //      });

      //      mainChart.Series = seriesViews;
      //    }
      mainChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> {4, 6, 5, 2, 7}
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> {6, 7, 3, 4, 6},
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> {5, 2, 8, 3},
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };

      mainChart.AxisX.Add(new Axis
      {
        Title = "Month",
        Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" }
      });

      mainChart.AxisY.Add(new Axis
      {
        Title = "Sales",
        LabelFormatter = value => value.ToString("C")
      });

      mainChart.LegendLocation = LegendLocation.Right;

      //modifying the series collection will animate and update the chart
      mainChart.Series.Add(new LineSeries
      {
        Values = new ChartValues<double> { 5, 3, 2, 4, 5 },
        LineSmoothness = 0, //straight lines, 1 really smooth lines
        PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
        PointGeometrySize = 50,
        PointForeground = Brushes.Gray
      });

      //modifying any series values will also animate and update the chart
      mainChart.Series[2].Values.Add(5d);
    }
  }
}
