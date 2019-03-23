using System;
using System.Collections.Generic;

namespace Core.MainClasses
{
  public class District
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    public List<Building> Buildings { get; set; } = new List<Building>();
    public List<Metro> Metros { get; set; } = new List<Metro>();
  }
}
