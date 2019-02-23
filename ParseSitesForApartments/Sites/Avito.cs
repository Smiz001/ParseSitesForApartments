using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using ParseSitesForApartments.Export;
using ParseSitesForApartments.Export.Creators;
using ParseSitesForApartments.UnionWithBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ParseSitesForApartments.Enum;

namespace ParseSitesForApartments.Sites
{
  public class Avito : BaseParse
  {
    #region Fields

    private int minPage = 1;
    private int maxPage = 100;
    private List<string> stantions = new List<string>() { "Автово", "Адмиралтейская", "Академическая", "Балтийская", "Беговая", "Бухарестская", "Василеостровская", "Владимирская", "Волковская", "Выборгская", "Горьковская", "Гостиный двор", "Гражданский проспект", "Девяткино", "Достоевская", "Елизаровская", "Звёздная", "Звенигородская", "Кировский завод", "Комендантский проспект", "Крестовский остров", "Купчино", "Ладожская", "Ленинский проспект", "Лесная", "Лиговский проспект", "Ломоносовская", "Маяковская", "Международная", "Московская", "Московские ворота", "Нарвская", "Невский проспект", "Новокрестовская", "Новочеркасская", "Обводный канал", "Обухово", "Озерки", "Парк Победы", "Парнас", "Петроградская", "Пионерская", "Площадь Александра Невского", "Площадь Восстания", "Площадь Ленина", "Площадь Мужества", "Политехническая", "Приморская", "Пролетарская", "Проспект Большевиков", "Проспект Ветеранов", "Проспект Просвещения", "Пушкинская", "Рыбацкое", "Садовая", "Сенная площадь", "Спасская", "Спортивная", "Старая Деревня", "Технологический институт", "Удельная", "Улица Дыбенко", "Фрунзенская", "Чёрная речка", "Чернышевская", "Чкаловская", "Электросила" };
    private static object locker = new object();
    private string filename = @"d:\ParserInfo\Appartament\AvitoProdam.csv";
    private CoreExport export;
    public delegate void Append(object sender, AppendFlatEventArgs e);
    public event Append OnAppend;
    private readonly UnionParseInfoWithDataBase unionInfo = new UnionParseInfoWithDataBase();

    private Thread studiiThread;
    private Thread oneThread;
    private Thread twoThread;
    private Thread threeThread;
    private Thread fourThread;
    private Thread fiveThread;
    private Thread sixThread;
    private Thread sevenThread;
    private Thread eightThread;
    private Thread nineThread;
    private Thread moreNineThread;
    private Thread studiiNewThread;
    private Thread oneNewThread;
    private Thread twoNewThread;
    private Thread threeNewThread;
    private Thread fourNewThread;
    private Thread fiveNewThread;
    private Thread sixNewThread;
    private Thread sevenNewThread;
    private Thread eightNewThread;
    private Thread nineNewThread;

    #endregion

    #region Properties

    public override string Filename
    {
      get => filename;
      set => filename = value;
    }
    public override string FilenameSdam { get => @"d:\ParserInfo\Appartament\AvitoSdam.csv"; }
    public override string FilenameWithinfo { get => @"d:\ParserInfo\Appartament\AvitoProdamWithInfo.csv"; }
    public override string FilenameWithinfoSdam { get => @"d:\ParserInfo\Appartament\AvitoSdamWithInfo.csv"; }
    public override string NameSite => "Avito";

    #endregion

    public Avito(List<District> listDistricts, List<Metro> lisMetro) : base(listDistricts, lisMetro)
    {
    }

