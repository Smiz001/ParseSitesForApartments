using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ParseSitesForApartments.Sites
{
  public class Avito:BaseParse
  {
    private int minPage = 1;
    private int maxPage = 100;

    private const string Filename = @"D:\AvitoProdam.csv";
    private const string FilenameSdam = @"D:\AvitoSdam.csv";
    private const string FilenameWithinfo = @"D:\AvitoProdamWithInfo.csv";
    private List<string> stantions = new List<string>() { "Автово","Адмиралтейская","Академическая","Балтийская", "Беговая", "Бухарестская","Василеостровская","Владимирская","Волковская","Выборгская","Горьковская","Гостиный двор","Гражданский проспект","Девяткино","Достоевская","Елизаровская","Звёздная","Звенигородская","Кировский завод","Комендантский проспект","Крестовский остров","Купчино","Ладожская","Ленинский проспект","Лесная","Лиговский проспект","Ломоносовская","Маяковская","Международная","Московская","Московские ворота","Нарвская","Невский проспект", "Новокрестовская", "Новочеркасская","Обводный канал","Обухово","Озерки","Парк Победы","Парнас","Петроградская","Пионерская","Площадь Александра Невского","Площадь Восстания","Площадь Ленина","Площадь Мужества","Политехническая","Приморская","Пролетарская","Проспект Большевиков","Проспект Ветеранов","Проспект Просвещения","Пушкинская","Рыбацкое","Садовая","Сенная площадь","Спасская","Спортивная","Старая Деревня","Технологический институт","Удельная","Улица Дыбенко","Фрунзенская","Чёрная речка","Чернышевская","Чкаловская","Электросила"};

    private static object locker = new object();

    public void Parsing0_3Rooms()
    {
      using (var sw = new StreamWriter(Filename, true, System.Text.Encoding.UTF8))
      {
        sw.WriteLine($@"Нас. пункт;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние");
      }
      var studiiThread = new Thread(ParsingStudio);
      studiiThread.Start();
      Thread.Sleep(55000);
      var oneThread = new Thread(ParsingOneRoom);
      oneThread.Start();
      Thread.Sleep(55000);
      var twoThread = new Thread(ParsingTwoRoom);
      twoThread.Start();
      Thread.Sleep(55000);
      var threeThread = new Thread(ParsingThreeRoom);
      threeThread.Start();
    }

    public void Parsing4_9Rooms()
    {
      var fourThread = new Thread(ParsingFourRoom);
      fourThread.Start();
      Thread.Sleep(55000);
      var fiveThread = new Thread(ParsingFiveRoom);
      fiveThread.Start();
      Thread.Sleep(55000);
      var sixThread = new Thread(ParsingSixRoom);
      sixThread.Start();
      Thread.Sleep(55000);
      var sevenThread = new Thread(ParsingSevenRoom);
      sevenThread.Start();
      Thread.Sleep(55000);
      var eightThread = new Thread(ParsingEightRoom);
      eightThread.Start();
      Thread.Sleep(55000);
      var nineThread = new Thread(ParsingNineRoom);
      nineThread.Start();
      Thread.Sleep(55000);
      //ParsingMoreNineRoom();
    }

    public void ParsingStudio()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/studii?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);

          var collections = document.GetElementsByClassName("description item_table-description");
          if (collections.Length > 0)
            ParsingSheet("Студия", collections);
          else
            break;
        }
      }
    }
    public void ParsingOneRoom()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 6000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/1-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);

          var collections = document.GetElementsByClassName("description item_table-description");
          if (collections.Length > 0)
            ParsingSheet("1 км.кв.", collections);
          else
            break;
        }
      }
    }
    public void ParsingTwoRoom()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/2-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          var collections = document.GetElementsByClassName("description item_table-description");
          if (collections.Length > 0)
            ParsingSheet("2 км.кв.", collections);
          else
            break;
        }
      }
    }
    public void ParsingThreeRoom()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/3-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          var collections = document.GetElementsByClassName("description item_table-description");
          if (collections.Length > 0)
            ParsingSheet("3 км.кв.", collections);
          else
            break;
        }
      }
    }
    public void ParsingFourRoom()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/4-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          var collections = document.GetElementsByClassName("description item_table-description");
          if (collections.Length > 0)
            ParsingSheet("4 км.кв.", collections);
          else
            break;
        }
      }
    }
    public void ParsingFiveRoom()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/5-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          var collections = document.GetElementsByClassName("description item_table-description");
          if (collections.Length > 0)
            ParsingSheet("5 км.кв.", collections);
          else
            break;
        }
      }
    }
    public void ParsingSixRoom()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/6-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          var collections = document.GetElementsByClassName("description item_table-description");
          if (collections.Length > 0)
            ParsingSheet("6 км.кв.", collections);
          else
            break;
        }
      }
    }
    public void ParsingSevenRoom()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/7-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          var collections = document.GetElementsByClassName("description item_table-description");
          if (collections.Length > 0)
            ParsingSheet("7 км.кв.", collections);
          else
            break;
        }
      }
    }
    public void ParsingEightRoom()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/8-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          var collections = document.GetElementsByClassName("description item_table-description");
          if (collections.Length > 0)
            ParsingSheet("8 км.кв.", collections);
          else
            break;
        }
      }
    }
    public void ParsingNineRoom()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/9-komnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          var collections = document.GetElementsByClassName("description item_table-description");
          if (collections.Length > 0)
            ParsingSheet("9 км.кв.", collections);
          else
            break;
        }
      }
    }
    public void ParsingMoreNineRoom()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/mnogokomnatnye?p={i}";

          webClient.Encoding = System.Text.Encoding.UTF8;
          var responce = webClient.DownloadString(prodam);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);
          var collections = document.GetElementsByClassName("description item_table-description");
          if (collections.Length > 0)
            ParsingSheet(">9 км.кв.", collections);
          else
            break;
        }
      }
    }

    private void ParsingSheet(string typeRoom, IHtmlCollection<IElement> collection)
    {
      for (int k = 0; k < collection.Length; k++)
      {
        var build = new Build();
        build.CountRoom = typeRoom;

        build.Price = int.Parse(collection[k].GetElementsByClassName("price")[0].TextContent.Trim('\n').Trim('₽').Trim().Replace(" ", ""));

        var aboutBuild = collection[k].GetElementsByClassName("item-description-title-link")[0].TextContent;
        var regex = new Regex(@"(\d+\s+м²)");
        build.Square = regex.Match(aboutBuild).Value;
        regex = new Regex(@"(\d+\/\d+)");
        var floor = regex.Match(aboutBuild).Value;
        regex = new Regex(@"(\/\d+)");
        build.Floor = floor.Replace(regex.Match(floor).Value,"");

        var adress = collection[k].GetElementsByClassName("address");
        int count = 0;
        if (adress.Length > 0)
        {
          var adres = adress[0].TextContent.Trim();

          var distance = collection[k].GetElementsByClassName("c-2");
          if (distance.Length > 0)
            build.Distance = distance[0].TextContent.Trim();
          adres = adres.Replace(build.Distance,"").Replace("Санкт-Петербург,","").Replace("посёлок Парголово,", "").Replace("СПб Красное село", "").Replace("г. Ломоносов,", "").Replace("Россия,", "").Replace("Сестрорецк г,", "").Replace("Сестрорецк", "").Replace("Парголово п,", "").Replace("Колпино,", "").Replace("Мурино,", "").Replace("посёлок Шушары,", "").Replace("г. Петергоф,", "");

          #region Удаление лишнего
          regex = new Regex(@"(\,\s+подъезд\s+\d+)|(\,\s+подъезд\d+)");
          var gov = regex.Match(adres).Value;
          if (!string.IsNullOrEmpty(gov))
            adres = adres.Replace(gov, "");

          regex = new Regex(@"(\,\s+стр\. \d+)|(\,\s+стр\.\s+\d+)|(\,\s+стр\.\d+)");
          gov = regex.Match(adres).Value;
          if (!string.IsNullOrEmpty(gov))
            adres = adres.Replace(gov, "");

          #endregion

          var ar = adres.Split(',');
          count = ar.Length;
          if (ar.Length == 3)
          {
            build.Metro = ar[0];
            build.Street = ar[1];
            build.Number = ar[2];
            build.Number = build.Number.Replace("А","").Replace("А", "").Replace("дом ", "").Replace("д.","").Trim();
          }
          else if(ar.Length == 1)
          {
            regex = new Regex(@"(д\. \d+\s+к\.\d+)|(д\.\s+\d+\s+к\.\d+)");
            build.Number = regex.Match(ar[0]).Value;
            if (!string.IsNullOrEmpty(build.Number))
            {
              build.Street = ar[0].Replace(build.Number, "").Trim();
              regex = new Regex(@"(к\.\d+)");
              build.Building = regex.Match(build.Number).Value;
              build.Number = build.Number.Replace(build.Building, "").Replace("д. ", "").Replace("д. ", "");
              build.Building = build.Building.Replace("к.","");
            }
            else
            {
              regex = new Regex(@"(д\.\d+\s+к\.\d+)");
              build.Number = regex.Match(ar[0]).Value;
              if (!string.IsNullOrEmpty(build.Number))
              {
                build.Street = ar[0].Replace(build.Number, "").Trim();
                regex = new Regex(@"(к\.\d+)");
                build.Building = regex.Match(build.Number).Value;
                build.Number = build.Number.Replace(build.Building, "").Replace("д.", "").Trim();
                build.Building = build.Building.Replace("к.", "");
              }
              else
              {
                regex = new Regex(@"(д\. \d+\s+корп\.\s+\d+)|(д\.\s+\d+\s+корп\.\s+\d+)");
                build.Number = regex.Match(ar[0]).Value;
                if (!string.IsNullOrEmpty(build.Number))
                {
                  build.Street = ar[0].Replace(build.Number, "").Trim();
                  regex = new Regex(@"(корп\.\s+\d+)");
                  build.Building = regex.Match(build.Number).Value;
                  build.Number = build.Number.Replace(build.Building, "").Replace("д. ", "").Replace("д. ", "");
                  build.Building = build.Building.Replace("корп.", "");
                }
                else
                {
                  regex = new Regex(@"(дом\s+\d+\s+корпус\s+\d+)");
                  build.Number = regex.Match(ar[0]).Value;
                  if (!string.IsNullOrEmpty(build.Number))
                  {
                    build.Street = ar[0].Replace(build.Number, "").Trim();
                    regex = new Regex(@"(корпус\s+\d+)");
                    build.Building = regex.Match(build.Number).Value;
                    build.Number = build.Number.Replace(build.Building, "").Replace("дом ", "").Replace("дом ", "");
                    build.Building = build.Building.Replace("корпус", "");
                  }
                  else
                  {
                    regex = new Regex(@"(дом\s+\d+\s+корп\.\s+\d+)");
                    build.Number = regex.Match(ar[0]).Value;
                    if (!string.IsNullOrEmpty(build.Number))
                    {
                      build.Street = ar[0].Replace(build.Number, "").Trim();
                      regex = new Regex(@"(корп\.\s+\d+)");
                      build.Building = regex.Match(build.Number).Value;
                      build.Number = build.Number.Replace(build.Building, "").Replace("дом ", "").Replace("дом ", "");
                      build.Building = build.Building.Replace("корп.", "");
                    }
                    else
                    {
                      regex = new Regex(@"(д\.\d+$)");
                      build.Number = regex.Match(ar[0]).Value;
                      if (!string.IsNullOrEmpty(build.Number))
                      {
                        build.Street = ar[0].Replace(build.Number, "").Trim();
                        build.Number = build.Number.Replace("д.", "");
                      }
                      else
                      {
                        regex = new Regex(@"(\d+с\d+$)");
                        build.Number = regex.Match(ar[0]).Value;
                        if (!string.IsNullOrEmpty(build.Number))
                        {
                          build.Street = ar[0].Replace(build.Number, "").Trim();
                        }
                        else
                        {
                          regex = new Regex(@"(д\d+$)");
                          build.Number = regex.Match(ar[0]).Value;
                          if (!string.IsNullOrEmpty(build.Number))
                          {
                            build.Street = ar[0].Replace(build.Number, "").Trim();
                          }
                          else
                          {
                            regex = new Regex(@"(\d+\/\d+)");
                            build.Number = regex.Match(ar[0]).Value;
                            if (!string.IsNullOrEmpty(build.Number))
                            {
                              build.Street = ar[0].Replace(build.Number, "").Trim();
                              build.Building = build.Number.Split('/')[1];
                              build.Number = build.Number.Split('/')[1];
                            }
                            else
                            {
                              regex = new Regex(@"(\d+$)");
                              build.Number = regex.Match(ar[0]).Value;
                              if (!string.IsNullOrEmpty(build.Number))
                              {
                                build.Street = ar[0].Replace(build.Number, "").Trim();
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
            build.Street = build.Street.Replace("Ул.","").Replace("г.", "").Replace(".", "").Trim();
          }
          else if(ar.Length == 2)
          {
            if(stantions.Contains(ar[0].Trim()))
            {
              build.Metro = ar[0].Trim();
              build.Street = ar[1];
            }
          }
          else if (ar.Length == 4)
          {
            if (stantions.Contains(ar[0].Trim()))
            {
              build.Metro = ar[0].Trim();
              build.Street = ar[1];
            }
          }
          else
            build.Street = adres;
        }


        regex = new Regex(@"(к\d+)");
        build.Building = regex.Match(build.Number).Value;
        if(string.IsNullOrWhiteSpace(build.Building))
        {
          regex = new Regex(@"(к \d+)");
          build.Building = regex.Match(build.Number).Value;
          if (string.IsNullOrWhiteSpace(build.Building))
          {
            regex = new Regex(@"(кор\.\d+)");
            build.Building = regex.Match(build.Number).Value;
            if (string.IsNullOrWhiteSpace(build.Building))
            {

            }
            else
            {
              build.Number = build.Number.Replace(build.Building, "").Trim();
              build.Building = build.Building.Replace("кор.", "");
            }
          }
          else
          {
            build.Number = build.Number.Replace(build.Building, "");
            build.Building = build.Building.Replace("к","");
          }
        }
        else
        {
          build.Number = build.Number.Replace(build.Building,"");
          build.Building = build.Building.Replace("к", "");
        }

        regex = new Regex(@"(\D$)");
        build.Liter = regex.Match(build.Number).Value;
        if(!string.IsNullOrEmpty(build.Liter))
        {
          build.Number = build.Number.Replace(build.Liter, "");
        }

        build.Street = build.Street.Replace("улица", "").Replace("ул. ", "").Replace("проезд", "").Replace("переулок", "").Replace("переулок", "").Replace("бульвар", "").Replace("б-р", "").Replace("проспект", "пр.").Replace("пр-кт", "пр.").Replace("Васильевского острова", "В.О.").Replace("Васильевского острова", "В.О.").Replace("Петроградской стороны", "П.С.").Replace("ш.", "").Replace("пер", "").Replace(" ул", "").Replace("аллея", "").Replace("дорога ", "").Replace("набережная ", "").Trim();

        regex = new Regex(@"(^пр\.)");
        var pr = regex.Match(build.Street).Value;
        if(!string.IsNullOrWhiteSpace(pr))
        {
          build.Street = build.Street.Replace(pr,"").Trim() + $" {pr}";
        }

        Monitor.Enter(locker);
        using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Open), Encoding.UTF8))
        {
          sw.BaseStream.Position = sw.BaseStream.Length;
          sw.WriteLine($@"{count};{build.Street};{build.Number};{build.Building};{build.Liter};{build.CountRoom};{build.Square};{build.Price};{build.Floor};{build.Metro};{build.Distance}");
        }
        Monitor.Exit(locker);
      }
    }

    public void GetInfoAboutBuilding()
    {
      if (File.Exists(Filename))
      {
        using (var sr = new StreamReader(Filename, Encoding.UTF8))
        {
          using (var sw = new StreamWriter(FilenameWithinfo, true, Encoding.UTF8))
          {
            using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
            {
              connection.Open();

              sw.WriteLine($@"Улица;Номер;Корпус;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Расстояние;Дата постройки;Дата реконструкции;Даты кап. ремонты;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Кол-во встроенных нежилых помещений;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов");
              string line;
              sr.ReadLine();
              while ((line = sr.ReadLine()) != null)
              {
                string street = string.Empty;
                string number = string.Empty;
                string building = string.Empty;
                string letter = string.Empty;
                string typeRoom = string.Empty;
                string square = string.Empty;
                string floor = string.Empty;
                string countFloor = string.Empty;
                string price = string.Empty;
                string metro = string.Empty;
                string distance = string.Empty;
                string dateBuild = string.Empty;
                string dateRecon = string.Empty;
                string dateRepair = string.Empty;
                string buildingSquare = string.Empty;
                string livingSquare = string.Empty;
                string noLivingSqaure = string.Empty;
                string residents = string.Empty;
                string mansardaSquare = string.Empty;
                string otoplenie = string.Empty;
                string gvs = string.Empty;
                string es = string.Empty;
                string gs = string.Empty;
                string typeApartaments = string.Empty;
                string countApartaments = string.Empty;
                string countInternal = string.Empty;
                DateTime dateTep = DateTime.Now;
                string typeRepair = string.Empty;
                string countLift = string.Empty;

                var arr = line.Split(';');
                street = arr[1];
                number = arr[2];
                building = arr[3];
                letter = arr[4];
                typeRoom = arr[5];
                square = arr[6];
                price = arr[7];
                floor = arr[8];
                metro = arr[9];
                distance = arr[10];

                string select = "";
                if (string.IsNullOrWhiteSpace(letter))
                {
                  if (string.IsNullOrWhiteSpace(building))
                  {
                    select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumber '{street}', '{number}'";
                  }
                  else
                  {
                    select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumberAndBuilbind '{street}', '{number}', '{building}'";
                  }
                }
                else
                {
                  if (string.IsNullOrWhiteSpace(building))
                  {
                    select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumberAndLetter '{street}', '{number}', '{letter}'";
                  }
                  else
                  {
                    select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumberAndBuilbindAndLetter '{street}', '{number}', '{building}', '{letter}'";
                  }
                }

                var command = new SqlCommand(select, connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                  dateBuild = reader.GetString(1);
                  dateRecon = reader.GetString(3);
                  dateRepair = reader.GetString(4);
                  buildingSquare = reader.GetDouble(5).ToString();
                  livingSquare = reader.GetDouble(6).ToString();
                  noLivingSqaure = reader.GetDouble(7).ToString();
                  countFloor = reader.GetInt32(9).ToString();
                  residents = reader.GetInt32(10).ToString();
                  mansardaSquare = reader.GetDouble(11).ToString();
                  otoplenie = reader.GetBoolean(12).ToString();
                  gvs = reader.GetBoolean(13).ToString();
                  es = reader.GetBoolean(14).ToString();
                  gs = reader.GetBoolean(15).ToString();
                  typeApartaments = reader.GetString(16);
                  countApartaments = reader.GetString(17);
                  countInternal = reader.GetInt32(18).ToString();
                  dateTep = reader.GetDateTime(19);
                  typeRepair = reader.GetString(21);
                  countLift = reader.GetInt32(22).ToString();
                }
                reader.Close();

                sw.WriteLine($@"{street};{number};{building};{typeRoom};{square};{floor};{countFloor};{price};{metro};{distance};{dateBuild};{dateRecon};{dateRepair};{buildingSquare};{livingSquare};{noLivingSqaure};{mansardaSquare};{residents};{otoplenie};{gvs};{es};{gs};{typeApartaments};{countApartaments};{countInternal};{dateTep.ToShortDateString()};{typeRepair};{countLift}");
              }
            }
          }
        }
      }
      else
      {
        MessageBox.Show("Нет файла с данными");
      }
    }

    public override void ParsingAll()
    {
     
    }

    public override void ParsingSdamAll()
    {
      using (var sw = new StreamWriter(FilenameSdam, true, System.Text.Encoding.UTF8))
      {
        sw.WriteLine($@"Нас. пункт;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние");
      }
      var studiiThread = new Thread(ParsingStudioSdam);
      studiiThread.Start();
      //Thread.Sleep(55000);
      //var oneThread = new Thread(ParsingOneRoom);
      //oneThread.Start();
      //Thread.Sleep(55000);
      //var twoThread = new Thread(ParsingTwoRoom);
      //twoThread.Start();
      //Thread.Sleep(55000);
      //var threeThread = new Thread(ParsingThreeRoom);
      //threeThread.Start();
    }

    public void ParsingStudioSdam()
    {
      try
      {
        using (var webClient = new WebClient())
        {
          Random random = new Random();
          for (int i = minPage; i < maxPage; i++)
          {
            Thread.Sleep(random.Next(9000, 12000));
            string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/sdam/na_dlitelnyy_srok/studii?p={i}";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            webClient.Encoding = System.Text.Encoding.UTF8;
            var responce = webClient.DownloadString(prodam);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var collections = document.GetElementsByClassName("description item_table-description");
            if (collections.Length > 0)
              ParsingSheetSdam("Студия", collections);
            else
              break;
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }

    }

    private void ParsingSheetSdam(string typeRoom, IHtmlCollection<IElement> collection)
    {
      for (int k = 0; k < collection.Length; k++)
      {
        var build = new Build();
        build.CountRoom = typeRoom;

        string price = collection[k].GetElementsByClassName("price")[0].TextContent.Trim('\n').Trim('₽').Replace(" ", "").Replace("₽\nвмесяц","");
        build.Price = int.Parse(price);

        var aboutBuild = collection[k].GetElementsByClassName("item-description-title-link")[0].TextContent;
        var regex = new Regex(@"(\d+\s+м²)");
        build.Square = regex.Match(aboutBuild).Value;
        regex = new Regex(@"(\d+\/\d+)");
        var floor = regex.Match(aboutBuild).Value;
        regex = new Regex(@"(\/\d+)");
        build.Floor = floor.Replace(regex.Match(floor).Value, "");

        var adress = collection[k].GetElementsByClassName("address");
        int count = 0;
        if (adress.Length > 0)
        {
          var adres = adress[0].TextContent.Trim();

          var distance = collection[k].GetElementsByClassName("c-2");
          if (distance.Length > 0)
            build.Distance = distance[0].TextContent.Trim();
          adres = adres.Replace(build.Distance, "").Replace("Санкт-Петербург,", "").Replace("посёлок Парголово,", "").Replace("СПб Красное село", "").Replace("г. Ломоносов,", "").Replace("Россия,", "").Replace("Сестрорецк г,", "").Replace("Сестрорецк", "").Replace("Парголово п,", "").Replace("Колпино,", "").Replace("Мурино,", "").Replace("посёлок Шушары,", "").Replace("г. Петергоф,", "");

          #region Удаление лишнего
          regex = new Regex(@"(\,\s+подъезд\s+\d+)|(\,\s+подъезд\d+)");
          var gov = regex.Match(adres).Value;
          if (!string.IsNullOrEmpty(gov))
            adres = adres.Replace(gov, "");

          regex = new Regex(@"(\,\s+стр\. \d+)|(\,\s+стр\.\s+\d+)|(\,\s+стр\.\d+)");
          gov = regex.Match(adres).Value;
          if (!string.IsNullOrEmpty(gov))
            adres = adres.Replace(gov, "");

          #endregion

          var ar = adres.Split(',');
          count = ar.Length;
          if (ar.Length == 3)
          {
            build.Metro = ar[0];
            build.Street = ar[1];
            build.Number = ar[2];
            build.Number = build.Number.Replace("А", "").Replace("А", "").Replace("дом ", "").Replace("д.", "").Trim();
          }
          else if (ar.Length == 1)
          {
            regex = new Regex(@"(д\. \d+\s+к\.\d+)|(д\.\s+\d+\s+к\.\d+)");
            build.Number = regex.Match(ar[0]).Value;
            if (!string.IsNullOrEmpty(build.Number))
            {
              build.Street = ar[0].Replace(build.Number, "").Trim();
              regex = new Regex(@"(к\.\d+)");
              build.Building = regex.Match(build.Number).Value;
              build.Number = build.Number.Replace(build.Building, "").Replace("д. ", "").Replace("д. ", "");
              build.Building = build.Building.Replace("к.", "");
            }
            else
            {
              regex = new Regex(@"(д\.\d+\s+к\.\d+)");
              build.Number = regex.Match(ar[0]).Value;
              if (!string.IsNullOrEmpty(build.Number))
              {
                build.Street = ar[0].Replace(build.Number, "").Trim();
                regex = new Regex(@"(к\.\d+)");
                build.Building = regex.Match(build.Number).Value;
                build.Number = build.Number.Replace(build.Building, "").Replace("д.", "").Trim();
                build.Building = build.Building.Replace("к.", "");
              }
              else
              {
                regex = new Regex(@"(д\. \d+\s+корп\.\s+\d+)|(д\.\s+\d+\s+корп\.\s+\d+)");
                build.Number = regex.Match(ar[0]).Value;
                if (!string.IsNullOrEmpty(build.Number))
                {
                  build.Street = ar[0].Replace(build.Number, "").Trim();
                  regex = new Regex(@"(корп\.\s+\d+)");
                  build.Building = regex.Match(build.Number).Value;
                  build.Number = build.Number.Replace(build.Building, "").Replace("д. ", "").Replace("д. ", "");
                  build.Building = build.Building.Replace("корп.", "");
                }
                else
                {
                  regex = new Regex(@"(дом\s+\d+\s+корпус\s+\d+)");
                  build.Number = regex.Match(ar[0]).Value;
                  if (!string.IsNullOrEmpty(build.Number))
                  {
                    build.Street = ar[0].Replace(build.Number, "").Trim();
                    regex = new Regex(@"(корпус\s+\d+)");
                    build.Building = regex.Match(build.Number).Value;
                    build.Number = build.Number.Replace(build.Building, "").Replace("дом ", "").Replace("дом ", "");
                    build.Building = build.Building.Replace("корпус", "");
                  }
                  else
                  {
                    regex = new Regex(@"(дом\s+\d+\s+корп\.\s+\d+)");
                    build.Number = regex.Match(ar[0]).Value;
                    if (!string.IsNullOrEmpty(build.Number))
                    {
                      build.Street = ar[0].Replace(build.Number, "").Trim();
                      regex = new Regex(@"(корп\.\s+\d+)");
                      build.Building = regex.Match(build.Number).Value;
                      build.Number = build.Number.Replace(build.Building, "").Replace("дом ", "").Replace("дом ", "");
                      build.Building = build.Building.Replace("корп.", "");
                    }
                    else
                    {
                      regex = new Regex(@"(д\.\d+$)");
                      build.Number = regex.Match(ar[0]).Value;
                      if (!string.IsNullOrEmpty(build.Number))
                      {
                        build.Street = ar[0].Replace(build.Number, "").Trim();
                        build.Number = build.Number.Replace("д.", "");
                      }
                      else
                      {
                        regex = new Regex(@"(\d+с\d+$)");
                        build.Number = regex.Match(ar[0]).Value;
                        if (!string.IsNullOrEmpty(build.Number))
                        {
                          build.Street = ar[0].Replace(build.Number, "").Trim();
                        }
                        else
                        {
                          regex = new Regex(@"(д\d+$)");
                          build.Number = regex.Match(ar[0]).Value;
                          if (!string.IsNullOrEmpty(build.Number))
                          {
                            build.Street = ar[0].Replace(build.Number, "").Trim();
                          }
                          else
                          {
                            regex = new Regex(@"(\d+\/\d+)");
                            build.Number = regex.Match(ar[0]).Value;
                            if (!string.IsNullOrEmpty(build.Number))
                            {
                              build.Street = ar[0].Replace(build.Number, "").Trim();
                              build.Building = build.Number.Split('/')[1];
                              build.Number = build.Number.Split('/')[1];
                            }
                            else
                            {
                              regex = new Regex(@"(\d+$)");
                              build.Number = regex.Match(ar[0]).Value;
                              if (!string.IsNullOrEmpty(build.Number))
                              {
                                build.Street = ar[0].Replace(build.Number, "").Trim();
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
            build.Street = build.Street.Replace("Ул.", "").Replace("г.", "").Replace(".", "").Trim();
          }
          else if (ar.Length == 2)
          {
            if (stantions.Contains(ar[0].Trim()))
            {
              build.Metro = ar[0].Trim();
              build.Street = ar[1];
            }
          }
          else if (ar.Length == 4)
          {
            if (stantions.Contains(ar[0].Trim()))
            {
              build.Metro = ar[0].Trim();
              build.Street = ar[1];
            }
          }
          else
            build.Street = adres;
        }


        regex = new Regex(@"(к\d+)");
        build.Building = regex.Match(build.Number).Value;
        if (string.IsNullOrWhiteSpace(build.Building))
        {
          regex = new Regex(@"(к \d+)");
          build.Building = regex.Match(build.Number).Value;
          if (string.IsNullOrWhiteSpace(build.Building))
          {
            regex = new Regex(@"(кор\.\d+)");
            build.Building = regex.Match(build.Number).Value;
            if (string.IsNullOrWhiteSpace(build.Building))
            {

            }
            else
            {
              build.Number = build.Number.Replace(build.Building, "").Trim();
              build.Building = build.Building.Replace("кор.", "");
            }
          }
          else
          {
            build.Number = build.Number.Replace(build.Building, "");
            build.Building = build.Building.Replace("к", "");
          }
        }
        else
        {
          build.Number = build.Number.Replace(build.Building, "");
          build.Building = build.Building.Replace("к", "");
        }

        regex = new Regex(@"(\D$)");
        build.Liter = regex.Match(build.Number).Value;
        if (!string.IsNullOrEmpty(build.Liter))
        {
          build.Number = build.Number.Replace(build.Liter, "");
        }

        build.Street = build.Street.Replace("улица", "").Replace("ул. ", "").Replace("проезд", "").Replace("переулок", "").Replace("переулок", "").Replace("бульвар", "").Replace("б-р", "").Replace("проспект", "пр.").Replace("пр-кт", "пр.").Replace("Васильевского острова", "В.О.").Replace("Васильевского острова", "В.О.").Replace("Петроградской стороны", "П.С.").Replace("ш.", "").Replace("пер", "").Replace(" ул", "").Replace("аллея", "").Replace("дорога ", "").Replace("набережная ", "").Trim();

        regex = new Regex(@"(^пр\.)");
        var pr = regex.Match(build.Street).Value;
        if (!string.IsNullOrWhiteSpace(pr))
        {
          build.Street = build.Street.Replace(pr, "").Trim() + $" {pr}";
        }

        Monitor.Enter(locker);
        using (var sw = new StreamWriter(new FileStream(FilenameSdam, FileMode.Open), Encoding.UTF8))
        {
          sw.BaseStream.Position = sw.BaseStream.Length;
          sw.WriteLine($@"{count};{build.Street};{build.Number};{build.Building};{build.Liter};{build.CountRoom};{build.Square};{build.Price};{build.Floor};{build.Metro};{build.Distance}");
        }
        Monitor.Exit(locker);
      }
    }
  }
}
