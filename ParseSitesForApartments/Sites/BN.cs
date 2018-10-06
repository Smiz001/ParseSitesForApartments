﻿using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    private Dictionary<int, string> district = new Dictionary<int, string>() { { 1, "Адмиралтейский" }, { 2, "Василеостровский" }, { 3, "Выборгский" }, { 4, "Калининский" }, { 5, "Кировский" }, { 6, "Колпинский" }, { 7, "Красногвардейский" }, { 8, "Красносельский" }, { 9, "Кронштадтский" }, { 10, "Курортный" }, { 11, "Московский" }, { 12, "Невский" }, { 13, "Петроградский" }, { 14, "Петродворцовый" }, { 15, "Приморский" }, { 16, "Пушкинский" }, { 17, "Фрунзенский" }, { 18, "Центральный" }, };

    public override string Filename => @"d:\ParserInfo\Appartament\BNProdam.csv";
    public override string FilenameSdam => @"d:\ParserInfo\Appartament\BNSdam.csv";
    public override string FilenameWithinfo => @"d:\ParserInfo\Appartament\BNProdamWithInfo.csv";
    public override string FilenameWithinfoSdam => @"d:\ParserInfo\Appartament\BNSdamWithInfo.csv";
    public override string NameSite => "БН";

    public override void ParsingAll()
    {
      using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Create), Encoding.UTF8))
      {
        sw.WriteLine($@"Район;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние(км)");
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

      var studiiThreadOld = new Thread(ParseStudiiOld);
      studiiThreadOld.Start();
      var oneThreadOld = new Thread(ParseOneRoomOld);
      oneThreadOld.Start();
      var twoThreadOld = new Thread(ParseTwoRoomOld);
      twoThreadOld.Start();
      var threeThreadOld = new Thread(ParseThreeRoomOld);
      threeThreadOld.Start();
      var fourThreadOld = new Thread(ParseFourRoomOld);
      fourThreadOld.Start();
    }

    public void ParseStudiiOld()
    {
      int minPage = 1;
      int maxPage = 17;
      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < district.Count; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-0-city_district-{k}/?from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptNewBuildings=1&exceptPortion=1&formName=secondary&page={i}";
            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);
            ParseSheet("Студия", document, district[k]);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }
      MessageBox.Show("Закончили студии");
    }
    public void ParseOneRoomOld()
    {
      int minPage = 1;
      int maxPage = 17;

      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < district.Count; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-1-city_district-{k}/?cpu=kkv-1-city_district-1&kkv%5B0%5D=1&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptNewBuildings=1&exceptPortion=1&formName=secondary&page={i}";
            webClient.Encoding = Encoding.UTF8;
            try
            {
              var responce = webClient.DownloadString(sdam);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);
              ParseSheet("1 км. кв.", document, district[k]);
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
    public void ParseTwoRoomOld()
    {
      int minPage = 1;
      int maxPage = 17;

      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < district.Count; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-2-city_district-{k}/?cpu=kkv-2-city_district-1&kkv%5B0%5D=2&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptNewBuildings=1&exceptPortion=1&formName=secondary&page={i}";
            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);
            ParseSheet("2 км. кв.", document, district[k]);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }

      MessageBox.Show("Закончили 2 км. кв.");
    }
    public void ParseThreeRoomOld()
    {
      int minPage = 1;
      int maxPage = 17;

      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < district.Count; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-3-city_district-{k}/?cpu=kkv-3-city_district-1&kkv%5B0%5D=3&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptNewBuildings=1&exceptPortion=1&formName=secondary&page={i}";
            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);
            ParseSheet("3 км. кв.", document, district[k]);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }
      MessageBox.Show("Закончили 3 км. кв.");
    }
    public void ParseFourRoomOld()
    {
      int minPage = 1;
      int maxPage = 17;

      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < district.Count; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-4-city_district-{k}/?cpu=kkv-4-city_district-1&kkv%5B0%5D=4&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptNewBuildings=1&exceptPortion=1&formName=secondary&page={i}";
            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);
            ParseSheet("4 км. кв.", document, district[k]);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }
      MessageBox.Show("Закончили 4+ км. кв.");
    }

    public void ParseStudii()
    {
      int minPage = 1;
      int maxPage = 17;
      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < district.Count; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/kvartiry-novostroiki/kkv-0-city_district-{k}/?cpu=kkv-0-city_district-1&kkv%5B0%5D=0&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=newbuild&page={i}";
            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);
            ParseSheet("Студия Н", document, district[k]);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }
      MessageBox.Show("Закончили студии Н");
    }
    public void ParseOneRoom()
    {
      int minPage = 1;
      int maxPage = 17;

      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < district.Count; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/kvartiry-novostroiki/kkv-1-city_district-{k}/?cpu=kkv-1-city_district-1&kkv%5B0%5D=1&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=newbuild&page={i}";
            webClient.Encoding = Encoding.UTF8;
            try
            {
              var responce = webClient.DownloadString(sdam);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);
              ParseSheet("1 км. кв. Н", document, district[k]);
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

      MessageBox.Show("Закончили 1 км. кв. Н");
    }
    public void ParseTwoRoom()
    {
      int minPage = 1;
      int maxPage = 17;

      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < district.Count; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/kvartiry-novostroiki/kkv-2-city_district-{k}/?cpu=kkv-2-city_district-1&kkv%5B0%5D=2&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=newbuild&page={i}";
            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);
            ParseSheet("2 км. кв.", document, district[k]);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }

      MessageBox.Show("Закончили 2 км. кв. Н");
    }
    public void ParseThreeRoom()
    {
      int minPage = 1;
      int maxPage = 17;

      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < district.Count; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/kvartiry-novostroiki/kkv-3-city_district-{i}/?cpu=kkv-3-city_district-1&kkv%5B0%5D=3&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=newbuild&page={i}";
            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);
            ParseSheet("3 км. кв. Н", document, district[k]);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }
      MessageBox.Show("Закончили 3 км. кв. Н");
    }
    public void ParseFourRoom()
    {
      int minPage = 1;
      int maxPage = 17;

      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < district.Count; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/kvartiry-novostroiki/kkv-4-city_district-{k}/?cpu=kkv-4-city_district-1&kkv%5B0%5D=4&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=newbuild&page={i}";
            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);
            ParseSheet("4 км. кв. Н", document, district[k]);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;

          }
        }
      }
      MessageBox.Show("Закончили 4+ км. кв. Н");
    }

    private void ParseSheet(string typeRoom, IHtmlDocument document, string districtName)
    {
      var apartaments = document.GetElementsByClassName("object--item");

      for (int i = 0; i < apartaments.Length; i++)
      {
        var flat = new Flat();
        if (apartaments[i].GetElementsByClassName("object__square").Length > 0)
          flat.Square = apartaments[i].GetElementsByClassName("object__square")[0].TextContent.Trim().Replace(".", ",");
        flat.CountRoom = typeRoom;
        if (typeRoom == "4 км. кв." || typeRoom == "4 км. кв. Н")
        {
          var rx = new Regex(@"(\d+)");
          if (apartaments[i].GetElementsByClassName("object--title").Length > 0)
          {
            flat.CountRoom = $@"{rx.Match(apartaments[i].GetElementsByClassName("object--title")[0].TextContent).Value} км. кв.";
          }
        }

        var regex = new Regex(@"(\d+)");

        var priceString = apartaments[i].GetElementsByClassName("object--price_original")[0].TextContent.Trim();
        var ms = regex.Matches(priceString);
        if (ms.Count > 1)
          flat.Price = int.Parse($"{ms[0].Value}{ms[1].Value}000");
        else
          flat.Price = int.Parse($"{ms[0].Value}000");

        if (apartaments[i].GetElementsByClassName("object--metro").Length > 0)
          flat.Building.Metro = apartaments[i].GetElementsByClassName("object--metro")[0].TextContent.Trim();
        if (apartaments[i].GetElementsByClassName("object--metro-distance").Length > 0)
        {
          regex = new Regex(@"(\d+\.\d+)");
          flat.Building.Distance = apartaments[i].GetElementsByClassName("object--metro-distance")[0].TextContent.Replace(",", "").Replace(" ", "");
          flat.Building.Distance = regex.Match(flat.Building.Distance).Value;
        }
        flat.Building.Distance = flat.Building.Distance.Replace(".", ",");

        if (apartaments[i].GetElementsByClassName("object--floor").Length > 0)
        {
          var floor = apartaments[i].GetElementsByClassName("object--floor")[0].TextContent;
          regex = new Regex(@"(\d+)");
          var mas = regex.Matches(floor);
          if (mas.Count > 0)
            flat.Floor = mas[0].Value;
        }

        flat.Building.Street = apartaments[i].GetElementsByClassName("object--address")[0].TextContent.Trim().Replace("Санкт-Петербург, ", "").Replace("Санкт-Петербург г.", "");


        regex = new Regex(@"(\d+к\d+)");
        flat.Building.Number = regex.Match(flat.Building.Street).Value;
        if (!string.IsNullOrEmpty(flat.Building.Number))
        {
          flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
          regex = new Regex(@"(к\d+)");
          flat.Building.Structure = regex.Match(flat.Building.Number).Value.Replace("к", "");
          flat.Building.Number = flat.Building.Number.Replace($"к{flat.Building.Structure}", "");
        }
        else
        {
          regex = new Regex(@"(\d+\/\d+)");
          flat.Building.Number = regex.Match(flat.Building.Street).Value;
          if (!string.IsNullOrEmpty(flat.Building.Number))
          {
            flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
            regex = new Regex(@"(\/\d+)");
            flat.Building.Structure = regex.Match(flat.Building.Number).Value.Replace(@"/", "");
            flat.Building.Number = flat.Building.Number.Replace($@"/{flat.Building.Structure}", "");
          }
          else
          {
            regex = new Regex(@"(\d+\sк\.\d+)|(\d+к\.\d+)|(\d+\sк\.\s\d+)|(\d+к\.\s\d+)");
            flat.Building.Number = regex.Match(flat.Building.Street).Value;
            if (!string.IsNullOrEmpty(flat.Building.Number))
            {
              flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
              flat.Building.Number = flat.Building.Number.Replace(" ", "");
              regex = new Regex(@"(к.\d+)");
              flat.Building.Structure = regex.Match(flat.Building.Number).Value.Replace(@"к.", "");
              flat.Building.Number = flat.Building.Number.Replace($@"к.{flat.Building.Structure}", "");
            }
            else
            {
              regex = new Regex(@"(ул\.\s\d+$)|(ул\,\s\d+$)");
              flat.Building.Number = regex.Match(flat.Building.Street).Value;
              if (!string.IsNullOrEmpty(flat.Building.Number))
              {
                flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                flat.Building.Number = flat.Building.Number.Replace("ул. ", "").Replace("ул, ", "");
              }
              else
              {
                regex = new Regex(@"(ул\.,\sд\.\s\d+)$");
                flat.Building.Number = regex.Match(flat.Building.Street).Value;
                if (!string.IsNullOrEmpty(flat.Building.Number))
                {
                  flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                  flat.Building.Number = flat.Building.Number.Replace("ул., д. ", "");
                }
                else
                {
                  regex = new Regex(@"(пр\.\,\s\d+$)|(пр\.\s\d+$)$");
                  flat.Building.Number = regex.Match(flat.Building.Street).Value;
                  if (!string.IsNullOrEmpty(flat.Building.Number))
                  {
                    flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                    flat.Building.Number = flat.Building.Number.Replace("пр., ", "").Replace("пр. ", "");
                  }
                  else
                  {
                    regex = new Regex(@"(\,\s\d+$)");
                    flat.Building.Number = regex.Match(flat.Building.Street).Value;
                    if (!string.IsNullOrEmpty(flat.Building.Number))
                    {
                      flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                      flat.Building.Number = flat.Building.Number.Replace(", ", "");
                    }
                    else
                    {
                      regex = new Regex(@"(д\.\s\d+$)|(д\.\d+$)");
                      flat.Building.Number = regex.Match(flat.Building.Street).Value;
                      if (!string.IsNullOrEmpty(flat.Building.Number))
                      {
                        flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                        flat.Building.Number = flat.Building.Number.Replace(" ", "").Replace("д.", "");
                      }
                      else
                      {
                        regex = new Regex(@"(дом\s+\d+$)|(дом\d+$)");
                        flat.Building.Number = regex.Match(flat.Building.Street).Value;
                        if (!string.IsNullOrEmpty(flat.Building.Number))
                        {
                          flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                          flat.Building.Number = flat.Building.Number.Replace(" ", "").Replace("дом", "");
                        }
                        else
                        {
                          regex = new Regex(@"(пер\.\s+\d+)");
                          flat.Building.Number = regex.Match(flat.Building.Street).Value;
                          if (!string.IsNullOrEmpty(flat.Building.Number))
                          {
                            flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                            flat.Building.Number = flat.Building.Number.Replace("пер. ", "");
                          }
                          else
                          {
                            regex = new Regex(@"(наб\.\s+\d+)");
                            flat.Building.Number = regex.Match(flat.Building.Street).Value;
                            if (!string.IsNullOrEmpty(flat.Building.Number))
                            {
                              flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                              flat.Building.Number = flat.Building.Number.Replace("наб. ", "");
                            }
                            else
                            {
                              regex = new Regex(@"(пл\.\s+\d+)");
                              flat.Building.Number = regex.Match(flat.Building.Street).Value;
                              if (!string.IsNullOrEmpty(flat.Building.Number))
                              {
                                flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                                flat.Building.Number = flat.Building.Number.Replace("пл. ", "");
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }


        string town = "";
        if (flat.Building.Street.Contains("Колпино") || flat.Building.Street.Contains("г. Колпино"))
        {
          town = "Колпино";
          flat.Building.Street = flat.Building.Street.Replace(town, "").Replace("г. Колпино", "");
        }
        else if (flat.Building.Street.Contains("Песочный") || flat.Building.Street.Contains("пос. Песочный"))
        {
          town = "Песочный";
          flat.Building.Street = flat.Building.Street.Replace(town, "").Replace("пос. Песочный", "");
        }
        else if (flat.Building.Street.Contains("г. Кронштадт"))
        {
          town = "Кронштадт";
          flat.Building.Street = flat.Building.Street.Replace("г. Кронштадт", "");
        }
        else if (flat.Building.Street.Contains("Парголово"))
        {
          town = "Парголово";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else if (flat.Building.Street.Contains("Красное Село г"))
        {
          town = "Красное Село г";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else
        {
          town = "Санкт-Петербург";
          flat.Building.Street = flat.Building.Street.Replace("СПб", "");
        }

        flat.Building.Street = flat.Building.Street.Replace("ул.", "").Replace("ул", "").Replace("пр-кт", "").Replace("проспект", "").Replace("наб", "").Replace("б-р", "").Replace("б-р/2", "").Replace("б-р/4", "").Replace("проезд", "").Replace("пр", "").Replace("шос к", "").Replace("бульвар", "").Replace(" б", "").Replace("  к", "").Replace("  д", "").Replace("пл", "").Replace(",", "").Replace(".", "").Trim();

        regex = new Regex(@"(\/А\d+А)");
        var str = regex.Match(flat.Building.Street).Value;
        if (!string.IsNullOrEmpty(str))
          flat.Building.Street = flat.Building.Street.Replace(str, "");

        Monitor.Enter(locker);
        if (!string.IsNullOrEmpty(flat.Building.Number))
        {
          using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Open), Encoding.UTF8))
          {
            sw.BaseStream.Position = sw.BaseStream.Length;
            sw.WriteLine($@"{districtName};{flat.Building.Street};{flat.Building.Number};{flat.Building.Structure};{flat.Building.Liter};{flat.CountRoom};{flat.Square};{flat.Price};{ flat.Floor};{flat.Building.Metro};{flat.Building.Distance}");
          }
        }
        Monitor.Exit(locker);
      }
    }

    public override void ParsingSdamAll()
    {
      using (var sw = new StreamWriter(new FileStream(FilenameSdam, FileMode.Create), Encoding.UTF8))
      {
        sw.WriteLine($@"Нас. пункт;Улица;Номер;Корпус;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние(км)");
      }
      var studiiSdamThread = new Thread(ParseStudiiSdam);
      studiiSdamThread.Start();
      var oneSdamThread = new Thread(ParseOneSdam);
      oneSdamThread.Start();
      var twoThread = new Thread(ParseTwoSdam);
      twoThread.Start();
      var threeThread = new Thread(ParseThreeSdam);
      threeThread.Start();
      var fourThread = new Thread(ParseFourSdam);
      fourThread.Start();
    }

    public void ParseStudiiSdam()
    {
      int minPage = 1;
      int maxPage = 17;
      using (var webClient = new WebClient())
      {
        var random = new Random();
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/arenda-kvartiry/kkv-0/?cpu=kkv-0&kkv%5B0%5D=0&from=&to=&lease_period%5B0%5D=1&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=rent&page={i}";
            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);
          ParseSheetSdam("Студия", document);
            if (document.GetElementsByClassName("object--item").Length < 30)
              break;
          }
      }
      MessageBox.Show("Закончили студии");
    }
    public void ParseOneSdam()
    {
      int minPage = 1;
      int maxPage = 17;

      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < district.Count; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/arenda-kvartiry/kkv-1-city_district-{k}/?cpu=kkv-1-city_district-1&kkv%5B0%5D=1&city_district%5B0%5D=1&from=&to=&lease_period%5B0%5D=1&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=rent&page={i}";
            webClient.Encoding = Encoding.UTF8;
            try
            {
              var responce = webClient.DownloadString(sdam);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);
              ParseSheetSdam("1 км. кв.", document);
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
    public void ParseTwoSdam()
    {
      int minPage = 1;
      int maxPage = 17;

      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < district.Count; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/arenda-kvartiry/kkv-2-city_district-{k}/?cpu=kkv-2-city_district-1&kkv%5B0%5D=2&city_district%5B0%5D=1&from=&to=&lease_period%5B0%5D=1&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=rent&page={i}";
            webClient.Encoding = Encoding.UTF8;
            try
            {
              var responce = webClient.DownloadString(sdam);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);
              ParseSheetSdam("1 км. кв.", document);
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
      MessageBox.Show("Закончили 2 км. кв.");
    }
    public void ParseThreeSdam()
    {
      int minPage = 1;
      int maxPage = 17;

      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < district.Count; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/arenda-kvartiry/kkv-3-city_district-{k}/?cpu=kkv-3-city_district-1&kkv%5B0%5D=3&city_district%5B0%5D=1&from=&to=&lease_period%5B0%5D=1&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=rent&page={i}";
            webClient.Encoding = Encoding.UTF8;
            try
            {
              var responce = webClient.DownloadString(sdam);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);
              ParseSheetSdam("3 км. кв.", document);
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
      MessageBox.Show("Закончили 3 км. кв.");
    }
    public void ParseFourSdam()
    {
      int minPage = 1;
      int maxPage = 17;

      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < district.Count; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/arenda-kvartiry/kkv-4-city_district-{k}/?cpu=kkv-4-city_district-13&kkv%5B0%5D=4&city_district%5B0%5D=13&from=&to=&lease_period%5B0%5D=1&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=rent&page={i}";
            webClient.Encoding = Encoding.UTF8;
            try
            {
              var responce = webClient.DownloadString(sdam);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);
              ParseSheetSdam("4 км. кв.", document);
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
      MessageBox.Show("Закончили 4+ км. кв.");
    }

    private void ParseSheetSdam(string typeRoom, IHtmlDocument document)
    {
      var apartaments = document.GetElementsByClassName("object--item");

      for (int i = 0; i < apartaments.Length; i++)
      {
        var flat = new Flat();
        if (apartaments[i].GetElementsByClassName("object__square").Length > 0)
          flat.Square = apartaments[i].GetElementsByClassName("object__square")[0].TextContent.Trim();
        flat.CountRoom = typeRoom;
        if (typeRoom == "4 км. кв.")
        {
          var rx = new Regex(@"(\d+)");
          if (apartaments[i].GetElementsByClassName("object--title").Length > 0)
          {
            flat.CountRoom = $@"{rx.Match(apartaments[i].GetElementsByClassName("object--title")[0].TextContent).Value} км. кв.";
          }
        }

        var regex = new Regex(@"(\d+)");

        var priceString = apartaments[i].GetElementsByClassName("object--price_original")[0].TextContent.Trim();
        var ms = regex.Matches(priceString);
        if (ms.Count > 1)
          flat.Price = int.Parse($"{ms[0].Value}{ms[1].Value}000");
        else
          flat.Price = int.Parse($"{ms[0].Value}");

        if (apartaments[i].GetElementsByClassName("object--metro").Length > 0)
          flat.Building.Metro = apartaments[i].GetElementsByClassName("object--metro")[0].TextContent.Trim();
        if (apartaments[i].GetElementsByClassName("object--metro-distance").Length > 0)
        {
          regex = new Regex(@"(\d+\.\d+)");
          flat.Building.Distance = apartaments[i].GetElementsByClassName("object--metro-distance")[0].TextContent.Replace(",", "").Replace(" ", "");
          flat.Building.Distance = regex.Match(flat.Building.Distance).Value;
        }
        flat.Building.Distance = flat.Building.Distance.Replace(".", ",");

        if (apartaments[i].GetElementsByClassName("object--floor").Length > 0)
        {
          var floor = apartaments[i].GetElementsByClassName("object--floor")[0].TextContent;
          regex = new Regex(@"(\d+)");
          var mas = regex.Matches(floor);
          if (mas.Count > 0)
            flat.Floor = mas[0].Value;
        }

        flat.Building.Street = apartaments[i].GetElementsByClassName("object--address")[0].TextContent.Trim().Replace("Санкт-Петербург, ", "").Replace("Санкт-Петербург г.", "");


        regex = new Regex(@"(\d+к\d+)");
        flat.Building.Number = regex.Match(flat.Building.Street).Value;
        if (!string.IsNullOrEmpty(flat.Building.Number))
        {
          flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
          regex = new Regex(@"(к\d+)");
          flat.Building.Structure = regex.Match(flat.Building.Number).Value.Replace("к", "");
          flat.Building.Number = flat.Building.Number.Replace($"к{flat.Building.Structure}", "");
        }
        else
        {
          regex = new Regex(@"(\d+\/\d+)");
          flat.Building.Number = regex.Match(flat.Building.Street).Value;
          if (!string.IsNullOrEmpty(flat.Building.Number))
          {
            flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
            regex = new Regex(@"(\/\d+)");
            flat.Building.Structure = regex.Match(flat.Building.Number).Value.Replace(@"/", "");
            flat.Building.Number = flat.Building.Number.Replace($@"/{flat.Building.Structure}", "");
          }
          else
          {
            regex = new Regex(@"(\d+\sк\.\d+)|(\d+к\.\d+)|(\d+\sк\.\s\d+)|(\d+к\.\s\d+)");
            flat.Building.Number = regex.Match(flat.Building.Street).Value;
            if (!string.IsNullOrEmpty(flat.Building.Number))
            {
              flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
              flat.Building.Number = flat.Building.Number.Replace(" ", "");
              regex = new Regex(@"(к.\d+)");
              flat.Building.Structure = regex.Match(flat.Building.Number).Value.Replace(@"к.", "");
              flat.Building.Number = flat.Building.Number.Replace($@"к.{flat.Building.Structure}", "");
            }
            else
            {
              regex = new Regex(@"(ул\.\s\d+$)|(ул\,\s\d+$)");
              flat.Building.Number = regex.Match(flat.Building.Street).Value;
              if (!string.IsNullOrEmpty(flat.Building.Number))
              {
                flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                flat.Building.Number = flat.Building.Number.Replace("ул. ", "").Replace("ул, ", "");
              }
              else
              {
                regex = new Regex(@"(ул\.,\sд\.\s\d+)$");
                flat.Building.Number = regex.Match(flat.Building.Street).Value;
                if (!string.IsNullOrEmpty(flat.Building.Number))
                {
                  flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                  flat.Building.Number = flat.Building.Number.Replace("ул., д. ", "");
                }
                else
                {
                  regex = new Regex(@"(пр\.\,\s\d+$)|(пр\.\s\d+$)$");
                  flat.Building.Number = regex.Match(flat.Building.Street).Value;
                  if (!string.IsNullOrEmpty(flat.Building.Number))
                  {
                    flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                    flat.Building.Number = flat.Building.Number.Replace("пр., ", "").Replace("пр. ", "");
                  }
                  else
                  {
                    regex = new Regex(@"(\,\s\d+$)");
                    flat.Building.Number = regex.Match(flat.Building.Street).Value;
                    if (!string.IsNullOrEmpty(flat.Building.Number))
                    {
                      flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                      flat.Building.Number = flat.Building.Number.Replace(", ", "");
                    }
                    else
                    {
                      regex = new Regex(@"(д\.\s\d+$)|(д\.\d+$)");
                      flat.Building.Number = regex.Match(flat.Building.Street).Value;
                      if (!string.IsNullOrEmpty(flat.Building.Number))
                      {
                        flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                        flat.Building.Number = flat.Building.Number.Replace(" ", "").Replace("д.", "");
                      }
                      else
                      {
                        regex = new Regex(@"(дом\s+\d+$)|(дом\d+$)");
                        flat.Building.Number = regex.Match(flat.Building.Street).Value;
                        if (!string.IsNullOrEmpty(flat.Building.Number))
                        {
                          flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                          flat.Building.Number = flat.Building.Number.Replace(" ", "").Replace("дом", "");
                        }
                        else
                        {
                          regex = new Regex(@"(пер\.\s+\d+)");
                          flat.Building.Number = regex.Match(flat.Building.Street).Value;
                          if (!string.IsNullOrEmpty(flat.Building.Number))
                          {
                            flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                            flat.Building.Number = flat.Building.Number.Replace("пер. ", "");
                          }
                          else
                          {
                            regex = new Regex(@"(наб\.\s+\d+)");
                            flat.Building.Number = regex.Match(flat.Building.Street).Value;
                            if (!string.IsNullOrEmpty(flat.Building.Number))
                            {
                              flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                              flat.Building.Number = flat.Building.Number.Replace("наб. ", "");
                            }
                            else
                            {
                              regex = new Regex(@"(пл\.\s+\d+)");
                              flat.Building.Number = regex.Match(flat.Building.Street).Value;
                              if (!string.IsNullOrEmpty(flat.Building.Number))
                              {
                                flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                                flat.Building.Number = flat.Building.Number.Replace("пл. ", "");
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }


        string town = "";
        if (flat.Building.Street.Contains("Колпино") || flat.Building.Street.Contains("г. Колпино"))
        {
          town = "Колпино";
          flat.Building.Street = flat.Building.Street.Replace(town, "").Replace("г. Колпино", "");
        }
        else if (flat.Building.Street.Contains("Песочный") || flat.Building.Street.Contains("пос. Песочный"))
        {
          town = "Песочный";
          flat.Building.Street = flat.Building.Street.Replace(town, "").Replace("пос. Песочный", "");
        }
        else if (flat.Building.Street.Contains("г. Кронштадт"))
        {
          town = "Кронштадт";
          flat.Building.Street = flat.Building.Street.Replace("г. Кронштадт", "");
        }
        else if (flat.Building.Street.Contains("Парголово"))
        {
          town = "Парголово";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else if (flat.Building.Street.Contains("Красное Село г"))
        {
          town = "Красное Село г";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else if (flat.Building.Street.Contains("г Красное Село"))
        {
          town = "Красное Село г";
          flat.Building.Street = flat.Building.Street.Replace("г Красное Село", "");
        }
        else if (flat.Building.Street.Contains("Мурино"))
        {
          town = "Мурино";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else if (flat.Building.Street.Contains("Кудрово"))
        {
          town = "Кудрово";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else
        {
          town = "Санкт-Петербург";
          flat.Building.Street = flat.Building.Street.Replace("СПб", "");
        }

        flat.Building.Street = flat.Building.Street.Replace("ул.", "").Replace("ул", "").Replace("пр-кт", "").Replace("проспект", "").Replace("наб", "").Replace("б-р", "").Replace("б-р/2", "").Replace("б-р/4", "").Replace("проезд", "").Replace("пр", "").Replace("шос к", "").Replace("бульвар", "").Replace(" б", "").Replace("  к", "").Replace("  д", "").Replace("пл", "").Replace(",", "").Replace(".", "").Trim();

        regex = new Regex(@"(\/А\d+А)");
        var str = regex.Match(flat.Building.Street).Value;
        if (!string.IsNullOrEmpty(str))
          flat.Building.Street = flat.Building.Street.Replace(str, "");

        Monitor.Enter(locker);
        if(!string.IsNullOrWhiteSpace(flat.Building.Number))
        {
          using (var sw = new StreamWriter(new FileStream(FilenameSdam, FileMode.Open), Encoding.UTF8))
          {
            sw.BaseStream.Position = sw.BaseStream.Length;
            sw.WriteLine($@"{town};{flat.Building.Street};{flat.Building.Number};{flat.Building.Structure};{flat.Building.Liter};{flat.CountRoom};{flat.Square};{flat.Price};{ flat.Floor};{flat.Building.Metro};{flat.Building.Distance}");
          }
        }
        Monitor.Exit(locker);
      }
    }
  }
}