    private void CreateExport()
    {
      CoreCreator creator = new CsvExportCreator();
      export = creator.FactoryCreate(Filename);
      OnAppend += export.AddFlatInList;

      if (export is CsvExport)
      {
        if (!File.Exists(Filename))
        {
          using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Create), Encoding.UTF8))
          {
            //sw.WriteLine($@"Район;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние(км);URL");
            sw.WriteLine(@"Район;Улица;Номер;Корпус;Литер;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Дата постройки;Дата реконструкции;Даты кап. ремонты;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов;Расстояние пешком;Время пешком;Расстояние на машине;Время на машине;Откуда взято");
          }
        }
      }
    }

    private void ChangeDistrictAndPage(object typeRoom)
    {
      HtmlParser parser = new HtmlParser();
      using (var webClient = new WebClient())
      {
        webClient.Encoding = Encoding.UTF8;
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        string url = "";
        for (int i = minPage; i < maxPage; i++)
        {
          switch (typeRoom)
          {
            case "Студия":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/studii/vtorichka?p={i}";
              break;
            case "1 км. кв.":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/1-komnatnye/vtorichka?p={i}";
              break;
            case "2 км. кв.":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/2-komnatnye/vtorichka?p={i}";
              break;
            case "3 км. кв.":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/3-komnatnye/vtorichka?p={i}";
              break;
            case "4 км. кв.":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/4-komnatnye/vtorichka?p={i}";
              break;
            case "5 км. кв.":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/5-komnatnye/vtorichka?p={i}";
              break;
            case "6 км. кв.":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/6-komnatnye/vtorichka?p={i}";
              break;
            case "7 км. кв.":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/7-komnatnye/vtorichka?p={i}";
              break;
            case "8 км. кв.":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/8-komnatnye/vtorichka?p={i}";
              break;
            case "9 км. кв.":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/9-komnatnye/vtorichka?p={i}";
              break;
            case "9 км. кв. +":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/mnogokomnatnye/vtorichka?s_trg=4";
              break;
            case "Студия Н":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/studii/novostroyka?p={i}";
              break;
            case "1 км. кв. Н":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/1-komnatnye/novostroyka?p={i}";
              break;
            case "2 км. кв. Н":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/2-komnatnye/novostroyka?p={i}";
              break;
            case "3 км. кв. Н":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/3-komnatnye/novostroyka?p={i}";
              break;
            case "4 км. кв. Н":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/4-komnatnye/novostroyka?p={i}";
              break;
            case "5 км. кв. Н":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/5-komnatnye/novostroyka?p={i}";
              break;
            case "6 км. кв. Н":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/6-komnatnye/novostroyka?p={i}";
              break;
            case "7 км. кв. Н":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/7-komnatnye/novostroyka?p={i}";
              break;
            case "8 км. кв. Н":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/8-komnatnye/novostroyka?p={i}";
              break;
            case "9 км. кв. Н":
              url =
                $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/9-komnatnye/novostroyka?p={i}";
              break;
          }
          if (!ExecuteParse(url, webClient, parser, (string)typeRoom))
            break;
        }
      }
      MessageBox.Show($"Закончили - {typeRoom}");
    }

    private bool ExecuteParse(string url, WebClient webClient, HtmlParser parser, string typeRoom)
    {
      var random = new Random();
      Thread.Sleep(random.Next(2000, 4000));
      try
      {
        Log.Debug("-----------URL-----------");
        Log.Debug(url);
        var responce = webClient.DownloadString(url);
        var document = parser.Parse(responce);

        var collections = document.GetElementsByClassName("description item_table-description");
        if (collections.Length > 0)
          ParsingSheet(typeRoom, collections);
        return true;
      }
      catch (Exception e)
      {
        Log.Error(e.Message);
        //TODO Если страница долго не отвечает то пропускаем ее
        Thread.Sleep(1000);
        return true;
      }
    }

    public override void ParsingAll()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        studiiThread = new Thread(ChangeDistrictAndPage);
        studiiThread.Start("Студия");
        //oneThread = new Thread(ChangeDistrictAndPage);
        //oneThread.Start("1 км. кв.");
        //twoThread = new Thread(ChangeDistrictAndPage);
        //twoThread.Start("2 км. кв.");
        //threeThread = new Thread(ChangeDistrictAndPage);
        //threeThread.Start("3 км. кв.");
        //fourThread = new Thread(ChangeDistrictAndPage);
        //fourThread.Start("4 км. кв.");
        //fiveThread = new Thread(ChangeDistrictAndPage);
        //fiveThread.Start("5 км. кв.");
        //sixThread = new Thread(ChangeDistrictAndPage);
        //sixThread.Start("6 км. кв.");
        //sevenThread = new Thread(ChangeDistrictAndPage);
        //sevenThread.Start("7 км. кв.");
        //eightThread = new Thread(ChangeDistrictAndPage);
        //eightThread.Start("8 км. кв.");
        //nineThread = new Thread(ChangeDistrictAndPage);
        //nineThread.Start("9 км. кв.");
        //moreNineThread = new Thread(ChangeDistrictAndPage);
        //moreNineThread.Start("9 км. кв. +");

        //studiiNewThread = new Thread(ChangeDistrictAndPage);
        //studiiNewThread.Start("Студия Н");
        //oneNewThread = new Thread(ChangeDistrictAndPage);
        //oneNewThread.Start("1 км. кв. Н");
        //twoNewThread = new Thread(ChangeDistrictAndPage);
        //twoNewThread.Start("2 км. кв. Н");
        //threeNewThread = new Thread(ChangeDistrictAndPage);
        //threeNewThread.Start("3 км. кв. Н");
        //fourNewThread = new Thread(ChangeDistrictAndPage);
        //fourNewThread.Start("4 км. кв. Н");
        //fiveNewThread = new Thread(ChangeDistrictAndPage);
        //fiveNewThread.Start("5 км. кв. Н");
        //sixNewThread = new Thread(ChangeDistrictAndPage);
        //sixNewThread.Start("6 км. кв. Н");
        //sevenNewThread = new Thread(ChangeDistrictAndPage);
        //sevenNewThread.Start("7 км. кв. Н");
        //eightNewThread = new Thread(ChangeDistrictAndPage);
        //eightNewThread.Start("8 км. кв. Н");
        //nineNewThread = new Thread(ChangeDistrictAndPage);
        //nineNewThread.Start("9 км. кв. Н");
      }
      else
      {
        //studiiRentThread = new Thread(ChangeDistrictAndPageForRent);
        //studiiRentThread.Start("Студия");
        //oneRentThread = new Thread(ChangeDistrictAndPageForRent);
        //oneRentThread.Start("1 км. кв.");
        //twoRentThread = new Thread(ChangeDistrictAndPageForRent);
        //twoRentThread.Start("2 км. кв.");
        //threeRentThread = new Thread(ChangeDistrictAndPageForRent);
        //threeRentThread.Start("3 км. кв.");
        //fourRentThread = new Thread(ChangeDistrictAndPageForRent);
        //fourRentThread.Start("4 км. кв.");
        //fiveRentThread = new Thread(ChangeDistrictAndPageForRent);
        //fiveRentThread.Start("5 км. кв.");
      }
    }

    public override void ParsingStudii()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        studiiThread = new Thread(ChangeDistrictAndPage);
        studiiThread.Start("Студия");
      }
      else
      {
        //studiiRentThread = new Thread(ChangeDistrictAndPage);
        //studiiRentThread.Start("Студия");
      }
    }

    public override void ParsingOne()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        oneThread = new Thread(ChangeDistrictAndPage);
        oneThread.Start("1 км. кв.");
      }
      else
      {
        //oneRentThread = new Thread(ChangeDistrictAndPageForRent);
        //oneRentThread.Start("1 км. кв.");
      }
    }

    public override void ParsingTwo()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        twoThread = new Thread(ChangeDistrictAndPage);
        twoThread.Start("2 км. кв.");
      }
      else
      {
        //twoRentThread = new Thread(ChangeDistrictAndPageForRent);
        //twoRentThread.Start("2 км. кв.");
      }
    }

    public override void ParsingThree()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        threeThread = new Thread(ChangeDistrictAndPage);
        threeThread.Start("3 км. кв.");
      }
      else
      {
        //threeRentThread = new Thread(ChangeDistrictAndPageForRent);
        //threeRentThread.Start("3 км. кв.");
      }
    }

    public override void ParsingFour()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        fourThread = new Thread(ChangeDistrictAndPage);
        fourThread.Start("4 км. кв.");
      }
      else
      {
        //fourRentThread = new Thread(ChangeDistrictAndPageForRent);
        //fourRentThread.Start("4 км. кв.");
      }
    }

    public override void ParsingMoreFour()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        fiveThread = new Thread(ChangeDistrictAndPage);
        fiveThread.Start("5 км. кв.");
      }
      else
      {
        //fiveRentThread = new Thread(ChangeDistrictAndPageForRent);
        //fiveRentThread.Start("5 км. кв.");
      }
    }

    public void ParsingStudio()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/studii/vtorichka?p={i}";
        if (!LinkProcessingProdam(prodam, "Студия"))
          break;
      }
      MessageBox.Show("Закончил студии");
    }
    public void ParsingOneRoom()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/1-komnatnye/vtorichka?p={i}";
        if (!LinkProcessingProdam(prodam, "1 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 1 км. кв.");
    }
    public void ParsingTwoRoom()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/2-komnatnye/vtorichka?p={i}";
        if (!LinkProcessingProdam(prodam, "2 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 2 км. кв.");
    }
    public void ParsingThreeRoom()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/3-komnatnye/vtorichka?p={i}";
        if (!LinkProcessingProdam(prodam, "3 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 3 км. кв.");
    }
    public void ParsingFourRoom()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/4-komnatnye/vtorichka?p={i}";
        if (!LinkProcessingProdam(prodam, "4 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 4 км. кв.");
    }
    public void ParsingFiveRoom()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/5-komnatnye/vtorichka?p={i}";
        if (!LinkProcessingProdam(prodam, "5 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 5 км. кв.");
    }
    public void ParsingSixRoom()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/6-komnatnye/vtorichka?p={i}";
        if (!LinkProcessingProdam(prodam, "6 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 6 км. кв.");
    }
    public void ParsingSevenRoom()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/7-komnatnye/vtorichka?p={i}";
        if (!LinkProcessingProdam(prodam, "7 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 7 км. кв.");
    }
    public void ParsingEightRoom()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/8-komnatnye/vtorichka?p={i}";
        if (!LinkProcessingProdam(prodam, "8 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 8 км. кв.");
    }
    public void ParsingNineRoom()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/9-komnatnye/vtorichka?p={i}";
        if (!LinkProcessingProdam(prodam, "9 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 9 км. кв.");
    }
    public void ParsingMoreNineRoom()
    {
      using (var webClient = new WebClient())
      {
        Random random = new Random();
        for (int i = minPage; i < maxPage; i++)
        {
          Thread.Sleep(random.Next(5000, 10000));
          string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/mnogokomnatnye/vtorichka?s_trg=4";

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

    public void ParsingStudioNew()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/studii/novostroyka?p={i}";
        if (!LinkProcessingProdam(prodam, "Студия Н"))
          break;
      }
      MessageBox.Show("Закончил студии Н");
    }
    public void ParsingOneRoomNew()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/1-komnatnye/novostroyka?p={i}";
        if (!LinkProcessingProdam(prodam, "1 км. кв. Н"))
          break;
      }
      MessageBox.Show("Закончил 1 км. кв. Н");
    }
    public void ParsingTwoRoomNew()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/2-komnatnye/novostroyka?p={i}";
        if (!LinkProcessingProdam(prodam, "2 км. кв. Н"))
          break;
      }
      MessageBox.Show("Закончил 2 км. кв. Н");
    }
    public void ParsingThreeRoomNew()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/3-komnatnye/novostroyka?p={i}";
        if (!LinkProcessingProdam(prodam, "3 км. кв. Н"))
          break;
      }
      MessageBox.Show("Закончил 3 км. кв. Н");
    }
    public void ParsingFourRoomNew()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/4-komnatnye/novostroyka?p={i}";
        if (!LinkProcessingProdam(prodam, "4 км. кв. Н"))
          break;
      }
      MessageBox.Show("Закончил 4 км. кв. Н");
    }
    public void ParsingFiveRoomNew()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/5-komnatnye/novostroyka?p={i}";
        if (!LinkProcessingProdam(prodam, "5 км. кв. Н"))
          break;
      }
      MessageBox.Show("Закончил 5 км. кв.");
    }
    public void ParsingSixRoomNew()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/6-komnatnye/novostroyka?p={i}";
        if (!LinkProcessingProdam(prodam, "6 км. кв. Н"))
          break;
      }
      MessageBox.Show("Закончил 6 км. кв. Н");
    }
    public void ParsingSevenRoomNew()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/7-komnatnye/novostroyka?p={i}";
        if (!LinkProcessingProdam(prodam, "7 км. кв. Н"))
          break;
      }
      MessageBox.Show("Закончил 7 км. кв. Н");
    }
    //public void ParsingEightRoomNew()
    //{
    //  for (int i = minPage; i < maxPage; i++)
    //  {
    //    string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/8-komnatnye?p={i}";
    //    if (!LinkProcessingProdam(prodam, "8 км. кв."))
    //      break;
    //  }
    //  MessageBox.Show("Закончил 8 км. кв.");
    //}
    //public void ParsingNineRoomNew()
    //{
    //  for (int i = minPage; i < maxPage; i++)
    //  {
    //    string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/prodam/9-komnatnye?p={i}";
    //    if (!LinkProcessingProdam(prodam, "9 км. кв."))
    //      break;
    //  }
    //  MessageBox.Show("Закончил 9 км. кв.");
    //}

    private void ParsingSheet(string typeRoom, IHtmlCollection<IElement> collection)
    {
      for (int k = 0; k < collection.Length; k++)
      {
        var flat = new Flat();
        flat.CountRoom = typeRoom;

        flat.Price = int.Parse(collection[k].GetElementsByClassName("price")[0].TextContent.Trim('\n').Trim('₽').Trim().Replace(" ", ""));

        var aboutBuild = collection[k].GetElementsByClassName("item-description-title-link")[0].TextContent;
        var regex = new Regex(@"(\d+\s+м²)");
        flat.Square = regex.Match(aboutBuild).Value.Replace(".", ",");
        regex = new Regex(@"(\d+\/\d+)");
        var floor = regex.Match(aboutBuild).Value;
        regex = new Regex(@"(\/\d+)");
        flat.Floor = floor.Replace(regex.Match(floor).Value, "");

        var adress = collection[k].GetElementsByClassName("address");
        if (adress.Length > 0)
        {
          var adres = adress[0].TextContent.Trim();

          var distance = collection[k].GetElementsByClassName("c-2");
          if (distance.Length > 0)
            flat.Building.Distance = distance[0].TextContent.Trim();
          adres = adres.Replace(flat.Building.Distance, "").Replace("Санкт-Петербург,", "").Replace("посёлок Парголово,", "").Replace("СПб Красное село", "").Replace("г. Ломоносов,", "").Replace("Россия,", "").Replace("Сестрорецк г,", "").Replace("Сестрорецк", "").Replace("Парголово п,", "").Replace("Колпино,", "").Replace("Мурино,", "").Replace("посёлок Шушары,", "").Replace("г. Петергоф,", "");

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
          ProcessingBuilding(flat, ar);
        }


        regex = new Regex(@"(к\d+)");
        flat.Building.Structure = regex.Match(flat.Building.Number).Value;
        if (string.IsNullOrWhiteSpace(flat.Building.Structure))
        {
          regex = new Regex(@"(к \d+)");
          flat.Building.Structure = regex.Match(flat.Building.Number).Value;
          if (string.IsNullOrWhiteSpace(flat.Building.Structure))
          {
            regex = new Regex(@"(кор\.\d+)");
            flat.Building.Structure = regex.Match(flat.Building.Number).Value;
            if (string.IsNullOrWhiteSpace(flat.Building.Structure))
            {

            }
            else
            {
              flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Trim();
              flat.Building.Structure = flat.Building.Structure.Replace("кор.", "");
            }
          }
          else
          {
            flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "");
            flat.Building.Structure = flat.Building.Structure.Replace("к", "");
          }
        }
        else
        {
          flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "");
          flat.Building.Structure = flat.Building.Structure.Replace("к", "");
        }

        regex = new Regex(@"(\D$)");
        flat.Building.Liter = regex.Match(flat.Building.Number).Value;
        if (!string.IsNullOrEmpty(flat.Building.Liter))
        {
          flat.Building.Number = flat.Building.Number.Replace(flat.Building.Liter, "");
        }


        flat.Building.Street = flat.Building.Street.Replace("улица", "").Replace("ул. ", "").Replace("проезд", "").Replace("переулок", "").Replace("переулок", "").Replace("бульвар", "").Replace("б-р", "").Replace("проспект", "пр.").Replace("пр-кт", "пр.").Replace("Васильевского острова", "В.О.").Replace("Васильевского острова", "В.О.").Replace("Петроградской стороны", "П.С.").Replace("ш.", "").Replace("пер", "").Replace(" ул", "").Replace("аллея", "").Replace("дорога ", "").Replace("набережная ", "").Replace("пр-т", "пр.").Trim();

        regex = new Regex(@"(^пр\.)");
        var pr = regex.Match(flat.Building.Street).Value;
        if (!string.IsNullOrWhiteSpace(pr))
        {
          flat.Building.Street = flat.Building.Street.Replace(pr, "").Trim() + $" {pr}";
        }

        if (flat.Building.Distance.Contains("день") || flat.Building.Distance.Contains("дня") ||
            flat.Building.Distance.Contains("минут") || flat.Building.Distance.Contains("час") ||
            flat.Building.Distance.Contains("дней") || flat.Building.Distance.Contains("недел"))
          flat.Building.Distance = string.Empty;


        //Обработка некоторых улиц
        if (flat.Building.Street.Contains("В.О."))
          flat.Building.Street = "Большой пр. В.О.";
        if (flat.Building.Street.Contains("П.С."))
          flat.Building.Street = "Большой П.С. пр.";

        if (!string.IsNullOrWhiteSpace(flat.Building.Number))
        {
          if (!string.IsNullOrWhiteSpace(flat.Square))
          {
            Monitor.Enter(locker);
            //if (string.IsNullOrWhiteSpace(flat.Building.DateBuild))
            //{
            //  unionInfo.UnionInfoProdam(flat);
            //}
            OnAppend(this, new AppendFlatEventArgs { Flat = flat });
            Monitor.Exit(locker);
          }
        }
      }
    }

    //public override void ParsingAll()
    //{
    //  using (var sw = new StreamWriter(Filename, true, System.Text.Encoding.UTF8))
    //  {
    //    sw.WriteLine($@"Район;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние");
    //  }
    //  var studiiThread = new Thread(ParsingStudio);
    //  studiiThread.Start();
    //  Thread.Sleep(55000);
    //  var oneThread = new Thread(ParsingOneRoom);
    //  oneThread.Start();
    //  Thread.Sleep(55000);
    //  var twoThread = new Thread(ParsingTwoRoom);
    //  twoThread.Start();
    //  Thread.Sleep(55000);
    //  var threeThread = new Thread(ParsingThreeRoom);
    //  threeThread.Start();
    //  Thread.Sleep(55000);
    //  var fourThread = new Thread(ParsingFourRoom);
    //  fourThread.Start();
    //  Thread.Sleep(55000);
    //  var fiveThread = new Thread(ParsingFiveRoom);
    //  fiveThread.Start();
    //  Thread.Sleep(55000);
    //  var sixThread = new Thread(ParsingSixRoom);
    //  sixThread.Start();
    //  Thread.Sleep(55000);
    //  var sevenThread = new Thread(ParsingSevenRoom);
    //  sevenThread.Start();
    //  Thread.Sleep(55000);
    //  var eightThread = new Thread(ParsingEightRoom);
    //  eightThread.Start();
    //  Thread.Sleep(55000);
    //  var nineThread = new Thread(ParsingNineRoom);
    //  nineThread.Start();
    //  Thread.Sleep(55000);

    //  var studiiThreadNew = new Thread(ParsingStudioNew);
    //  studiiThreadNew.Start();
    //  Thread.Sleep(55000);
    //  var oneThreadNew = new Thread(ParsingOneRoomNew);
    //  oneThreadNew.Start();
    //  Thread.Sleep(55000);
    //  var twoThreadNew = new Thread(ParsingTwoRoomNew);
    //  twoThreadNew.Start();
    //  Thread.Sleep(55000);
    //  var threeThreadNew = new Thread(ParsingThreeRoomNew);
    //  threeThreadNew.Start();
    //  Thread.Sleep(55000);
    //  var fourThreadNew = new Thread(ParsingFourRoomNew);
    //  fourThreadNew.Start();
    //  Thread.Sleep(55000);
    //  var fiveThreadNew = new Thread(ParsingFiveRoomNew);
    //  fiveThreadNew.Start();
    //  Thread.Sleep(55000);
    //  var sixThreadNew = new Thread(ParsingSixRoomNew);
    //  sixThreadNew.Start();
    //  Thread.Sleep(55000);
    //  var sevenThreadNew = new Thread(ParsingSevenRoomNew);
    //  sevenThreadNew.Start();

    //}

    //public override void ParsingStudii()
    //{
    //  throw new NotImplementedException();
    //}

    //public override void ParsingOne()
    //{
    //  throw new NotImplementedException();
    //}

    //public override void ParsingTwo()
    //{
    //  throw new NotImplementedException();
    //}

    //public override void ParsingThree()
    //{
    //  throw new NotImplementedException();
    //}

    //public override void ParsingFour()
    //{
    //  throw new NotImplementedException();
    //}

    //public override void ParsingMoreFour()
    //{
    //  throw new NotImplementedException();
    //}

    private bool LinkProcessingProdam(string link, string typeRoom)
    {
      try
      {
        using (var webClient = new WebClient())
        {
          var random = new Random();
          Thread.Sleep(random.Next(2000, 4000));
          ServicePointManager.Expect100Continue = true;
          ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
          ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

          webClient.Encoding = Encoding.UTF8;
          var responce = webClient.DownloadString(link);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);

          var collections = document.GetElementsByClassName("description item_table-description");
          if (collections.Length > 0)
            ParsingSheet(typeRoom, collections);
        }
      }
      catch(Exception ex)
      {
        string msg = ex.Message;
        return false;
      }
      return true;
    }

    private void ParseMetro(string parseMetro, Building building, SqlConnection connection)
    {

      string select = $@"SELECT [Name]
  FROM [ParseBulding].[dbo].[Metro]
  where Name Like '%{parseMetro}%'";
      var command = new SqlCommand(select, connection);
      var metroName = (string)command.ExecuteScalar();
      if (!string.IsNullOrEmpty(metroName))
      {
        building.Metro = metroName;
      }
    }

    private void ProcessingBuilding(Flat flat, string[] ar)
    {
      if (ar.Length == 3)
      {
        ar[0] = ar[0].Replace("ин-т", "").Replace("Площадь А.", "").Replace("I", "").Replace("II", "").Trim();
        //ParseMetro(ar[0], flat.Building, connection);

        flat.Building.Street = ar[1];
        flat.Building.Number = ar[2];
        flat.Building.Number = flat.Building.Number.Replace("А", "").Replace("А", "").Replace("дом ", "").Replace("д.", "").Trim();
      }
      else if (ar.Length == 1)
      {
        var regex = new Regex(@"(д\. \d+\s+к\.\d+)|(д\.\s+\d+\s+к\.\d+)");
        flat.Building.Number = regex.Match(ar[0]).Value;
        if (!string.IsNullOrEmpty(flat.Building.Number))
        {
          flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
          regex = new Regex(@"(к\.\d+)");
          flat.Building.Structure = regex.Match(flat.Building.Number).Value;
          flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("д. ", "").Replace("д. ", "");
          flat.Building.Structure = flat.Building.Structure.Replace("к.", "");
        }
        else
        {
          regex = new Regex(@"(д\.\d+\s+к\.\d+)");
          flat.Building.Number = regex.Match(ar[0]).Value;
          if (!string.IsNullOrEmpty(flat.Building.Number))
          {
            flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
            regex = new Regex(@"(к\.\d+)");
            flat.Building.Structure = regex.Match(flat.Building.Number).Value;
            flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("д.", "").Trim();
            flat.Building.Structure = flat.Building.Structure.Replace("к.", "");
          }
          else
          {
            regex = new Regex(@"(д\. \d+\s+корп\.\s+\d+)|(д\.\s+\d+\s+корп\.\s+\d+)");
            flat.Building.Number = regex.Match(ar[0]).Value;
            if (!string.IsNullOrEmpty(flat.Building.Number))
            {
              flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
              regex = new Regex(@"(корп\.\s+\d+)");
              flat.Building.Structure = regex.Match(flat.Building.Number).Value;
              flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("д. ", "").Replace("д. ", "");
              flat.Building.Structure = flat.Building.Structure.Replace("корп.", "");
            }
            else
            {
              regex = new Regex(@"(дом\s+\d+\s+корпус\s+\d+)");
              flat.Building.Number = regex.Match(ar[0]).Value;
              if (!string.IsNullOrEmpty(flat.Building.Number))
              {
                flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
                regex = new Regex(@"(корпус\s+\d+)");
                flat.Building.Structure = regex.Match(flat.Building.Number).Value;
                flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("дом ", "").Replace("дом ", "");
                flat.Building.Structure = flat.Building.Structure.Replace("корпус", "");
              }
              else
              {
                regex = new Regex(@"(дом\s+\d+\s+корп\.\s+\d+)");
                flat.Building.Number = regex.Match(ar[0]).Value;
                if (!string.IsNullOrEmpty(flat.Building.Number))
                {
                  flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
                  regex = new Regex(@"(корп\.\s+\d+)");
                  flat.Building.Structure = regex.Match(flat.Building.Number).Value;
                  flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("дом ", "").Replace("дом ", "");
                  flat.Building.Structure = flat.Building.Structure.Replace("корп.", "");
                }
                else
                {
                  regex = new Regex(@"(д\.\d+$)");
                  flat.Building.Number = regex.Match(ar[0]).Value;
                  if (!string.IsNullOrEmpty(flat.Building.Number))
                  {
                    flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
                    flat.Building.Number = flat.Building.Number.Replace("д.", "");
                  }
                  else
                  {
                    regex = new Regex(@"(\d+с\d+$)");
                    flat.Building.Number = regex.Match(ar[0]).Value;
                    if (!string.IsNullOrEmpty(flat.Building.Number))
                    {
                      flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
                    }
                    else
                    {
                      regex = new Regex(@"(д\d+$)");
                      flat.Building.Number = regex.Match(ar[0]).Value;
                      if (!string.IsNullOrEmpty(flat.Building.Number))
                      {
                        flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
                      }
                      else
                      {
                        regex = new Regex(@"(\d+\/\d+)");
                        flat.Building.Number = regex.Match(ar[0]).Value;
                        if (!string.IsNullOrEmpty(flat.Building.Number))
                        {
                          flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
                          flat.Building.Structure = flat.Building.Number.Split('/')[1];
                          flat.Building.Number = flat.Building.Number.Split('/')[1];
                        }
                        else
                        {
                          regex = new Regex(@"(\d+$)");
                          flat.Building.Number = regex.Match(ar[0]).Value;
                          if (!string.IsNullOrEmpty(flat.Building.Number))
                          {
                            flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
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
        flat.Building.Street = flat.Building.Street.Replace("Ул.", "").Replace("г.", "").Replace(".", "").Trim();
      }
      else if (ar.Length == 2)
      {
        if (stantions.Contains(ar[0].Trim()))
        {
          //ParseMetro(ar[0].Trim(), flat.Building, connection);
          flat.Building.Street = ar[1];
        }
      }
      else if (ar.Length == 4)
      {
        if (stantions.Contains(ar[0].Trim()))
        {
         // ParseMetro(ar[0].Trim(), flat.Building, connection);
          flat.Building.Street = ar[1];
        }
      }
      //else
      //  flat.Building.Street = adres;
    }
    //public override void ParsingSdamAll()
    //{
    //  using (var sw = new StreamWriter(FilenameSdam, true, System.Text.Encoding.UTF8))
    //  {
    //    sw.WriteLine($@"Нас. пункт;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние");
    //  }
    //  var studiiThread = new Thread(ParsingStudioSdam);
    //  studiiThread.Start();
    //  Thread.Sleep(55000);
    //  var oneThread = new Thread(ParsingOneSdam);
    //  oneThread.Start();
    //  Thread.Sleep(55000);
    //  var twoThread = new Thread(ParsingTwoSdam);
    //  twoThread.Start();
    //  Thread.Sleep(55000);
    //  var threeThread = new Thread(ParsingThreeSdam);
    //  threeThread.Start();
    //  var fourThread = new Thread(ParsingFourSdam);
    //  fourThread.Start();
    //  Thread.Sleep(55000);
    //  var fiveThread = new Thread(ParsingFiveSdam);
    //  fiveThread.Start();
    //  Thread.Sleep(55000);
    //  var sixThread = new Thread(ParsingSixSdam);
    //  sixThread.Start();
    //  Thread.Sleep(55000);
    //  var sevenThread = new Thread(ParsingSevenSdam);
    //  sevenThread.Start();
    //  Thread.Sleep(55000);
    //  var eightThread = new Thread(ParsingEightSdam);
    //  eightThread.Start();
    //  Thread.Sleep(55000);
    //  var nineThread = new Thread(ParsingNineSdam);
    //  nineThread.Start();
    //  Thread.Sleep(55000);
    //}

    private bool LinkProcessingSdam(string link, string typeRoom)
    {
      try
      {
        using (var webClient = new WebClient())
        {
          var random = new Random();
          Thread.Sleep(random.Next(2000, 4000));
          ServicePointManager.Expect100Continue = true;
          ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
          ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

          webClient.Encoding = Encoding.UTF8;
          var responce = webClient.DownloadString(link);
          var parser = new HtmlParser();
          var document = parser.Parse(responce);

          var collections = document.GetElementsByClassName("description item_table-description");
          if (collections.Length > 0)
            ParsingSheetSdam(typeRoom, collections);
        }
      }
      catch
      {
        return false;
      }
      return true;
    }

    public void ParsingStudioSdam()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/sdam/na_dlitelnyy_srok/studii?p={i}";
        if (!LinkProcessingSdam(prodam, "Студия"))
          break;
      }
      MessageBox.Show("Закончил студии");
    }
    public void ParsingOneSdam()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/sdam/na_dlitelnyy_srok/1-komnatnye?p={i}";
        if (!LinkProcessingSdam(prodam, "1 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 1 км. кв.");
    }
    public void ParsingTwoSdam()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/sdam/na_dlitelnyy_srok/2-komnatnye?p={i}";
        if (!LinkProcessingSdam(prodam, "2 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 2 км. кв.");
    }
    public void ParsingThreeSdam()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/sdam/na_dlitelnyy_srok/3-komnatnye?p={i}";
        if (!LinkProcessingSdam(prodam, "3 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 3 км. кв.");
    }
    public void ParsingFourSdam()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/sdam/na_dlitelnyy_srok/4-komnatnye?p={i}";
        if (!LinkProcessingSdam(prodam, "4 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 4 км. кв.");
    }
    public void ParsingFiveSdam()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/sdam/na_dlitelnyy_srok/5-komnatnye?p={i}";
        if (!LinkProcessingSdam(prodam, "5 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 5 км. кв.");
    }
    public void ParsingSixSdam()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/sdam/na_dlitelnyy_srok/6-komnatnye?p={i}";
        if (!LinkProcessingSdam(prodam, "6 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 6 км. кв.");
    }
    public void ParsingSevenSdam()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/sdam/na_dlitelnyy_srok/7-komnatnye?p={i}";
        if (!LinkProcessingSdam(prodam, "7 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 7 км. кв.");
    }
    public void ParsingEightSdam()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/sdam/na_dlitelnyy_srok/8-komnatnye?p={i}";
        if (!LinkProcessingSdam(prodam, "8 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 8 км. кв.");
    }
    public void ParsingNineSdam()
    {
      for (int i = minPage; i < maxPage; i++)
      {
        string prodam = $@"https://www.avito.ru/sankt-peterburg/kvartiry/sdam/na_dlitelnyy_srok/9-komnatnye?p={i}";
        if (!LinkProcessingSdam(prodam, "9 км. кв."))
          break;
      }
      MessageBox.Show("Закончил 9 км. кв.");
    }

    private void ParsingSheetSdam(string typeRoom, IHtmlCollection<IElement> collection)
    {
      for (int k = 0; k < collection.Length; k++)
      {
        var flat = new Flat();
        flat.CountRoom = typeRoom;

        string price = collection[k].GetElementsByClassName("price")[0].TextContent.Trim('\n').Trim('₽').Replace(" ", "").Replace("₽\nвмесяц", "");
        flat.Price = int.Parse(price);

        var aboutBuild = collection[k].GetElementsByClassName("item-description-title-link")[0].TextContent;
        var regex = new Regex(@"(\d+\s+м²)");
        flat.Square = regex.Match(aboutBuild).Value;
        regex = new Regex(@"(\d+\/\d+)");
        var floor = regex.Match(aboutBuild).Value;
        regex = new Regex(@"(\/\d+)");
        flat.Floor = floor.Replace(regex.Match(floor).Value, "");

        var adress = collection[k].GetElementsByClassName("address");
        int count = 0;
        if (adress.Length > 0)
        {
          var adres = adress[0].TextContent.Trim();

          var distance = collection[k].GetElementsByClassName("c-2");
          if (distance.Length > 0)
            flat.Building.Distance = distance[0].TextContent.Trim();

          #region Удаление лишнего

          adres = adres.Replace(flat.Building.Distance, "").Replace("Санкт-Петербург,", "").Replace("посёлок Парголово,", "").Replace("СПб Красное село", "").Replace("г. Ломоносов,", "").Replace("Россия,", "").Replace("Сестрорецк г,", "").Replace("Сестрорецк", "").Replace("Парголово п,", "").Replace("Колпино,", "").Replace("Мурино,", "").Replace("посёлок Шушары,", "").Replace("г. Петергоф,", "").Replace("Ленинградская область,", "").Replace("Ломоносовский район,", "").Replace("посёлок  Всеволожский район,", "").Replace("Всеволожский район,", "").Replace("Санкт-Петербург.", "").Replace("Приморский р-н.", "").Replace("г.", "").Replace("посёлок", "").Replace("метро", "").Trim();

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
            flat.Building.Metro = ar[0];
            flat.Building.Street = ar[1];
            flat.Building.Number = ar[2];
            flat.Building.Number = flat.Building.Number.Replace("А", "").Replace("А", "").Replace("дом ", "").Replace("д.", "").Trim();
          }
          else if (ar.Length == 1)
          {
            regex = new Regex(@"(д\. \d+\s+к\.\d+)|(д\.\s+\d+\s+к\.\d+)");
            flat.Building.Number = regex.Match(ar[0]).Value;
            if (!string.IsNullOrEmpty(flat.Building.Number))
            {
              flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
              regex = new Regex(@"(к\.\d+)");
              flat.Building.Structure = regex.Match(flat.Building.Number).Value;
              flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("д. ", "").Replace("д. ", "");
              flat.Building.Structure = flat.Building.Structure.Replace("к.", "");
            }
            else
            {
              regex = new Regex(@"(д\.\d+\s+к\.\d+)");
              flat.Building.Number = regex.Match(ar[0]).Value;
              if (!string.IsNullOrEmpty(flat.Building.Number))
              {
                flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
                regex = new Regex(@"(к\.\d+)");
                flat.Building.Structure = regex.Match(flat.Building.Number).Value;
                flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("д.", "").Trim();
                flat.Building.Structure = flat.Building.Structure.Replace("к.", "");
              }
              else
              {
                regex = new Regex(@"(д\. \d+\s+корп\.\s+\d+)|(д\.\s+\d+\s+корп\.\s+\d+)");
                flat.Building.Number = regex.Match(ar[0]).Value;
                if (!string.IsNullOrEmpty(flat.Building.Number))
                {
                  flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
                  regex = new Regex(@"(корп\.\s+\d+)");
                  flat.Building.Structure = regex.Match(flat.Building.Number).Value;
                  flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("д. ", "").Replace("д. ", "");
                  flat.Building.Structure = flat.Building.Structure.Replace("корп.", "");
                }
                else
                {
                  regex = new Regex(@"(дом\s+\d+\s+корпус\s+\d+)");
                  flat.Building.Number = regex.Match(ar[0]).Value;
                  if (!string.IsNullOrEmpty(flat.Building.Number))
                  {
                    flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
                    regex = new Regex(@"(корпус\s+\d+)");
                    flat.Building.Structure = regex.Match(flat.Building.Number).Value;
                    flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("дом ", "").Replace("дом ", "");
                    flat.Building.Structure = flat.Building.Structure.Replace("корпус", "");
                  }
                  else
                  {
                    regex = new Regex(@"(дом\s+\d+\s+корп\.\s+\d+)");
                    flat.Building.Number = regex.Match(ar[0]).Value;
                    if (!string.IsNullOrEmpty(flat.Building.Number))
                    {
                      flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
                      regex = new Regex(@"(корп\.\s+\d+)");
                      flat.Building.Structure = regex.Match(flat.Building.Number).Value;
                      flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Replace("дом ", "").Replace("дом ", "");
                      flat.Building.Structure = flat.Building.Structure.Replace("корп.", "");
                    }
                    else
                    {
                      regex = new Regex(@"(д\.\d+$)");
                      flat.Building.Number = regex.Match(ar[0]).Value;
                      if (!string.IsNullOrEmpty(flat.Building.Number))
                      {
                        flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
                        flat.Building.Number = flat.Building.Number.Replace("д.", "");
                      }
                      else
                      {
                        regex = new Regex(@"(\d+с\d+$)");
                        flat.Building.Number = regex.Match(ar[0]).Value;
                        if (!string.IsNullOrEmpty(flat.Building.Number))
                        {
                          flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
                        }
                        else
                        {
                          regex = new Regex(@"(д\d+$)");
                          flat.Building.Number = regex.Match(ar[0]).Value;
                          if (!string.IsNullOrEmpty(flat.Building.Number))
                          {
                            flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
                          }
                          else
                          {
                            regex = new Regex(@"(\d+\/\d+)");
                            flat.Building.Number = regex.Match(ar[0]).Value;
                            if (!string.IsNullOrEmpty(flat.Building.Number))
                            {
                              flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
                              flat.Building.Structure = flat.Building.Number.Split('/')[1];
                              flat.Building.Number = flat.Building.Number.Split('/')[1];
                            }
                            else
                            {
                              regex = new Regex(@"(\d+$)");
                              flat.Building.Number = regex.Match(ar[0]).Value;
                              if (!string.IsNullOrEmpty(flat.Building.Number))
                              {
                                flat.Building.Street = ar[0].Replace(flat.Building.Number, "").Trim();
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
            flat.Building.Street = flat.Building.Street.Replace("Ул.", "").Replace("г.", "").Replace(".", "").Trim();
          }
          else if (ar.Length == 2)
          {
            if (stantions.Contains(ar[0].Trim()))
            {
              flat.Building.Metro = ar[0].Trim();
              flat.Building.Street = ar[1];
            }
          }
          else if (ar.Length == 4)
          {
            if (stantions.Contains(ar[0].Trim()))
            {
              flat.Building.Metro = ar[0].Trim();
              flat.Building.Street = ar[1];
            }
          }
          else
            flat.Building.Street = adres;
        }


        regex = new Regex(@"(к\d+)");
        flat.Building.Structure = regex.Match(flat.Building.Number).Value;
        if (string.IsNullOrWhiteSpace(flat.Building.Structure))
        {
          regex = new Regex(@"(к \d+)");
          flat.Building.Structure = regex.Match(flat.Building.Number).Value;
          if (string.IsNullOrWhiteSpace(flat.Building.Structure))
          {
            regex = new Regex(@"(кор\.\d+)");
            flat.Building.Structure = regex.Match(flat.Building.Number).Value;
            if (string.IsNullOrWhiteSpace(flat.Building.Structure))
            {

            }
            else
            {
              flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "").Trim();
              flat.Building.Structure = flat.Building.Structure.Replace("кор.", "");
            }
          }
          else
          {
            flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "");
            flat.Building.Structure = flat.Building.Structure.Replace("к", "");
          }
        }
        else
        {
          flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "");
          flat.Building.Structure = flat.Building.Structure.Replace("к", "");
        }

        regex = new Regex(@"(\D$)");
        flat.Building.Liter = regex.Match(flat.Building.Number).Value;
        if (!string.IsNullOrEmpty(flat.Building.Liter))
        {
          flat.Building.Number = flat.Building.Number.Replace(flat.Building.Liter, "");
        }

        flat.Building.Street = flat.Building.Street.Replace("улица", "").Replace("ул. ", "").Replace("проезд", "").Replace("переулок", "").Replace("переулок", "").Replace("бульвар", "").Replace("б-р", "").Replace("проспект", "пр.").Replace("пр-кт", "пр.").Replace("Васильевского острова", "В.О.").Replace("Васильевского острова", "В.О.").Replace("Петроградской стороны", "П.С.").Replace("ш.", "").Replace("пер", "").Replace(" ул", "").Replace("аллея", "").Replace("дорога ", "").Replace("набережная ", "").Trim();

        regex = new Regex(@"(^пр\.)");
        var pr = regex.Match(flat.Building.Street).Value;
        if (!string.IsNullOrWhiteSpace(pr))
        {
          flat.Building.Street = flat.Building.Street.Replace(pr, "").Trim() + $" {pr}";
        }

        Monitor.Enter(locker);
        if(string.IsNullOrEmpty(flat.Building.Number))
        {
          using (var sw = new StreamWriter(new FileStream(FilenameSdam, FileMode.Open), Encoding.UTF8))
          {
            sw.BaseStream.Position = sw.BaseStream.Length;
            sw.WriteLine($@"{count};{flat.Building.Street};{flat.Building.Number};{flat.Building.Structure};{flat.Building.Liter};{flat.CountRoom};{flat.Square};{flat.Price};{flat.Floor};{flat.Building.Metro};{flat.Building.Distance}");
          }
        }
        Monitor.Exit(locker);
      }
    }

    public override void ParsingSdamAll()
    {
      throw new NotImplementedException();
    }
  }
}
