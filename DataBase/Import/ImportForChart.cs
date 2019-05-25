using Core.MainClasses.Chart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Import
{
  public class ImportForChart
  {
    #region Fields
    private string path;
    #endregion

    #region Constructors
    public ImportForChart(string pathFolder)
    {
      path = pathFolder;
    }
    #endregion

    #region Properties
    #endregion

    #region Methods
    public List<DataForChart> Execute()
    {
      var list = new List<DataForChart>();
      FileAttributes attr = File.GetAttributes(path);
      if (attr.HasFlag(FileAttributes.Directory))
      {
        var files = Directory.GetFiles(path);
        foreach (var file in files)
        {
          var data = ParseFile(file);
          if (data != null)
            list.Add(data);
        }
      }
      return list;
    }

    public DataForChart ParseFile(string file)
    {
      DataForChart dataForChart = null;
      using(var sr = new StreamReader(file))
      {
        string line = sr.ReadLine();
        if(line.Contains("Район"))
        {
          dataForChart = new DataForChart();
          
        }
      }

      return dataForChart;
    }
    #endregion
  }
}
