using AngleSharp.Dom;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ParseSitesForApartments.Export;
using ParseSitesForApartments.Export.Creators;
using ParseSitesForApartments.UnionWithBase;

namespace ParseSitesForApartments.Sites
{
  public class ELMS : BaseParse
  {
    private List<int> listDistrict = new List<int>() { 38, 12, 43, 13, 4, 20, 6, 14, 7, 15, 8, 39, 9 };

    private Dictionary<int, string> district = new Dictionary<int, string>() { { 38, "Адмиралтейский" }, { 43, "Василеостровский" }, { 4, "Выборгский" }, { 6, "Калининский" }, { 7, "Кировский" }, { 9, "Красногвардейский" }, { 8, "Красносельский" }, { 12, "Московский" }, { 13, "Невский" }, { 20, "Петроградский" }, { 14, "Приморский" }, { 15, "Фрунзенский" }, { 39, "Центральный" }, };

    private List<Flat> listBuild = new List<Flat>();
    static object locker = new object();
    private static object lockerDistrict = new object();
    private const int MaxPage = 20;

    private int minPage = 1;
    private int maxPage = 20;
    private string filename = @"d:\ParserInfo\Appartament\ElmsProdam.csv";

    public override string Filename
    {
      get => filename;
      set => filename = value;
    }


    public override string FilenameSdam => @"d:\ParserInfo\Appartament\ElmsSdam.csv";
    public override string FilenameWithinfo => @"d:\ParserInfo\Appartament\ElmsProdamWithInfo.csv";
    public override string FilenameWithinfoSdam => @"d:\ParserInfo\Appartament\ElmsSdamWithInfo.csv";
    public override string NameSite => "ELMS";

    public delegate void Append(object sender, AppendFlatEventArgs e);
    public event Append OnAppend;
    private readonly UnionParseInfoWithDataBase unionInfo = new UnionParseInfoWithDataBase();

    private Thread studiiThread;
    private Thread oneThread;
    private Thread twoThread;
    private Thread threeThread;
    private Thread fourThread;
    private Thread fiveThread;
    private Thread studiiThreadOld;
    private Thread oneThreadOld;
    private Thread twoThreadOld;
    private Thread threeThreadOld;
    private Thread fourThreadOld;
    private Thread fiveThreadOld;
    private CoreExport export;

