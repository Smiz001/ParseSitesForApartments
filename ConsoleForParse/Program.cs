using System;
using System.Collections.Generic;
using System.IO;
using Core.Connections;
using Core.Enum;
using Core.MainClasses;
using Core.Proxy;
using Core.Sites;
using ParseSitesForApartments;

namespace ConsoleForParse
{
  class Program
  {
    private static List<District> listDistricts = new List<District>();
    private static List<Metro> listMetros = new List<Metro>();
    private static List<ProxyInfo> listProxy = new List<ProxyInfo>();
    static void Main(string[] args)
    {
      ReadConfig();
      ReadMetroAndDistric();
      listProxy.Add(new ProxyInfo
      {
        Address = "185.233.202.204",
        Port = 9975,
        User = "BGXDdU",
        Password = "vy4ubS"
      });
      listProxy.Add(new ProxyInfo
      {
        Address = "185.233.201.211",
        Port = 9312,
        User = "BGXDdU",
        Password = "vy4ubS"
      });

      BaseParse parser = null;
      if (args.Length == 4)
      {
        var site = args[0];
        string siteName = string.Empty;
        switch (site)
        {
          case "all":
            parser = new AllSites(listDistricts, listMetros, listProxy);
              //siteName = "Все сайты";
            break;
          case "elms":
            parser = new ELMS(listDistricts, listMetros, listProxy);
            //siteName = "ELMS";
            break;
          case "bkn":
            parser = new BKN(listDistricts, listMetros, listProxy);
            //siteName = "BKN";
            break;
          case "bn":
            parser = new BN(listDistricts, listMetros, listProxy);
            //siteName = "BN";
            break;
        }
        if (parser != null)
        {
          var typeParse = args[1];
          switch (typeParse)
          {
            case "sale":
              parser.TypeParseFlat = TypeParseFlat.Sale;
              break;
            case "rent":
              parser.TypeParseFlat = TypeParseFlat.Rent;
              break;
          }
          parser.Filename = $@"{args[3]}{siteName}-{DateTime.Now.ToShortDateString()}.csv";

          var typeRoom = args[2];
          switch (typeRoom)
          {
            case "all":
              parser.ParsingAll();
              break;
            case "studii":
              parser.ParsingStudii();
              break;
            case "one":
              parser.ParsingOne();
              break;
            case "two":
              parser.ParsingTwo();
              break;
            case "three":
              parser.ParsingThree();
              break;
            case "four":
              parser.ParsingFour();
              break;
            case "five":
              parser.ParsingMoreFour();
              break;
          }
        }
      }
    }
    private static void ReadConfig()
    {
      var st = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      var fileCatalog = st + @"\ParseFlat";
      Directory.CreateDirectory(fileCatalog);
      string fileName = fileCatalog + @"\parseflat.config";

      if (File.Exists(fileName))
      {
        ImportConfigDataBase inmportConfigDataBase = new ImportConfigDataBase();
        inmportConfigDataBase.Import();
      }
    }
    private static void ReadMetroAndDistric()
    {
      var connection = ConnetionToSqlServer.Default();
      connection.Connect();
      string select = "SELECT [ID],[Name] FROM [ParseBulding].[dbo].[District]";
      var reader = connection.ExecuteReader(select);
      if (reader != null)
      {
        while (reader.Read())
        {
          listDistricts.Add(new District { Id = reader.GetGuid(0), Name = reader.GetString(1) });
        }
        reader.Close();
        foreach (var district in listDistricts)
        {
          select = $@"SELECT [Id]
              ,[Name]
              ,[XCoor]
              ,[YCoor]
              ,[IdRegion]
            FROM[ParseBulding].[dbo].[Metro]
            where IdRegion = '{district.Id}'";
          reader = connection.ExecuteReader(select);
          while (reader.Read())
          {
            var metro = new Metro
            {
              Id = reader.GetGuid(0),
              Name = reader.GetString(1),
              XCoor = (float)reader.GetDouble(2),
              YCoor = (float)reader.GetDouble(3)
            };
            district.Metros.Add(metro);
            listMetros.Add(metro);
          }
          reader.Close();
        }
      }
    }
  }
}
