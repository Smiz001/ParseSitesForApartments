using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
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
        ParsingStudio(sw);
        ParsingOneRoom(sw);
        ParsingTwoRoom(sw);
        ParsingThreeRoom(sw);
        ParsingFourRoom(sw);
        ParsingFiveAndMoreRoom(sw);
      }
    }

    public void ParsingStudio(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.bkn.ru/prodazha/kvartiri/studii?page={i}";

          webClient.Encoding = Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          if (!ParsingSheet("Студия", sw, document))
            return;
        }
      }
    }

    public void ParsingOneRoom(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.bkn.ru/prodazha/kvartiri/odnokomnatnye-kvartiry?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          if(!ParsingSheet("1 км. кв.", sw, document))
             return;
        }
      }
    }

    public void ParsingTwoRoom(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.bkn.ru/prodazha/kvartiri/dvuhkomnatnye-kvartiry?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          if (!ParsingSheet("2 км. кв.", sw, document))
            return;
        }
      }
    }

    public void ParsingThreeRoom(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.bkn.ru/prodazha/kvartiri/trehkomnatnye-kvartiry?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          if (!ParsingSheet("3 км. кв.", sw, document))
            return;
        }
      }
    }

    public void ParsingFourRoom(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.bkn.ru/prodazha/kvartiri/chetyrehkomnatnye-kvartiry?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          if (!ParsingSheet("4 км. кв.", sw, document))
            return;
        }
      }
    }

    public void ParsingFiveAndMoreRoom(StreamWriter sw)
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.bkn.ru/prodazha/kvartiri/pyatikomnatnye-kvartiry?p={i}";

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
          if(ms.Count>0)
           build.Floor = ms[0].Value;
          else
            build.Floor = "";

          district = collection[j].GetElementsByClassName("overflow")[2].TextContent;
          build.Street = collection[j].GetElementsByClassName("overflow")[3].TextContent;

          regex = new Regex(@"(д. \d+)|(\d+)");
          build.Number = regex.Match(build.Street).Value.Replace("д. ", "");

          regex = new Regex(@"(к. \d+)");
          building = regex.Match(build.Street).Value.Replace("к. ", "");

          if(build.Street.Contains("Сестрорецк г."))
          {
            town = "Сестрорецк г.";
            build.Street = build.Street.Replace(town, "");
          }
          else if(build.Street.Contains("Шушары пос."))
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
          else
            town = "Санкт-Петербург";

          if (string.IsNullOrEmpty(building))
          {
            if (string.IsNullOrEmpty(build.Number))
              build.Street = build.Street.Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();
            else
              build.Street = build.Street.Replace(build.Number, "").Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();
          }
          else if(string.IsNullOrEmpty(build.Number))
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
  }
}