    public ELMS(List<District> listDistricts, List<Metro> lisMetro) : base(listDistricts, lisMetro)
    {
      //CoreCreator creator = new CsvExportCreator();
      //export = creator.FactoryCreate(Filename);
      //OnAppend += export.AddFlatInList;
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

    public override void ParsingAll()
    {
      CreateExport();
      studiiThreadOld = new Thread(ChangeDistrictAndPage);
      studiiThreadOld.Start("Студия");
      oneThreadOld = new Thread(ChangeDistrictAndPage);
      oneThreadOld.Start("1 км. кв.");
      twoThreadOld = new Thread(ChangeDistrictAndPage);
      twoThreadOld.Start("2 км. кв.");
      threeThreadOld = new Thread(ChangeDistrictAndPage);
      threeThreadOld.Start("3 км. кв.");
      fourThreadOld = new Thread(ChangeDistrictAndPage);
      fourThreadOld.Start("4 км. кв.");
      fiveThreadOld = new Thread(ChangeDistrictAndPage);
      fiveThreadOld.Start("5 км. кв.");

      studiiThread = new Thread(ChangeDistrictAndPage);
      studiiThread.Start("Студия Н");
      oneThread = new Thread(ChangeDistrictAndPage);
      oneThread.Start("1 км. кв. Н");
      twoThread = new Thread(ChangeDistrictAndPage);
      twoThread.Start("2 км. кв. Н");
      threeThread = new Thread(ChangeDistrictAndPage);
      threeThread.Start("3 км. кв. Н");
      fourThread = new Thread(ChangeDistrictAndPage);
      fourThread.Start("4 км. кв. Н");
      fiveThread = new Thread(ChangeDistrictAndPage);
      fiveThread.Start("5 км. кв. Н");
    }

    public override void ParsingStudii()
    {
      studiiThreadOld = new Thread(ChangeDistrictAndPage);
      studiiThreadOld.Start("Студия");
      studiiThread = new Thread(ChangeDistrictAndPage);
      studiiThread.Start("Студия Н");
    }

    public override void ParsingOne()
    {
      oneThreadOld = new Thread(ChangeDistrictAndPage);
      oneThreadOld.Start("1 км. кв.");
      oneThread = new Thread(ChangeDistrictAndPage);
      oneThread.Start("1 км. кв. Н");
    }

    public override void ParsingTwo()
    {
      twoThreadOld = new Thread(ChangeDistrictAndPage);
      twoThreadOld.Start("2 км. кв.");
      twoThread = new Thread(ChangeDistrictAndPage);
      twoThread.Start("2 км. кв. Н");
    }

    public override void ParsingThree()
    {
      threeThreadOld = new Thread(ChangeDistrictAndPage);
      threeThreadOld.Start("3 км. кв.");
      threeThread = new Thread(ChangeDistrictAndPage);
      threeThread.Start("3 км. кв. Н");
    }

    public override void ParsingFour()
    {
      fourThreadOld = new Thread(ChangeDistrictAndPage);
      fourThreadOld.Start("4 км. кв.");
      fourThread = new Thread(ChangeDistrictAndPage);
      fourThread.Start("4 км. кв. Н");
    }

    public override void ParsingMoreFour()
    {
      fiveThreadOld = new Thread(ChangeDistrictAndPage);
      fiveThreadOld.Start("5 км. кв.");
      fiveThread = new Thread(ChangeDistrictAndPage);
      fiveThread.Start("5 км. кв. Н");
    }

    private void ChangeDistrictAndPage(object typeRoom)
    {
      HtmlParser parser = new HtmlParser();
      using (var webClient = new WebClient())
      {
        webClient.Encoding = Encoding.GetEncoding("windows-1251");
        string url = "";
        foreach (var distr in district)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            switch (typeRoom)
            {
              case "Студия":
                url =
                  $@"https://www.emls.ru/flats/page{i}.html?query=s/1/r0/1/is_auction/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
                break;
              case "Студия Н":
                url = $@"https://www.emls.ru/new/page{i}.html?query=s/1/dist/{distr.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{distr.Key}/by_room/1";
                break;
              case "1 км. кв.":
                url = $@"https://www.emls.ru/flats/page{i}.html?query=s/1/r1/1/is_auction/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
                break;
              case "1 км. кв. Н":
                url = $@"https://www.emls.ru/new/page{i}.html?query=s/1/dist/{distr.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{distr.Key}/r1/1/by_room/1";
                break;
              case "2 км. кв.":
                url = $@"https://www.emls.ru/flats/page{i}.html?query=s/1/r2/1/is_auction/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
                break;
              case "2 км. кв. Н":
                url = $@"https://www.emls.ru/new/page{i}.html?query=s/1/dist/{distr.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{distr.Key}/r2/1/by_room/1";
                break;
              case "3 км. кв.":
                url = $@"https://www.emls.ru/flats/page{i}.html?query=s/1/r3/1/is_auction/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
                break;
              case "3 км. кв. Н":
                url = $@"https://www.emls.ru/new/page{i}.html?query=s/1/dist/{distr.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{distr.Key}/r3/1/by_room/1";
                break;
              case "4 км. кв. Н":
                url = $@"https://www.emls.ru/new/page{i}.html?query=s/1/dist/{distr.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{distr.Key}/r4/1/by_room/1";
                break;
              case "4 км. кв.":
                url = $@"https://www.emls.ru/flats/page{i}.html?query=s/1/r4/1/is_auction/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
                break;
              case "5 км. кв. Н":
                url = $@"https://www.emls.ru/new/page{i}.html?query=s/1/dist/{distr.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{distr.Key}/r5/1/by_room/1";
                break;
              case "5 км. кв.":
                url = $@"https://www.emls.ru/flats/page{i}.html?query=s/1/r5/1/is_auction/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
                break;
            }
            if (!ExecuteParse(url, webClient, parser, (string)typeRoom,
              ListDistricts.Where(x => x.Name.ToLower() == distr.Value.ToLower()).First()))
              break;
          }
        }
      }

      MessageBox.Show($"Закончили - {typeRoom}");
    }

