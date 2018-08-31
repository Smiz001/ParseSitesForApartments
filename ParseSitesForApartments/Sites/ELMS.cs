using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
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
    private const string Filename = @"D:\ElmsProdam.csv";
    static object locker = new object();
    private const int MaxPage = 10;

    public override void ParsingAll()
    {
      using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Create), Encoding.UTF8))
      {
        sw.WriteLine($@"Нас. пункт;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние");
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
    }
    
    public void ParseStudii()
    {
      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int i = 0; i < listDistrict.Count; i++)
        {
          for (int j = 0; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r0/1/is_auction/2/place/address/reg/2/dept/2/dist/{listDistrict[i]}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheet(tableElements, "Студия");
          }
        }
      }
      MessageBox.Show("Закончили студии");
    }

    public void ParseOneRoom()
    {
      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int i = 0; i < listDistrict.Count; i++)
        {
          for (int j = 0; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r1/1/is_auction/2/place/address/reg/2/dept/2/dist/{listDistrict[i]}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheet(tableElements, "1 км. кв.");
          }
        }
      }
      MessageBox.Show("Закончили 1 км. кв.");
    }

    public void ParseTwoRoom()
    {
      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int i = 0; i < listDistrict.Count; i++)
        {
          for (int j = 0; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r2/1/is_auction/2/place/address/reg/2/dept/2/dist/{listDistrict[i]}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheet(tableElements, "2 км. кв.");
          }
        }
      }
      MessageBox.Show("Закончили 2 км. кв.");
    }

    public void ParseThreeRoom()
    {
      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int i = 0; i < listDistrict.Count; i++)
        {
          for (int j = 0; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r3/1/is_auction/2/place/address/reg/2/dept/2/dist/{listDistrict[i]}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheet(tableElements, "3 км. кв.");
          }
        }
      }
      MessageBox.Show("Закончили 3 км. кв.");
    }

    public void ParseFourRoom()
    {
      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int i = 0; i < listDistrict.Count; i++)
        {
          for (int j = 0; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r4/1/is_auction/2/place/address/reg/2/dept/2/dist/{listDistrict[i]}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheet(tableElements, "4 км. кв.");
          }
        }
      }
      MessageBox.Show("Закончили 4 км. кв.");
    }

    public void ParseFiveRoom()
    {
      using (var webClient = new WebClient())
      {
        var random = new Random();
        for (int i = 0; i < listDistrict.Count; i++)
        {
          for (int j = 0; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r5/1/is_auction/2/place/address/reg/2/dept/2/dist/{listDistrict[i]}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheet(tableElements, "5 км. кв.");
          }
        }
      }
      MessageBox.Show("Закончили 5 км. кв.");
    }

    public void ParseSheet(IHtmlCollection<IElement> collection, string typeRoom)
    {
      for (int i = 0; i < collection.Length; i++)
      {
        var build = new Build
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
              build.Square = square[0].TextContent;

            var adr = collection[i].GetElementsByClassName("address-geo")[0].TextContent.Split(',');
            if (adr.Length == 3)
            {
              build.Street = adr[0] + " " + adr[1];
              build.Number = adr[2];
            }
            else
            {
              build.Street = adr[0];
              if (adr.Length > 1)
                build.Number = adr[1].Trim();
            }
            var regex = new Regex(@"(к\d+)");
            build.Building = regex.Match(build.Number).Value;
            if(!string.IsNullOrEmpty(build.Building))
            {
              build.Number = build.Number.Replace(build.Building, "");
              build.Building = build.Building.Replace("к", "");
            }
            regex = new Regex(@"(\D)");
            build.Liter = regex.Match(build.Number).Value;
            if (!string.IsNullOrEmpty(build.Liter))
              build.Number = build.Number.Replace(build.Liter, "");


            var metro = collection[i].GetElementsByClassName("metroline-2");
            if (metro.Length > 0)
              build.Metro = metro[0].TextContent;

            regex = new Regex(@"(\d+)");
            var floor = collection[i].GetElementsByClassName("w-floor");
            if (floor.Length > 0)
            {
              var ms = regex.Matches(floor[0].TextContent);
              if (ms.Count > 0)
                build.Floor = ms[0].Value;
            }

            regex = new Regex(@"(\d+\s+\d+\s+метров)|(\d+\s+метров)");
            var distance = collection[i].GetElementsByClassName("ellipsis em");
            if (distance.Length > 0)
            {
              build.Distance = regex.Match(distance[0].TextContent.Replace("\n", "").Trim()).Value;
            }

            //year = collection[i].GetElementsByClassName("w-year")[0].TextContent;
            var pr = collection[i].GetElementsByClassName("price");
            if (pr.Length > 0)
            {
              string priceStr = pr[0].TextContent.Replace(" a", "").Replace(" ", "");
              int price;
              if (int.TryParse(priceStr, out price))
              {
                build.Price = price;
              }
            }

            string town = string.Empty;
            if (build.Street.Contains("(Горелово)"))
            {
              town = "Горелово";
              build.Street = build.Street.Replace("(Горелово)", "");
            }
            else if (build.Street.Contains("Красное Село"))
            {
              town = "Красное Село";
              build.Street = build.Street.Replace(town, "");
            }
            else if (build.Street.Contains("Парголово"))
            {
              town = "Парголово";
              build.Street = build.Street.Replace(town, "");
            }
            else
              town = "Санкт-Петербург";


            build.Street = build.Street.Replace("ул.", "").Replace("ал.", "").Replace("бул.", "").Replace("ш.", "").Replace("пр.", "").Replace("пер.", "").Replace("пр-д", "").Replace(" б","").Trim();

            Monitor.Enter(locker);
            using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Open), Encoding.UTF8))
            {
              sw.BaseStream.Position = sw.BaseStream.Length;
              sw.WriteLine($@"{town};{build.Street};{build.Number};{build.Building};{build.Liter};{build.CountRoom};{build.Square};{build.Price};{build.Floor};{build.Metro};{build.Distance}");
            }
            Monitor.Exit(locker);
          }
        }
        catch(Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
      }
    }
  }
}
