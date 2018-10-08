using System;
using System.Collections.Generic;

namespace ParseSitesForApartments
{
  public class Flat
  {
    public string CountRoom { get; set; } = string.Empty;
    public int Price { get; set; } = 0;
    public string Square { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public Building Building { get; set; } = new Building();
    public string Url { get; set; }= string.Empty;

    public override bool Equals(object obj)
    {
      var flat = obj as Flat;
      if (flat != null)
      {
        if (this.Building.Equals(flat.Building))
        {
          if (flat.CountRoom == this.CountRoom)
          {
            if (flat.Square == this.Square)
            {
              if (flat.Floor == this.Floor)
              {
                if (flat.Price == this.Price)
                  return true;
              }
            }
          }
        }
      }
      return false;
    }

    public override int GetHashCode()
    {
      var hashCode = 1793542146;
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CountRoom);
      hashCode = hashCode * -1521134295 + Price.GetHashCode();
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Square);
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Floor);
      hashCode = hashCode * -1521134295 + EqualityComparer<Building>.Default.GetHashCode(Building);
      return hashCode;
    }
  }
}
