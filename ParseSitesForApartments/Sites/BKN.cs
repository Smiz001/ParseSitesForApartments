using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ParseSitesForApartments.Sites
{
  public class BKN : BaseParse
  {
    private int minPage = 1;
    private int maxPage = 100;
    private object webClient;

    public override void ParsingAll()
    {
      var random = new Random();
      using (var sw = new StreamWriter(@"D:\BKNProdam.csv", true, System.Text.Encoding.UTF8))
      {


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
          ParsingSheet("Студия", sw, document);
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
          ParsingSheet("1 км. кв.", sw, document);
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
          ParsingSheet("2 км. кв.", sw, document);
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
          ParsingSheet("3 км. кв.", sw, document);
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
          ParsingSheet("4 км. кв.", sw, document);
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
          ParsingSheet("5 км. кв. и более", sw, document);
        }
      }
    }

    private void ParsingSheet(string typeRoom, StreamWriter sw, IHtmlDocument document)
    {
      string rooms = string.Empty;
      string square = string.Empty;
      string year = string.Empty;
      string price = string.Empty;
      string floor = string.Empty;
      string street = string.Empty;
      string number = string.Empty;
      string metro = string.Empty;
      string distanceInMinute = string.Empty;
      string district = string.Empty;
      string building = string.Empty;

      var newApartment = document.GetElementsByClassName("main NewApartment");
      var apartment = document.GetElementsByClassName("main Apartments");
      if (newApartment.Length == 0 && apartment.Length == 0)
        return;

      for (int j = 0; j < apartment.Length; j++)
      {
        var priceDiv = apartment[j].GetElementsByClassName("price overflow");
        if (priceDiv.Length == 0)
          break;
        else
        {
          Regex regex = new Regex(@"(\d+\,\d+\sм2)|(\d+\sм2)");
          var title = apartment[j].GetElementsByClassName("title")[0].TextContent;
          square = regex.Match(title).Value;
          rooms = title.Replace(square, "").Trim();
          price = priceDiv[0].TextContent.Replace(" ", "").Replace("a", "");

          floor = apartment[j].GetElementsByClassName("floor overflow")[0].TextContent;
          district = apartment[j].GetElementsByClassName("overflow")[2].TextContent;
          street = apartment[j].GetElementsByClassName("overflow")[3].TextContent;

          regex = new Regex(@"(д. \d+)|(\d+)");
          number = regex.Match(street).Value;

          regex = new Regex(@"(к. \d+)");
          building = regex.Match(street).Value;

          if (string.IsNullOrEmpty(building))
            street = street.Replace(number, "").Replace(",", "").Replace(" ", "");
          else
            street = street.Replace(number, "").Replace(building, "").Replace(",", "").Replace(" ", "");

          metro = apartment[j].GetElementsByClassName("subwaystring")[0].TextContent;

          sw.WriteLine($@"{street};{number};{building};{rooms};{square};{price};{floor};{metro}");
        }
      }

      for (int j = 0; j < newApartment.Length; j++)
      {
        var priceDiv = newApartment[j].GetElementsByClassName("price overflow");
        if (priceDiv.Length == 0)
          break;
        else
        {
          Regex regex = new Regex(@"(\d+\,\d+\sм2)|(\d+\sм2)");
          var title = apartment[j].GetElementsByClassName("title")[0].TextContent;
          square = regex.Match(title).Value;
          rooms = title.Replace(square, "").Trim();
          price = priceDiv[0].TextContent.Replace(" ", "").Replace("a", "");

          floor = apartment[j].GetElementsByClassName("floor overflow")[0].TextContent;
          district = apartment[j].GetElementsByClassName("overflow")[2].TextContent;
          street = apartment[j].GetElementsByClassName("overflow")[3].TextContent;

          regex = new Regex(@"(д. \d+)|(\d+)");
          number = regex.Match(street).Value;

          regex = new Regex(@"(к. \d+)");
          building = regex.Match(street).Value;

          if (string.IsNullOrEmpty(building))
            street = street.Replace(number, "").Replace(",", "").Replace(" ", "");
          else
            street = street.Replace(number, "").Replace(building, "").Replace(",", "").Replace(" ", "");

          metro = apartment[j].GetElementsByClassName("subwaystring")[0].TextContent;

          sw.WriteLine($@"{street};{number};{building};{rooms};{square};{price};{floor};{metro}");
        }
      }
    }
  }
}
