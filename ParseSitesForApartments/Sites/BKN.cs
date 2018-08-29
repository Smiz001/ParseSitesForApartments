using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ParseSitesForApartments.Sites
{
  public class BKN : BaseParse
  {
    private int minPage = 1;
    private int maxPage = 100;

    public override void ParsingAll()
    {
      var random = new Random();
      using (var sw = new StreamWriter(@"D:\BKNProdam.csv", true, System.Text.Encoding.UTF8))
      {
        //ParsingVtorichka(sw);
        ParsingNovostroiki(sw);
      }
    }

    public void ParsingVtorichka(StreamWriter sw)
    {
      ParsingStudioVtorichka(sw);
      ParsingOneRoomVtorichka(sw);
      ParsingTwoRoomVtorichka(sw);
      ParsingThreeRoomVtorichka(sw);
      ParsingFourRoomVtorichka(sw);
      ParsingFiveAndMoreRoomVtorichka(sw);
    }

    public void ParsingStudioVtorichka(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.bkn.ru/prodazha/vtorichka/studii?page={i}";

          webClient.Encoding = Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          if (!ParsingSheet("Студия", sw, document))
            return;
        }
      }
    }

    public void ParsingOneRoomVtorichka(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.bkn.ru/prodazha/vtorichka/odnokomnatnye-kvartiry?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          if (!ParsingSheet("1 км. кв.", sw, document))
            return;
        }
      }
    }

    public void ParsingTwoRoomVtorichka(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.bkn.ru/prodazha/vtorichka/dvuhkomnatnye-kvartiry?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          if (!ParsingSheet("2 км. кв.", sw, document))
            return;
        }
      }
    }

    public void ParsingThreeRoomVtorichka(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.bkn.ru/prodazha/vtorichka/trehkomnatnye-kvartiry?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          if (!ParsingSheet("3 км. кв.", sw, document))
            return;
        }
      }
    }

    public void ParsingFourRoomVtorichka(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.bkn.ru/prodazha/vtorichka/chetyrehkomnatnye-kvartiry?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          if (!ParsingSheet("4 км. кв.", sw, document))
            return;
        }
      }
    }

    public void ParsingFiveAndMoreRoomVtorichka(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.bkn.ru/prodazha/vtorichka/pyatikomnatnye-kvartiry?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          if (!ParsingSheet("5 км. кв. и более", sw, document))
            return;
        }
      }
    }

    private bool ParsingSheet(string typeRoom, StreamWriter sw, IHtmlDocument document)
    {
      var newApartment = document.GetElementsByClassName("main NewApartment");
      var apartment = document.GetElementsByClassName("main Apartments");
      if (newApartment.Length == 0 && apartment.Length == 0)
        return false;

      Parse(apartment, typeRoom, sw);
      Parse(newApartment, typeRoom, sw);
      return true;
    }

    private void Parse(IHtmlCollection<IElement> collection, string typeRoom, StreamWriter sw)
    {
      string year = string.Empty;
      string distanceInMinute = string.Empty;
      string district = string.Empty;
      string building = string.Empty;

      for (int j = 0; j < collection.Length; j++)
      {
        string town = string.Empty;

        var build = new Build();
        build.CountRoom = typeRoom;

        var priceDiv = collection[j].GetElementsByClassName("price overflow");
        if (priceDiv.Length == 0)
          break;
        else
        {
          var regex = new Regex(@"(\d+\,\d+\sм2)|(\d+\sм2)");
          var title = collection[j].GetElementsByClassName("title")[0].TextContent;
          build.Square = regex.Match(title).Value;

          regex = new Regex(@"(\d+)");
          var ms = regex.Matches(priceDiv[0].TextContent);
          var price = int.Parse($"{ms[0].Value}{ms[1].Value}{ms[2].Value}");
          build.Price = price;

          regex = new Regex(@"(\d+)");
          ms = regex.Matches(collection[j].GetElementsByClassName("floor overflow")[0].TextContent);
          if (ms.Count > 0)
            build.Floor = ms[0].Value;
          else
            build.Floor = "";

          district = collection[j].GetElementsByClassName("overflow")[2].TextContent;
          build.Street = collection[j].GetElementsByClassName("overflow")[3].TextContent;

          regex = new Regex(@"(д. \d+)|(\d+)");
          build.Number = regex.Match(build.Street).Value.Replace("д. ", "");

          regex = new Regex(@"(к. \d+)");
          building = regex.Match(build.Street).Value.Replace("к. ", "");

          if (build.Street.Contains("Сестрорецк г."))
          {
            town = "Сестрорецк г.";
            build.Street = build.Street.Replace(town, "");
          }
          else if (build.Street.Contains("Шушары пос."))
          {
            town = "Шушары пос.";
            build.Street = build.Street.Replace(town, "");
          }
          else if (build.Street.Contains("Петергоф г."))
          {
            town = "Петергоф г.";
            build.Street = build.Street.Replace(town, "");
          }
          else if (build.Street.Contains("Пушкин г."))
          {
            town = "Пушкин г.";
            build.Street = build.Street.Replace(town, "");
          }
          else if (build.Street.Contains("Зеленогорск г."))
          {
            town = "Зеленогорск г.";
            build.Street = build.Street.Replace(town, "");
          }
          else if (build.Street.Contains("Металлострой пос."))
          {
            town = "Металлострой пос.";
            build.Street = build.Street.Replace(town, "");
          }
          else if (build.Street.Contains("Колпино г."))
          {
            town = "Колпино г.";
            build.Street = build.Street.Replace(town, "");
          }
          else if (build.Street.Contains("Парголово пос."))
          {
            town = "Парголово пос.";
            build.Street = build.Street.Replace(town, "");
          }
          else if (build.Street.Contains("Красное Село г."))
          {
            town = "Красное Село г.";
            build.Street = build.Street.Replace(town, "");
          }
          else if (build.Street.Contains("Понтонный пос"))
          {
            town = "Понтонный пос";
            build.Street = build.Street.Replace(town, "");
          }
          else if (build.Street.Contains("Санкт-Петербург г."))
          {
            town = "Санкт-Петербург г.";
            build.Street = build.Street.Replace(town, "");
          }
          else
            town = "Санкт-Петербург г.";

          if (string.IsNullOrEmpty(building))
          {
            if (string.IsNullOrEmpty(build.Number))
              build.Street = build.Street.Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();
            else
              build.Street = build.Street.Replace(build.Number, "").Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();
          }
          else if (string.IsNullOrEmpty(build.Number))
          {
            if (string.IsNullOrEmpty(building))
              build.Street = build.Street.Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();
            else
              build.Street = build.Street.Replace(building, "").Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();
          }
          else
            build.Street = build.Street.Replace(build.Number, "").Replace(building, "").Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();

          build.Metro = collection[j].GetElementsByClassName("subwaystring")[0].TextContent;

          regex = new Regex(@"(\d+\sмин.\sна\sтранспорте)|(\d+\sмин\.\sпешком)");
          build.Distance = regex.Match(build.Metro).Value;

          if (!string.IsNullOrWhiteSpace(build.Distance))
            build.Metro = build.Metro.Replace(build.Distance, "").Replace("●", "").Replace(",", "").Trim();

          sw.WriteLine($@"{town};{build.Street};{build.Number};{building};{build.CountRoom};{build.Square};{build.Price};{build.Floor};{build.Metro};{build.Distance}");
        }
      }
    }

    private void ParseOneElement(IElement element, string typeRoom, StreamWriter sw)
    {
      string year = string.Empty;
      string distanceInMinute = string.Empty;
      string district = string.Empty;
      string building = string.Empty;
      string town = string.Empty;

      var build = new Build();
      build.CountRoom = typeRoom;

      var priceDiv = element.GetElementsByClassName("price overflow");
      if (priceDiv.Length == 0)
        return;
      else
      {
        var regex = new Regex(@"(\d+\,\d+\sм2)|(\d+\sм2)");
        var title = element.GetElementsByClassName("title")[0].TextContent;
        build.Square = regex.Match(title).Value;

        regex = new Regex(@"(\d+)");
        var ms = regex.Matches(priceDiv[0].TextContent);
        var price = int.Parse($"{ms[0].Value}{ms[1].Value}{ms[2].Value}");
        build.Price = price;

        regex = new Regex(@"(\d+)");
        ms = regex.Matches(element.GetElementsByClassName("floor overflow")[0].TextContent);
        if (ms.Count > 0)
          build.Floor = ms[0].Value;
        else
          build.Floor = "";

        district = element.GetElementsByClassName("overflow")[2].TextContent;
        build.Street = element.GetElementsByClassName("overflow")[3].TextContent;

        regex = new Regex(@"(д. \d+)|(\d+)");
        build.Number = regex.Match(build.Street).Value.Replace("д. ", "");

        regex = new Regex(@"(к. \d+)");
        building = regex.Match(build.Street).Value.Replace("к. ", "");

        if (build.Street.Contains("Сестрорецк г."))
        {
          town = "Сестрорецк г.";
          build.Street = build.Street.Replace(town, "");
        }
        else if (build.Street.Contains("Шушары пос."))
        {
          town = "Шушары пос.";
          build.Street = build.Street.Replace(town, "");
        }
        else if (build.Street.Contains("Петергоф г."))
        {
          town = "Петергоф г.";
          build.Street = build.Street.Replace(town, "");
        }
        else if (build.Street.Contains("Пушкин г."))
        {
          town = "Пушкин г.";
          build.Street = build.Street.Replace(town, "");
        }
        else if (build.Street.Contains("Зеленогорск г."))
        {
          town = "Зеленогорск г.";
          build.Street = build.Street.Replace(town, "");
        }
        else if (build.Street.Contains("Металлострой пос."))
        {
          town = "Металлострой пос.";
          build.Street = build.Street.Replace(town, "");
        }
        else if (build.Street.Contains("Колпино г."))
        {
          town = "Колпино г.";
          build.Street = build.Street.Replace(town, "");
        }
        else if (build.Street.Contains("Парголово пос."))
        {
          town = "Парголово пос.";
          build.Street = build.Street.Replace(town, "");
        }
        else if (build.Street.Contains("Красное Село г."))
        {
          town = "Красное Село г.";
          build.Street = build.Street.Replace(town, "");
        }
        else if (build.Street.Contains("Понтонный пос"))
        {
          town = "Понтонный пос";
          build.Street = build.Street.Replace(town, "");
        }
        else if (build.Street.Contains("Санкт-Петербург г."))
        {
          town = "Санкт-Петербург г.";
          build.Street = build.Street.Replace(town, "");
        }
        else
          town = "Санкт-Петербург г.";

        if (string.IsNullOrEmpty(building))
        {
          if (string.IsNullOrEmpty(build.Number))
            build.Street = build.Street.Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();
          else
            build.Street = build.Street.Replace(build.Number, "").Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();
        }
        else if (string.IsNullOrEmpty(build.Number))
        {
          if (string.IsNullOrEmpty(building))
            build.Street = build.Street.Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();
          else
            build.Street = build.Street.Replace(building, "").Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();
        }
        else
          build.Street = build.Street.Replace(build.Number, "").Replace(building, "").Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();

        build.Metro = element.GetElementsByClassName("subwaystring")[0].TextContent;

        regex = new Regex(@"(\d+\sмин.\sна\sтранспорте)|(\d+\sмин\.\sпешком)");
        build.Distance = regex.Match(build.Metro).Value;

        if (!string.IsNullOrWhiteSpace(build.Distance))
          build.Metro = build.Metro.Replace(build.Distance, "").Replace("●", "").Replace(",", "").Trim();

        sw.WriteLine($@"{town};{build.Street};{build.Number};{building};{build.CountRoom};{build.Square};{build.Price};{build.Floor};{build.Metro};{build.Distance}");

      }
    }

    public void ParsingNovostroiki(StreamWriter sw)
    {
      //ParsingStudioNovostroiki(sw);
      ParsingOneRoomNovostroiki(sw);
      //ParsingTwoRoomNovostroiki(sw);
      //ParsingThreeRoomNovostroiki(sw);
    }

    public void ParsingStudioNovostroiki(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < 6; i++)
        {
          Thread.Sleep(random.Next(1000, 3000));
          string prodam = $@"https://www.bkn.ru/prodazha/novostroiki/studii?page={i}";

          webClient.Encoding = Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);

          var newApartment = document.GetElementsByClassName("main NewApartment");
          if (newApartment.Length == 0)
            break;
          ParseStudiiNovostroikiMain(newApartment, sw);
        }
      }
    }

    private void ParseStudiiNovostroikiMain(IHtmlCollection<IElement> collection, StreamWriter sw)
    {
      Random random = new Random();
      using (var wb = new WebClient())
      {
        for (int i = 0; i < collection.Length; i++)
        {
          if (!collection[i].GetElementsByClassName("name")[0].TextContent.Contains("Студия"))
          {
            var str = collection[i].GetElementsByClassName("name")[0].GetAttribute("href");
            var hrefGk = $@"https://www.bkn.ru{str}";

            wb.Encoding = Encoding.UTF8;
            var responce = wb.DownloadString(hrefGk);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var content = document.GetElementsByClassName("complex-mode-content")[0];
            var list = content.GetElementsByClassName("row offset-bottom-30");
            var elementStudiiGk = list[2];
            var nameElement = elementStudiiGk.GetElementsByClassName("bold nopadding nomargin font-size-20")[0].TextContent;
            if (nameElement == "Студии")
            {
              int length = 15;
              for (int j = 1; j < length; j++)
              {
                str = elementStudiiGk.GetElementsByClassName("white-focus-font btn button-red-noradius")[0].GetAttribute("href").Replace("[]=0", $@"%5b%5d=0&Page={j}");
                var hrefStudiiGk = $@"https://www.bkn.ru{str}";
                Thread.Sleep(random.Next(5000, 6000));

                responce = wb.DownloadString(hrefStudiiGk);
                document = parser.Parse(responce);

                var newApartment = document.GetElementsByClassName("main NewApartment");
                Parse(newApartment, "Студия", sw);
              }
            }
          }
          else
          {
            var parent = collection[i].GetElementsByClassName("name")[0].PreviousElementSibling;
            //var a = parent.PreviousElementSibling;
            //ParseOneElement(parent, "Студия", sw);
          }
        }
      }
    }

    public void ParsingOneRoomNovostroiki(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < 15; i++)
        {
          Thread.Sleep(random.Next(1000, 2000));
          string prodam = $@"https://www.bkn.ru/prodazha/novostroiki/odnokomnatnye-kvartiry?page={i}";

          webClient.Encoding = Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);

          var newApartment = document.GetElementsByClassName("main NewApartment");
          if (newApartment.Length == 0)
            break;
          ParseOneRoomNovostroikiMain(newApartment, sw);
        }
      }
    }

    private void ParseOneRoomNovostroikiMain(IHtmlCollection<IElement> collection, StreamWriter sw)
    {
      Random random = new Random();
      using (var wb = new WebClient())
      {
        for (int i = 0; i < collection.Length; i++)
        {
          if (!collection[i].GetElementsByClassName("name")[0].TextContent.Contains("1-комн."))
          {
            var str = collection[i].GetElementsByClassName("name")[0].GetAttribute("href");
            var hrefGk = $@"https://www.bkn.ru{str}";

            wb.Encoding = Encoding.UTF8;
            var responce = wb.DownloadString(hrefGk);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var content = document.GetElementsByClassName("complex-mode-content")[0];
            var list = content.GetElementsByClassName("row offset-bottom-30");
            for (int k = 0; k < list.Length; k++)
            {
              var listBold = list[k].GetElementsByClassName("bold nopadding nomargin font-size-20");
              if(listBold.Length != 0 )
              {
                if (list[k].GetElementsByClassName("bold nopadding nomargin font-size-20")[0].TextContent == "Однокомнатные квартиры")
                {
                  var elementOneRoomGk = list[k];

                  int length = 30;
                  for (int j = 1; j < length; j++)
                  {
                    if (elementOneRoomGk.GetElementsByClassName("white-focus-font btn button-red-noradius").Length == 0)
                      break;
                    str = elementOneRoomGk.GetElementsByClassName("white-focus-font btn button-red-noradius")[0].GetAttribute("href").Replace("[]=0", $@"%5b%5d=0&Page={j}");
                    var hrefStudiiGk = $@"https://www.bkn.ru{str}";
                    Thread.Sleep(random.Next(1000, 2000));

                    responce = wb.DownloadString(hrefStudiiGk);
                    document = parser.Parse(responce);

                    var newApartment = document.GetElementsByClassName("main NewApartment");
                    Parse(newApartment, "1 км. кв.", sw);
                  }
                }
              }
            }
          }
          else
          {
            var parent = collection[i].GetElementsByClassName("name")[0].PreviousElementSibling;
            //var a = parent.PreviousElementSibling;
            //ParseOneElement(parent, "Студия", sw);
          }
        }
      }
    }

    public void ParsingTwoRoomNovostroiki(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < 15; i++)
        {
          Thread.Sleep(random.Next(2000, 3000));
          string prodam = $@"https://www.bkn.ru/prodazha/novostroiki/dvuhkomnatnye-kvartiry?page={i}";

          webClient.Encoding = Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);

          var newApartment = document.GetElementsByClassName("main NewApartment");
          if (newApartment.Length == 0)
            break;
          ParseTwoRoomNovostroikiMain(newApartment, sw);
        }
      }
    }

    private void ParseTwoRoomNovostroikiMain(IHtmlCollection<IElement> collection, StreamWriter sw)
    {
      Random random = new Random();
      using (var wb = new WebClient())
      {
        for (int i = 0; i < collection.Length; i++)
        {
          try
          {
            if (!collection[i].GetElementsByClassName("name")[0].TextContent.Contains("2-комн"))
            {
              var str = collection[i].GetElementsByClassName("name")[0].GetAttribute("href");
              var hrefGk = $@"https://www.bkn.ru{str}";

              wb.Encoding = Encoding.UTF8;
              var responce = wb.DownloadString(hrefGk);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var content = document.GetElementsByClassName("complex-mode-content")[0];
              var list = content.GetElementsByClassName("row offset-bottom-30");
              if (list.Length < 4)
                break;
              var elementOneRoomGk = list[3];
              var nameElement = elementOneRoomGk.GetElementsByClassName("bold nopadding nomargin font-size-20")[0].TextContent;
              if (nameElement != "Двухкомнатные квартиры")
                elementOneRoomGk = list[4];

              int length = 30;
              for (int j = 1; j < length; j++)
              {
                if (elementOneRoomGk.GetElementsByClassName("white-focus-font btn button-red-noradius").Length == 0)
                  break;
                str = elementOneRoomGk.GetElementsByClassName("white-focus-font btn button-red-noradius")[0].GetAttribute("href").Replace("[]=0", $@"%5b%5d=0&Page={j}");
                var hrefStudiiGk = $@"https://www.bkn.ru{str}";
                Thread.Sleep(random.Next(2000, 3000));

                responce = wb.DownloadString(hrefStudiiGk);
                document = parser.Parse(responce);

                var newApartment = document.GetElementsByClassName("main NewApartment");
                Parse(newApartment, "2 км. кв.", sw);
              }
            }
            else
            {
              var parent = collection[i].GetElementsByClassName("name")[0].PreviousElementSibling;
              //var a = parent.PreviousElementSibling;
              //ParseOneElement(parent, "Студия", sw);
            }
          }
          catch (Exception ex)
          {
            var str = ex.Message;
          }
        }
      }
    }

    public void ParsingThreeRoomNovostroiki(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < 12; i++)
        {
          Thread.Sleep(random.Next(2000, 3000));
          string prodam = $@"https://www.bkn.ru/prodazha/novostroiki/trehkomnatnye-kvartiry?page={i}";

          webClient.Encoding = Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);

          var newApartment = document.GetElementsByClassName("main NewApartment");
          if (newApartment.Length == 0)
            break;
          ParseThreeRoomNovostroikiMain(newApartment, sw);
        }
      }
    }

    private void ParseThreeRoomNovostroikiMain(IHtmlCollection<IElement> collection, StreamWriter sw)
    {
      Random random = new Random();
      using (var wb = new WebClient())
      {
        for (int i = 0; i < collection.Length; i++)
        {
          try
          {
            if (!collection[i].GetElementsByClassName("name")[0].TextContent.Contains("3-комн"))
            {
              var str = collection[i].GetElementsByClassName("name")[0].GetAttribute("href");
              var hrefGk = $@"https://www.bkn.ru{str}";

              wb.Encoding = Encoding.UTF8;
              var responce = wb.DownloadString(hrefGk);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var content = document.GetElementsByClassName("complex-mode-content")[0];
              var list = content.GetElementsByClassName("row offset-bottom-30");
              if (list.Length < 5)
                break;
              var elementOneRoomGk = list[4];
              var nameElement = elementOneRoomGk.GetElementsByClassName("bold nopadding nomargin font-size-20")[0].TextContent;
              if (nameElement != "Трехкомнатные квартиры")
                elementOneRoomGk = list[5];

              int length = 30;
              for (int j = 1; j < length; j++)
              {
                if (elementOneRoomGk.GetElementsByClassName("white-focus-font btn button-red-noradius").Length == 0)
                  break;
                str = elementOneRoomGk.GetElementsByClassName("white-focus-font btn button-red-noradius")[0].GetAttribute("href").Replace("[]=0", $@"%5b%5d=0&Page={j}");
                var hrefStudiiGk = $@"https://www.bkn.ru{str}";
                Thread.Sleep(random.Next(2000, 3000));

                responce = wb.DownloadString(hrefStudiiGk);
                document = parser.Parse(responce);

                var newApartment = document.GetElementsByClassName("main NewApartment");
                Parse(newApartment, "3 км. кв.", sw);
              }
            }
            else
            {
              var parent = collection[i].GetElementsByClassName("name")[0].PreviousElementSibling;
              //var a = parent.PreviousElementSibling;
              //ParseOneElement(parent, "Студия", sw);
            }
          }
          catch (Exception ex)
          {
            var str = ex.Message;
          }
        }
      }
    }
  }
}
