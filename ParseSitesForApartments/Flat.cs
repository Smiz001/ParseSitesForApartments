using System.Collections.Generic;

namespace ParseSitesForApartments
{
  public class Flat
  {
    public string Metro { get; set; } = string.Empty;
    public string Distance { get; set; } = string.Empty;
    public string CountRoom { get; set; } = string.Empty;
    public int Price { get; set; } = 0;
    public string Square { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Building { get; set; } = string.Empty;
    public string Liter { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string DateBuild { get; set; } = string.Empty;
    public string DateRepair { get; set; } = string.Empty;

    public override bool Equals(object obj)
    {
      var flat = obj as Flat;
      if (flat == null)
        return false;
      else
      {
        if (flat.Street == this.Street)
        {
          if (flat.Number == this.Number)
          {
            if (flat.Building == this.Building)
            {
              if (flat.Liter == this.Liter)
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
          }
        }
      }
      return false;
    }

    public override int GetHashCode()
    {
      var hashCode = 1793542146;
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Distance);
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CountRoom);
      hashCode = hashCode * -1521134295 + Price.GetHashCode();
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Square);
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Floor);
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Number);
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Building);
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Liter);
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Street);
      return hashCode;
    }
  }
}
