using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseSitesForApartments.Export
{
  public abstract class CoreExport
  {
    public CoreExport()
    {
    }
    protected List<Flat> listFlats;


    public abstract void ExecuteWithBaseInfo();
    public abstract void Execute();

    public void DownloadInfoAboutFlat(Flat flat)
    {

    }

    public virtual void AddFilesInList(object sender, AppendFlatEventArgs arg)
    {
      listFlats.Add(arg.Flat);
    }
  }
}
