using AngleSharp.Parser.Html;
using System;
using System.Net;
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
    
      using (var webClient = new WebClient())
      {
        for (int i = pageMin; i < pageMaz; i++)
        {
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam?p={i}";
          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);

          document.GetElementsByClassName("item_table-header");
        }
      }
    }
  }
}
