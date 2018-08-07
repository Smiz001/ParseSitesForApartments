using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
        {
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

                var adArr = adress.TextContent.Split(',');
                build.Street = adArr[adArr.Length - 2].Replace("проспект", "").Replace("пр.", "").Replace("пр-т", "").Replace("ул.", "").Replace("улица", "").Trim();
                build.Building = adArr[adArr.Length - 1];

                sw.WriteLine($@"{build.Street};{build.Building};{build.Price};{build.CountRoom};{build.Square};{build.Floor}");
              }
            }
          }
        }
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      /*
      using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
      {
        connection.Open();
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
      }
      */
    }
  }
}
