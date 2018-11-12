using System;
using System.IO;
using System.Text;

namespace ParseSitesForApartments.Export
{
  public class CsvExport:CoreExport
  {
    public CsvExport(string filename):base(filename)
    {
    }

    public override void ExecuteWithBaseInfo()
    {
      throw new NotImplementedException();
    }

    public override void Execute()
    {
      throw new NotImplementedException();
    }

    public override void AddFilesInList(object sender, AppendFlatEventArgs arg)
    {
      base.AddFilesInList(sender, arg);
      if(listFlats.Count==20)
      {
        using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Open), Encoding.UTF8))
        {
          foreach (var flat in listFlats)
          {
            if (!string.IsNullOrEmpty(flat.Building.Number) || !string.IsNullOrEmpty(flat.Building.Street))
            {

              sw.BaseStream.Position = sw.BaseStream.Length;
              sw.WriteLine($@"{flat.Building.District.Name};{flat.Building.Street};{flat.Building.Number};{flat.Building.Structure};{flat.Building.Liter};{flat.CountRoom};{flat.Square};{flat.Price};{ flat.Floor};{flat.Building?.MetroObj?.Name};{flat.Building.Distance};{flat.Url}");
            }
          }
          listFlats.Clear();
        }
      }
    }
  }
}
