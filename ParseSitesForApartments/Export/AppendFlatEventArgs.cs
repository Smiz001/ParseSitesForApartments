using System;

namespace ParseSitesForApartments.Export
{
  public class AppendFlatEventArgs: EventArgs
  {
    public Flat Flat { get; set; }
  }
}
