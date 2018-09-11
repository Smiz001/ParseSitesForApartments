using AngleSharp.Parser.Html;
using ParseSitesForApartments.Sites;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace ParseSitesForApartments
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }
    private int pageMin = 1;
    private int pageMaz = 100;
    private void button1_Click(object sender, EventArgs e)
    {
      var avito = new Avito();
      avito.ParsingAll();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      /*
      using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
      {
        connection.Open();
        /*
        using (var sr = new StreamReader(@"D:\BuldingData.csv"))
        {
          string line;
          sr.ReadLine();
          while ((line = sr.ReadLine()) != null)
          {
            var arr = line.Split(';');
            string street = arr[0];
            string number = arr[1];
            string bulding = arr[2];
            string district = arr[3];
            string buldingDate = arr[4];
            string repairDate = arr[5];

            string insert = $@"insert into dbo.InfoAboutBulding (District, Street, Number, Bulding, BuldingDate, RepairDate)
values('{district}','{street}','{number}','{bulding}','{buldingDate}','{repairDate}')";

            var command = new SqlCommand(insert, connection);
            command.ExecuteNonQuery();
          }
        }
        */
      /*
      using (var sr = new StreamReader(@"D:\district.txt"))
      {
        string line;
        while ((line = sr.ReadLine()) != null)
        {
          string insert = $@"insert into ParseBulding.dbo.District (ID, Name)
values (newid(), '{line}')";
          var command = new SqlCommand(insert, connection);
          command.ExecuteNonQuery();
        }
      }
      */
      /*
      using (var sr = new StreamReader(@"D:\Manager Company.txt"))
      {
        string line;
        while ((line = sr.ReadLine()) != null)
        {
          string insert = $@"insert into ParseBulding.dbo.ManageCompany (ID, Name)
values (newid(), '{line}')";
          var command = new SqlCommand(insert, connection);
          command.ExecuteNonQuery();
        }
      }
      */
      /*
      using (var sr = new StreamReader(@"D:\Series.txt"))
      {
        string line;
        while ((line = sr.ReadLine()) != null)
        {
          string insert = $@"insert into ParseBulding.dbo.Series (ID, Name)
values (newid(), '{line}')";
          var command = new SqlCommand(insert, connection);
          command.ExecuteNonQuery();
        }
      }

      using (var sr = new StreamReader(@"D:\data1.csv"))
      {
        string line;
        sr.ReadLine();
        string number = string.Empty;
        string bulding = string.Empty;
        string letter = string.Empty;
        Guid district;
        string dateBulding = string.Empty;
        Guid series;
        int countCommApartament = 0;
        string dateReconstruct = string.Empty;
        string dateRepair = string.Empty;
        float buldingArea;
        float livingArea;
        float noLivingArea;
        int stairs;
        int storeys;
        int residents;
        float mansardArea;
        short heatingCentral;
        short electroCentral;
        short gascCntral;
        string flatType = string.Empty;
        string flatNum = string.Empty;
        int internalNum;
        DateTime tepCreateDate;
        Guid managCompanyId;
        short failure;
        string repairJob = string.Empty;
        int liftCount;
        float basementArea;


        while ((line = sr.ReadLine()) != null)
        {
          var ar = line.Split(';');

          if (!string.IsNullOrEmpty(ar[5]))
          {
            var ar1 = ar[5].Split(',');
            foreach (var item in ar1)
            {
              countCommApartament += int.Parse(item);
            }
          }
          string select = $@"SELECT [ID]
FROM [ParseBulding].[dbo].[District]
WHERE Name = '{ar[4]}'";
          var command = new SqlCommand(select, connection);
          district = Guid.Parse(command.ExecuteScalar().ToString());

          var manageName = ar[24].Replace("\"", "");
          select = $@"SELECT [Id]
FROM [ParseBulding].[dbo].[ManageCompany]
WHERE NAME = '{manageName}'";
          command = new SqlCommand(select, connection);
          managCompanyId = Guid.Parse(command.ExecuteScalar().ToString());

          select = $@"SELECT [Id]
FROM [ParseBulding].[dbo].[Series]
WHERE NAME ='{ar[6]}'";
          command = new SqlCommand(select, connection);
          series = Guid.Parse(command.ExecuteScalar().ToString());

          string insert = $@"INSERT INTO [dbo].[MainInfoAboutBulding]
         ([Id]
         ,[Street]
         ,[Number]
         ,[Bulding]
         ,[Letter]
         ,[DistrictId]
         ,[DateBulding]
         ,[SeriesID]
         ,[CountCommApartament]
         ,[DateReconstruct]
         ,[DateRepair]
         ,[BuldingArea]
         ,[LivingArea]
         ,[NoLivingArea]
         ,[Stairs]
         ,[Storeys]
         ,[Residents]
         ,[MansardArea]
         ,[HeatingCentral]
         ,[HotWaterCentral]
         ,[ElectroCentral]
         ,[GascCntral]
         ,[FlatType]
         ,[FlatNum]
         ,[InternalNum]
         ,[TepCreateDate]
         ,[ManagCompanyId]
         ,[Failure]
         ,[RepairJob]
         ,[LiftCount])
   VALUES
         (newid()
         ,'{ar[0]}'
         ,'{ar[1]}'
         ,'{ar[2]}'
         ,'{ar[3]}'
         ,'{district}'
         ,'{ar[7]}'
         ,'{series}'
         ,{countCommApartament}
         ,'{ar[8]}'
         ,'{ar[26]}'
         ,{(string.IsNullOrEmpty(ar[9]) ? 0.ToString() : ar[9])}
         ,{(string.IsNullOrEmpty(ar[10]) ? 0.ToString() : ar[10])}
         ,{(string.IsNullOrEmpty(ar[11]) ? 0.ToString() : ar[11])}
         ,{(string.IsNullOrEmpty(ar[12]) ? 0.ToString() : ar[12])}
         ,{(string.IsNullOrEmpty(ar[13]) ? 0.ToString() : ar[13])}
         ,{(string.IsNullOrEmpty(ar[14]) ? 0.ToString() : ar[14])}
         ,{(string.IsNullOrEmpty(ar[15]) ? 0.ToString() : ar[15])}
         ,{(string.IsNullOrEmpty(ar[16]) ? 0.ToString() : ar[16])}
         ,{(string.IsNullOrEmpty(ar[17]) ? 0.ToString() : ar[17])}
         ,{(string.IsNullOrEmpty(ar[18]) ? 0.ToString() : ar[18])}
         ,{(string.IsNullOrEmpty(ar[19]) ? 0.ToString() : ar[19])}
         ,'{ar[20]}'
         ,'{ar[21]}'
         ,{(string.IsNullOrEmpty(ar[22]) ? 0.ToString() : ar[22])}
         ,'{ar[23]}'
         ,'{managCompanyId}'
         ,{(string.IsNullOrEmpty(ar[25]) ? 0.ToString() : ar[25])}
         ,'{ar[27]}'
         ,{(string.IsNullOrEmpty(ar[28]) ? 0.ToString() : ar[28])})";

          command = new SqlCommand(insert, connection);
          command.ExecuteNonQuery();
        }
      }
    }
  */
    }

    private void button3_Click(object sender, EventArgs e)
    {
      var avito = new Avito();
      avito.ParsingSdamAll();
    }

    private void button4_Click(object sender, EventArgs e)
    {
      int minPage = 1;
      int maxPage = 5;
      using (var webClient = new WebClient())
      {
        using (var sw = new StreamWriter(@"D:\AvitoKupluy.csv", true, System.Text.Encoding.UTF8))
        {
          Random random = new Random();
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(5000, 10000));
            string sdam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/kuplyu?p={i}";

            webClient.Encoding = System.Text.Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var elem = document.GetElementsByClassName("item_table-header");
            var adresses = document.GetElementsByClassName("address");
            for (int k = 0; k < elem.Length; k++)
            {
              var price = (elem[k].GetElementsByClassName("price")[0].TextContent.Trim('\n').Trim('₽').Trim().Replace(" ", "").Replace("\n", " "));

              var adress = adresses[k];
              string metro = string.Empty;
              string distance = string.Empty;
              if (adress.ChildNodes.Length > 1)
                metro = adress.ChildNodes[2].NodeValue.Trim();
              if (adresses[k].GetElementsByClassName("c-2").Length > 0)
                distance = adresses[k].GetElementsByClassName("c-2")[0].TextContent;

              var typeBuy = elem[k].GetElementsByTagName("span")[0].TextContent;

              sw.WriteLine($@"{typeBuy};{metro};{distance};{price}");
            }
          }
        }
      }
    }

    private void button5_Click(object sender, EventArgs e)
    {
      int minPage = 1;
      int maxPage = 10;

      using (var webClient = new WebClient())
      {
        using (var sw = new StreamWriter(@"D:\AvitoSnimu.csv", true, System.Text.Encoding.UTF8))
        {
          Random random = new Random();
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(5000, 10000));
            string sdam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/snimu?p={i}";

            webClient.Encoding = System.Text.Encoding.UTF8;
            var responce = webClient.DownloadString(sdam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var elem = document.GetElementsByClassName("item_table-header");
            var adresses = document.GetElementsByClassName("address");
            for (int k = 0; k < elem.Length; k++)
            {
              var price = (elem[k].GetElementsByClassName("price")[0].TextContent.Trim('\n').Trim('₽').Trim().Replace(" ", "").Replace("\n", " "));

              string metro = string.Empty;
              string distance = string.Empty;
              string typeBuy = string.Empty;
              if (adresses.Length >= elem.Length)
              {
                var adress = adresses[k];
                if (adress.ChildNodes.Length > 1)
                  metro = adress.ChildNodes[2].NodeValue.Trim();
                if (adresses[k].GetElementsByClassName("c-2").Length > 0)
                  distance = adresses[k].GetElementsByClassName("c-2")[0].TextContent;

                typeBuy = elem[k].GetElementsByTagName("span")[0].TextContent;
              }
              sw.WriteLine($@"{typeBuy};{metro};{distance};{price}");
            }
          }
        }
      }
    }

    private void button6_Click(object sender, EventArgs e)
    {
      int minPage = 1;
      int maxPage = 1500;
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
        {
          connection.Open();
          using (var sw = new StreamWriter(@"D:\YandexProdam.csv", true, System.Text.Encoding.UTF8))
          {
            for (int i = minPage; i < maxPage; i++)
            {
              Thread.Sleep(random.Next(5000, 15000));
              string sdam = $@"https://realty.yandex.ru/sankt-peterburg/kupit/kvartira/?page={i}";
              webClient.Encoding = System.Text.Encoding.UTF8;
              var responce = webClient.DownloadString(sdam);


              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var elem = document.GetElementsByClassName("OffersSerpItem OffersSerp__list-item OffersSerp__list-item_type_offer");

              string rooms = string.Empty;
              string square = string.Empty;
              string year = string.Empty;
              string price = string.Empty;
              string floor = string.Empty;
              string street = string.Empty;
              string number = string.Empty;
              string metro = string.Empty;
              string distanceInMinute = string.Empty;

              for (int j = 0; j < elem.Length; j++)
              {
                var div = elem[i].GetElementsByClassName("OffersSerpItem__main")[0];
                var txt = div.GetElementsByClassName("OffersSerpItem__title")[0].TextContent;
                var countRoomAndSquare = div.GetElementsByClassName("OffersSerpItem__title")[0].TextContent.Split(',');
                square = countRoomAndSquare[0];
                rooms = countRoomAndSquare[1];
                var yearAndFloor = div.GetElementsByClassName("OffersSerpItem__building")[0].TextContent.Trim().Split(',');
                if (yearAndFloor.Length == 2)
                {
                  year = yearAndFloor[0];
                  floor = yearAndFloor[1];
                }
                else if (yearAndFloor.Length == 3)
                {
                  year = yearAndFloor[0];
                  floor = yearAndFloor[3];
                }
                else if (yearAndFloor.Length == 1)
                {
                  floor = yearAndFloor[0];
                }
                var streetAndNumber = div.GetElementsByClassName("OffersSerpItem__address")[0].TextContent.Trim().Split(',');
                if (streetAndNumber.Length == 2)
                {
                  street = streetAndNumber[0];
                  number = streetAndNumber[1];
                }
                metro = div.GetElementsByClassName("MetroStation__title")[0].TextContent;
                distanceInMinute = div.GetElementsByClassName("MetroWithTime__distance")[0].TextContent;

                price = elem[i].GetElementsByClassName("price")[0].TextContent;

                sw.WriteLine($@"{street};{number};{rooms};{square};{price};{floor};{year};{metro};{distanceInMinute}");

                Thread.Sleep(random.Next(1000, 5000));
                if (i % 2 == 0)
                {
                  sdam = $@"https://realty.yandex.ru/sankt-peterburg/kupit/kvartira/?page={i}";
                  webClient.Encoding = System.Text.Encoding.UTF8;
                  responce = webClient.DownloadString(sdam);
                }
              }
            }
          }
        }
      }
    }

    private void button7_Click(object sender, EventArgs e)
    {
      var bkn = new BKN();
      bkn.ParsingAll();
    }

    private void button8_Click(object sender, EventArgs e)
    {
      var bn = new BN();
      bn.ParsingAll();
    }

    private void button9_Click(object sender, EventArgs e)
    {
      var elms = new ELMS();
      elms.ParsingAll();
    }

    private void button10_Click(object sender, EventArgs e)
    {
      /*
      var yandex = new Yandex();
      using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
      {
        connection.Open();
        string select = $@"select [Street],[Number],[Bulding],[Letter], id 
from [dbo].[MainInfoAboutBulding]
where Xcoor = 0";

        var command = new SqlCommand(select, connection);
        var reader = command.ExecuteReader();
        string address = string.Empty;
        var list = new List<BuildForCoordinate>();
        while (reader.Read())
        {
          var build = new BuildForCoordinate()
          {
            Street = reader.GetString(0),
            Number = reader.GetString(1),
            Bulding = reader.GetString(2),
            Letter = reader.GetString(3),
            Id = reader.GetGuid(4)
          };
          list.Add(build);
        }
        reader.Close();

        foreach (var item in list)
        {
          address = $@"Санкт-Петербург {item.Street}, {item.Number}к{item.Bulding} лит.{item.Letter}";

          var doc1 = yandex.SearchObjectByAddress(address);
          using (var sw = new StreamWriter(@"D:\Coord.xml", false, System.Text.Encoding.UTF8))
          {
            sw.WriteLine(doc1);
          }

          XmlDocument doc = new XmlDocument();
          doc.Load(@"D:\Coord.xml");
          var root = doc.DocumentElement;
          var GeoObjectCollection = root.GetElementsByTagName("GeoObjectCollection")[0];
          if (GeoObjectCollection.ChildNodes.Count > 1)
          {
            var featureMember = GeoObjectCollection.ChildNodes[1];
            if (featureMember.ChildNodes.Count > 0)
            {
              var GeoObject = featureMember.ChildNodes[0];
              if (GeoObject.ChildNodes.Count > 4)
              {
                var Point = GeoObject.ChildNodes[4];
                var coor = Point.InnerText.Split(' ');
                double x = float.Parse(coor[1].Replace(".", ","));
                double y = float.Parse(coor[0].Replace(".", ","));

                string insert = $@"update [ParseBulding].[dbo].[MainInfoAboutBulding] 
SET Xcoor = {x.ToString(CultureInfo.GetCultureInfo("en-US"))},
Ycoor = {y.ToString(CultureInfo.GetCultureInfo("en-US"))}
WHERE ID ='{item.Id}'";
                command = new SqlCommand(insert, connection);
                command.ExecuteNonQuery();
              }
            }
          }
          File.Delete(@"D:\Coord.xml");
        }
      }
      */
      GetCoordBuilding(@"D:\ElmsBuilding.csv", @"D:\ElmsBuildingWithCoor.csv");
    }
    private void GetCoordBuilding(string inputFile, string outputFile)
    {
      if(!string.IsNullOrEmpty(inputFile))
      {
        if (File.Exists(inputFile))
        {
          using (var sr = new StreamReader(inputFile, Encoding.UTF8))
          {
            using (var sw = new StreamWriter(outputFile, true, Encoding.UTF8))
            {
              var yandex = new Yandex();
              string line;
              sr.ReadLine();
              while ((line = sr.ReadLine()) != null)
              {
                var arr = line.Split(';');
                var doc1 = yandex.SearchObjectByAddress($@"Санкт-Петербург {arr[1]}, {arr[2]}к{arr[3]} лит.{arr[4]}");
                using (var sw1 = new StreamWriter(@"D:\Coord.xml", false, System.Text.Encoding.UTF8))
                {
                  sw1.WriteLine(doc1);
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(@"D:\Coord.xml");
                var root = doc.DocumentElement;
                var GeoObjectCollection = root.GetElementsByTagName("GeoObjectCollection")[0];
                if (GeoObjectCollection.ChildNodes.Count > 1)
                {
                  var featureMember = GeoObjectCollection.ChildNodes[1];
                  if (featureMember.ChildNodes.Count > 0)
                  {
                    var GeoObject = featureMember.ChildNodes[0];
                    if (GeoObject.ChildNodes.Count > 4)
                    {
                      var Point = GeoObject.ChildNodes[4];
                      var coor = Point.InnerText.Split(' ');
                      float x = float.Parse(coor[1].Replace(".", ","));
                      float y = float.Parse(coor[0].Replace(".", ","));
                      sw.WriteLine($@"{arr[0]};{arr[1]};{arr[2]};{arr[3]};{arr[4]};{x};{y}");
                    }
                  }
                }
                File.Delete(@"D:\Coord.xml");
              }
            }
          }
        }
      }
    }

    private void button12_Click(object sender, EventArgs e)
    {
      var elms = new ELMS();
      elms.GetInfoAboutBuilding();
    }

    private void button11_Click(object sender, EventArgs e)
    {
      var bkn = new BKN();
      bkn.GetInfoAboutBuilding();
    }

    private void button13_Click(object sender, EventArgs e)
    {
      var bn = new BN();
      bn.GetInfoAboutBuilding();
    }

    private void button15_Click(object sender, EventArgs e)
    {
      var avito = new Avito();
      avito.GetInfoAboutBuilding();
    }


    private List<Metro> metroStantions = new List<Metro>() { new Metro { Name = "Автово" }, new Metro { Name = "Адмиралтейская" }, new Metro { Name = "Академическая" }, new Metro { Name = "Балтийская" }, new Metro { Name = "Беговая" }, new Metro { Name = "Бухарестская" }, new Metro { Name = "Василеостровская" }, new Metro { Name = "Владимирская" }, new Metro { Name = "Волковская" }, new Metro { Name = "Выборгская" }, new Metro { Name = "Горьковская" }, new Metro { Name = "Гостиный двор" }, new Metro { Name = "Гражданский проспект" }, new Metro { Name = "Девяткино" }, new Metro { Name = "Достоевская" }, new Metro { Name = "Елизаровская" }, new Metro { Name = "Звёздная" }, new Metro { Name = "Звенигородская" }, new Metro { Name = "Кировский завод" }, new Metro { Name = "Комендантский проспект" }, new Metro { Name = "Крестовский остров" }, new Metro { Name = "Купчино" }, new Metro { Name = "Ладожская" }, new Metro { Name = "Ленинский проспект" }, new Metro { Name = "Лесная" }, new Metro { Name = "Лиговский проспект" }, new Metro { Name = "Ломоносовская" }, new Metro { Name = "Маяковская" }, new Metro { Name = "Международная" }, new Metro { Name = "Московская" }, new Metro { Name = "Московские ворота" }, new Metro { Name = "Нарвская" }, new Metro { Name = "Невский проспект" }, new Metro { Name = "Новокрестовская" }, new Metro { Name = "Новочеркасская" }, new Metro { Name = "Обводный канал" }, new Metro { Name = "Обухово" }, new Metro { Name = "Озерки" }, new Metro { Name = "Парк Победы" }, new Metro { Name = "Парнас" }, new Metro { Name = "Петроградская" }, new Metro { Name = "Пионерская" }, new Metro { Name = "Площадь Александра Невского 1" }, new Metro { Name = "Площадь Восстания" }, new Metro { Name = "Площадь Ленина" }, new Metro { Name = "Площадь Мужества" }, new Metro { Name = "Политехническая" }, new Metro { Name = "Приморская" }, new Metro { Name = "Пролетарская" }, new Metro { Name = "Проспект Большевиков" }, new Metro { Name = "Проспект Ветеранов" }, new Metro { Name = "Проспект Просвещения" }, new Metro { Name = "Пушкинская" }, new Metro { Name = "Рыбацкое" }, new Metro { Name = "Садовая" }, new Metro { Name = "Сенная площадь" }, new Metro { Name = "Спасская" }, new Metro { Name = "Спортивная" }, new Metro { Name = "Старая Деревня" }, new Metro { Name = "Технологический институт 1" }, new Metro { Name = "Удельная" }, new Metro { Name = "Улица Дыбенко" }, new Metro { Name = "Фрунзенская" }, new Metro { Name = "Чёрная речка" }, new Metro { Name = "Чернышевская" }, new Metro { Name = "Чкаловская" }, new Metro { Name = "Электросила" } };

    private void button16_Click(object sender, EventArgs e)
    {
      var yandex = new Yandex();
      using (var swMain = new StreamWriter(@"D:\CoordMetro.csv", false, System.Text.Encoding.UTF8))
      {
        foreach (var item in metroStantions)
        {
          string address = $@"Санкт-Петербург метро {item.Name}";

          var doc1 = yandex.SearchObjectByAddress(address);
          using (var sw = new StreamWriter(@"D:\Coord.xml", false, System.Text.Encoding.UTF8))
          {
            sw.WriteLine(doc1);
          }

          XmlDocument doc = new XmlDocument();
          doc.Load(@"D:\Coord.xml");
          var root = doc.DocumentElement;
          var GeoObjectCollection = root.GetElementsByTagName("GeoObjectCollection")[0];
          if (GeoObjectCollection.ChildNodes.Count > 1)
          {
            var featureMember = GeoObjectCollection.ChildNodes[1];
            if (featureMember.ChildNodes.Count > 0)
            {
              var GeoObject = featureMember.ChildNodes[0];
              if (GeoObject.ChildNodes.Count > 4)
              {
                var Point = GeoObject.ChildNodes[4];
                var coor = Point.InnerText.Split(' ');
                item.XCoor = float.Parse(coor[1].Replace(".", ","));
                item.YCoor = float.Parse(coor[0].Replace(".", ","));
              }
            }
            File.Delete(@"D:\Coord.xml");
          }
          swMain.WriteLine($"{item.Name};{item.XCoor};{item.YCoor}");
        }
      }
    }

    class BuildForCoordinate
    {
      public string Street { get; set; }
      public string Number { get; set; }
      public string Bulding { get; set; }
      public string Letter { get; set; }
      public Guid Id { get; set; }
    }

    private void button17_Click(object sender, EventArgs e)
    {
      var elms = new ELMS();
      elms.ParsingSdamAll();
    }

    private void button18_Click(object sender, EventArgs e)
    {
      var bn = new BN();
      bn.ParsingSdamAll();
    }

    private void button19_Click(object sender, EventArgs e)
    {
      var bkn = new BKN();
      bkn.ParsingSdamAll();
    }

    private void button14_Click(object sender, EventArgs e)
    {
      using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
      {
        connection.Open();
        using (var sr = new StreamReader(@"D:\CoordMetro.csv", Encoding.UTF8))
        {
          string line = "";

          while ((line = sr.ReadLine()) != null)
          {
            var arr = line.Split(';');
            var metro = new Metro() { Name = arr[0], XCoor = float.Parse(arr[1]), YCoor = float.Parse(arr[2]) };
            string select = $@"SELECT [ID]
  FROM [ParseBulding].[dbo].[District]
  WHERE LOWER(Name) = LOWER('{arr[3].Replace("район","").Trim()}')";

            var command = new SqlCommand(select, connection);
            var reader = command.ExecuteReader();
            if(reader.Read())
            {
              metro.IdDistrict = reader.GetGuid(0);
            }
            reader.Close();

            string insert = $@" insert into [dbo].[Metro] (Id, Name, XCoor, YCoor, IdRegion)
 values ('{metro.Id}','{metro.Name}',{metro.XCoor.ToString().Replace(",",".")},{metro.YCoor.ToString().Replace(",", ".")},'{metro.IdDistrict}')";

            command = new SqlCommand(insert, connection);
            command.ExecuteNonQuery();
          }
        }
      }
    }

    private void button20_Click(object sender, EventArgs e)
    {
      using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
      {
        connection.Open();
        using (var sr = new StreamReader(@"D:\PostPoint.csv", Encoding.UTF8))
        {
          string line = "";

          while ((line = sr.ReadLine()) != null)
          {
            var arr = line.Split(',');
            string select = $@"SELECT [ID]
  FROM [ParseBulding].[dbo].[District]
  WHERE LOWER(Name) = LOWER('{arr[1].Replace("район", "").Trim()}')";

            Guid idDistrict = Guid.Empty;
            var command = new SqlCommand(select, connection);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
              idDistrict = reader.GetGuid(0);
            }
            reader.Close();

            string insert = $@"insert into [dbo].[PostPoint] (Id, PostName, DistrictId)
  values (newid(),{arr[0]},'{idDistrict}')";

            command = new SqlCommand(insert, connection);
            command.ExecuteNonQuery();
          }
        }
      }
    }

    private void button21_Click(object sender, EventArgs e)
    {
      var bkn = new BKN();
      bkn.GetInfoAboutBuildingSdam();
    }

    private void button22_Click(object sender, EventArgs e)
    {
      var bn = new BN();
      bn.GetInfoAboutBuildingSdam();
    }

    private void button23_Click(object sender, EventArgs e)
    {
      var elms = new ELMS();
      elms.GetInfoAboutBuildingSdam();
    }
  }
}
