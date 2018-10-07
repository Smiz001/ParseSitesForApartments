using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseSitesForApartments.Sites
{
  public abstract class BaseParse
  {
    public BaseParse(List<District> listDistricts)
    {
      ListDistricts = new List<District>(listDistricts);
    }

    protected List<District> ListDistricts { get; private set; }
    public abstract string Filename{get;}
    public abstract string FilenameSdam { get; }
    public abstract string FilenameWithinfo { get; }
    public abstract string FilenameWithinfoSdam { get; }
    public abstract string NameSite { get; }
    public abstract void ParsingAll();
    public abstract void ParsingSdamAll();
  }
}
