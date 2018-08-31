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
    private const string Filename = @"D:\BNProdam.csv";

    public override void ParsingAll()
    {
      using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Create), Encoding.UTF8))
      {
        sw.WriteLine($@"Нас. пункт;Улица;Номер;Корпус;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние(км)");
      }
      var studiiThread = new Thread(ParseStudii);
      studiiThread.Start();
      var oneThread = new Thread(ParseOneRoom);
      oneThread.Start();
      var twoThread = new Thread(ParseTwoRoom);
      twoThread.Start();
      var threeThread = new Thread(ParseThreeRoom);
      threeThread.Start();
      var fourThread = new Thread(ParseFourRoom);
      fourThread.Start();
    }

    public void ParseStudii()
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
            ParseSheet("Студия", document);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }
      MessageBox.Show("Закончили студии");
    }
    public void ParseOneRoom()
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
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-1-city_district-{k}/?cpu=kkv-1-city_district-13&kkv%5B0%5D=1&city_district%5B0%5D=13&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptPortion=1&formName=secondary&page={i}";
            webClient.Encoding = Encoding.UTF8;
            try
            {
              var responce = webClient.DownloadString(sdam);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);
              ParseSheet("1 км. кв.", document);
              if (document.GetElementsByClassName("object--item").Length < 30)
                break;
            }
            catch (Exception ex)
            {
              MessageBox.Show(ex.Message);
            }

          }
        }
      }

      MessageBox.Show("Закончили 1 км. кв.");
    }
    public void ParseTwoRoom()
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
            ParseSheet("2 км. кв.", document);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }

      MessageBox.Show("Закончили 2 км. кв.");
    }
    public void ParseThreeRoom()
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
            ParseSheet("3 км. кв.", document);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }
      MessageBox.Show("Закончили 3 км. кв.");
    }
    public void ParseFourRoom()
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
            ParseSheet("3 км. кв.", document);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }
      MessageBox.Show("Закончили 4+ км. кв.");
    }
    private void ParseSheet(string typeRoom, IHtmlDocument document)
    {
      try
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
          if (ms.Count > 1)
            build.Price = int.Parse($"{ms[0].Value}{ms[1].Value}000");
          else
            build.Price = int.Parse($"{ms[0].Value}000");

          if (apartaments[i].GetElementsByClassName("object--metro").Length > 0)
            build.Metro = apartaments[i].GetElementsByClassName("object--metro")[0].TextContent.Trim();
          if (apartaments[i].GetElementsByClassName("object--metro-distance").Length > 0)
          {
            regex = new Regex(@"(\d+\.\d+)");
            build.Distance = apartaments[i].GetElementsByClassName("object--metro-distance")[0].TextContent.Replace(",", "").Replace(" ", "");
            build.Distance = regex.Match(build.Distance).Value;
          }
          build.Distance = build.Distance.Replace(".", ",");

          if (apartaments[i].GetElementsByClassName("object--floor").Length > 0)
          {
            var floor = apartaments[i].GetElementsByClassName("object--floor")[0].TextContent;
            regex = new Regex(@"(\d+)");
            var mas = regex.Matches(floor);
            if (mas.Count > 0)
              build.Floor = mas[0].Value;
          }

          build.Street = apartaments[i].GetElementsByClassName("object--address")[0].TextContent.Trim().Replace("Санкт-Петербург, ", "").Replace("Санкт-Петербург г.", "");


          regex = new Regex(@"(\d+к\d+)");
          build.Number = regex.Match(build.Street).Value;
          if (!string.IsNullOrEmpty(build.Number))
          {
            build.Street = build.Street.Replace(build.Number,"");
            regex = new Regex(@"(к\d+)");
            build.Building = regex.Match(build.Number).Value.Replace("к", "");
            build.Number= build.Number.Replace($"к{build.Building}", "");
          }
          else
          {
            regex = new Regex(@"(\d+\/\d+)");
            build.Number = regex.Match(build.Street).Value;
            if (!string.IsNullOrEmpty(build.Number))
            {
              build.Street = build.Street.Replace(build.Number, "");
              regex = new Regex(@"(\/\d+)");
              build.Building = regex.Match(build.Number).Value.Replace(@"/", "");
              build.Number = build.Number.Replace($@"/{build.Building}", "");
            }
            else
            {
              regex = new Regex(@"(\d+\sк\.\d+)|(\d+к\.\d+)|(\d+\sк\.\s\d+)|(\d+к\.\s\d+)");
              build.Number = regex.Match(build.Street).Value;
              if (!string.IsNullOrEmpty(build.Number))
              {
                build.Street = build.Street.Replace(build.Number, "");
                build.Number = build.Number.Replace(" ", "");
                regex = new Regex(@"(к.\d+)");
                build.Building = regex.Match(build.Number).Value.Replace(@"к.", "");
                build.Number = build.Number.Replace($@"к.{build.Building}", "");
              }
              else
              {
                regex = new Regex(@"(ул\.\s\d+$)|(ул\,\s\d+$)");
                build.Number = regex.Match(build.Street).Value;
                if (!string.IsNullOrEmpty(build.Number))
                {
                  build.Street = build.Street.Replace(build.Number, "");
                  build.Number = build.Number.Replace("ул. ","").Replace("ул, ", "");
                }
                else
                {
                  regex = new Regex(@"(ул\.,\sд\.\s\d+)$");
                  build.Number = regex.Match(build.Street).Value;
                  if (!string.IsNullOrEmpty(build.Number))
                  {
                    build.Street = build.Street.Replace(build.Number, "");
                    build.Number = build.Number.Replace("ул., д. ", "");
                  }
                  else
                  {
                    regex = new Regex(@"(пр\.\,\s\d+$)|(пр\.\s\d+$)$");
                    build.Number = regex.Match(build.Street).Value;
                    if (!string.IsNullOrEmpty(build.Number))
                    {
                      build.Street = build.Street.Replace(build.Number, "");
                      build.Number = build.Number.Replace("пр., ", "").Replace("пр. ", "");
                    }
                    else
                    {
                      regex = new Regex(@"(\,\s\d+$)");
                      build.Number = regex.Match(build.Street).Value;
                      if (!string.IsNullOrEmpty(build.Number))
                      {
                        build.Street = build.Street.Replace(build.Number, "");
                        build.Number = build.Number.Replace(", ", "");
                      }
                      else
                      {
                        regex = new Regex(@"(д\.\s\d+$)|(д\.\d+$)");
                        build.Number = regex.Match(build.Street).Value;
                        if (!string.IsNullOrEmpty(build.Number))
                        {
                          build.Street = build.Street.Replace(build.Number, "");
                          build.Number = build.Number.Replace(" ", "").Replace("д.","");
                        }
                        else
                        {
                          regex = new Regex(@"(дом\s+\d+$)|(дом\d+$)");
                          build.Number = regex.Match(build.Street).Value;
                          if (!string.IsNullOrEmpty(build.Number))
                          {
                            build.Street = build.Street.Replace(build.Number, "");
                            build.Number = build.Number.Replace(" ", "").Replace("дом", "");
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }

          /*
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
            //build.Street = build.Street.Replace(build.Number, "");

          if (string.IsNullOrEmpty(build.Number))
            build.Number = "Новостройка";
          else
            build.Number = build.Number.Replace(".", ",");
          //build.Number = build.Number.Replace("д,", "").Replace(" к, ", "/").Replace("ул, ", "").Replace(" к,", "/");
          */


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

          // build.Street = build.Street.Replace(" ул.", "").Replace("ул.", "").Replace("пр-кт.", "").Replace("пр.", "").Replace("бульвар ", "").Replace("б-р", "").Replace(", строение 1", "").Replace(" б-р/2", "").Replace("/3", "").Replace("/2", "").Replace("проспект", "").Replace("улица ", "").Replace(" улица", "").Replace("пр-кт", "").Replace(",  ", "").Replace(", ", "").Replace("ш.","").Replace(", д.","").Replace("г. , ","").Replace("б, д.", "").Replace("шос.","").Replace(",", "").Trim();

          //regex = new Regex(@"(к\.\d+)|(к\d+)");
          //var building = regex.Match(build.Street).Value;
          //if (!string.IsNullOrEmpty(building))
          //{
          //   build.Street = build.Street.Replace(building, "").Trim();
          //   building = building.Replace("к.","").Replace("к", "");
          //  build.Number = $@"{build.Number}/{building}";
          //}

          regex = new Regex(@"(\/А\d+А)");
          var str = regex.Match(build.Street).Value;
          if (!string.IsNullOrEmpty(str))
            build.Street = build.Street.Replace(str, "");
          Monitor.Enter(locker);
          using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Open), Encoding.UTF8))
          {
            sw.BaseStream.Position = sw.BaseStream.Length;
            sw.WriteLine($@"{town};{build.Street};{build.Number};{build.Building};{build.CountRoom};{build.Square};{build.Price};{ build.Floor};{build.Metro};{build.Distance}");
          }
          
        }
      }
      finally
      {
        Monitor.Exit(locker);
      }
    }

    private void WriteInFile(string str)
    {
      using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Open), Encoding.UTF8))
      {
        sw.BaseStream.Position = sw.BaseStream.Length;
        sw.WriteLine(str);
      }
    }
  }
}
