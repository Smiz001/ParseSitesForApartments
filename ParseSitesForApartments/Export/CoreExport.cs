using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseSitesForApartments.Export
{
  public abstract class CoreExport
  {
    public CoreExport(List<Flat> flats)
    {
      Flats = flats;
    }

    public List<Flat> Flats { get; private set; }

    public abstract void ExecuteWithBaseInfo();
    public abstract void Execute();
  }
}
