using System.Collections.Generic;
using ParseSitesForApartments.Export;

namespace ParseSitesForApartments.Sites
{
  public abstract class BaseParse
  {
    public BaseParse(List<District> listDistricts, List<Metro> listMetros)
    {
      ListDistricts = new List<District>(listDistricts);
      ListMetros = new List<Metro>(listMetros);
    }

    protected List<District> ListDistricts { get; private set; }
    protected List<Metro> ListMetros { get; private set; }
    public abstract string Filename{get;}
    public abstract string FilenameSdam { get; }
    public abstract string FilenameWithinfo { get; }
    public abstract string FilenameWithinfoSdam { get; }
    public abstract string NameSite { get; }
    public abstract void ParsingAll();
    public abstract void ParsingSdamAll();
  }
}
