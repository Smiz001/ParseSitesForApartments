using System.Collections.Generic;

namespace ParseSitesForApartments.Export.Creators
{
  public class CsvExportCreator : CoreCreator
  {
    public override CoreExport FactoryCreate(List<Flat> flats)
    {
      return new CsvExport(flats);
    }
  }
}
