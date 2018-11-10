using System;

namespace ParseSitesForApartments
{
  public class Metro
  {
    public string Name { get; set; } = string.Empty;
    public float XCoor { get; set; }
    public float YCoor { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid IdDistrict { get; set; }
  }
}
