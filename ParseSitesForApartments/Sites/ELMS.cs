using AngleSharp.Dom;
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
  public class ELMS : BaseParse
  {
    private List<int> listDistrict = new List<int>() { 38, 12, 43, 13, 4, 20, 6, 14, 7, 15, 8, 39, 9 };

    private Dictionary<int, string> district = new Dictionary<int, string>() { { 38, "Адмиралтейский" }, { 43, "Василеостровский" }, { 4, "Выборгский" }, { 6, "Калининский" }, { 7, "Кировский" }, { 9, "Красногвардейский" }, { 8, "Красносельский" }, { 12, "Московский" }, { 13, "Невский" }, { 20, "Петроградский" }, { 14, "Приморский" }, { 15, "Фрунзенский" }, { 39, "Центральный" }, };

    private List<Flat> listBuild = new List<Flat>();

    private const string Filename = @"D:\ElmsProdam.csv";
    private const string FilenameSdam = @"D:\ElmsSdam.csv";
    private const string FilenameWithinfo = @"D:\ElmsProdamWithInfo.csv";
    static object locker = new object();
    private const int MaxPage = 20;

    public override void ParsingAll()
    {
      using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Create), Encoding.UTF8))
      {
        sw.WriteLine($@"Нас. пункт;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние;Район");
      }
      var studiiThread = new Thread(ParseStudii);
      studiiThread.Start();
      Thread.Sleep(55000);
      var oneThread = new Thread(ParseOneRoom);
      oneThread.Start();
      Thread.Sleep(55000);
      var twoThread = new Thread(ParseTwoRoom);
      twoThread.Start();
      Thread.Sleep(55000);
      var threeThread = new Thread(ParseThreeRoom);
      threeThread.Start();
      Thread.Sleep(55000);
      var fourThread = new Thread(ParseFourRoom);
      fourThread.Start();
      Thread.Sleep(55000);
      var fiveThread = new Thread(ParseFiveRoom);
      fiveThread.Start();
      Thread.Sleep(55000);

      var studiiNovThread = new Thread(ParsingStudiiNov);
      studiiNovThread.Start();
      Thread.Sleep(55000);
      var oneNovThread = new Thread(ParsingOneNov);
      oneNovThread.Start();
      Thread.Sleep(55000);
      var twoNovThread = new Thread(ParsingTwoNov);
      twoNovThread.Start();
      Thread.Sleep(55000);
      var threeNovThread = new Thread(ParsingThreeNov);
      threeNovThread.Start();
      Thread.Sleep(55000);
      var fourNovThread = new Thread(ParsingFourNov);
      fourNovThread.Start();
      Thread.Sleep(55000);
      var fiveNovThread = new Thread(ParsingFiveNov);
      fiveNovThread.Start();
    }

    public void ParseStudii()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r0/1/is_auction/2/place/address/reg/2/dept/2/dist/{item.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              else
                ParseSheet(tableElements, "Студия", item.Value);
            }
          }
      }
        catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
      MessageBox.Show("Закончили студии");
    }

    public void ParseOneRoom()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r1/1/is_auction/2/place/address/reg/2/dept/2/dist/{item.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              else
                ParseSheet(tableElements, "1 км. кв.", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 1 км. кв.");
    }

    public void ParseTwoRoom()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r2/1/is_auction/2/place/address/reg/2/dept/2/dist/{item.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              else
                ParseSheet(tableElements, "2 км. кв.", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 2 км. кв.");
    }

    public void ParseThreeRoom()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r3/1/is_auction/2/place/address/reg/2/dept/2/dist/{item.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              else
                ParseSheet(tableElements, "3 км. кв.", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 3 км. кв.");
    }

    public void ParseFourRoom()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r4/1/is_auction/2/place/address/reg/2/dept/2/dist/{item.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              else
                ParseSheet(tableElements, "4 км. кв.", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 4 км. кв.");
    }

    public void ParseFiveRoom()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r5/1/is_auction/2/place/address/reg/2/dept/2/dist/{item.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              else
                ParseSheet(tableElements, "5 км. кв.", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 5 км. кв.");
    }

    private void ParseSheet(IHtmlCollection<IElement> collection, string typeRoom, string district)
    {
      for (int i = 0; i < collection.Length; i++)
      {
        var flat = new Flat
        {
          CountRoom = typeRoom
        };
        try
        {
          if (collection[i].GetElementsByClassName("w-image").Length > 0)
          {
            var divImage = collection[i].GetElementsByClassName("w-image")[0];
            var square = collection[i].GetElementsByClassName("space-all");
            if (square.Length > 0)
              flat.Square = square[0].TextContent;

            if (collection[i].GetElementsByClassName("address-geo").Length > 0)
            {
              var adr = collection[i].GetElementsByClassName("address-geo")[0].TextContent.Split(',');
              if (adr.Length == 3)
              {
                flat.Building.Street = adr[0] + " " + adr[1];
                flat.Building.Number = adr[2];
              }
              else
              {
                flat.Building.Street = adr[0];
                if (adr.Length > 1)
                  flat.Building.Number = adr[1].Trim();
              }
            }
            var regex = new Regex(@"(к\d+)");
            flat.Building.Structure = regex.Match(flat.Building.Number).Value;
            if (!string.IsNullOrEmpty(flat.Building.Structure))
            {
              flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "");
              flat.Building.Structure = flat.Building.Structure.Replace("к", "");
            }
            regex = new Regex(@"(\D)");
            flat.Building.Liter = regex.Match(flat.Building.Number).Value;
            if (!string.IsNullOrEmpty(flat.Building.Liter))
              flat.Building.Number = flat.Building.Number.Replace(flat.Building.Liter, "");


            var metro = collection[i].GetElementsByClassName("metroline-2");
            if (metro.Length > 0)
              flat.Building.Metro = metro[0].TextContent;

            regex = new Regex(@"(\d+)");
            var floor = collection[i].GetElementsByClassName("w-floor");
            if (floor.Length > 0)
            {
              var ms = regex.Matches(floor[0].TextContent);
              if (ms.Count > 0)
                flat.Floor = ms[0].Value;
            }

            regex = new Regex(@"(\d+\s+\d+\s+метров)|(\d+\s+метров)");
            var distance = collection[i].GetElementsByClassName("ellipsis em");
            if (distance.Length > 0)
            {
              flat.Building.Distance = regex.Match(distance[0].TextContent.Replace("\n", "").Trim()).Value;
            }

            var pr = collection[i].GetElementsByClassName("price");
            if (pr.Length > 0)
            {
              string priceStr = pr[0].TextContent.Replace(" a", "").Replace(" ", "");
              int price;
              if (int.TryParse(priceStr, out price))
              {
                flat.Price = price;
              }
            }

            string town = string.Empty;
            if (flat.Building.Street.Contains("(Горелово)"))
            {
              town = "Горелово";
              flat.Building.Street = flat.Building.Street.Replace("(Горелово)", "");
            }
            else if (flat.Building.Street.Contains("Красное Село"))
            {
              town = "Красное Село";
              flat.Building.Street = flat.Building.Street.Replace(town, "");
            }
            else if (flat.Building.Street.Contains("Парголово"))
            {
              town = "Парголово";
              flat.Building.Street = flat.Building.Street.Replace(town, "");
            }
            else
              town = "Санкт-Петербург";


            flat.Building.Street = flat.Building.Street.Replace("ул.", "").Replace("ал.", "").Replace("бул.", "").Replace("ш.", "").Replace("пр.", "").Replace("пер.", "").Replace("пр-д", "").Replace(" б", "").Trim();

            Monitor.Enter(locker);
            bool flag = false;
            foreach (var bl in listBuild)
            {
              if (flat.Building.Equals(bl))
              {
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              if (!string.IsNullOrEmpty(flat.Building.Number))
              {
                listBuild.Add(flat);

                using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Open), Encoding.UTF8))
                {
                  sw.BaseStream.Position = sw.BaseStream.Length;
                  sw.WriteLine($@"{town};{flat.Building.Street};{flat.Building.Number};{flat.Building.Structure};{flat.Building.Liter};{flat.CountRoom};{flat.Square};{flat.Price};{flat.Floor};{flat.Building.Metro};{flat.Building.Distance};{district}");
                }
              }
            }
            Monitor.Exit(locker);
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
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
                letter = arr[4];
                typeRoom = arr[5];
                square = arr[6];
                price = arr[7];
                floor = arr[8];
                metro = arr[9];
                distance = arr[10];
                district = arr[11];

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

    public void ParsingStudiiNov()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/new/page{j}.html?query=s/1/dist/{item.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{item.Key}/by_room/1";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              else
                ParseSheetNov(tableElements, "Студия", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили студии");
    }
    public void ParsingOneNov()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/new/page{j}.html?query=s/1/dist/{item.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{item.Key}/r1/1/by_room/1";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              else
                ParseSheetNov(tableElements, "1 км. кв.", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 1 км. кв. нов.");
    }
    public void ParsingTwoNov()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/new/page{j}.html?query=s/1/dist/{item.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{item.Key}/r2/1/by_room/1";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              else
                ParseSheetNov(tableElements, "2 км. кв.", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 2 км. кв. нов.");
    }
    public void ParsingThreeNov()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/new/page{j}.html?query=s/1/dist/{item.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{item.Key}/r3/1/by_room/1";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              else
                ParseSheetNov(tableElements, "3 км. кв.", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 3 км. кв. нов.");
    }
    public void ParsingFourNov()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/new/page{j}.html?query=s/1/dist/{item.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{item.Key}/r4/1/by_room/1";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              else
                ParseSheetNov(tableElements, "4 км. кв.", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 4 км. кв. нов.");
    }
    public void ParsingFiveNov()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/new/page{j}.html?query=s/1/dist/{item.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{item.Key}/r5/1/by_room/1";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              else
                ParseSheetNov(tableElements, "5 км. кв.", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 5 км. кв. нов.");
    }

    private void ParseSheetNov(IHtmlCollection<IElement> collection, string typeRoom, string district)
    {
      for (int i = 0; i < collection.Length; i++)
      {
        var listFlat = new List<Flat>();

        string street = "";
        string number = "";
        string building = "";
        string liter = "";
        string metro = "";
        string distance = "";

        #region Адрес

        if (collection[i].GetElementsByClassName("address-geo").Length > 1)
        {
          var adr = collection[i].GetElementsByClassName("address-geo")[1].TextContent.Split(',');
          if (adr.Length == 3)
          {
            street = adr[0] + " " + adr[1];
            number = adr[2];
          }
          else
          {
            street = adr[0];
            if (adr.Length > 1)
              number = adr[1].Trim();
          }
        }
        var regex = new Regex(@"(к\d+)");
        building = regex.Match(number).Value;
        if (!string.IsNullOrEmpty(building))
        {
          number = number.Replace(building, "");
          building = building.Replace("к", "");
        }
        regex = new Regex(@"(\D)");
        liter = regex.Match(number).Value;
        if (!string.IsNullOrEmpty(liter))
          number = number.Replace(liter, "");

        var met = collection[i].GetElementsByClassName("metroline-2");
        if (met.Length > 0)
          metro = met[0].TextContent;

        regex = new Regex(@"(\d+\s+\d+\s+метров)|(\d+\s+метров)");
        var dis = collection[i].GetElementsByClassName("ellipsis em");
        if (dis.Length > 0)
        {
          distance = dis[0].TextContent.Replace("\n", "").Trim();
        }
        if (!string.IsNullOrEmpty(distance))
          metro = metro.Replace(distance, "").Trim();
        #endregion

        var pr = collection[i].GetElementsByClassName("price");
        if (pr.Length > 0)
        {
          string priceStr = pr[0].TextContent.Replace(" a", "").Replace(" ", "");
          if (!string.IsNullOrEmpty(priceStr))
          {
            var flat = new Flat
            {
              CountRoom = typeRoom,
              Building = new Building()
              {
                Street = street,
                Number = number,
                Liter = liter,
                Metro = metro,
                Distance = distance,
                Structure = building
              }
            };

            int price;
            if (int.TryParse(priceStr, out price))
            {
              flat.Price = price;
            }

            if (collection[i].GetElementsByClassName("w-image").Length > 0)
            {
              var divImage = collection[i].GetElementsByClassName("w-image")[0];
              var square = collection[i].GetElementsByClassName("space-all");
              if (square.Length > 0)
                flat.Square = square[0].TextContent;
            }

            regex = new Regex(@"(\d+)");
            var floor = collection[i].GetElementsByClassName("w-floor");
            if (floor.Length > 0)
            {
              var ms = regex.Matches(floor[0].TextContent);
              if (ms.Count > 0)
                flat.Floor = ms[0].Value;
            }
            listFlat.Add(flat);
          }
          else
          {
            var rows = collection[i].GetElementsByClassName("w-kv-row");
            for (int j = 0; j < rows.Length; j++)
            {
              var flat = new Flat
              {
                CountRoom = typeRoom,
                Building = new Building()
                {
                  Street = street,
                  Number = number,
                  Liter = liter,
                  Metro = metro,
                  Distance = distance,
                  Structure = building
                }
              };
              var floor = rows[j].GetElementsByClassName("circle-floor");
              if (floor.Length > 0)
                flat.Floor = floor[0].TextContent.Trim();
              var sq = rows[j].GetElementsByClassName("w-kv-area");
              if (sq.Length > 0)
                flat.Square = sq[0].TextContent.Trim();
              var price = rows[j].GetElementsByClassName("w-kv-price");
              if (price.Length > 0)
              {
                regex = new Regex(@"(\d+\s+\d+\s+\d+)");
                priceStr = regex.Match(price[0].TextContent).Value.Replace(" ", "");
                int pri;
                if (int.TryParse(priceStr, out pri))
                {
                  flat.Price = pri;
                }
              }
              listFlat.Add(flat);
            }
          }
        }

        foreach (var item in listFlat)
        {
          string town = string.Empty;
          if (item.Building.Street.Contains("(Горелово)"))
          {
            town = "Горелово";
            item.Building.Street = item.Building.Street.Replace("(Горелово)", "");
          }
          else if (item.Building.Street.Contains("Красное Село"))
          {
            town = "Красное Село";
            item.Building.Street = item.Building.Street.Replace(town, "");
          }
          else if (item.Building.Street.Contains("Парголово"))
          {
            town = "Парголово";
            item.Building.Street = item.Building.Street.Replace(town, "");
          }
          else
            town = "Санкт-Петербург";


          item.Building.Street = item.Building.Street.Replace("ул.", "").Replace("ал.", "").Replace("бул.", "").Replace("ш.", "").Replace("пр.", "").Replace("пер.", "").Replace("пр-д", "").Replace(" б", "").Trim();
          if (!string.IsNullOrEmpty(item.Building.Number))
          {
            Monitor.Enter(locker);
            bool flag = false;
            foreach (var bl in listBuild)
            {
              if (item.Equals(bl))
              {
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              listBuild.Add(item);
              using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Open), Encoding.UTF8))
              {
                sw.BaseStream.Position = sw.BaseStream.Length;
                sw.WriteLine($@"{town};{item.Building.Street};{item.Building.Number};{item.Building};{item.Building.Liter};{item.CountRoom};{item.Square};{item.Price};{item.Floor};{item.Building.Metro};{item.Building.Distance};{district}");
              }
            }
            Monitor.Exit(locker);
          }
        }
      }
    }

    public override void ParsingSdamAll()
    {
      using (var sw = new StreamWriter(new FileStream(FilenameSdam, FileMode.Create), Encoding.UTF8))
      {
        sw.WriteLine($@"Нас. пункт;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние");
      }

      var studiiThread = new Thread(ParseStudiiSdam);
      studiiThread.Start();
      Thread.Sleep(30000);
      var oneThread = new Thread(ParseOneSdam);
      oneThread.Start();
      Thread.Sleep(30000);
      var twoThread = new Thread(ParseTwoSdam);
      twoThread.Start();
      Thread.Sleep(30000);
      var threeThread = new Thread(ParseThreeSdam);
      threeThread.Start();
      Thread.Sleep(30000);
      var fourThread = new Thread(ParseFourSdam);
      fourThread.Start();
      Thread.Sleep(30000);
      var fiveThread = new Thread(ParseFiveSdam);
      fiveThread.Start();
    }

    public void ParseStudiiSdam()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          for (int j = 1; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/arenda/page{j}.html?query=s/1/r0/1/type/2/rtype/2/place/address/reg/2/dept/2/sort1/4/dir1/1/dir2/2/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheetSdam(tableElements, "Студия");
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили студии сдам");
    }
    public void ParseOneSdam()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          for (int j = 1; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/arenda/page{j}.html?query=s/1/r1/1/type/2/rtype/2/place/address/reg/2/dept/2/sort1/4/dir1/1/dir2/2/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheetSdam(tableElements, "1 км. кв.");
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 1 км. кв. сдам");
    }
    public void ParseTwoSdam()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          for (int j = 1; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/arenda/page{j}.html?query=s/1/r2/1/type/2/rtype/2/place/address/reg/2/dept/2/sort1/4/dir1/1/dir2/2/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheetSdam(tableElements, "2 км. кв.");
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 2 км. кв. сдам");
    }
    public void ParseThreeSdam()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          for (int j = 1; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/arenda/page{j}.html?query=s/1/r3/1/type/2/rtype/2/place/address/reg/2/dept/2/sort1/4/dir1/1/dir2/2/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheetSdam(tableElements, "3 км. кв.");
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 3 км. кв. сдам");
    }
    public void ParseFourSdam()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          for (int j = 1; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/arenda/page{j}.html?query=s/1/r4/1/type/2/rtype/2/place/address/reg/2/dept/2/sort1/4/dir1/1/dir2/2/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheetSdam(tableElements, "4 км. кв.");
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 4 км. кв. сдам");
    }
    public void ParseFiveSdam()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          for (int j = 1; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/arenda/page{j}.html?query=s/1/r5/1/type/2/rtype/2/place/address/reg/2/dept/2/sort1/4/dir1/1/dir2/2/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheetSdam(tableElements, "5 км. кв.");
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 5 км. кв. сдам");
    }


    private void ParseSheetSdam(IHtmlCollection<IElement> collection, string typeRoom)
    {
      for (int i = 0; i < collection.Length; i++)
      {
        var flat = new Flat
        {
          CountRoom = typeRoom
        };
        try
        {
          if (collection[i].GetElementsByClassName("w-image").Length > 0)
          {
            var divImage = collection[i].GetElementsByClassName("w-image")[0];
            var square = collection[i].GetElementsByClassName("space-all");
            if (square.Length > 0)
              flat.Square = square[0].TextContent;

            if (collection[i].GetElementsByClassName("address-geo").Length > 0)
            {
              var adr = collection[i].GetElementsByClassName("address-geo")[0].TextContent.Split(',');
              if (adr.Length == 3)
              {
                flat.Building.Street = adr[0] + " " + adr[1];
                flat.Building.Number = adr[2];
              }
              else
              {
                flat.Building.Street = adr[0];
                if (adr.Length > 1)
                  flat.Building.Number = adr[1].Trim();
              }
            }
            var regex = new Regex(@"(к\d+)");
            flat.Building.Structure = regex.Match(flat.Building.Number).Value;
            if (!string.IsNullOrEmpty(flat.Building.Structure))
            {
              flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "");
              flat.Building.Structure = flat.Building.Structure.Replace("к", "");
            }
            regex = new Regex(@"(\D)");
            flat.Building.Liter = regex.Match(flat.Building.Number).Value;
            if (!string.IsNullOrEmpty(flat.Building.Liter))
              flat.Building.Number = flat.Building.Number.Replace(flat.Building.Liter, "");


            var metro = collection[i].GetElementsByClassName("metroline-2");
            if (metro.Length > 0)
              flat.Building.Metro = metro[0].TextContent;

            regex = new Regex(@"(\d+)");
            var floor = collection[i].GetElementsByClassName("w-floor");
            if (floor.Length > 0)
            {
              var ms = regex.Matches(floor[0].TextContent);
              if (ms.Count > 0)
                flat.Floor = ms[0].Value;
            }

            regex = new Regex(@"(\d+\s+\d+\s+метров)|(\d+\s+метров)");
            var distance = collection[i].GetElementsByClassName("ellipsis em");
            if (distance.Length > 0)
            {
              flat.Building.Distance = regex.Match(distance[0].TextContent.Replace("\n", "").Trim()).Value;
            }

            var pr = collection[i].GetElementsByClassName("price");
            if (pr.Length > 0)
            {
              string priceStr = pr[0].TextContent.Replace(" a", "").Replace("a/мес", "").Replace(" ", "").Trim();
              int price;
              if (int.TryParse(priceStr, out price))
              {
                flat.Price = price;
              }
            }

            string town = string.Empty;
            if (flat.Building.Street.Contains("(Горелово)"))
            {
              town = "Горелово";
              flat.Building.Street = flat.Building.Street.Replace("(Горелово)", "");
            }
            else if (flat.Building.Street.Contains("Красное Село"))
            {
              town = "Красное Село";
              flat.Building.Street = flat.Building.Street.Replace(town, "");
            }
            else if (flat.Building.Street.Contains("Парголово"))
            {
              town = "Парголово";
              flat.Building.Street = flat.Building.Street.Replace(town, "");
            }
            else
              town = "Санкт-Петербург";


            flat.Building.Street = flat.Building.Street.Replace("ул.", "").Replace("ал.", "").Replace("бул.", "").Replace("ш.", "").Replace("пр.", "").Replace("пер.", "").Replace("пр-д", "").Replace(" б", "").Trim();

            Monitor.Enter(locker);
            bool flag = false;
            foreach (var bl in listBuild)
            {
              if (flat.Equals(bl))
              {
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              if (!string.IsNullOrEmpty(flat.Building.Number))
              {
                listBuild.Add(flat);

                using (var sw = new StreamWriter(new FileStream(FilenameSdam, FileMode.Open), Encoding.UTF8))
                {
                  sw.BaseStream.Position = sw.BaseStream.Length;
                  sw.WriteLine($@"{town};{flat.Building.Street};{flat.Building.Number};{flat.Building};{flat.Building.Liter};{flat.CountRoom};{flat.Square};{flat.Price};{flat.Floor};{flat.Building.Metro};{flat.Building.Distance}");
                }
              }
            }
            Monitor.Exit(locker);
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
      }
    }
  }
}