    private bool ExecuteParse(string url, WebClient webClient, HtmlParser parser, string typeRoom, District district)
    {
      var random = new Random();
      Thread.Sleep(random.Next(2000, 4000));
      //try
      //{
      var responce = webClient.DownloadString(url);
      var document = parser.Parse(responce);
      var tableElements = document.GetElementsByClassName("row1");

      if (typeRoom.Contains("Н"))
        ParseSheetNov(tableElements, typeRoom, district);
      else
        ParseSheet(tableElements, typeRoom, district);

      if (tableElements.Length == 0)
        return false;
      return true;
      //}
      //catch (Exception e)
      //{
      //  //TODO Если страница долго не отвечает то пропускаем ее
      //  Thread.Sleep(1000);
      //  return true;
      //}
    }

    public void ParseStudii()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r0/1/is_auction/2/place/address/reg/2/dept/2/dist/{item.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              //else
              //  ParseSheet(tableElements, "Студия", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили студии");
    }
    public void ParseOneRoom()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r1/1/is_auction/2/place/address/reg/2/dept/2/dist/{item.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              //else
              // ParseSheet(tableElements, "1 км. кв.", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 1 км. кв.");
    }
    public void ParseTwoRoom()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r2/1/is_auction/2/place/address/reg/2/dept/2/dist/{item.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              //else
              //  ParseSheet(tableElements, "2 км. кв.", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 2 км. кв.");
    }
    public void ParseThreeRoom()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r3/1/is_auction/2/place/address/reg/2/dept/2/dist/{item.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              //else
              //  ParseSheet(tableElements, "3 км. кв.", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 3 км. кв.");
    }
    public void ParseFourRoom()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r4/1/is_auction/2/place/address/reg/2/dept/2/dist/{item.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              //else
              //  ParseSheet(tableElements, "4 км. кв.", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 4 км. кв.");
    }
    public void ParseFiveRoom()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/flats/page{j}.html?query=s/1/r5/1/is_auction/2/place/address/reg/2/dept/2/dist/{item.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              //else
              //  ParseSheet(tableElements, "5 км. кв.", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 5 км. кв.");
    }



    public void ParsingStudiiNov()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/new/page{j}.html?query=s/1/dist/{item.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{item.Key}/by_room/1";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              //else
              //  ParseSheetNov(tableElements, "Студия Н", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили студии");
    }
    public void ParsingOneNov()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/new/page{j}.html?query=s/1/dist/{item.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{item.Key}/r1/1/by_room/1";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              //else
              //  ParseSheetNov(tableElements, "1 км. кв. Н", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 1 км. кв. нов.");
    }
    public void ParsingTwoNov()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/new/page{j}.html?query=s/1/dist/{item.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{item.Key}/r2/1/by_room/1";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              //else
              //  ParseSheetNov(tableElements, "2 км. кв. Н", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 2 км. кв. нов.");
    }
    public void ParsingThreeNov()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/new/page{j}.html?query=s/1/dist/{item.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{item.Key}/r3/1/by_room/1";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              //else
              //  ParseSheetNov(tableElements, "3 км. кв. Н", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 3 км. кв. нов.");
    }
    public void ParsingFourNov()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/new/page{j}.html?query=s/1/dist/{item.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{item.Key}/r4/1/by_room/1";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              //else
              //  ParseSheetNov(tableElements, "4 км. кв. Н", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 4 км. кв. нов.");
    }
    public void ParsingFiveNov()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          foreach (var item in district)
          {
            for (int j = 1; j < MaxPage; j++)
            {
              Thread.Sleep(random.Next(2000, 3000));
              string sdutii = $@"https://www.emls.ru/new/page{j}.html?query=s/1/dist/{item.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{item.Key}/r5/1/by_room/1";
              webClient.Encoding = Encoding.GetEncoding("windows-1251");
              var responce = webClient.DownloadString(sdutii);
              var parser = new HtmlParser();
              var document = parser.Parse(responce);

              var tableElements = document.GetElementsByClassName("row1");
              if (tableElements.Length == 0)
                break;
              //else
              //  ParseSheetNov(tableElements, "5 км. кв. Н", item.Value);
            }
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 5 км. кв. нов.");
    }

