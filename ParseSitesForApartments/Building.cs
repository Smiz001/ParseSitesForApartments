using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseSitesForApartments
{
  public class Building
  {
    public string Metro { get; set; } = string.Empty;
    public string Distance { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Structure { get; set; } = string.Empty;
    public string Liter { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;

    public string DateBuild { get; set; } = string.Empty;
    public string DateRepair { get; set; } = string.Empty;
    public string DateReconstruct { get; set; } = string.Empty;
    public List<Flat> FlatsOnSale { get; set; } = new List<Flat>();
    public string DistanceOnFoot { get; set; } = string.Empty;
    public string DistanceOnCar { get; set; } = string.Empty;
    public Metro MetroObj { get; set; }
    public string TimeOnFootToMetro { get; set; } = string.Empty;
    public string TimeOnCarToMetro { get; set; } = string.Empty;
    public District District { get; set; }


    public override bool Equals(object obj)
    {
      var building = obj as Building;
      if (building != null)
      {
        if (building.Street == this.Street)
        {
          if (building.Number == this.Number)
          {
            if (building.Structure == this.Structure)
            {
              if (building.Liter == this.Liter)
                return true;
            }
          }
        }
      }
      return false;
    }

    public override int GetHashCode()
    {
      var hashCode = -1541394451;
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Number);
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Structure);
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Liter);
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Street);
      return hashCode;
    }

    public bool IsEmpty()
    {
      if (string.IsNullOrWhiteSpace(Street) && string.IsNullOrWhiteSpace(Number) && string.IsNullOrWhiteSpace(Metro))
        return true;
      else
        return false;
    }
  }
}
