using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace ParseSitesForApartments
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      int pageMin = 1;
      int pageMaz = 100;

      List<Build> list = new List<Build>();
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        using (var sw = new StreamWriter(@"D:\Avito1.csv", true, System.Text.Encoding.UTF8))
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
              build.Metro = adress.ChildNodes[2].NodeValue.Trim();
              build.Distance = adresses[k].GetElementsByClassName("c-2")[0].TextContent;
              sw.WriteLine($@"{build.Adress};{build.Price};{build.CountRoom};{build.Square};{build.Floor}");
            }
          }
        }
      }
    }
  }
}
