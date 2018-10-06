using System.Collections.Generic;

namespace ParseSitesForApartments.Export.Creators
{
  public abstract class CoreCreator
  {
    public abstract CoreExport FactoryCreate(List<Flat> flats);
  }
}
