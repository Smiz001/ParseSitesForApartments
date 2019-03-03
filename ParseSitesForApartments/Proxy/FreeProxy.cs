using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;

namespace ParseSitesForApartments.Proxy
{
  //https://free.proxy-sale.com
  public class FreeProxy
  {
    private static Dictionary<string, int> dictionaryProxy;
    private static object syncRoot = new object();
    private static int counPage = 8;

    public static Dictionary<string, int> DictionaryProxy()
    {
      lock (syncRoot)
      {
        if (dictionaryProxy == null)
        {
          dictionaryProxy = new Dictionary<string, int>();
          GetProxy();
        }
      }
      return dictionaryProxy;
    }

    private static void GetProxy()
    {
      HtmlParser parser = new HtmlParser();
      using (var webClient = new WebClient())
      {
        string url;
        for (int i = 1; i <= counPage; i++)
        {
          url =
            $@"https://free.proxy-sale.com/http/?proxy_country=%5B%22Ukraine%22%5D&proxy_type=%5B%222%22%5D&proxy_anonymity=%5B%222%22%5D&proxy_page=1";
        }
      }
    }
  }
}
