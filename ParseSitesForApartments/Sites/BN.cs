using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ParseSitesForApartments.Sites
{
  public class BN : BaseParse
  {
    static object locker = new object();
    public override void ParsingAll()
    {
      using (var sw = new StreamWriter(@"D:\BNProdam.csv", true, Encoding.UTF8))
      {
        sw.WriteLine($@"Нас. пункт;Улица;Номер;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние(км)");
        var studiiThread = new Thread(new ParameterizedThreadStart(ParseStudii));
        studiiThread.Start(sw);
        var oneThread = new Thread(new ParameterizedThreadStart(ParseOneRoom));
        oneThread.Start(sw);
        //ParseStudii(sw);
        //ParseOneRoom(sw);
        //ParseTwoRoom(sw);
        //ParseThreeRoom(sw);
        //ParseFourRoom(sw);
      }
    }

    public void ParseStudii(object sw)
    {
      int minPage = 1;
      int maxPage = 17;
      int countDistrict = 18;

      var writer = sw as StreamWriter;

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
            ParseSheet(writer, "Студия", document);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }
      MessageBox.Show("Закончили студии");
    }
    public void ParseOneRoom(object sw)
    {
      int minPage = 1;
      int maxPage = 17;
      int countDistrict = 18;

      var writer = sw as StreamWriter;

      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < countDistrict; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-1-city_district-{k}/?cpu=kkv-1-city_district-13&kkv%5B0%5D=1&city_district%5B0%5D=13&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptPortion=1&formName=secondary&page={i}";
            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);
            ParseSheet(writer, "1 км. кв.", document);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }

      MessageBox.Show("Закончили 1 км. кв.");
    }

    public void ParseTwoRoom(StreamWriter sw)
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
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-2-city_district-{k}/?cpu=kkv-2-city_district-13&kkv%5B0%5D=2&city_district%5B0%5D=13&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptPortion=1&formName=secondary&page={i}";
            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);
            ParseSheet(sw, "2 км. кв.", document);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }
    }
    public void ParseThreeRoom(StreamWriter sw)
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
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-3-city_district-{k}/?cpu=kkv-3-city_district-13&kkv%5B0%5D=3&city_district%5B0%5D=13&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptPortion=1&formName=secondary&page={i}";
            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);
            ParseSheet(sw, "3 км. кв.", document);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }
    }
    public void ParseFourRoom(StreamWriter sw)
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
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-4-city_district-{k}/?cpu=kkv-4-city_district-13&kkv%5B0%5D=4&city_district%5B0%5D=13&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptPortion=1&formName=secondary&page={i}";
            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);
            ParseSheet(sw, "3 км. кв.", document);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }
    }

    private void ParseSheet(StreamWriter sw, string typeRoom, IHtmlDocument document)
    {
      try
      {
        Monitor.Enter(locker);
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

          if (apartaments[i].GetElementsByClassName("object--floor").Length > 0)
          {
            var floor = apartaments[i].GetElementsByClassName("object--floor")[0].TextContent;
            regex = new Regex(@"(\d+)");
            var mas = regex.Matches(floor);
            if (mas.Count > 0)
              build.Floor = mas[0].Value;
          }

          build.Street = apartaments[i].GetElementsByClassName("object--address")[0].TextContent.Trim().Replace("Санкт-Петербург, ", "").Replace("Санкт-Петербург г.", "");

          if (build.Street.Contains("Социалистическая"))
          {
            string a1 = "";
          }

          regex = new Regex(@"(\d+\,\s+\d+\,\s+\d+)|(\d+\,\s+\d+)");
          build.Number = regex.Match(build.Street).Value;

          if (string.IsNullOrEmpty(build.Number))
          {
            var arr = build.Street.Split(',');
            int number;
            if (arr.Length >= 2)
            {
              if (int.TryParse(arr[1], out number))
                build.Number = number.ToString();
            }
          }

          if (string.IsNullOrEmpty(build.Number))
          {
            regex = new Regex(@"(д.\d+\sк.\d+)|(бул.\s+\d+\sк.\d+)|(ул.\s+\d+)|(\d+\/\d+)|(д.\d+)|(бул.\s+\d+)|(\d+\sк.\s+\d+)|(пр-кт\d+)|(пр\.\s+\d+)|(ул\.\d+)|(д.\s+\d+)|(пр\.\d+)|(Улица\d+)");
            build.Number = regex.Match(build.Street).Value;
          }

          if (!string.IsNullOrEmpty(build.Number))
            build.Street = build.Street.Replace(build.Number, "");

          if (string.IsNullOrEmpty(build.Number))
            build.Number = "Новостройка";
          else
            build.Number = build.Number.Replace(".", ",");
          build.Number = build.Number.Replace("д,", "").Replace(" к, ", "/").Replace("ул, ", "");

          build.Distance = build.Distance.Replace(".", ",");

          string town = "";
          if (build.Street.Contains("Колпино") || build.Street.Contains("г. Колпино"))
          {
            town = "Колпино";
            build.Street = build.Street.Replace(town, "").Replace("г. Колпино", "");
          }
          else if (build.Street.Contains("Песочный") || build.Street.Contains("пос. Песочный"))
          {
            town = "Песочный";
            build.Street = build.Street.Replace(town, "").Replace("пос. Песочный", "");
          }
          else if (build.Street.Contains("г. Кронштадт"))
          {
            town = "Кронштадт";
            build.Street = build.Street.Replace("г. Кронштадт", "");
          }
          else if (build.Street.Contains("Парголово"))
          {
            town = "Парголово";
            build.Street = build.Street.Replace(town, "");
          }
          else
          {
            town = "Санкт-Петербург";
            build.Street = build.Street.Replace("СПб", "");
          }

          build.Street = build.Street.Replace(" ул.", "").Replace("ул.", "").Replace("пр-кт.", "").Replace("пр.", "").Replace("бульвар ", "").Replace("б-р", "").Replace(", строение 1", "").Replace(" б-р/2", "").Replace("/3", "").Replace("/2", "").Replace(" проспект", "").Replace("улица ", "").Replace(" улица", "").Replace("пр-кт", "").Trim();

          regex = new Regex(@"(\/А\d+А)");
          var str = regex.Match(build.Street).Value;
          if (!string.IsNullOrEmpty(str))
            build.Street = build.Street.Replace(str, "");

          sw.WriteLine($@"{town};{build.Street};{build.Number};{build.CountRoom};{build.Square};{build.Price};{ build.Floor};{build.Metro};{build.Distance}");
        }
      }
      finally
      {
        Monitor.Exit(locker);
      }
    }
  }
}
