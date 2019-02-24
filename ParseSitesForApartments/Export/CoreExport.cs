using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace ParseSitesForApartments.Export
{
  public abstract class CoreExport
  {
    protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    public CoreExport(string filename)
    {
      Filename = filename;
    }
    protected List<Flat> listFlats = new List<Flat>();
    protected string Filename { get; set; }


    public abstract void ExecuteWithBaseInfo();
    public abstract void Execute();

    public void DownloadInfoAboutFlat(Flat flat)
    {

    }
    public virtual void AddFlatInList(object sender, AppendFlatEventArgs arg)
    {
      Log.Debug($"Add flat - {arg.Flat}");
      listFlats.Add(arg.Flat);
    }
  }
}
