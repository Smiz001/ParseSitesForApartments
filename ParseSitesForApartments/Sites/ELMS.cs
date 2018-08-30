using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace ParseSitesForApartments.Sites
{
  public class ELMS : BaseParse
  {
    private List<int> listDistrict = new List<int>() { 38, 12, 43, 13, 4, 20, 6, 14, 7, 15, 8, 39, 9 };
    public override void ParsingAll()
    {
      throw new NotImplementedException();
    }

    public void ParseStudii()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = 0; i < listDistrict.Count; i++)
        {
          for (int j = 0; j < 5; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r0/1/is_auction/2/place/address/reg/2/dept/2/dist/{i}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("html_table_tr_1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheet(tableElements);
          }
        }
      }
    }

    public void ParseSheet(IHtmlCollection<IElement> collection)
    {
      for (int i = 0; i < collection.Length; i++)
      {
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

            //sw.WriteLine($@"{street};{number};{rooms};{square};{price};{floor};{metro};{distance}");
          }
        }
      }
    }
  }
}
}
