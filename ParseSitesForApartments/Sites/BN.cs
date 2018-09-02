using AngleSharp.Dom.Html;
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
    private const string Filename = @"D:\BNProdam.csv";
    private const string FilenameWithinfo = @"D:\BNProdamWithInfo.csv";
    private Dictionary<int, string> district = new Dictionary<int, string>() { { 1, "Адмиралтейский" }, { 2, "Василеостровский" }, { 3, "Выборгский" }, { 4, "Калининский" }, { 5, "Кировский" }, { 6, "Колпинский" }, { 7, "Красногвардейский" }, { 8, "Красносельский" }, { 9, "Кронштадтский" }, { 10, "Курортный" }, { 11, "Московский" }, { 12, "Невский" }, { 13, "Петроградский" }, { 14, "Петродворцовый" }, { 15, "Приморский" }, { 16, "Пушкинский" }, { 17, "Фрунзенский" }, { 18, "Центральный" }, };

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
      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int k = 1; k < district.Count; k++)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(2000, 4000));
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-0-city_district-{k}/?cpu=kkv-0-city_district-1&kkv%5B0%5D=0&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptPortion=1&formName=secondary&page={i}";
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
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-1-city_district-{k}/?cpu=kkv-1-city_district-13&kkv%5B0%5D=1&city_district%5B0%5D=13&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptPortion=1&formName=secondary&page={i}";
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
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-2-city_district-{k}/?cpu=kkv-2-city_district-13&kkv%5B0%5D=2&city_district%5B0%5D=13&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptPortion=1&formName=secondary&page={i}";
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
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-3-city_district-{k}/?cpu=kkv-3-city_district-13&kkv%5B0%5D=3&city_district%5B0%5D=13&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptPortion=1&formName=secondary&page={i}";
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
            string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/kkv-4-city_district-{k}/?cpu=kkv-4-city_district-13&kkv%5B0%5D=4&city_district%5B0%5D=13&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptPortion=1&formName=secondary&page={i}";
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
    private void ParseSheet(string typeRoom, IHtmlDocument document, string districtName)
    {
      var apartaments = document.GetElementsByClassName("object--item");

      for (int i = 0; i < apartaments.Length; i++)
      {
        var build = new Build();
        if (apartaments[i].GetElementsByClassName("object__square").Length > 0)
          build.Square = apartaments[i].GetElementsByClassName("object__square")[0].TextContent.Trim();
        build.CountRoom = typeRoom;
        if (typeRoom == "4 км. кв.")
        {
          var rx = new Regex(@"(\d+)");
          if (apartaments[i].GetElementsByClassName("object--title").Length > 0)
          {
            build.CountRoom = $@"{rx.Match(apartaments[i].GetElementsByClassName("object--title")[0].TextContent).Value} км. кв.";
          }
        }

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
          build.Street = build.Street.Replace(build.Number, "");
          regex = new Regex(@"(к\d+)");
          build.Building = regex.Match(build.Number).Value.Replace("к", "");
          build.Number = build.Number.Replace($"к{build.Building}", "");
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
                build.Number = build.Number.Replace("ул. ", "").Replace("ул, ", "");
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
                        build.Number = build.Number.Replace(" ", "").Replace("д.", "");
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
                        else
                        {
                          regex = new Regex(@"(пер\.\s+\d+)");
                          build.Number = regex.Match(build.Street).Value;
                          if (!string.IsNullOrEmpty(build.Number))
                          {
                            build.Street = build.Street.Replace(build.Number, "");
                            build.Number = build.Number.Replace("пер. ", "");
                          }
                          else
                          {
                            regex = new Regex(@"(наб\.\s+\d+)");
                            build.Number = regex.Match(build.Street).Value;
                            if (!string.IsNullOrEmpty(build.Number))
                            {
                              build.Street = build.Street.Replace(build.Number, "");
                              build.Number = build.Number.Replace("наб. ", "");
                            }
                            else
                            {
                              regex = new Regex(@"(пл\.\s+\d+)");
                              build.Number = regex.Match(build.Street).Value;
                              if (!string.IsNullOrEmpty(build.Number))
                              {
                                build.Street = build.Street.Replace(build.Number, "");
                                build.Number = build.Number.Replace("пл. ", "");
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
        else if (build.Street.Contains("Красное Село г"))
        {
          town = "Красное Село г";
          build.Street = build.Street.Replace(town, "");
        }
        else
        {
          town = "Санкт-Петербург";
          build.Street = build.Street.Replace("СПб", "");
        }

        build.Street = build.Street.Replace("ул.", "").Replace("ул", "").Replace("пр-кт", "").Replace("проспект", "").Replace("наб", "").Replace("б-р", "").Replace("б-р/2", "").Replace("б-р/4", "").Replace("проезд", "").Replace("пр", "").Replace("шос к", "").Replace("бульвар", "").Replace(" б", "").Replace("  к", "").Replace("  д", "").Replace("пл", "").Replace(",", "").Replace(".", "").Trim();

        regex = new Regex(@"(\/А\d+А)");
        var str = regex.Match(build.Street).Value;
        if (!string.IsNullOrEmpty(str))
          build.Street = build.Street.Replace(str, "");

        Monitor.Enter(locker);
        using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Open), Encoding.UTF8))
        {
          sw.BaseStream.Position = sw.BaseStream.Length;
          sw.WriteLine($@"{town};{build.Street};{build.Number};{build.Building};{build.CountRoom};{build.Square};{build.Price};{ build.Floor};{build.Metro};{build.Distance};{districtName}");
        }
        Monitor.Exit(locker);
      }
    }

    public void GetInfoAboutBuilding()
    {
      if (File.Exists(Filename))
      {
        using (var sr = new StreamReader(Filename, Encoding.UTF8))
        {
          using (var sw = new StreamWriter(FilenameWithinfo, true, Encoding.UTF8))
          {
            using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
            {
              connection.Open();

              sw.WriteLine($@"Район;Улица;Номер;Корпус;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Расстояние;Дата постройки;Дата реконструкции;Даты кап. ремонты;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Кол-во встроенных нежилых помещений;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов");
              string line;
              sr.ReadLine();
              while ((line = sr.ReadLine()) != null)
              {
                string street = string.Empty;
                string number = string.Empty;
                string building = string.Empty;
                string letter = string.Empty;
                string typeRoom = string.Empty;
                string square = string.Empty;
                string floor = string.Empty;
                string countFloor = string.Empty;
                string price = string.Empty;
                string metro = string.Empty;
                string distance = string.Empty;
                string dateBuild = string.Empty;
                string dateRecon = string.Empty;
                string dateRepair = string.Empty;
                string buildingSquare = string.Empty;
                string livingSquare = string.Empty;
                string noLivingSqaure = string.Empty;
                string residents = string.Empty;
                string mansardaSquare = string.Empty;
                string otoplenie = string.Empty;
                string gvs = string.Empty;
                string es = string.Empty;
                string gs = string.Empty;
                string typeApartaments = string.Empty;
                string countApartaments = string.Empty;
                string countInternal = string.Empty;
                DateTime dateTep = DateTime.Now;
                string typeRepair = string.Empty;
                string countLift = string.Empty;
                string district = string.Empty;

                var arr = line.Split(';');
                street = arr[1];
                number = arr[2];
                building = arr[3];
                typeRoom = arr[4];
                square = arr[5];
                price = arr[6];
                floor = arr[7];
                metro = arr[8];
                distance = arr[9];
                district = arr[10];

                string select = "";
                if (string.IsNullOrWhiteSpace(letter))
                {
                  if (string.IsNullOrWhiteSpace(building))
                  {
                    select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumber '{street}', '{number}'";
                  }
                  else
                  {
                    select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumberAndBuilbind '{street}', '{number}', '{building}'";
                  }
                }
                else
                {
                  if (string.IsNullOrWhiteSpace(building))
                  {
                    select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumberAndLetter '{street}', '{number}', '{letter}'";
                  }
                  else
                  {
                    select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumberAndBuilbindAndLetter '{street}', '{number}', '{building}', '{letter}'";
                  }
                }

                var command = new SqlCommand(select, connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                  dateBuild = reader.GetString(1);
                  dateRecon = reader.GetString(3);
                  dateRepair = reader.GetString(4);
                  buildingSquare = reader.GetDouble(5).ToString();
                  livingSquare = reader.GetDouble(6).ToString();
                  noLivingSqaure = reader.GetDouble(7).ToString();
                  countFloor = reader.GetInt32(9).ToString();
                  residents = reader.GetInt32(10).ToString();
                  mansardaSquare = reader.GetDouble(11).ToString();
                  otoplenie = reader.GetBoolean(12).ToString();
                  gvs = reader.GetBoolean(13).ToString();
                  es = reader.GetBoolean(14).ToString();
                  gs = reader.GetBoolean(15).ToString();
                  typeApartaments = reader.GetString(16);
                  countApartaments = reader.GetString(17);
                  countInternal = reader.GetInt32(18).ToString();
                  dateTep = reader.GetDateTime(19);
                  typeRepair = reader.GetString(21);
                  countLift = reader.GetInt32(22).ToString();
                }
                reader.Close();

                sw.WriteLine($@"{district};{street};{number};{building};{typeRoom};{square};{floor};{countFloor};{price};{metro};{distance};{dateBuild};{dateRecon};{dateRepair};{buildingSquare};{livingSquare};{noLivingSqaure};{mansardaSquare};{residents};{otoplenie};{gvs};{es};{gs};{typeApartaments};{countApartaments};{countInternal};{dateTep.ToShortDateString()};{typeRepair};{countLift}");
              }
            }
          }
        }
      }
      else
      {
        MessageBox.Show("Нет файла с данными");
      }
    }
  }
}
