using System.Collections.Generic;

namespace Core.MainClasses.Chart
{
  public class DataForChart
  {
    #region Fields
    private List<Point> points;
    private string name;
    #endregion

    #region Constructors
    public DataForChart()
    {
      Points = new List<Point>();
    }
    #endregion

    #region Properties

    public List<Point> Points { get => points; set => points = value; }
    public string Name { get => name; set => name = value; }

    #endregion

    #region Methods
    #endregion
  }
}
