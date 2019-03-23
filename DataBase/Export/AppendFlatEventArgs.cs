using System;
using Core.MainClasses;

namespace Core.Export
{
  public class AppendFlatEventArgs: EventArgs
  {
    public Flat Flat { get; set; }
  }
}