    private void ParseSheet(IHtmlCollection<IElement> collection, string typeRoom, District district)
    {
      for (int i = 0; i < collection.Length; i++)
      {
        var flat = new Flat
        {
          CountRoom = typeRoom
        };
        //try
        //{
        flat.Url = $@"https://www.emls.ru{collection[i].ParentElement.GetAttribute("href")}";
        if (collection[i].GetElementsByClassName("w-image").Length > 0)
        {
          //var divImage = collection[i].GetElementsByClassName("w-image")[0];
          var square = collection[i].GetElementsByClassName("space-all");
          if (square.Length > 0)
            flat.Square = square[0].TextContent.Replace(".", ",");

          string street = string.Empty;
          string number = string.Empty;
          string structure = string.Empty;
          string liter = string.Empty;

          if (collection[i].GetElementsByClassName("address-geo").Length > 0)
          {
            var adr = collection[i].GetElementsByClassName("address-geo")[0].TextContent.Split(',');
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
          }
          var regex = new Regex(@"(к\d+)");
          structure = regex.Match(number).Value;
          if (!string.IsNullOrEmpty(structure))
          {
            number = number.Replace(structure, "");
            structure = structure.Replace("к", "");
          }
          regex = new Regex(@"(\D)");
          liter = regex.Match(number).Value;
          if (!string.IsNullOrEmpty(liter))
            number = number.Replace(liter, "");

          if (street.Contains("(Горелово)"))
          {
            street = street.Replace("(Горелово)", "");
          }
          else if (street.Contains("Красное Село"))
          {
            street = street.Replace("Красное Село", "");
          }
          else if (street.Contains("Парголово"))
          {
            street = street.Replace("Парголово", "");
          }
          street = street.Replace("ул.", "").Replace("ал.", "").Replace("бул.", "").Replace("ш.", "").Replace("пр.", "").Replace("пер.", "").Replace("пр-д", "").Replace(" б", "").Trim();

          Building building = null;
          Monitor.Enter(lockerDistrict);
          if (district.Buildings.Count != 0)
          {
            var bldsEnum =
              district.Buildings.Where(x => x.Street == street && x.Number == number && x.Structure == structure);
            if (bldsEnum.Count() > 0)
              building = bldsEnum.First();
          }
          if (building == null)
          {
            building = new Building
            {
              Street = street,
              Number = number,
              Structure = structure,
              District = district,
            };
            district.Buildings.Add(building);
          }
          Monitor.Exit(lockerDistrict);

          if (building.MetroObj == null)
          {
            var metro = collection[i].GetElementsByClassName("metroline-2");
            if (metro.Length > 0)
            {
              var mt = metro[0].TextContent.Replace("пр.", "").Replace("ул.", "").Replace("и-т", "");
              if (mt == "А.Hевского пл.")
                mt = "Площадь Александра Невского";
              else if (mt == "Мужества пл.")
                mt = "Площадь Мужества";
              else if (mt == "Восстания пл.")
                mt = "Площадь Восстания";
              else if (mt == "Ленина пл.")
                mt = "Площадь Ленина  ";
              else if (mt == "Сенная пл.")
                mt = "Сенная площадь";
              var metroObjEnum = ListMetros.Where(x => x.Name.ToUpper().Contains(mt.ToUpper()));
              if (metroObjEnum.Count() > 0)
              {
                building.MetroObj = metroObjEnum.First();
              }
            }
          }

          regex = new Regex(@"(\d+)");
          var floor = collection[i].GetElementsByClassName("w-floor");
          if (floor.Length > 0)
          {
            var ms = regex.Matches(floor[0].TextContent);
            if (ms.Count > 0)
              flat.Floor = ms[0].Value;
          }

          flat.Building = building;

          //regex = new Regex(@"(\d+\s+\d+\s+метров)|(\d+\s+метров)");
          //var distance = collection[i].GetElementsByClassName("ellipsis em");
          //if (distance.Length > 0)
          //{
          //  flat.Building.Distance = regex.Match(distance[0].TextContent.Replace("\n", "").Trim()).Value;
          //}

          var pr = collection[i].GetElementsByClassName("price");
          if (pr.Length > 0)
          {
            string priceStr = pr[0].TextContent.Replace(" a", "").Replace(" ", "");
            int price;
            if (int.TryParse(priceStr, out price))
            {
              flat.Price = price;
            }
          }
          //flat.Building.Metro = flat.Building.Metro.Replace(" и-т", "");


          if (!string.IsNullOrWhiteSpace(flat.Building.Number))
          {
            Monitor.Enter(locker);
            if (string.IsNullOrWhiteSpace(flat.Building.DateBuild))
            {
              unionInfo.UnionInfoProdam(flat);
            }
            OnAppend(this, new AppendFlatEventArgs { Flat = flat });
            Monitor.Exit(locker);
          }

          //regex = new Regex(@"(\d+)");
          //var val = regex.Match(flat.Building.Liter).Value;
          //if (string.IsNullOrWhiteSpace(val))
          //{
          //  Monitor.Enter(locker);
          //  bool flag = false;
          //  foreach (var bl in listBuild)
          //  {
          //    if (flat.Building.Equals(bl))
          //    {
          //      flag = true;
          //      break;
          //    }
          //  }
          //  if (!flag)
          //  {
          //    if (!string.IsNullOrWhiteSpace(flat.Building.Number))
          //    {
          //      if (flat.Building.Liter != "-" && !flat.Building.Liter.Contains("/"))
          //      {
          //        listBuild.Add(flat);
          //        using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Open), Encoding.UTF8))
          //        {
          //          sw.BaseStream.Position = sw.BaseStream.Length;
          //          sw.WriteLine($@"{district};{flat.Building.Street};{flat.Building.Number};{flat.Building.Structure};{flat.Building.Liter};{flat.CountRoom};{flat.Square};{flat.Price};{flat.Floor};{flat.Building.Metro};{flat.Building.Distance}");
          //        }
          //      }
          //    }
          //  }
          //  Monitor.Exit(locker);
          //}
        }
        //}
        //catch (Exception ex)
        //{
        //  MessageBox.Show(ex.Message);
        //}
      }
    }

