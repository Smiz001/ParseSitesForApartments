using System;
using System.IO;
using System.Text;
using System.Threading;

namespace ParseSitesForApartments.Export
{
  public class CsvExport : CoreExport
  {

    public CsvExport(string filename) : base(filename)
    {
    }

    public override void ExecuteWithBaseInfo()
    {
      throw new NotImplementedException();
    }

    public override void Execute()
    {
      using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Open), Encoding.UTF8))
      {
        foreach (var flat in listFlats)
        {
          sw.BaseStream.Position = sw.BaseStream.Length;
          sw.WriteLine($@"{flat.Building.District?.Name};{flat.Building.Street};{flat.Building.Number};{flat.Building.Structure};{flat.Building.Liter};{flat.CountRoom};{flat.Square};{ flat.Floor};{flat.Building.CountFloor};{flat.Price};{flat.Building.MetroObj?.Name};{flat.Building.DateBuild};{flat.Building.DateReconstruct};{flat.Building.DateRepair};{flat.Building.BuildingSquare};{flat.Building.LivingSquare};{flat.Building.NoLivingSqaure};{flat.Building.MansardaSquare};{flat.Building.CountInternal};{flat.Building.Otoplenie};{flat.Building.Gvs};{flat.Building.Es};{flat.Building.Gs};{flat.Building.TypeApartaments};{flat.Building.CountApartaments};{flat.Building.DateTep.ToShortDateString()};{flat.Building.TypeRepair};{flat.Building.CountLift};{flat.Building.DistanceOnFoot};{flat.Building.TimeOnFootToMetro};{flat.Building.DistanceOnCar};{flat.Building.TimeOnCarToMetro};{flat.Url}");
        }
        listFlats.Clear();
      }
    }

    public override void AddFlatInList(object sender, AppendFlatEventArgs arg)
    {
      base.AddFlatInList(sender, arg);
      if (listFlats.Count == 10)
      {
        Execute();
      }
    }
  }
}
