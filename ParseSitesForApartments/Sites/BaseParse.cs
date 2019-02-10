using System.Collections.Generic;
using System.Reflection;
using log4net;

namespace ParseSitesForApartments.Sites
{
  public abstract class BaseParse
  {
    public BaseParse(List<District> listDistricts, List<Metro> listMetros)
    {
      ListDistricts = new List<District>(listDistricts);
      ListMetros = new List<Metro>(listMetros);
    }

    protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected List<District> ListDistricts { get; private set; }
    protected List<Metro> ListMetros { get; private set; }
    public abstract string Filename{get; set; }
    public abstract string FilenameSdam { get; }
    public abstract string FilenameWithinfo { get; }
    public abstract string FilenameWithinfoSdam { get; }
    public abstract string NameSite { get; }
    public abstract void ParsingSdamAll();

    public abstract void ParsingAll();
    public abstract void ParsingStudii();
    public abstract void ParsingOne();
    public abstract void ParsingTwo();
    public abstract void ParsingThree();
    public abstract void ParsingFour();
    public abstract void ParsingMoreFour();
  }
}
