using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseSitesForApartments.Sites
{
  public abstract class BaseParse
  {
    public abstract string Filename{get;}
    public abstract string FilenameSdam { get; }
    public abstract string FilenameWithinfo { get; }
    public abstract string FilenameWithinfoSdam { get; }
    public abstract string NameSite { get; }
    public abstract void ParsingAll();
    public abstract void ParsingSdamAll();
  }
}
