using System;
using System.Collections.Generic;

namespace ParseSitesForApartments.Export.Creators
{
  public class CsvExportCreator : CoreCreator
  {
    public override CoreExport FactoryCreate(List<Flat> flats)
    {
      throw new NotImplementedException();
    }

    public override CoreExport FactoryCreate(string fileName)
    {
      return new CsvExport(fileName);
    }
  }
}