    private void ParseSheetNov(IHtmlCollection<IElement> collection, string typeRoom, District district)
    {
      for (int i = 0; i < collection.Length; i++)
      {
        var flat = new Flat
        {
          CountRoom = typeRoom
        };
        var href = collection[i].ParentElement.GetAttribute("href");
        if(href != "javascript:void(0)")
          flat.Url = $@"https://www.emls.ru{href}";
        string street = string.Empty;
        string number = string.Empty;
        string structure = string.Empty;
        string liter = string.Empty;

        //string street = "";
        //string number = "";
        //string building = "";
        //string liter = "";
        //string metro = "";
        //string distance = "";

        #region Адрес

        if (collection[i].GetElementsByClassName("address-geo").Length > 1)
        {
          var adr = collection[i].GetElementsByClassName("address-geo")[1].TextContent.Split(',');
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
        }
        var regex = new Regex(@"(к\d+)");
        structure = regex.Match(number).Value;
        if (!string.IsNullOrEmpty(structure))
        {
          number = number.Replace(structure, "");
          structure = structure.Replace("к", "");
        }
        regex = new Regex(@"(\D)");
        liter = regex.Match(number).Value;
        if (!string.IsNullOrEmpty(liter))
          number = number.Replace(liter, "");

        //var met = collection[i].GetElementsByClassName("metroline-2");
        //if (met.Length > 0)
        //  metro = met[0].TextContent;

        //regex = new Regex(@"(\d+\s+\d+\s+метров)|(\d+\s+метров)");
        //var dis = collection[i].GetElementsByClassName("ellipsis em");
        //if (dis.Length > 0)
        //{
        //  distance = dis[0].TextContent.Replace("\n", "").Trim();
        //}
        //if (!string.IsNullOrEmpty(distance))
        //  metro = metro.Replace(distance, "").Trim();
        #endregion

        var pr = collection[i].GetElementsByClassName("price");
        if (pr.Length > 0)
        {
          string priceStr = pr[0].TextContent.Replace(" a", "").Replace(" ", "");
          if (!string.IsNullOrEmpty(priceStr))
          {
            int price;
            if (int.TryParse(priceStr, out price))
            {
              flat.Price = price;
            }

            if (collection[i].GetElementsByClassName("w-image").Length > 0)
            {
              var divImage = collection[i].GetElementsByClassName("w-image")[0];
              var square = collection[i].GetElementsByClassName("space-all");
              if (square.Length > 0)
                flat.Square = square[0].TextContent;
            }

            regex = new Regex(@"(\d+)");
            var floor = collection[i].GetElementsByClassName("w-floor");
            if (floor.Length > 0)
            {
              var ms = regex.Matches(floor[0].TextContent);
              if (ms.Count > 0)
                flat.Floor = ms[0].Value;
            }
          }
          else
          {
            var rows = collection[i].GetElementsByClassName("w-kv-row");
            for (int j = 0; j < rows.Length; j++)
            {
              var floor = rows[j].GetElementsByClassName("circle-floor");
              if (floor.Length > 0)
                flat.Floor = floor[0].TextContent.Trim();
              var sq = rows[j].GetElementsByClassName("w-kv-area");
              if (sq.Length > 0)
                flat.Square = sq[0].TextContent.Trim();
              var price = rows[j].GetElementsByClassName("w-kv-price");
              if (price.Length > 0)
              {
                regex = new Regex(@"(\d+\s+\d+\s+\d+)");
                priceStr = regex.Match(price[0].TextContent).Value.Replace(" ", "");
                int pri;
                if (int.TryParse(priceStr, out pri))
                {
                  flat.Price = pri;
                }
              }
            }
          }
        }

        liter = regex.Match(number).Value;
        if (!string.IsNullOrEmpty(liter))
          number = number.Replace(liter, "");

        if (street.Contains("(Горелово)"))
        {
          street = street.Replace("(Горелово)", "");
        }
        else if (street.Contains("Красное Село"))
        {
          street = street.Replace("Красное Село", "");
        }
        else if (street.Contains("Парголово"))
        {
          street = street.Replace("Парголово", "");
        }
        street = street.Replace("ул.", "").Replace("ал.", "").Replace("бул.", "").Replace("ш.", "").Replace("пр.", "").Replace("пер.", "").Replace("пр-д", "").Replace(" б", "").Trim();

        Building building = null;
        Monitor.Enter(lockerDistrict);
        if (district.Buildings.Count != 0)
        {
          var bldsEnum =
            district.Buildings.Where(x => x.Street == street && x.Number == number && x.Structure == structure);
          if (bldsEnum.Count() > 0)
            building = bldsEnum.First();
        }
        if (building == null)
        {
          building = new Building
          {
            Street = street,
            Number = number,
            Structure = structure,
            District = district,
          };
          district.Buildings.Add(building);
        }
        Monitor.Exit(lockerDistrict);

        if (building.MetroObj == null)
        {
          var metro = collection[i].GetElementsByClassName("metroline-2");
          if (metro.Length > 0)
            building.Metro = metro[0].TextContent;

          regex = new Regex(@"(\d+)");
          var floor = collection[i].GetElementsByClassName("w-floor");
          if (floor.Length > 0)
          {
            var ms = regex.Matches(floor[0].TextContent);
            if (ms.Count > 0)
              flat.Floor = ms[0].Value;
          }
        }

        flat.Building = building;

        regex = new Regex(@"(\d+\s+\d+\s+метров)|(\d+\s+метров)");
        var distance = collection[i].GetElementsByClassName("ellipsis em");
        if (distance.Length > 0)
        {
          flat.Building.Distance = regex.Match(distance[0].TextContent.Replace("\n", "").Trim()).Value;
        }
        //flat.Building.Metro = flat.Building.Metro.Replace(" и-т", "");


        if (!string.IsNullOrWhiteSpace(flat.Building.Number))
        {
          Monitor.Enter(locker);
          if (string.IsNullOrWhiteSpace(flat.Building.DateBuild))
          {
            unionInfo.UnionInfoProdam(flat);
          }
          OnAppend(this, new AppendFlatEventArgs { Flat = flat });
          Monitor.Exit(locker);
        }
        //if (!string.IsNullOrEmpty(flat.Building.Number))
        //{
        //  regex = new Regex(@"(\d+)");
        //  var val = regex.Match(flat.Building.Liter).Value;
        //  if (string.IsNullOrEmpty(val))
        //  {
        //Monitor.Enter(locker);
        //bool flag = false;
        //foreach (var bl in listBuild)
        //{
        //  if (item.Equals(bl))
        //  {
        //    flag = true;
        //    break;
        //  }
        //}
        //if (!flag)
        //{
        //  if (item.Building.Liter != "-" && !item.Building.Liter.Contains("/"))
        //  {
        //    listBuild.Add(item);
        //    using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Open), Encoding.UTF8))
        //    {
        //      sw.BaseStream.Position = sw.BaseStream.Length;
        //      sw.WriteLine($@"{town};{item.Building.Street};{item.Building.Number};{item.Building.Structure};{item.Building.Liter};{item.CountRoom};{item.Square};{item.Price};{item.Floor};{item.Building.Metro};{item.Building.Distance};{district}");
        //    }
        //  }
        //}
        //Monitor.Exit(locker);

        //  if (string.IsNullOrWhiteSpace(flat.Building.DateBuild))
        //  {
        //    if (!string.IsNullOrWhiteSpace(flat.Building.Number))
        //    {
        //      Monitor.Enter(locker);
        //      unionInfo.UnionInfoProdam(flat);
        //      Monitor.Exit(locker);
        //      OnAppend(this, new AppendFlatEventArgs { Flat = flat });
        //    }
        //  }
        //  else
        //  {
        //    OnAppend(this, new AppendFlatEventArgs { Flat = flat });
        //  }
        //}
        //}
      }
    }

