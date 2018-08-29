using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ParseSitesForApartments.Sites
{
  public class BN : BaseParse
  {
    public override void ParsingAll()
    {
      using (var sw = new StreamWriter(@"D:\BNProdam.csv", true, Encoding.UTF8))
      {
        ParseStudii(sw);
      }
    }

    public void ParseStudii(StreamWriter sw)
    {
      int minPage = 1;
      int maxPage = 17;
      int countDistrict = 18;

      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < countDistrict; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-0-city_district-{k}/?cpu=kkv-0-city_district-1&kkv%5B0%5D=0&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptPortion=1&formName=secondary&page={i}";
            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);
            ParseSheet(sw, "Студия", document);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }
    }

    private void ParseSheet(StreamWriter sw, string typeRoom, IHtmlDocument document)
    {
      var apartaments = document.GetElementsByClassName("object--item");

      for (int i = 0; i < apartaments.Length; i++)
      {
        var build = new Build();
        if (apartaments[i].GetElementsByClassName("object__square").Length > 0)
          build.Square = apartaments[i].GetElementsByClassName("object__square")[0].TextContent.Trim();
        build.CountRoom = typeRoom;

        var regex = new Regex(@"(\d+)");

        var priceString = apartaments[i].GetElementsByClassName("object--price_original")[0].TextContent.Trim();
        var ms = regex.Matches(priceString);
        var price = int.Parse($"{ms[0].Value}{ms[1].Value}000");
        build.Price = price;

        if (apartaments[i].GetElementsByClassName("object--metro").Length > 0)
          build.Metro = apartaments[i].GetElementsByClassName("object--metro")[0].TextContent.Trim();
        if (apartaments[i].GetElementsByClassName("object--metro-distance").Length > 0)
        {
          regex = new Regex(@"(\d+\.\d+)");
          build.Distance = apartaments[i].GetElementsByClassName("object--metro-distance")[0].TextContent.Replace(",", "").Replace(" ", "");
          build.Distance = regex.Match(build.Distance).Value;
        }
          

        build.Street = apartaments[i].GetElementsByClassName("object--address")[0].TextContent.Trim().Replace("Санкт-Петербург, ", "");
        var arr = build.Street.Split(',');
        int number;
        if(arr.Length >= 2)
        {
          if (int.TryParse(arr[1], out number))
            build.Number = number.ToString();
          build.Street = build.Street.Replace($@", {build.Number}", "");
        }
        if(string.IsNullOrEmpty(build.Number))
        {
          regex = new Regex(@"(д.\d+\sк.\d+)|(бул.\s+\d+\sк.\d+)|(ул.\s+\d+)|(\d+\/\d+)|(д.\d+)|(бул.\s+\d+)|(\d+\sк.\s+\d+)");
          build.Number = regex.Match(build.Street).Value;
          if(!string.IsNullOrEmpty(build.Number))
            build.Street = build.Street.Replace(build.Number, "");
        }
        //regex = new Regex(@"(\d+, \d+, \d+)|(\d+, \d+)|(\d+)");
        //build.Number = regex.Match(build.Street).Value;
        //if (!string.IsNullOrEmpty(build.Number))
        //  build.Street = build.Street.Replace(build.Number, "");

        build.Distance = build.Distance.Replace(".", ",");
        if (string.IsNullOrEmpty(build.Number))
          build.Number = "Новостройка";
        else
          build.Number = build.Number.Replace(".", ",");
        sw.WriteLine($@"{build.Street};{build.Number};{build.CountRoom};{build.Square};{build.Price};{ build.Floor};{build.Metro};{build.Distance}");
      }
    }
  }
}
