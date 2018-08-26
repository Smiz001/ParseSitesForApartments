using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace ParseSitesForApartments.Sites
{
  public class Avito: BaseParse
  {
    private int minPage = 1;
    private int maxPage = 100;

    public override void ParsingAll()
    {
      using (var sw = new StreamWriter(@"D:\AvitoProdam.csv", true, System.Text.Encoding.UTF8))
      {
        sw.WriteLine($@"Улица;Номер дома;Станция метро;Расстояние до метро;Цена;Кол-во комнат;общая площадь;Этаж;Дата постройик дома;Дата кап. ремонта");
        using (var swInvalid = new StreamWriter(@"D:\AvitoProdamInvalidHtml.txt", true, System.Text.Encoding.UTF8))
        {
          try
          {
            ParsingStudio(sw, swInvalid);
            ParsingOneRoom(sw, swInvalid);
            ParsingTwoRoom(sw, swInvalid);
            ParsingThreeRoom(sw, swInvalid);
            ParsingFourRoom(sw, swInvalid);
            ParsingFiveRoom(sw, swInvalid);
            ParsingSixRoom(sw, swInvalid);
            ParsingSevenRoom(sw, swInvalid);
            ParsingEightRoom(sw, swInvalid);
            ParsingNineRoom(sw, swInvalid);
            ParsingMoreNineRoom(sw, swInvalid);
          }
          catch (WebException ex)
          {
            var e = ex;
            var result = e.HResult;
          }
        }
      }
    }

    public void ParsingStudio(StreamWriter sw, StreamWriter swInvalid)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/studii?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          ParsingSheet("Студия", sw, document, swInvalid);
        }
      }
    }
    public void ParsingOneRoom(StreamWriter sw, StreamWriter swInvalid)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/1-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          ParsingSheet("1 км. кв.", sw, document, swInvalid);
        }
      }
    }
    public void ParsingTwoRoom(StreamWriter sw, StreamWriter swInvalid)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/2-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          ParsingSheet("2 км. кв.", sw, document, swInvalid);
        }
      }
    }
    public void ParsingThreeRoom(StreamWriter sw, StreamWriter swInvalid)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/3-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          ParsingSheet("3 км. кв.", sw, document, swInvalid);
        }
      }
    }
    public void ParsingFourRoom(StreamWriter sw, StreamWriter swInvalid)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/4-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          ParsingSheet("4 км. кв.", sw, document, swInvalid);
        }
      }
    }
    public void ParsingFiveRoom(StreamWriter sw, StreamWriter swInvalid)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/5-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          ParsingSheet("5 км. кв.", sw, document, swInvalid);
        }
      }
    }
    public void ParsingSixRoom(StreamWriter sw, StreamWriter swInvalid)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/6-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          ParsingSheet("6 км. кв.", sw, document, swInvalid);
        }
      }
    }
    public void ParsingSevenRoom(StreamWriter sw, StreamWriter swInvalid)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/7-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          ParsingSheet("7 км. кв.", sw, document, swInvalid);
        }
      }
    }
    public void ParsingEightRoom(StreamWriter sw, StreamWriter swInvalid)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/8-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          ParsingSheet("8 км. кв.", sw, document, swInvalid);
        }
      }
    }
    public void ParsingNineRoom(StreamWriter sw, StreamWriter swInvalid)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/9-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          ParsingSheet("9 км. кв.", sw, document, swInvalid);
        }
      }
    }
    public void ParsingMoreNineRoom(StreamWriter sw, StreamWriter swInvalid)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/mnogokomnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          ParsingSheet(">9 км. кв.", sw, document, swInvalid);
        }
      }
    }

    private void ParsingSheet(string typeRoom, StreamWriter sw, IHtmlDocument doc, StreamWriter swInvalid)
    {
      var elem = doc.GetElementsByClassName("item_table-header");
      var adresses = doc.GetElementsByClassName("address");
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
        build.CountRoom = typeRoom;
        build.Square = aboutBuild[1];
        build.Floor = aboutBuild[2].Split('/')[0];

        var adress = adresses[k];

        if (adress.ChildNodes.Length > 1)
          build.Metro = adress.ChildNodes[2].NodeValue.Trim();
        if (adresses[k].GetElementsByClassName("c-2").Length > 0)
          build.Distance = adresses[k].GetElementsByClassName("c-2")[0].TextContent;

        var adArr = adress.TextContent.Split(',');
        if (adArr.Length == 4)
        {
          if ((adArr.Contains("посёлок Шушары") && (!adress.TextContent.Contains("проспект") || !adress.TextContent.Contains("пр.") || !adress.TextContent.Contains("пр-т") || !adress.TextContent.Contains("ул.") || !adress.TextContent.Contains("ул") || !adress.TextContent.Contains("пр-кт"))))
            build.Street = "посёлок Шушары";
          else if ((adArr.Contains("Колпино") && (!adress.TextContent.Contains("проспект") || !adress.TextContent.Contains("пр.") || !adress.TextContent.Contains("пр-т") || !adress.TextContent.Contains("ул.") || !adress.TextContent.Contains("ул") || !adress.TextContent.Contains("пр-кт"))))
            build.Street = "Колпино";
          else if ((adArr.Contains("Красное Село") && (!adress.TextContent.Contains("проспект") || !adress.TextContent.Contains("пр.") || !adress.TextContent.Contains("пр-т") || !adress.TextContent.Contains("ул.") || !adress.TextContent.Contains("ул") || !adress.TextContent.Contains("пр-кт"))))
            build.Street = "Красное Село";
          else if ((adArr.Contains("посёлок Мурино") && (!adress.TextContent.Contains("проспект") || !adress.TextContent.Contains("пр.") || !adress.TextContent.Contains("пр-т") || !adress.TextContent.Contains("ул.") || !adress.TextContent.Contains("ул") || !adress.TextContent.Contains("пр-кт"))))
            build.Street = "посёлок Мурино";
          else
          {
            if (adArr[1].Trim() == "Санкт-Петербург")
            {
              build.Street = adArr[2].Trim();
              build.Number = adArr[3].Trim();
            }
            else
            {
              build.Street = adArr[1];
              build.Number = adArr[2].Trim();
            }
          }
        }
        else if (adArr.Length == 5)
        {
          if (adArr[2].Trim() == "Санкт-Петербург")
          {
            build.Street = adArr[3];
            build.Number = adArr[4].Trim();
          }
          else if (adArr[1].Trim() == "Санкт-Петербург")
          {
            if (adArr[2].Trim().Contains("район") || adArr[2].Trim().Contains("территория"))
            {
              build.Street = adArr[3];
              build.Number = adArr[4].Trim();
            }
            else
            {
              build.Street = adArr[2];
              build.Number = adArr[3].Trim();
            }
          }
          else
          {
            build.Street = adArr[1];
            build.Number = adArr[2].Trim();
          }
        }
        else if (adArr.Length == 2)
        {
          build.Street = adArr[1].Trim().Split(' ')[0];
        }
        else if (adArr.Length == 3)
        {
          build.Street = adArr[1].Trim();
          build.Number = adArr[2].Trim();
        }

        if (string.IsNullOrEmpty(build.Number))
        {
          if (adArr.Length > 1)
            build.Number = adArr[adArr.Length - 1].Trim();
        }
        //string a;
        //if (string.IsNullOrEmpty(build.Street))
        //  a = "";

        //build.Street = build.Street.Replace("проспект", "").Replace("пр.", "").Replace("пр-т", "").Replace("ул.", "").Replace("улица", "").Replace("ул", "").Replace("Санкт-Петербург", "").Replace("пр-кт", "").Replace("Колпино", "").Replace("Красное Село", "").Trim();
        

        int num;
        if (int.TryParse(build.Street, out num))
        {
          if (string.IsNullOrWhiteSpace(build.Metro))
            break;
        }
        if (string.IsNullOrWhiteSpace(build.Street))
          break;

        build.Street = build.Street.Replace("проспект", "").Replace("пр.", "").Replace("пр-т", "").Replace("ул.", "").Replace("улица", "").Replace("ул", "").Replace("пр-кт", "").Replace("ул ", "").Trim();


        if (build.Number.Contains("Санкт-Петербург"))
        {
          string invalidHtml = adress.InnerHtml;
          swInvalid.WriteLine(invalidHtml);
          break;
        }

        sw.WriteLine($@"{build.Street};{build.Number};{build.Metro};{build.Distance};{build.Price};{build.CountRoom};{build.Square};{build.Floor};{build.DateBuild};{build.DateRepair}");
      }
    }
  }
}
