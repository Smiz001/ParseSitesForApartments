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
      /*
      List<Build> list = new List<Build>();
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
        {
          connection.Open();
          using (var sw = new StreamWriter(@"D:\AvitoProdam.csv", true, System.Text.Encoding.UTF8))
          {
            for (int i = pageMin; i < pageMaz; i++)
            {
              Thread.Sleep(random.Next(5000, 10000));
              string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam?p={i}";
              webClient.Encoding = System.Text.Encoding.UTF8;
              var responce = webClient.DownloadString(prodam);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var elem = document.GetElementsByClassName("item_table-header");
              var adresses = document.GetElementsByClassName("address");
              for (int k = 0; k < elem.Length; k++)
              {
                var build = new Build();
                var price = int.Parse(elem[k].GetElementsByClassName("price")[0].TextContent.Trim('\n').Trim('₽').Trim().Replace(" ", ""));
                build.Price = price;
                var aboutBuild = elem[k].GetElementsByClassName("item-description-title-link")[0].TextContent.Split(',').ToList();
                for (int j = 0; j < aboutBuild.Count; j++)
                {
                  aboutBuild[j] = aboutBuild[j].Trim();
                }
                build.CountRoom = aboutBuild[0];
                build.Square = aboutBuild[1];
                build.Floor = aboutBuild[2];

                var adress = adresses[k];

                if (adress.ChildNodes.Length > 1)
                  build.Metro = adress.ChildNodes[2].NodeValue.Trim();
                if (adresses[k].GetElementsByClassName("c-2").Length > 0)
                  build.Distance = adresses[k].GetElementsByClassName("c-2")[0].TextContent;

                var adArr = adress.TextContent.Split(',');
                if (adArr.Length > 2)
                {
                  var street = adArr[adArr.Length - 2];
                  build.Street = street.Replace("проспект", "").Replace("пр.", "").Replace("пр-т", "").Replace("ул.", "").Replace("улица", "").Replace("ул", "").Replace("Санкт-Петербург", "").Replace("пр-кт", "").Replace("Колпино", "").Replace("Красное Село", "").Trim();

                }
                else if (adArr.Length == 2)
                {
                  var street = adArr[1].Trim().Split(' ')[0];
                }

                if (adArr.Length > 1)
                  build.Number = adArr[adArr.Length - 1].Trim();

                string select = $@"SELECT [BuldingDate]
      ,[RepairDate]
  FROM [ParseBulding].[dbo].[InfoAboutBulding]
  where LOWER(Street) Like ('%{build.Street}%')
  and Number = '{build.Number}'";

                var command = new SqlCommand(select, connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                  build.DateBuild = reader.GetString(0);
                  build.DateRepair = reader.GetString(1);
                  break;
                }
                reader.Close();
                sw.WriteLine($@"{build.Street};{build.Number};{build.Metro};{build.Distance};{build.Price};{build.CountRoom};{build.Square};{build.Floor};{build.DateBuild};{build.DateRepair}");
              }
            }
          }
        }
      }
      */
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
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
        {
          connection.Open();
          using (var sw = new StreamWriter(@"D:\AvitoSdam.csv", true, System.Text.Encoding.UTF8))
          {
            for (int i = pageMin; i < pageMaz; i++)
            {
              Thread.Sleep(random.Next(5000, 10000));
              string sdam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/sdam?p={i}";
              webClient.Encoding = System.Text.Encoding.UTF8;
              var responce = webClient.DownloadString(sdam);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var elem = document.GetElementsByClassName("item_table-header");
              var adresses = document.GetElementsByClassName("address");
              for (int k = 0; k < elem.Length; k++)
              {
                var build = new Build();
                var price = (elem[k].GetElementsByClassName("price")[0].TextContent.Trim('\n').Trim('₽').Trim().Replace(" ", "").Replace("\n", " "));

                var aboutBuild = elem[k].GetElementsByClassName("item-description-title-link")[0].TextContent.Split(',').ToList();
                for (int j = 0; j < aboutBuild.Count; j++)
                {
                  aboutBuild[j] = aboutBuild[j].Trim();
                }

                build.CountRoom = aboutBuild[0];
                build.Square = aboutBuild[1];
                build.Floor = aboutBuild[2];

                var adress = adresses[k];

                if (adress.ChildNodes.Length > 1)
                  build.Metro = adress.ChildNodes[2].NodeValue.Trim();
                if (adresses[k].GetElementsByClassName("c-2").Length > 0)
                  build.Distance = adresses[k].GetElementsByClassName("c-2")[0].TextContent;

                var adArr = adress.TextContent.Split(',');
                if (adArr.Length > 2)
                {
                  var street = adArr[adArr.Length - 2];
                  build.Street = street.Replace("проспект", "").Replace("пр.", "").Replace("пр-т", "").Replace("ул.", "").Replace("улица", "").Replace("ул", "").Replace("Санкт-Петербург", "").Replace("пр-кт", "").Replace("Колпино", "").Replace("Красное Село", "").Trim();

                }
                else if (adArr.Length == 2)
                {
                  var street = adArr[1].Trim().Split(' ')[0];
                }

                if (adArr.Length > 1)
                  build.Number = adArr[adArr.Length - 1].Trim();

                string select = $@"SELECT [BuldingDate]
      ,[RepairDate]
  FROM [ParseBulding].[dbo].[InfoAboutBulding]
  where LOWER(Street) Like ('%{build.Street}%')
  and Number = '{build.Number}'";

                var command = new SqlCommand(select, connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                  build.DateBuild = reader.GetString(0);
                  build.DateRepair = reader.GetString(1);
                  break;
                }
                reader.Close();
                sw.WriteLine($@"{build.Street};{build.Number};{build.Metro};{build.Distance};{price};{build.CountRoom};{build.Square};{build.Floor};{build.DateBuild};{build.DateRepair}");
              }
            }
          }
        }
      }
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
      int minPage = 1;
      int maxPage = 17;
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
        {
          connection.Open();
          using (var sw = new StreamWriter(@"D:\BNProdam.csv", true, System.Text.Encoding.UTF8))
          {
            string rooms = string.Empty;
            string square = string.Empty;
            string year = string.Empty;
            string price = string.Empty;
            string floor = string.Empty;
            string street = string.Empty;
            string number = string.Empty;
            string metro = string.Empty;
            string distance = string.Empty;
            string district = string.Empty;
            string building = string.Empty;

            for (int i = minPage; i < maxPage; i++)
            {
              Thread.Sleep(random.Next(5000, 15000));
              string sdam = $@"https://www.bn.ru/kvartiry-vtorichka/?from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptPortion=1&formName=secondary&page={i}";
              webClient.Encoding = System.Text.Encoding.UTF8;
              var responce = webClient.DownloadString(sdam);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var newApartment = document.GetElementsByClassName("catalog_filter_body");

              for (int j = 0; j < newApartment.Length; j++)
              {
                if (newApartment[j].GetElementsByClassName("object__square").Length > 0)
                  square = newApartment[j].GetElementsByClassName("object__square")[0].TextContent.Trim();
                if (newApartment[j].GetElementsByClassName("catalog_filter_title-item_kkv").Length > 0)
                  rooms = newApartment[j].GetElementsByClassName("catalog_filter_title-item_kkv")[0].TextContent.Trim().Replace(square, "").Replace(",", "").Trim();

                if (newApartment[j].GetElementsByClassName("object__add-price-info").Length > 0 && newApartment[j].GetElementsByClassName("object__price").Length > 0)
                {
                  var kv = newApartment[j].GetElementsByClassName("object__add-price-info")[0].TextContent.Trim();
                  price = newApartment[j].GetElementsByClassName("object__price")[0].TextContent.Replace(kv, "").Replace(",", "").Replace("₽", "").Trim();
                }
                if (newApartment[j].GetElementsByClassName("metro").Length > 0)
                  metro = newApartment[j].GetElementsByClassName("metro")[0].TextContent.Trim();
                if (newApartment[j].GetElementsByClassName("metro-distance").Length > 0)
                  distance = newApartment[j].GetElementsByClassName("metro-distance")[0].TextContent.Replace(",", "").Trim();

                if (newApartment[j].GetElementsByClassName("catalog_filter_body-img-bottom-right-item obj-add-info").Length > 0)
                  floor = newApartment[j].GetElementsByClassName("catalog_filter_body-img-bottom-right-item obj-add-info")[0].TextContent.Replace("\n", "").Replace(" ", "").Trim();

                if (newApartment[j].GetElementsByClassName("catalog_filter_block-cb_address catalog_filter_block-cb_address_string").Length > 0)
                  street = newApartment[j].GetElementsByClassName("catalog_filter_block-cb_address catalog_filter_block-cb_address_string")[0].TextContent.Replace("\n", "").Trim();

                Regex regex = new Regex(@"(\d+, \d+, \d+)|(\d+, \d+)|(\d+)");
                number = regex.Match(street).Value;
                if (!string.IsNullOrEmpty(number))
                  street = street.Replace(number, "");

                sw.WriteLine($@"{street};{number};{rooms};{square};{price};{floor};{metro};{distance}");
              }
            }
          }
        }
      }
    }

    private void button9_Click(object sender, EventArgs e)
    {
      int minPage = 1;
      int maxPage = 20;
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
        {
          connection.Open();
          using (var sw = new StreamWriter(@"D:\EMLSProdam.csv", true, System.Text.Encoding.UTF8))
          {
            string rooms = string.Empty;
            string square = string.Empty;
            string year = string.Empty;
            string price = string.Empty;
            string floor = string.Empty;
            string street = string.Empty;
            string number = string.Empty;
            string metro = string.Empty;
            string distance = string.Empty;
            string district = string.Empty;
            string building = string.Empty;

            for (int i = minPage; i < maxPage; i++)
            {
              Thread.Sleep(random.Next(5000, 15000));
              string sdam = $@"https://www.emls.ru/flats/page{i}.html?query=s/1/is_auction/2/place/address/reg/2/dept/2/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdam);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var listing = document.GetElementsByClassName("listing")[0];

              var rows = listing.GetElementsByClassName("row1");
              for (int j = 0; j < rows.Length; j++)
              {
                if (rows[j].GetElementsByClassName("w-image").Length > 0)
                {
                  var divImage = rows[j].GetElementsByClassName("w-image")[0];
                  var divs = divImage.GetElementsByTagName("div");
                  rooms = divImage.GetElementsByTagName("div")[4].TextContent;
                  square = rows[j].GetElementsByClassName("space-all")[0].TextContent;

                  var adr = rows[j].GetElementsByClassName("address-geo")[0].TextContent.Split(',');
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

                  metro = rows[j].GetElementsByClassName("metroline-2")[0].TextContent;

                  distance = rows[j].GetElementsByClassName("ellipsis em")[0].TextContent.Replace("\n", "").Trim();
                  floor = rows[j].GetElementsByClassName("w-floor")[0].TextContent;
                  year = rows[j].GetElementsByClassName("w-year")[0].TextContent;
                  price = rows[j].GetElementsByClassName("price")[0].TextContent.Replace(" a", "");

                  sw.WriteLine($@"{street};{number};{rooms};{square};{price};{floor};{metro};{distance}");
                }
              }
            }
          }
        }
      }
    }

    private void button10_Click(object sender, EventArgs e)
    {
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
}
