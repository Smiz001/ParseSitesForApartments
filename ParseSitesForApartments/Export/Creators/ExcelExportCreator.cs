using System.Collections.Generic;

namespace ParseSitesForApartments.Export.Creators
{
  public class ExcelExportCreator : CoreCreator
  {
    public override CoreExport FactoryCreate(List<Flat> flats)
    {
      return new ExcelExport(flats);
    }
  }
}
