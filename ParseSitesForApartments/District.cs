using System;
using System.Collections.Generic;

namespace ParseSitesForApartments
{
  public class District
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;

    public List<Building> Buildings { get; set; } = new List<Building>();
  }
}
