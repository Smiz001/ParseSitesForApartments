using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ParseSitesForApartments.Sites
{
  public class BKN : BaseParse
  {
    private int minPage = 1;
    private int maxPage = 100;
    private const string Filename = @"D:\BKNProdam.csv";
    private static object locker = new object();

    public override void ParsingAll()
    {
      var random = new Random();
      using (var sw = new StreamWriter(@"D:\BKNProdam.csv", true, System.Text.Encoding.UTF8))
      {
        sw.WriteLine($@"Нас. пункт;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние");
      }
      ParsingVtorichka();
    }

    public void ParsingVtorichka()
    {
      var studiiThread = new Thread(ParsingStudioVtorichka);
      studiiThread.Start();
      Thread.Sleep(55000);
      var oneThread = new Thread(ParsingOneRoomVtorichka);
      oneThread.Start();
      Thread.Sleep(55000);
      var twoThread = new Thread(ParsingTwoRoomVtorichka);
      twoThread.Start();
      Thread.Sleep(55000);
      var threeThread = new Thread(ParsingThreeRoomVtorichka);
      threeThread.Start();
      Thread.Sleep(55000);
      var fourThread = new Thread(ParsingFourRoomVtorichka);
      fourThread.Start();
      Thread.Sleep(55000);
      var fiveThread = new Thread(ParsingFiveAndMoreRoomVtorichka);
      fiveThread.Start();
    }

    public void ParsingStudioVtorichka( )
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

          var apartment = document.GetElementsByClassName("main Apartments");
          if (apartment.Length > 0)
            ParsingSheet("Студия", apartment);
          else
            break;
        }
      }
      MessageBox.Show("Закончили Студии");
    }

    public void ParsingOneRoomVtorichka()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.bkn.ru/prodazha/kvartiri/odnokomnatnye-kvartiry?page={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);

          var apartment = document.GetElementsByClassName("main Apartments");
          if (apartment.Length > 0)
            ParsingSheet("1 км. кв.", apartment);
          else
            break;
        }
      }
      MessageBox.Show("Закончили 1");
    }

    public void ParsingTwoRoomVtorichka()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.bkn.ru/prodazha/vtorichka/dvuhkomnatnye-kvartiry?page={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          var apartment = document.GetElementsByClassName("main Apartments");
          if (apartment.Length > 0)
            ParsingSheet("2 км. кв.", apartment);
          else
            break;
        }
      }
      MessageBox.Show("Закончили 2");
    }

    public void ParsingThreeRoomVtorichka()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.bkn.ru/prodazha/vtorichka/trehkomnatnye-kvartiry?page={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          var apartment = document.GetElementsByClassName("main Apartments");
          if (apartment.Length > 0)
            ParsingSheet("3 км. кв.", apartment);
          else
            break;
        }
      }
      MessageBox.Show("Закончили 3");
    }

    public void ParsingFourRoomVtorichka()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.bkn.ru/prodazha/vtorichka/chetyrehkomnatnye-kvartiry?page={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          var apartment = document.GetElementsByClassName("main Apartments");
          if (apartment.Length > 0)
            ParsingSheet("4 км. кв.", apartment);
          else
            break;
        }
      }
      MessageBox.Show("Закончили 4");
    }

    public void ParsingFiveAndMoreRoomVtorichka()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.bkn.ru/prodazha/vtorichka/pyatikomnatnye-kvartiry?page={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          var apartment = document.GetElementsByClassName("main Apartments");
          if (apartment.Length > 0)
            ParsingSheet("5 км. кв.", apartment);
          else
            break;
        }
      }
      MessageBox.Show("Закончили 5+");
    }

    private void ParsingSheet(string typeRoom, IHtmlCollection<IElement> collection)
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
          else if (build.Street.Contains("г. Петергоф"))
          {
            town = "Петергоф г.";
            build.Street = build.Street.Replace("г. Петергоф", "");
          }
          else if (build.Street.Contains("Ломоносов г."))
          {
            town = "Ломоносов г.";
            build.Street = build.Street.Replace(town, "");
          }
          else if (build.Street.Contains("Стрельна г."))
          {
            town = "Стрельна г.";
            build.Street = build.Street.Replace(town, "");
          }
          else if (build.Street.Contains("Павловск г."))
          {
            town = "Павловск г.";
            build.Street = build.Street.Replace(town, "");
          }
          else if (build.Street.Contains("Кронштадт г."))
          {
            town = "Кронштадт г.";
            build.Street = build.Street.Replace(town, "");
          }
          else if (build.Street.Contains("Санкт-Петербург г."))
          {
            town = "Санкт-Петербург г.";
            build.Street = build.Street.Replace(town, "");
          }
          else
            town = "Санкт-Петербург г.";

          regex = new Regex(@"(д\.\s+\d+\,\s+к\.\s+\d+)");
          build.Number = regex.Match(build.Street).Value;

          if(!string.IsNullOrWhiteSpace(build.Number))
          {
            build.Street = build.Street.Replace(build.Number,"");
            regex = new Regex(@"(к\.\s+\d+)");
            build.Building = regex.Match(build.Number).Value;
            build.Number = build.Number.Replace(build.Building, "");

            build.Building = build.Building.Replace("к.", "").Trim();
            build.Number = build.Number.Replace("д.", "").Replace(",", "").Trim();
          }
          else
          {
            regex = new Regex(@"(д\.\s+\d+К\d+)");
            build.Number = regex.Match(build.Street).Value;

            if(string.IsNullOrWhiteSpace(build.Number))
            {
              regex = new Regex(@"(прд\.\,\d+)");
              build.Number = regex.Match(build.Street).Value;
              if (string.IsNullOrWhiteSpace(build.Number))
              {
                regex = new Regex(@"(д\.\d+\s+к\.\d+)");
                build.Number = regex.Match(build.Street).Value;

                if (string.IsNullOrWhiteSpace(build.Number))
                {
                  regex = new Regex(@"(д\.\s+\d+$)");
                  build.Number = regex.Match(build.Street).Value;

                  if (string.IsNullOrWhiteSpace(build.Number))
                  {
                    regex = new Regex(@"(д\.\s+\d+\/\d+)");
                    build.Number = regex.Match(build.Street).Value;
                    if (string.IsNullOrWhiteSpace(build.Number))
                    {
                      regex = new Regex(@"(д\.\d+\,\s+к\.\d+)");
                      build.Number = regex.Match(build.Street).Value;
                      if (string.IsNullOrWhiteSpace(build.Number))
                      {
                        regex = new Regex(@"(д\.\s+\d+\s+лит\.\s+\D)");
                        build.Number = regex.Match(build.Street).Value;
                        if (string.IsNullOrWhiteSpace(build.Number))
                        {
                          regex = new Regex(@"(д\.\d+)");
                          build.Number = regex.Match(build.Street).Value;
                          if (string.IsNullOrWhiteSpace(build.Number))
                          {
                            regex = new Regex(@"(\d+\s+к\.\d+)");
                            build.Number = regex.Match(build.Street).Value;
                            if (string.IsNullOrWhiteSpace(build.Number))
                            {
                              regex = new Regex(@"(д\.\s+\d+\D\s+к\.\s+\d+)");
                              build.Number = regex.Match(build.Street).Value;
                              if (string.IsNullOrWhiteSpace(build.Number))
                              {
                                regex = new Regex(@"(д\.\s+\d+)");
                                build.Number = regex.Match(build.Street).Value;
                                if (string.IsNullOrWhiteSpace(build.Number))
                                {

                                }
                                else
                                {
                                  build.Street = build.Street.Replace(build.Number, "");
                                  build.Number = build.Number.Replace("д.", "").Trim();
                                }
                              }
                              else
                              {
                                build.Street = build.Street.Replace(build.Number, "");
                                regex = new Regex(@"(к\.\s+\d+)");
                                build.Building = regex.Match(build.Number).Value;
                                build.Number = build.Number.Replace(build.Building, "").Trim();
                                regex = new Regex(@"(д\.\s+\d+)");
                                var num = regex.Match(build.Number).Value;
                                build.Liter = build.Number.Replace(num,"");
                                build.Number = num.Replace("д.","").Trim();
                              }
                            }
                            else
                            {
                              build.Street = build.Street.Replace(build.Number, "");
                              regex = new Regex(@"(к\.\d+)");
                              build.Building = regex.Match(build.Number).Value;
                              build.Number = build.Number.Replace(build.Building, "").Trim();
                              build.Building = build.Building.Replace("к.", "");
                            }
                          }
                          else
                          {
                            build.Street = build.Street.Replace(build.Number, "");
                            build.Number = build.Number.Replace("д.","");
                          }
                        }
                        else
                        {
                          build.Street = build.Street.Replace(build.Number, "");
                          regex = new Regex(@"(лит\.\s+\D)");
                          build.Liter = regex.Match(build.Number).Value;
                          build.Number = build.Number.Replace(build.Liter, "").Replace("д.", "").Trim();
                          build.Liter = build.Liter.Replace("лит.", "").Trim();
                        }
                      }
                      else
                      {
                        build.Street = build.Street.Replace(build.Number, "");
                        regex = new Regex(@"(к\.\d+)");
                        build.Building = regex.Match(build.Number).Value;
                        build.Number = build.Number.Replace(build.Building, "").Replace("д.","").Replace(",", "").Trim();
                        build.Building = build.Building.Replace("к.","");
                      }

                    }
                    else
                    {
                      build.Street = build.Street.Replace(build.Number, "");
                      var arr = build.Number.Split('/');
                      build.Number = arr[0].Replace("д.","").Trim();
                      build.Building = arr[1];
                    }
                  }
                  else
                  {
                    build.Street = build.Street.Replace(build.Number, "");
                    build.Number = build.Number.Replace("д.","").Trim();
                  }
                }
                else
                {
                  build.Street = build.Street.Replace(build.Number, "");
                  regex = new Regex(@"(к\.\d+)");
                  build.Building = regex.Match(build.Number).Value;
                  build.Number = build.Number.Replace(build.Building, "").Replace("д.", "").Trim();
                  build.Building = build.Building.Replace("к.", "");
                }
              }
              else
              {
                build.Street = build.Street.Replace(build.Number, "");
                build.Number = build.Number.Replace("прд.,", "");
              }
            }
            else
            {
              build.Street = build.Street.Replace(build.Number, "");
              regex = new Regex(@"(К\d+)");
              build.Building = regex.Match(build.Number).Value;
              build.Number = build.Number.Replace(build.Building, "").Replace("д.","");
              build.Building = build.Building.Replace("К", "");
            }
          }

          build.Metro = collection[j].GetElementsByClassName("subwaystring")[0].TextContent;

          regex = new Regex(@"(\d+\sмин.\sна\sтранспорте)|(\d+\sмин\.\sпешком)");
          build.Distance = regex.Match(build.Metro).Value;

          build.Street = build.Street.Replace("ул.", "").Replace("просп.", "").Replace("пр-кт", "").Replace("пер.", "").Replace("шос.", "").Replace("пр.", "").Replace("лит. а", "").Replace("лит. А", "").Replace("стр. 3", "").Replace("стр. 1", "").Replace("стр. 2", "").Replace("б-р.", "").Replace(" б", "").Replace("пр-д", "").Replace("тер.", "").Replace("пл.", "").Replace(",", "").Replace(".", "").Trim();

          if (!string.IsNullOrWhiteSpace(build.Distance))
            build.Metro = build.Metro.Replace(build.Distance, "").Replace("●", "").Replace(",", "").Trim();

          Monitor.Enter(locker);
          using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Open), Encoding.UTF8))
          {
            sw.BaseStream.Position = sw.BaseStream.Length;
            sw.WriteLine($@"{town};{build.Street};{build.Number};{build.Building};{build.Liter};{build.CountRoom};{build.Square};{build.Price};{build.Floor};{build.Metro};{build.Distance}");
          }
          Monitor.Exit(locker);
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

    public void ParsingNovostroiki()
    {
      ParsingStudioNovostroiki();
      ParsingOneRoomNovostroiki();
      ParsingTwoRoomNovostroiki();
      ParsingThreeRoomNovostroiki();
    }

    public void ParsingStudioNovostroiki()
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
          ParseStudiiNovostroikiMain(newApartment);
        }
      }
    }

    private void ParseStudiiNovostroikiMain(IHtmlCollection<IElement> collection)
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
                ParsingSheet("Студия", newApartment);
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

    public void ParsingOneRoomNovostroiki()
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
          ParseOneRoomNovostroikiMain(newApartment);
        }
      }
    }

    private void ParseOneRoomNovostroikiMain(IHtmlCollection<IElement> collection)
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
              if (listBold.Length != 0)
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
                    ParsingSheet("1 км. кв.", newApartment);
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

    public void ParsingTwoRoomNovostroiki()
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
          ParseTwoRoomNovostroikiMain(newApartment);
        }
      }
    }

    private void ParseTwoRoomNovostroikiMain(IHtmlCollection<IElement> collection)
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
                ParsingSheet("2 км. кв.", newApartment);
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

    public void ParsingThreeRoomNovostroiki()
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
          ParseThreeRoomNovostroikiMain(newApartment);
        }
      }
    }

    private void ParseThreeRoomNovostroikiMain(IHtmlCollection<IElement> collection)
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
                ParsingSheet("3 км. кв.", newApartment);
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
