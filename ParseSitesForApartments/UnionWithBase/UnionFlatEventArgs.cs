using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseSitesForApartments.UnionWithBase
{
  public class UnionFlatEventArgs : EventArgs
  {
    public Flat Flat { get; set; }
  }
}
