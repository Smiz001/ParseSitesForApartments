using System.Collections.Generic;
using Core.MainClasses;

namespace Core.Export.Creators
{
  public abstract class CoreCreator
  {
    public abstract CoreExport FactoryCreate(List<Flat> flats);
    public abstract CoreExport FactoryCreate(string fileName);
  }
}
