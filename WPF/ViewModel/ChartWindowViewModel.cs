using Core.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF.ViewModel
{
  public class ChartWindowViewModel:CoreViewModel
  {
    #region Fields
    private List<string> typeRooms;
    private List<string> dateStart;
    private List<string> dateEnd;
    private string selectedTypeRoom;
    private string selectedDateStart;
    private string selectedDateEnd;
    private ICommand downoadDataByParametrsCommand;
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
        dateStart = new List<string>();
        while (reader.Read())
        {
          dateStart.Add(reader.GetDateTime(0).ToShortDateString());
        }
        dateEnd = new List<string>(dateStart);
        reader.Close();
      }
      selectedTypeRoom = typeRooms.First();
      selectedDateStart = dateStart.First();
      selectedDateEnd = dateEnd.Last();
    }
    #endregion

    #region Properties
    public List<string> TypeRooms { get => typeRooms; }
    public List<string> DateStart { get => dateStart; }
    public List<string> DateEnd { get => dateEnd; }
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
    public string SelectedDateStart
    {
      get => selectedDateStart;
      set
      {
        if (selectedDateStart == value) return;
        selectedDateStart = value;
        OnPropertyChanged("SelectedDateStart");
      }
    }
    public string SelectedDateEnd
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
    #endregion
  }
}