    public override void ParsingSdamAll()
    {
      using (var sw = new StreamWriter(new FileStream(FilenameSdam, FileMode.Create), Encoding.UTF8))
      {
        sw.WriteLine($@"Нас. пункт;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние");
      }

      var studiiThread = new Thread(ParseStudiiSdam);
      studiiThread.Start();
      Thread.Sleep(30000);
      var oneThread = new Thread(ParseOneSdam);
      oneThread.Start();
      Thread.Sleep(30000);
      var twoThread = new Thread(ParseTwoSdam);
      twoThread.Start();
      Thread.Sleep(30000);
      var threeThread = new Thread(ParseThreeSdam);
      threeThread.Start();
      Thread.Sleep(30000);
      var fourThread = new Thread(ParseFourSdam);
      fourThread.Start();
      Thread.Sleep(30000);
      var fiveThread = new Thread(ParseFiveSdam);
      fiveThread.Start();
    }

    public void ParseStudiiSdam()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          for (int j = 1; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/arenda/page{j}.html?query=s/1/r0/1/type/2/rtype/2/place/address/reg/2/dept/2/sort1/4/dir1/1/dir2/2/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheetSdam(tableElements, "Студия");
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили студии сдам");
    }
    public void ParseOneSdam()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          for (int j = 1; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/arenda/page{j}.html?query=s/1/r1/1/type/2/rtype/2/place/address/reg/2/dept/2/sort1/4/dir1/1/dir2/2/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheetSdam(tableElements, "1 км. кв.");
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 1 км. кв. сдам");
    }
    public void ParseTwoSdam()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          for (int j = 1; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/arenda/page{j}.html?query=s/1/r2/1/type/2/rtype/2/place/address/reg/2/dept/2/sort1/4/dir1/1/dir2/2/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheetSdam(tableElements, "2 км. кв.");
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 2 км. кв. сдам");
    }
    public void ParseThreeSdam()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          for (int j = 1; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/arenda/page{j}.html?query=s/1/r3/1/type/2/rtype/2/place/address/reg/2/dept/2/sort1/4/dir1/1/dir2/2/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheetSdam(tableElements, "3 км. кв.");
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 3 км. кв. сдам");
    }
    public void ParseFourSdam()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          for (int j = 1; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/arenda/page{j}.html?query=s/1/r4/1/type/2/rtype/2/place/address/reg/2/dept/2/sort1/4/dir1/1/dir2/2/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheetSdam(tableElements, "4 км. кв.");
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 4 км. кв. сдам");
    }
    public void ParseFiveSdam()
    {
      using (var webClient = new WebClient())
      {
        try
        {
          var random = new Random();
          for (int j = 1; j < MaxPage; j++)
          {
            Thread.Sleep(random.Next(2000, 3000));
            string sdutii = $@"https://www.emls.ru/arenda/page{j}.html?query=s/1/r5/1/type/2/rtype/2/place/address/reg/2/dept/2/sort1/4/dir1/1/dir2/2/interval/3";
            webClient.Encoding = Encoding.GetEncoding("windows-1251");
            var responce = webClient.DownloadString(sdutii);
            var parser = new HtmlParser();
            var document = parser.Parse(responce);

            var tableElements = document.GetElementsByClassName("row1");
            if (tableElements.Length == 0)
              break;
            else
              ParseSheetSdam(tableElements, "5 км. кв.");
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      MessageBox.Show("Закончили 5 км. кв. сдам");
    }


    private void ParseSheetSdam(IHtmlCollection<IElement> collection, string typeRoom)
    {
      for (int i = 0; i < collection.Length; i++)
      {
        var flat = new Flat
        {
          CountRoom = typeRoom
        };
        try
        {
          if (collection[i].GetElementsByClassName("w-image").Length > 0)
          {
            var divImage = collection[i].GetElementsByClassName("w-image")[0];
            var square = collection[i].GetElementsByClassName("space-all");
            if (square.Length > 0)
              flat.Square = square[0].TextContent;

            if (collection[i].GetElementsByClassName("address-geo").Length > 0)
            {
              var adr = collection[i].GetElementsByClassName("address-geo")[0].TextContent.Split(',');
              if (adr.Length == 3)
              {
                flat.Building.Street = adr[0] + " " + adr[1];
                flat.Building.Number = adr[2];
              }
              else
              {
                flat.Building.Street = adr[0];
                if (adr.Length > 1)
                  flat.Building.Number = adr[1].Trim();
              }
            }
            var regex = new Regex(@"(к\d+)");
            flat.Building.Structure = regex.Match(flat.Building.Number).Value;
            if (!string.IsNullOrEmpty(flat.Building.Structure))
            {
              flat.Building.Number = flat.Building.Number.Replace(flat.Building.Structure, "");
              flat.Building.Structure = flat.Building.Structure.Replace("к", "");
            }
            regex = new Regex(@"(\D)");
            flat.Building.Liter = regex.Match(flat.Building.Number).Value;
            if (!string.IsNullOrEmpty(flat.Building.Liter))
              flat.Building.Number = flat.Building.Number.Replace(flat.Building.Liter, "");


            var metro = collection[i].GetElementsByClassName("metroline-2");
            if (metro.Length > 0)
              flat.Building.Metro = metro[0].TextContent;

            regex = new Regex(@"(\d+)");
            var floor = collection[i].GetElementsByClassName("w-floor");
            if (floor.Length > 0)
            {
              var ms = regex.Matches(floor[0].TextContent);
              if (ms.Count > 0)
                flat.Floor = ms[0].Value;
            }

            regex = new Regex(@"(\d+\s+\d+\s+метров)|(\d+\s+метров)");
            var distance = collection[i].GetElementsByClassName("ellipsis em");
            if (distance.Length > 0)
            {
              flat.Building.Distance = regex.Match(distance[0].TextContent.Replace("\n", "").Trim()).Value;
            }

            var pr = collection[i].GetElementsByClassName("price");
            if (pr.Length > 0)
            {
              string priceStr = pr[0].TextContent.Replace(" a", "").Replace("a/мес", "").Replace(" ", "").Trim();
              int price;
              if (int.TryParse(priceStr, out price))
              {
                flat.Price = price;
              }
            }

            string town = string.Empty;
            if (flat.Building.Street.Contains("(Горелово)"))
            {
              town = "Горелово";
              flat.Building.Street = flat.Building.Street.Replace("(Горелово)", "");
            }
            else if (flat.Building.Street.Contains("Красное Село"))
            {
              town = "Красное Село";
              flat.Building.Street = flat.Building.Street.Replace(town, "");
            }
            else if (flat.Building.Street.Contains("Парголово"))
            {
              town = "Парголово";
              flat.Building.Street = flat.Building.Street.Replace(town, "");
            }
            else
              town = "Санкт-Петербург";


            flat.Building.Street = flat.Building.Street.Replace("ул.", "").Replace("ал.", "").Replace("бул.", "").Replace("ш.", "").Replace("пр.", "").Replace("пер.", "").Replace("пр-д", "").Replace(" б", "").Trim();

            Monitor.Enter(locker);
            bool flag = false;
            foreach (var bl in listBuild)
            {
              if (flat.Equals(bl))
              {
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              if (!string.IsNullOrEmpty(flat.Building.Number))
              {
                listBuild.Add(flat);

                using (var sw = new StreamWriter(new FileStream(FilenameSdam, FileMode.Open), Encoding.UTF8))
                {
                  sw.BaseStream.Position = sw.BaseStream.Length;
                  sw.WriteLine($@"{town};{flat.Building.Street};{flat.Building.Number};{flat.Building.Structure};{flat.Building.Liter};{flat.CountRoom};{flat.Square};{flat.Price};{flat.Floor};{flat.Building.Metro};{flat.Building.Distance}");
                }
              }
            }
            Monitor.Exit(locker);
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
      }
    }
  }
}
