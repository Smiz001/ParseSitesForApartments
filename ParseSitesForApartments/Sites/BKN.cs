using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System;
using System.Data.SqlClient;
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
    private const string Apartaments = "main Apartments";
    private const string NewApartaments = "main NewApartment";
    private static object locker = new object();

    public override string Filename => @"d:\ParserInfo\Appartament\BKNProdam.csv";
    public override string FilenameSdam => @"d:\ParserInfo\Appartament\BKNSdam.csv";
    public override string FilenameWithinfo => @"d:\ParserInfo\Appartament\BKNProdamWithInfo.csv";
    public override string FilenameWithinfoSdam => @"d:\ParserInfo\Appartament\BKNSdamWithInfo.csv";
    public override string NameSite => "БКН";

    public override void ParsingAll()
    {
      var random = new Random();
      using (var sw = new StreamWriter(Filename, true, System.Text.Encoding.UTF8))
      {
        sw.WriteLine($@"Район;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние");
      }
      ParsingVtorichka();
      //ParsingNovostroiki();
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
    public void ParsingStudioVtorichka()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.bkn.ru/prodazha/vtorichka/studii?page={i}";
        if (!LinkProcessingProdam(prodam, "Студия"))
          break;
      }
      MessageBox.Show("Закончили Студии");
    }
    public void ParsingOneRoomVtorichka()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.bkn.ru/prodazha/kvartiri/odnokomnatnye-kvartiry?page={i}";
        if (!LinkProcessingProdam(prodam, "1 км. кв."))
          break;
      }
      MessageBox.Show("Закончили 1");
    }
    public void ParsingTwoRoomVtorichka()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.bkn.ru/prodazha/vtorichka/dvuhkomnatnye-kvartiry?page={i}";
        if (!LinkProcessingProdam(prodam, "2 км. кв."))
          break;
      }
      MessageBox.Show("Закончили 2");
    }
    public void ParsingThreeRoomVtorichka()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.bkn.ru/prodazha/vtorichka/trehkomnatnye-kvartiry?page={i}";
        if (!LinkProcessingProdam(prodam, "3 км. кв."))
          break;
      }
      MessageBox.Show("Закончили 3");
    }
    public void ParsingFourRoomVtorichka()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.bkn.ru/prodazha/vtorichka/chetyrehkomnatnye-kvartiry?page={i}";
        if (!LinkProcessingProdam(prodam, "4 км. кв."))
          break;
      }
      MessageBox.Show("Закончили 4");
    }
    public void ParsingFiveAndMoreRoomVtorichka()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.bkn.ru/prodazha/vtorichka/pyatikomnatnye-kvartiry?page={i}";
        if (!LinkProcessingProdam(prodam, "4 км. кв."))
          break;
      }
      MessageBox.Show("Закончили 5+");
    }

    private bool LinkProcessingProdam(string link, string typeRoom)
    {
      try
      {
        using (var webClient = new WebClient())
        {
          var random = new Random();
          Thread.Sleep(random.Next(2000, 4000));
          //ServicePointManager.Expect100Continue = true;
          //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
          //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

          webClient.Encoding = Encoding.UTF8;
          var responce = webClient.DownloadString(link);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);

          var collections = document.GetElementsByClassName(Apartaments);
          if (collections.Length > 0)
            ParsingSheet(typeRoom, collections);
          else
            return false;
        }
      }
      catch
      {
        return false;
      }
      return true;
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
        var flat = new Flat();
        flat.CountRoom = typeRoom;

        var priceDiv = collection[j].GetElementsByClassName("price overflow");
        if (priceDiv.Length == 0)
          break;
        else
        {
          var regex = new Regex(@"(\d+\,\d+\sм2)|(\d+\sм2)");
          var title = collection[j].GetElementsByClassName("title")[0].TextContent;
          flat.Square = regex.Match(title).Value.Replace(".", ",");
          if (typeRoom == "5 км. кв.")
          {
            regex = new Regex(@"(\d\-)");
            flat.CountRoom = regex.Match(title).Value.Replace("-", " км. кв.");
          }

          regex = new Regex(@"(\d+)");
          var ms = regex.Matches(priceDiv[0].TextContent);
          var price = int.Parse($"{ms[0].Value}{ms[1].Value}{ms[2].Value}");
          flat.Price = price;

          regex = new Regex(@"(\d+)");
          ms = regex.Matches(collection[j].GetElementsByClassName("floor overflow")[0].TextContent);
          if (ms.Count > 0)
            flat.Floor = ms[0].Value;
          else
            flat.Floor = "";

          district = collection[j].GetElementsByClassName("overflow")[2].TextContent;
          flat.Building.Street = collection[j].GetElementsByClassName("overflow")[3].TextContent;

          if (flat.Building.Street.Contains("Заводская"))
          {
            string a = "";
          }

          if (flat.Building.Street.Contains("Сестрорецк г."))
          {
            town = "Сестрорецк г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Шушары пос."))
          {
            town = "Шушары пос.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Петергоф г."))
          {
            town = "Петергоф г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Пушкин г."))
          {
            town = "Пушкин г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Зеленогорск г."))
          {
            town = "Зеленогорск г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Металлострой пос."))
          {
            town = "Металлострой пос.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Колпино г."))
          {
            town = "Колпино г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Парголово пос."))
          {
            town = "Парголово пос.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Красное Село г."))
          {
            town = "Красное Село г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Понтонный пос"))
          {
            town = "Понтонный пос";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("г. Петергоф"))
          {
            town = "Петергоф г.";
            flat.Building.Street = flat.Building.Street.Replace("г. Петергоф", "");
          }
          else if (flat.Building.Street.Contains("Ломоносов г."))
          {
            town = "Ломоносов г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Стрельна г."))
          {
            town = "Стрельна г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Павловск г."))
          {
            town = "Павловск г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Кронштадт г."))
          {
            town = "Кронштадт г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Санкт-Петербург г."))
          {
            town = "Санкт-Петербург г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else
            town = "Санкт-Петербург г.";

          regex = new Regex(@"(д\.\s+\d+\,\s+к\.\s+\d+)");
          flat.Building.Number = regex.Match(flat.Building.Street).Value;

          if (!string.IsNullOrWhiteSpace(flat.Building.Number))
          {
            flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
            regex = new Regex(@"(к\.\s+\d+)");
            flat.Building.Structure = regex.Match(flat.Building.Number).Value;
            flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "");

            flat.Building.Structure = flat.Building.Structure.Replace("к.", "").Trim();
            flat.Building.Number = flat.Building.Number.Replace("д.", "").Replace(",", "").Trim();
          }
          else
          {
            regex = new Regex(@"(д\.\s+\d+К\d+)");
            flat.Building.Number = regex.Match(flat.Building.Street).Value;

            if (string.IsNullOrWhiteSpace(flat.Building.Number))
            {
              regex = new Regex(@"(прд\.\,\d+)");
              flat.Building.Number = regex.Match(flat.Building.Street).Value;
              if (string.IsNullOrWhiteSpace(flat.Building.Number))
              {
                regex = new Regex(@"(д\.\d+\s+к\.\d+)");
                flat.Building.Number = regex.Match(flat.Building.Street).Value;

                if (string.IsNullOrWhiteSpace(flat.Building.Number))
                {
                  regex = new Regex(@"(д\.\s+\d+$)");
                  flat.Building.Number = regex.Match(flat.Building.Street).Value;

                  if (string.IsNullOrWhiteSpace(flat.Building.Number))
                  {
                    regex = new Regex(@"(д\.\s+\d+\/\d+)");
                    flat.Building.Number = regex.Match(flat.Building.Street).Value;
                    if (string.IsNullOrWhiteSpace(flat.Building.Number))
                    {
                      regex = new Regex(@"(д\.\d+\,\s+к\.\d+)");
                      flat.Building.Number = regex.Match(flat.Building.Street).Value;
                      if (string.IsNullOrWhiteSpace(flat.Building.Number))
                      {
                        regex = new Regex(@"(д\.\s+\d+\s+лит\.\s+\D)");
                        flat.Building.Number = regex.Match(flat.Building.Street).Value;
                        if (string.IsNullOrWhiteSpace(flat.Building.Number))
                        {
                          regex = new Regex(@"(д\.\d+\s+К\,\s+к\.\d+)");
                          flat.Building.Number = regex.Match(flat.Building.Street).Value;
                          if (string.IsNullOrWhiteSpace(flat.Building.Number))
                          {
                            regex = new Regex(@"(д\.\d+)");
                            flat.Building.Number = regex.Match(flat.Building.Street).Value;
                            if (string.IsNullOrWhiteSpace(flat.Building.Number))
                            {
                              regex = new Regex(@"(\d+\s+к\.\d+)");
                              flat.Building.Number = regex.Match(flat.Building.Street).Value;
                              if (string.IsNullOrWhiteSpace(flat.Building.Number))
                              {
                                regex = new Regex(@"(д\.\s+\d+\D\s+к\.\s+\d+)");
                                flat.Building.Number = regex.Match(flat.Building.Street).Value;
                                if (string.IsNullOrWhiteSpace(flat.Building.Number))
                                {
                                  regex = new Regex(@"(д\.\s+\d+)");
                                  flat.Building.Number = regex.Match(flat.Building.Street).Value;
                                  if (string.IsNullOrWhiteSpace(flat.Building.Number))
                                  {

                                  }
                                  else
                                  {
                                    flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                                    flat.Building.Number = flat.Building.Number.Replace("д.", "").Trim();
                                  }
                                }
                                else
                                {
                                  flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                                  regex = new Regex(@"(к\.\s+\d+)");
                                  flat.Building.Structure = regex.Match(flat.Building.Number).Value;
                                  flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Trim();
                                  regex = new Regex(@"(д\.\s+\d+)");
                                  var num = regex.Match(flat.Building.Number).Value;
                                  flat.Building.Liter = flat.Building.Number.Replace(num, "");
                                  flat.Building.Number = num.Replace("д.", "").Trim();
                                }
                              }
                              else
                              {
                                flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                                regex = new Regex(@"(к\.\d+)");
                                flat.Building.Structure = regex.Match(flat.Building.Number).Value;
                                flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Trim();
                                flat.Building.Structure = flat.Building.Structure.Replace("к.", "");
                              }
                            }
                            else
                            {
                              flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                              flat.Building.Number = flat.Building.Number.Replace("д.", "");
                            }
                          }
                          else
                          {
                            flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                            regex = new Regex(@"(к\.\d+)");
                            flat.Building.Structure = regex.Match(flat.Building.Number).Value;
                            flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Trim();
                            flat.Building.Structure = flat.Building.Structure.Replace("к.","");
                            regex = new Regex(@"(д\.\d+)");
                            flat.Building.Number = regex.Match(flat.Building.Number).Value.Replace("д.", "").Trim();
                          }
                        }
                        else
                        {
                          flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                          regex = new Regex(@"(лит\.\s+\D)");
                          flat.Building.Liter = regex.Match(flat.Building.Number).Value;
                          flat.Building.Number = flat.Building.Number.Replace(flat.Building.Liter, "").Replace("д.", "").Trim();
                          flat.Building.Liter = flat.Building.Liter.Replace("лит.", "").Trim();
                        }
                      }
                      else
                      {
                        flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                        regex = new Regex(@"(к\.\d+)");
                        flat.Building.Structure = regex.Match(flat.Building.Number).Value;
                        flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("д.", "").Replace(",", "").Trim();
                        flat.Building.Structure = flat.Building.Structure.Replace("к.", "");
                      }

                    }
                    else
                    {
                      flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                      var arr = flat.Building.Number.Split('/');
                      flat.Building.Number = arr[0].Replace("д.", "").Trim();
                      flat.Building.Structure = arr[1];
                    }
                  }
                  else
                  {
                    flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                    flat.Building.Number = flat.Building.Number.Replace("д.", "").Trim();
                  }
                }
                else
                {
                  flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                  regex = new Regex(@"(к\.\d+)");
                  flat.Building.Structure = regex.Match(flat.Building.Number).Value;
                  flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("д.", "").Trim();
                  flat.Building.Structure = flat.Building.Structure.Replace("к.", "");
                }
              }
              else
              {
                flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                flat.Building.Number = flat.Building.Number.Replace("прд.,", "");
              }
            }
            else
            {
              flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
              regex = new Regex(@"(К\d+)");
              flat.Building.Structure = regex.Match(flat.Building.Number).Value;
              flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("д.", "");
              flat.Building.Structure = flat.Building.Structure.Replace("К", "");
            }
          }
          flat.Building.Metro = collection[j].GetElementsByClassName("subwaystring")[0].TextContent;

          regex = new Regex(@"(\d+\sмин.\sна\sтранспорте)|(\d+\sмин\.\sпешком)");
          flat.Building.Distance = regex.Match(flat.Building.Metro).Value;

          flat.Building.Street = flat.Building.Street.Replace("ул.", "").Replace("просп.", "").Replace("пр-кт", "").Replace("пер.", "").Replace("шос.", "").Replace("пр.", "").Replace("лит. а", "").Replace("лит. А", "").Replace("стр. 3", "").Replace("стр. 1", "").Replace("стр. 2", "").Replace("б-р.", "").Replace(" б", "").Replace("пр-д", "").Replace("тер.", "").Replace("пл.", "").Replace(",", "").Replace(".", "").Replace("-1","").Trim();

          regex = new Regex(@"(к\d+)");
          var struc = regex.Match(flat.Building.Street).Value;
          if (!string.IsNullOrEmpty(struc))
          {
            flat.Building.Structure = struc.Replace("к", "");
            flat.Building.Street = flat.Building.Street.Replace(struc, "");
          }

          regex = new Regex(@"(кП\d+)");
          var kp = regex.Match(flat.Building.Street).Value;
          if (!string.IsNullOrEmpty(kp))
            flat.Building.Street = flat.Building.Street.Replace(kp, "").Trim();

          if (!string.IsNullOrWhiteSpace(flat.Building.Distance))
            flat.Building.Metro = flat.Building.Metro.Replace(flat.Building.Distance, "").Replace("●", "").Replace(",", "").Trim();

          regex = new Regex(@"(\d+ пеш)");
          var forDelete = regex.Match(flat.Building.Metro).Value;
          if(!string.IsNullOrEmpty(forDelete))
          {
            flat.Building.Metro = flat.Building.Metro.Replace(forDelete, "");
          }

          regex = new Regex(@"(\d+ тр)");
          forDelete = regex.Match(flat.Building.Metro).Value;
          if (!string.IsNullOrEmpty(forDelete))
          {
            flat.Building.Metro = flat.Building.Metro.Replace(forDelete, "");
          }

          regex = new Regex(@"(\d+ ост, \d+ остановок)");
          forDelete = regex.Match(flat.Building.Metro).Value;
          if (!string.IsNullOrEmpty(forDelete))
          {
            flat.Building.Metro = flat.Building.Metro.Replace(forDelete, "");
          }
          flat.Building.Metro = flat.Building.Metro.Replace("Пл.", "").Replace("пр.", "").Replace("Пр.", "").Replace("●", "").Replace("Пл. А.", "").Trim();


          Monitor.Enter(locker);
          if (!string.IsNullOrEmpty(flat.Building.Number))
          {
            using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Open), Encoding.UTF8))
            {
              sw.BaseStream.Position = sw.BaseStream.Length;
              sw.WriteLine($@"{town};{flat.Building.Street};{flat.Building.Number};{flat.Building.Structure};{flat.Building.Liter};{flat.CountRoom};{flat.Square};{flat.Price};{flat.Floor};{flat.Building.Metro};{flat.Building.Distance}");
            }
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

      var flat = new Flat();
      flat.CountRoom = typeRoom;

      var priceDiv = element.GetElementsByClassName("price overflow");
      if (priceDiv.Length == 0)
        return;
      else
      {
        var regex = new Regex(@"(\d+\,\d+\sм2)|(\d+\sм2)");
        var title = element.GetElementsByClassName("title")[0].TextContent;
        flat.Square = regex.Match(title).Value;

        regex = new Regex(@"(\d+)");
        var ms = regex.Matches(priceDiv[0].TextContent);
        var price = int.Parse($"{ms[0].Value}{ms[1].Value}{ms[2].Value}");
        flat.Price = price;

        regex = new Regex(@"(\d+)");
        ms = regex.Matches(element.GetElementsByClassName("floor overflow")[0].TextContent);
        if (ms.Count > 0)
          flat.Floor = ms[0].Value;
        else
          flat.Floor = "";

        district = element.GetElementsByClassName("overflow")[2].TextContent;
        flat.Building.Street = element.GetElementsByClassName("overflow")[3].TextContent;

        regex = new Regex(@"(д. \d+)|(\d+)");
        flat.Building.Number = regex.Match(flat.Building.Street).Value.Replace("д. ", "");

        regex = new Regex(@"(к. \d+)");
        building = regex.Match(flat.Building.Street).Value.Replace("к. ", "");

        if (flat.Building.Street.Contains("Сестрорецк г."))
        {
          town = "Сестрорецк г.";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else if (flat.Building.Street.Contains("Шушары пос."))
        {
          town = "Шушары пос.";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else if (flat.Building.Street.Contains("Петергоф г."))
        {
          town = "Петергоф г.";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else if (flat.Building.Street.Contains("Пушкин г."))
        {
          town = "Пушкин г.";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else if (flat.Building.Street.Contains("Зеленогорск г."))
        {
          town = "Зеленогорск г.";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else if (flat.Building.Street.Contains("Металлострой пос."))
        {
          town = "Металлострой пос.";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else if (flat.Building.Street.Contains("Колпино г."))
        {
          town = "Колпино г.";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else if (flat.Building.Street.Contains("Парголово пос."))
        {
          town = "Парголово пос.";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else if (flat.Building.Street.Contains("Красное Село г."))
        {
          town = "Красное Село г.";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else if (flat.Building.Street.Contains("Понтонный пос"))
        {
          town = "Понтонный пос";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else if (flat.Building.Street.Contains("Санкт-Петербург г."))
        {
          town = "Санкт-Петербург г.";
          flat.Building.Street = flat.Building.Street.Replace(town, "");
        }
        else
          town = "Санкт-Петербург г.";

        if (string.IsNullOrEmpty(building))
        {
          if (string.IsNullOrEmpty(flat.Building.Number))
            flat.Building.Street = flat.Building.Street.Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();
          else
            flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "").Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();
        }
        else if (string.IsNullOrEmpty(flat.Building.Number))
        {
          if (string.IsNullOrEmpty(building))
            flat.Building.Street = flat.Building.Street.Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();
          else
            flat.Building.Street = flat.Building.Street.Replace(building, "").Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();
        }
        else
          flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "").Replace(building, "").Replace(",", "").Replace("к.", "").Replace("д.", "").Replace("шос.", "").Replace("просп.", "").Replace("ул.", "").Trim();

        flat.Building.Metro = element.GetElementsByClassName("subwaystring")[0].TextContent;

        regex = new Regex(@"(\d+\sмин.\sна\sтранспорте)|(\d+\sмин\.\sпешком)");
        flat.Building.Distance = regex.Match(flat.Building.Metro).Value;

        if (!string.IsNullOrWhiteSpace(flat.Building.Distance))
          flat.Building.Metro = flat.Building.Metro.Replace(flat.Building.Distance, "").Replace("●", "").Replace(",", "").Trim();

        sw.WriteLine($@"{town};{flat.Building.Street};{flat.Building.Number};{building};{flat.CountRoom};{flat.Square};{flat.Price};{flat.Floor};{flat.Building.Metro};{flat.Building.Distance}");

      }
    }

    public void ParsingNovostroiki()
    {
      var studiiThread = new Thread(ParsingStudioNovostroiki);
      studiiThread.Start();
      Thread.Sleep(55000);
      //ParsingOneRoomNovostroiki();
      //ParsingTwoRoomNovostroiki();
      //ParsingThreeRoomNovostroiki();
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

          var newApartment = document.GetElementsByClassName(NewApartaments);
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
                Thread.Sleep(random.Next(2000, 3000));

                responce = wb.DownloadString(hrefStudiiGk);
                document = parser.Parse(responce);

                var newApartment = document.GetElementsByClassName("main NewApartment");
                ParsingSheet("Студия Н", newApartment);
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

          var newApartment = document.GetElementsByClassName(NewApartaments);
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
                    ParsingSheet("1 км. кв. Н", newApartment);
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

          var newApartment = document.GetElementsByClassName(NewApartaments);
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
                ParsingSheet("2 км. кв. Н", newApartment);
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

          var newApartment = document.GetElementsByClassName(NewApartaments);
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
                ParsingSheet("3 км. кв. Н", newApartment);
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

    public override void ParsingSdamAll()
    {
      using (var sw = new StreamWriter(FilenameSdam, true, System.Text.Encoding.UTF8))
      {
        sw.WriteLine($@"Нас. пункт;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние");
      }

      var studiiThread = new Thread(ParseStudiiSdam);
      studiiThread.Start();
      Thread.Sleep(55000);
      var oneThread = new Thread(ParseOneSdam);
      oneThread.Start();
      Thread.Sleep(55000);
      var twoThread = new Thread(ParseTwoSdam);
      twoThread.Start();
      Thread.Sleep(55000);
      var threeThread = new Thread(ParseThreeSdam);
      threeThread.Start();
      Thread.Sleep(55000);
      var fourThread = new Thread(ParseFourSdam);
      fourThread.Start();
      Thread.Sleep(55000);
      var fiveThread = new Thread(ParseFiveSdam);
      fiveThread.Start();
    }

    public void ParseStudiiSdam()
    {
      try
      {
        using (var webClient = new WebClient())
        {
          Random random = new Random();
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(5000, 6000));
            string prodam = $@"https://www.bkn.ru/arenda/vtorichka/studii?page={i}";

            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(prodam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var apartment = document.GetElementsByClassName("main Apartments");
            if (apartment.Length > 0)
              ParsingSheetSdam("Студия", apartment);
            else
              break;
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      MessageBox.Show("Закончили студии");
    }
    public void ParseOneSdam()
    {
      try
      {
        using (var webClient = new WebClient())
        {
          Random random = new Random();
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(5000, 6000));
            string prodam = $@"https://www.bkn.ru/arenda/vtorichka/odnokomnatnye-kvartiry?page={i}";

            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(prodam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var apartment = document.GetElementsByClassName("main Apartments");
            if (apartment.Length > 0)
              ParsingSheetSdam("1 км. кв.", apartment);
            else
              break;
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      MessageBox.Show("Закончили 1 км. кв.");
    }
    public void ParseTwoSdam()
    {
      try
      {
        using (var webClient = new WebClient())
        {
          Random random = new Random();
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(5000, 6000));
            string prodam = $@"https://www.bkn.ru/arenda/vtorichka/dvuhkomnatnye-kvartiry?page={i}";

            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(prodam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var apartment = document.GetElementsByClassName("main Apartments");
            if (apartment.Length > 0)
              ParsingSheetSdam("2 км. кв.", apartment);
            else
              break;
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      MessageBox.Show("Закончили 2 км. кв.");
    }
    public void ParseThreeSdam()
    {
      try
      {
        using (var webClient = new WebClient())
        {
          Random random = new Random();
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(5000, 6000));
            string prodam = $@"https://www.bkn.ru/arenda/vtorichka/trehkomnatnye-kvartiry?page={i}";

            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(prodam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var apartment = document.GetElementsByClassName("main Apartments");
            if (apartment.Length > 0)
              ParsingSheetSdam("3 км. кв.", apartment);
            else
              break;
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      MessageBox.Show("Закончили 3 км. кв.");
    }
    public void ParseFourSdam()
    {
      try
      {
        using (var webClient = new WebClient())
        {
          Random random = new Random();
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(5000, 6000));
            string prodam = $@"https://www.bkn.ru/arenda/vtorichka/chetyrehkomnatnye-kvartiry?page={i}";

            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(prodam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var apartment = document.GetElementsByClassName("main Apartments");
            if (apartment.Length > 0)
              ParsingSheetSdam("4 км. кв.", apartment);
            else
              break;
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      MessageBox.Show("Закончили 4 км. кв.");
    }
    public void ParseFiveSdam()
    {
      try
      {
        using (var webClient = new WebClient())
        {
          Random random = new Random();
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(5000, 6000));
            string prodam = $@"https://www.bkn.ru/arenda/vtorichka/pyatikomnatnye-kvartiry?page={i}";

            webClient.Encoding = Encoding.UTF8;
            var responce = webClient.DownloadString(prodam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var apartment = document.GetElementsByClassName("main Apartments");
            if (apartment.Length > 0)
              ParsingSheetSdam("5 км. кв.", apartment);
            else
              break;
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      MessageBox.Show("Закончили 5+ км. кв.");
    }

    private void ParsingSheetSdam(string typeRoom, IHtmlCollection<IElement> collection)
    {
      string year = string.Empty;
      string distanceInMinute = string.Empty;
      string district = string.Empty;
      string building = string.Empty;

      for (int j = 0; j < collection.Length; j++)
      {
        string town = string.Empty;
        var flat = new Flat();
        flat.CountRoom = typeRoom;

        var priceDiv = collection[j].GetElementsByClassName("price overflow");
        if (priceDiv.Length == 0)
          break;
        else if (priceDiv[0].TextContent.Contains("в сутки"))
          break;
        else
        {
          var regex = new Regex(@"(\d+\,\d+\sм2)|(\d+\sм2)");
          var title = collection[j].GetElementsByClassName("title")[0].TextContent;
          flat.Square = regex.Match(title).Value;
          if (typeRoom == "5 км. кв.")
          {
            regex = new Regex(@"(\d\-)");
            flat.CountRoom = regex.Match(title).Value.Replace("-", " км. кв.");
          }

          regex = new Regex(@"(\d+)");
          var ms = regex.Matches(priceDiv[0].TextContent);
          var price = int.Parse($"{ms[0].Value}{ms[1].Value}");
          flat.Price = price;

          regex = new Regex(@"(\d+)");
          ms = regex.Matches(collection[j].GetElementsByClassName("floor overflow")[0].TextContent);
          if (ms.Count > 0)
            flat.Floor = ms[0].Value;
          else
            flat.Floor = "";

          district = collection[j].GetElementsByClassName("overflow")[2].TextContent;
          flat.Building.Street = collection[j].GetElementsByClassName("overflow")[3].TextContent;

          if (flat.Building.Street.Contains("Сестрорецк г."))
          {
            town = "Сестрорецк г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Шушары пос."))
          {
            town = "Шушары пос.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Петергоф г."))
          {
            town = "Петергоф г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Пушкин г."))
          {
            town = "Пушкин г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Зеленогорск г."))
          {
            town = "Зеленогорск г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Металлострой пос."))
          {
            town = "Металлострой пос.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Колпино г."))
          {
            town = "Колпино г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Парголово пос."))
          {
            town = "Парголово пос.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Красное Село г."))
          {
            town = "Красное Село г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Понтонный пос"))
          {
            town = "Понтонный пос";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("г. Петергоф"))
          {
            town = "Петергоф г.";
            flat.Building.Street = flat.Building.Street.Replace("г. Петергоф", "");
          }
          else if (flat.Building.Street.Contains("Ломоносов г."))
          {
            town = "Ломоносов г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Стрельна г."))
          {
            town = "Стрельна г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Павловск г."))
          {
            town = "Павловск г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Кронштадт г."))
          {
            town = "Кронштадт г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else if (flat.Building.Street.Contains("Санкт-Петербург г."))
          {
            town = "Санкт-Петербург г.";
            flat.Building.Street = flat.Building.Street.Replace(town, "");
          }
          else
            town = "Санкт-Петербург г.";

          regex = new Regex(@"(д\.\s+\d+\,\s+к\.\s+\d+)");
          flat.Building.Number = regex.Match(flat.Building.Street).Value;

          if (!string.IsNullOrWhiteSpace(flat.Building.Number))
          {
            flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
            regex = new Regex(@"(к\.\s+\d+)");
            flat.Building.Structure = regex.Match(flat.Building.Number).Value;
            flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "");

            flat.Building.Structure = flat.Building.Structure.Replace("к.", "").Trim();
            flat.Building.Number = flat.Building.Number.Replace("д.", "").Replace(",", "").Trim();
          }
          else
          {
            regex = new Regex(@"(д\.\s+\d+К\d+)");
            flat.Building.Number = regex.Match(flat.Building.Street).Value;

            if (string.IsNullOrWhiteSpace(flat.Building.Number))
            {
              regex = new Regex(@"(прд\.\,\d+)");
              flat.Building.Number = regex.Match(flat.Building.Street).Value;
              if (string.IsNullOrWhiteSpace(flat.Building.Number))
              {
                regex = new Regex(@"(д\.\d+\s+к\.\d+)");
                flat.Building.Number = regex.Match(flat.Building.Street).Value;

                if (string.IsNullOrWhiteSpace(flat.Building.Number))
                {
                  regex = new Regex(@"(д\.\s+\d+$)");
                  flat.Building.Number = regex.Match(flat.Building.Street).Value;

                  if (string.IsNullOrWhiteSpace(flat.Building.Number))
                  {
                    regex = new Regex(@"(д\.\s+\d+\/\d+)");
                    flat.Building.Number = regex.Match(flat.Building.Street).Value;
                    if (string.IsNullOrWhiteSpace(flat.Building.Number))
                    {
                      regex = new Regex(@"(д\.\d+\,\s+к\.\d+)");
                      flat.Building.Number = regex.Match(flat.Building.Street).Value;
                      if (string.IsNullOrWhiteSpace(flat.Building.Number))
                      {
                        regex = new Regex(@"(д\.\s+\d+\s+лит\.\s+\D)");
                        flat.Building.Number = regex.Match(flat.Building.Street).Value;
                        if (string.IsNullOrWhiteSpace(flat.Building.Number))
                        {
                          regex = new Regex(@"(д\.\d+)");
                          flat.Building.Number = regex.Match(flat.Building.Street).Value;
                          if (string.IsNullOrWhiteSpace(flat.Building.Number))
                          {
                            regex = new Regex(@"(\d+\s+к\.\d+)");
                            flat.Building.Number = regex.Match(flat.Building.Street).Value;
                            if (string.IsNullOrWhiteSpace(flat.Building.Number))
                            {
                              regex = new Regex(@"(д\.\s+\d+\D\s+к\.\s+\d+)");
                              flat.Building.Number = regex.Match(flat.Building.Street).Value;
                              if (string.IsNullOrWhiteSpace(flat.Building.Number))
                              {
                                regex = new Regex(@"(д\.\s+\d+)");
                                flat.Building.Number = regex.Match(flat.Building.Street).Value;
                                if (string.IsNullOrWhiteSpace(flat.Building.Number))
                                {

                                }
                                else
                                {
                                  flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                                  flat.Building.Number = flat.Building.Number.Replace("д.", "").Trim();
                                }
                              }
                              else
                              {
                                flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                                regex = new Regex(@"(к\.\s+\d+)");
                                flat.Building.Structure = regex.Match(flat.Building.Number).Value;
                                flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Trim();
                                regex = new Regex(@"(д\.\s+\d+)");
                                var num = regex.Match(flat.Building.Number).Value;
                                flat.Building.Liter = flat.Building.Number.Replace(num, "");
                                flat.Building.Number = num.Replace("д.", "").Trim();
                              }
                            }
                            else
                            {
                              flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                              regex = new Regex(@"(к\.\d+)");
                              flat.Building.Structure = regex.Match(flat.Building.Number).Value;
                              flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Trim();
                              flat.Building.Structure = flat.Building.Structure.Replace("к.", "");
                            }
                          }
                          else
                          {
                            flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                            flat.Building.Number = flat.Building.Number.Replace("д.", "");
                          }
                        }
                        else
                        {
                          flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                          regex = new Regex(@"(лит\.\s+\D)");
                          flat.Building.Liter = regex.Match(flat.Building.Number).Value;
                          flat.Building.Number = flat.Building.Number.Replace(flat.Building.Liter, "").Replace("д.", "").Trim();
                          flat.Building.Liter = flat.Building.Liter.Replace("лит.", "").Trim();
                        }
                      }
                      else
                      {
                        flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                        regex = new Regex(@"(к\.\d+)");
                        flat.Building.Structure = regex.Match(flat.Building.Number).Value;
                        flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("д.", "").Replace(",", "").Trim();
                        flat.Building.Structure = flat.Building.Structure.Replace("к.", "");
                      }

                    }
                    else
                    {
                      flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                      var arr = flat.Building.Number.Split('/');
                      flat.Building.Number = arr[0].Replace("д.", "").Trim();
                      flat.Building.Structure = arr[1];
                    }
                  }
                  else
                  {
                    flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                    flat.Building.Number = flat.Building.Number.Replace("д.", "").Trim();
                  }
                }
                else
                {
                  flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                  regex = new Regex(@"(к\.\d+)");
                  flat.Building.Structure = regex.Match(flat.Building.Number).Value;
                  flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("д.", "").Trim();
                  flat.Building.Structure = flat.Building.Structure.Replace("к.", "");
                }
              }
              else
              {
                flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
                flat.Building.Number = flat.Building.Number.Replace("прд.,", "");
              }
            }
            else
            {
              flat.Building.Street = flat.Building.Street.Replace(flat.Building.Number, "");
              regex = new Regex(@"(К\d+)");
              flat.Building.Structure = regex.Match(flat.Building.Number).Value;
              flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("д.", "");
              flat.Building.Structure = flat.Building.Structure.Replace("К", "");
            }
          }

          flat.Building.Metro = collection[j].GetElementsByClassName("subwaystring")[0].TextContent;

          regex = new Regex(@"(\d+\sмин.\sна\sтранспорте)|(\d+\sмин\.\sпешком)");
          flat.Building.Distance = regex.Match(flat.Building.Metro).Value;

          flat.Building.Street = flat.Building.Street.Replace("ул.", "").Replace("просп.", "").Replace("пр-кт", "").Replace("пер.", "").Replace("шос.", "").Replace("пр.", "").Replace("лит. а", "").Replace("лит. А", "").Replace("стр. 3", "").Replace("стр. 1", "").Replace("стр. 2", "").Replace("б-р.", "").Replace(" б", "").Replace("пр-д", "").Replace("тер.", "").Replace("пл.", "").Replace(",", "").Replace(".", "").Trim();

          if (!string.IsNullOrWhiteSpace(flat.Building.Distance))
            flat.Building.Metro = flat.Building.Metro.Replace(flat.Building.Distance, "").Replace("●", "").Replace(",", "").Trim();

          Monitor.Enter(locker);
          if (!string.IsNullOrEmpty(flat.Building.Number))
          {
            using (var sw = new StreamWriter(new FileStream(FilenameSdam, FileMode.Open), Encoding.UTF8))
            {
              sw.BaseStream.Position = sw.BaseStream.Length;
              sw.WriteLine($@";{flat.Building.Street};{flat.Building.Number};{flat.Building.Structure};{flat.Building.Liter};{flat.CountRoom};{flat.Square};{flat.Price};{flat.Floor};{flat.Building.Metro};{flat.Building.Distance}");
            }
          }
          Monitor.Exit(locker);
        }
      }
    }
  }
}
