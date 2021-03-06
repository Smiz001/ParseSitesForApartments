﻿using AngleSharp.Dom;
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
using ParseSitesForApartments.Enum;
using ParseSitesForApartments.Export;
using ParseSitesForApartments.Export.Creators;
using ParseSitesForApartments.ParsClasses;
using ParseSitesForApartments.Proxy;
using ParseSitesForApartments.UI;
using ParseSitesForApartments.UnionWithBase;

namespace ParseSitesForApartments.Sites
{
  public class ELMS : BaseParse
  {

    private Dictionary<int, string> district = new Dictionary<int, string>() { { 38, "Адмиралтейский" }, { 43, "Василеостровский" }, { 4, "Выборгский" }, { 6, "Калининский" }, { 7, "Кировский" }, { 9, "Красногвардейский" }, { 8, "Красносельский" }, { 12, "Московский" }, { 13, "Невский" }, { 20, "Петроградский" }, { 14, "Приморский" }, { 15, "Фрунзенский" }, { 39, "Центральный" }, };

    private Dictionary<string, string> districtForNew = new Dictionary<string, string>() { { "%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9", "Адмиралтейский" }, { "%C2%E0%F1%E8%EB%E5%EE%F1%F2%F0%EE%E2%F1%EA%E8%E9", "Василеостровский" }, { "%C2%FB%E1%EE%F0%E3%F1%EA%E8%E9", "Выборгский" }, { "%CA%E0%EB%E8%ED%E8%ED%F1%EA%E8%E9", "Калининский" }, { "%CA%E8%F0%EE%E2%F1%EA%E8%E9", "Кировский" }, { "%CA%F0%E0%F1%ED%EE%E3%E2%E0%F0%E4%E5%E9%F1%EA%E8%E9", "Красногвардейский" }, { "%CA%F0%E0%F1%ED%EE%F1%E5%EB%FC%F1%EA%E8%E9", "Красносельский" }, { "%CC%EE%F1%EA%EE%E2%F1%EA%E8%E9", "Московский" }, { "%CD%E5%E2%F1%EA%E8%E9", "Невский" }, { "%CF%E5%F2%F0%EE%E3%F0%E0%E4%F1%EA%E8%E9", "Петроградский" }, { "%CF%F0%E8%EC%EE%F0%F1%EA%E8%E9", "Приморский" }, { "%D4%F0%F3%ED%E7%E5%ED%F1%EA%E8%E9", "Фрунзенский" }, { "%D6%E5%ED%F2%F0%E0%EB%FC%ED%FB%E9", "Центральный" }, };

    private List<Flat> listBuild = new List<Flat>();
    static object locker = new object();
    private static object lockerDistrict = new object();

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
    private Thread sixThreadOld;
    private Thread studiiThreadOld;
    private Thread oneThreadOld;
    private Thread twoThreadOld;
    private Thread threeThreadOld;
    private Thread fourThreadOld;
    private Thread fiveThreadOld;
    private CoreExport export;

    private Thread studiiRentThread;
    private Thread oneRentThread;
    private Thread twoRentThread;
    private Thread threeRentThread;
    private Thread fourRentThread;
    private Thread fiveRentThread;
    private Thread sixRentThreadOld;
    private ProgressForm progress;
    private int count = 1;

    public ELMS(List<District> listDistricts, List<Metro> listMetros, List<ProxyInfo> listProxy) : base(listDistricts, listMetros, listProxy)
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
            sw.WriteLine(@"Район;Улица;Номер;Корпус;Литер;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Дата постройки;Дата реконструкции;Даты кап. ремонты;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов;Расстояние пешком;Время пешком;Расстояние на машине;Время на машине;Откуда взято");
          }
        }
      }
    }

    private void CheckCloseThread(int countRoom)
    {
      if (countRoom == -1)
      {
        while (true)
        {
          if (!studiiThread.IsAlive)
          {
            if (!oneThread.IsAlive)
            {
              if (!twoThread.IsAlive)
              {
                if (!threeThread.IsAlive)
                {
                  if (!fourThread.IsAlive)
                  {
                    if (!fiveThread.IsAlive)
                    {
                      if (!studiiThreadOld.IsAlive)
                      {
                        if (!oneThreadOld.IsAlive)
                        {
                          if (!twoThreadOld.IsAlive)
                          {
                            if (!threeThreadOld.IsAlive)
                            {
                              if (!fourThreadOld.IsAlive)
                              {
                                if (!fiveThreadOld.IsAlive)
                                {
                                  break;
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
            }
          }
        }
      }

      if (countRoom == 0)
      {
        while (true)
        {
          if (!studiiThread.IsAlive)
          {
            if (!studiiThreadOld.IsAlive)
              break;
          }
        }
      }

      if (countRoom == 1)
      {
        while (true)
        {
          if (!oneThread.IsAlive)
          {
            if (!oneThreadOld.IsAlive)
              break;
          }
        }
      }

      if (countRoom == 2)
      {
        while (true)
        {
          if (!twoThread.IsAlive)
          {
            if (!twoThreadOld.IsAlive)
              break;
          }
        }
      }

      if (countRoom == 3)
      {
        while (true)
        {
          if (!threeThread.IsAlive)
          {
            if (!threeThreadOld.IsAlive)
              break;
          }
        }
      }

      if (countRoom == 4)
      {
        while (true)
        {
          if (!fourThread.IsAlive)
          {
            if (!fourThreadOld.IsAlive)
              break;
          }
        }
      }

      if (countRoom == 5)
      {
        while (true)
        {
          if (!fiveThread.IsAlive)
          {
            if (!fiveThreadOld.IsAlive)
              break;
          }
        }
      }
    }

    public override void ParsingAll()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("ELMS Все квартиры");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
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
                sixThreadOld = new Thread(ChangeDistrictAndPage);
                sixThreadOld.Start("6 км. кв.");

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

                Thread.Sleep(10000);
                CheckCloseThread(-1);
                export.Execute();

                MessageBox.Show("Загрузка завершена");
                progress.BeginInvoke(new Action(() => progress.Close()));
              }
              catch (Exception ex)
              {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
            }
          ));
        threadbackground.Start();
        progress.Show();
      }
      else
      {
        studiiRentThread = new Thread(ChangeDistrictAndPageForRent);
        studiiRentThread.Start("Студия");
        oneRentThread = new Thread(ChangeDistrictAndPageForRent);
        oneRentThread.Start("1 км. кв.");
        twoRentThread = new Thread(ChangeDistrictAndPageForRent);
        twoRentThread.Start("2 км. кв.");
        threeRentThread = new Thread(ChangeDistrictAndPageForRent);
        threeRentThread.Start("3 км. кв.");
        fourRentThread = new Thread(ChangeDistrictAndPageForRent);
        fourRentThread.Start("4 км. кв.");
        fiveRentThread = new Thread(ChangeDistrictAndPageForRent);
        fiveRentThread.Start("5 км. кв.");
        sixRentThreadOld = new Thread(ChangeDistrictAndPageForRent);
        sixRentThreadOld.Start("6 км. кв.");
      }
    }

    public override void ParsingStudii()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("ELMS Студии");
        var threadbackground = new Thread(
          new ThreadStart(() =>
          {
            try
            {
              studiiThreadOld = new Thread(ChangeDistrictAndPage);
              studiiThreadOld.Start("Студия");
              studiiThread = new Thread(ChangeDistrictAndPage);
              studiiThread.Start("Студия Н");

              Thread.Sleep(10000);
              CheckCloseThread(0);
              export.Execute();

              MessageBox.Show("Загрузка завершена");
              progress.BeginInvoke(new Action(() => progress.Close()));
            }
            catch (Exception ex)
            {
              MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          }
          ));
        threadbackground.Start();
        progress.Show();
      }
      else
      {
        studiiRentThread = new Thread(ChangeDistrictAndPageForRent);
        studiiRentThread.Start("Студия");
      }

    }

    public override void ParsingOne()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("ELMS 1 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                oneThreadOld = new Thread(ChangeDistrictAndPage);
                oneThreadOld.Start("1 км. кв.");
                oneThread = new Thread(ChangeDistrictAndPage);
                oneThread.Start("1 км. кв. Н");

                Thread.Sleep(10000);
                CheckCloseThread(1);
                export.Execute();

                MessageBox.Show("Загрузка завершена");
                progress.BeginInvoke(new Action(() => progress.Close()));
              }
              catch (Exception ex)
              {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
            }
          ));
        threadbackground.Start();
        progress.Show();
      }
      else
      {
        oneRentThread = new Thread(ChangeDistrictAndPageForRent);
        oneRentThread.Start("1 км. кв.");
      }
    }

    public override void ParsingTwo()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("ELMS 2 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                twoThreadOld = new Thread(ChangeDistrictAndPage);
                twoThreadOld.Start("2 км. кв.");
                twoThread = new Thread(ChangeDistrictAndPage);
                twoThread.Start("2 км. кв. Н");

                Thread.Sleep(10000);
                CheckCloseThread(2);
                export.Execute();

                MessageBox.Show("Загрузка завершена");
                progress.BeginInvoke(new Action(() => progress.Close()));
              }
              catch (Exception ex)
              {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
            }
          ));
        threadbackground.Start();
        progress.Show();
      }
      else
      {
        twoRentThread = new Thread(ChangeDistrictAndPageForRent);
        twoRentThread.Start("2 км. кв.");
      }
    }

    public override void ParsingThree()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("ELMS 3 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                threeThreadOld = new Thread(ChangeDistrictAndPage);
                threeThreadOld.Start("3 км. кв.");
                threeThread = new Thread(ChangeDistrictAndPage);
                threeThread.Start("3 км. кв. Н");

                Thread.Sleep(10000);
                CheckCloseThread(3);
                export.Execute();

                MessageBox.Show("Загрузка завершена");
                progress.BeginInvoke(new Action(() => progress.Close()));
              }
              catch (Exception ex)
              {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
            }
          ));
        threadbackground.Start();
        progress.Show();
      }
      else
      {
        threeRentThread = new Thread(ChangeDistrictAndPageForRent);
        threeRentThread.Start("3 км. кв.");
      }
    }

    public override void ParsingFour()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("ELMS 4 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                fourThreadOld = new Thread(ChangeDistrictAndPage);
                fourThreadOld.Start("4 км. кв.");
                fourThread = new Thread(ChangeDistrictAndPage);
                fourThread.Start("4 км. кв. Н");

                Thread.Sleep(10000);
                CheckCloseThread(4);
                export.Execute();

                MessageBox.Show("Загрузка завершена");
                progress.BeginInvoke(new Action(() => progress.Close()));
              }
              catch (Exception ex)
              {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
            }
          ));
        threadbackground.Start();
        progress.Show();
      }
      else
      {
        fourRentThread = new Thread(ChangeDistrictAndPageForRent);
        fourRentThread.Start("4 км. кв.");
      }
    }

    public override void ParsingMoreFour()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("ELMS 5+ км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                fiveThreadOld = new Thread(ChangeDistrictAndPage);
                fiveThreadOld.Start("5 км. кв.");
                fiveThread = new Thread(ChangeDistrictAndPage);
                fiveThread.Start("5 км. кв. Н");

                Thread.Sleep(10000);
                CheckCloseThread(5);
                export.Execute();

                MessageBox.Show("Загрузка завершена");
                progress.BeginInvoke(new Action(() => progress.Close()));
              }
              catch (Exception ex)
              {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
            }
          ));
        threadbackground.Start();
        progress.Show();
      }
      else
      {
        fiveRentThread = new Thread(ChangeDistrictAndPageForRent);
        fiveRentThread.Start("5 км. кв.");
        sixRentThreadOld = new Thread(ChangeDistrictAndPageForRent);
        sixRentThreadOld.Start("6 км. кв.");
      }
    }

    public override void ParsingSdamAll()
    {
      //throw new NotImplementedException();
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
              case "6 км. кв.":
                url = $@"https://www.emls.ru/flats/page{i}.html?query=s/1/r6/1/is_auction/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
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
      var parseStreet = new ParseStreet();
      Thread.Sleep(random.Next(2000, 4000));
      try
      {
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
      }
      catch (Exception e)
      {
        Log.Error(e.Message);
        //TODO Если страница долго не отвечает то пропускаем ее
        Thread.Sleep(1000);
        return true;
      }
    }

    private void ParseSheet(IHtmlCollection<IElement> collection, string typeRoom, District district)
    {
      var parseStreet = new ParseStreet();
      for (int i = 0; i < collection.Length; i++)
      {
        string street = string.Empty;
        string number = string.Empty;
        string structure = string.Empty;
        string liter = string.Empty;
        var flat = new Flat
        {
          CountRoom = typeRoom
        };
        //try
        //{
        var href = collection[i].ParentElement.GetAttribute("href");
        if (href != "javascript:void(0)")
          flat.Url = $@"https://www.emls.ru{href}";

        if (collection[i].GetElementsByClassName("w-image").Length > 0)
        {
          var divImage = collection[i].GetElementsByClassName("w-image")[0];
          if (typeRoom == "6 км. кв.")
          {
            var reg = new Regex(@"(\d+-комн\. квартира)");
            var room = reg.Match(divImage.TextContent).Value;
            reg = new Regex(@"(\d+)");
            room = reg.Match(room).Value;
            flat.CountRoom = $@"{room}  км. кв.";
          }

          var square = collection[i].GetElementsByClassName("space-all");
          if (square.Length > 0)
            flat.Square = square[0].TextContent.Replace(".", ",").Trim();


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
            //Удаление корпуса при условии что номер корпуса >7
            int valStr;
            if (int.TryParse(structure, out valStr))
            {
              if (valStr > 7)
              {
                number = $@"{number}/{structure}";
                structure = "";
              }
            }
          }
          regex = new Regex(@"(\D)");
          var mc = regex.Matches(number);
          if (mc.Count == 1)
          {
            liter = mc[0].Value;
          }
          else if(mc.Count == 2)
          {
            liter = mc[2].Value;
          }
          if (!string.IsNullOrEmpty(liter))
          {
            if (liter != "-" && liter != "/")
              number = number.Replace(liter, "");
            else
              liter = "";
          }

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
          //street = street.Replace("ул.", "").Replace("ал.", "").Replace("бул.", "").Replace("ш.", "").Replace("пр.", "").Replace("пер.", "").Replace("пр-д", "").Replace(" б", "").Trim();

          number = number.Trim();
          street = street.Trim();
          structure = structure.Trim();
          street = parseStreet.Execute(street, district);

          Building building = null;
          Monitor.Enter(lockerDistrict);
          if (district.Buildings.Count != 0)
          {
            var bldsEnum =
              district.Buildings.Where(x => x.Street == street && x.Number == number && x.Structure == structure &&x.Liter == liter);
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
              Liter = liter,
              District = district,
            };
            district.Buildings.Add(building);
          }
          Monitor.Exit(lockerDistrict);
          flat.Building = building;

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
                mt = "Площадь Ленина";
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

          var pr = collection[i].GetElementsByClassName("price");
          if (pr.Length > 0)
          {
            string priceStr = pr[0].TextContent.Replace("a", "").Replace(" ", "").Replace("\n","").Replace("\t", "").Replace("/мес", "");
            int price;
            if (int.TryParse(priceStr, out price))
            {
              flat.Price = price;
            }
          }
          

          if (!string.IsNullOrWhiteSpace(flat.Building.Street))
          {
            if (!string.IsNullOrWhiteSpace(flat.Building.Number))
            {
              if (!string.IsNullOrWhiteSpace(flat.Square))
              {
                Monitor.Enter(locker);
                if (flat.Building.Guid == Guid.Empty)
                {
                  unionInfo.UnionInfoProdam(flat);
                }
                OnAppend(this, new AppendFlatEventArgs { Flat = flat });
                progress.UpdateProgress(count);
                count++;
                Monitor.Exit(locker);
              }
            }
          }
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
      var parseStreet = new ParseStreet();
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
                flat.Square = square[0].TextContent.Replace(".",",").Trim();
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
                flat.Square = sq[0].TextContent.Replace(".",",").Trim();
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

        number = number.Trim();
        street = street.Trim();
        structure = structure.Trim();
        street = parseStreet.Execute(street, district);
        //street = street.Replace("ул.", "").Replace("ал.", "").Replace("бул.", "").Replace("ш.", "").Replace("пр.", "").Replace("пер.", "").Replace("пр-д", "").Replace(" б", "").Trim();

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


        if (!string.IsNullOrWhiteSpace(flat.Building.Street))
        {
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
              progress.UpdateProgress(count);
              count++;
              Monitor.Exit(locker);
            }
          }
        }
      }
    }

    private void ChangeDistrictAndPageForRent(object typeRoom)
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
                  $@"https://www.emls.ru/arenda/page{i}.html?query=s/1/r0/1/type/2/rtype/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/4/dir1/1/dir2/2/interval/3";
                break;
              case "1 км. кв.":
                url = $@"https://www.emls.ru/arenda/page{i}.html?query=s/1/r1/1/type/2/rtype/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/4/dir1/1/dir2/2/interval/3";
                break;
              case "2 км. кв.":
                url = $@"https://www.emls.ru/arenda/page{i}.html?query=s/1/r2/1/type/2/rtype/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/4/dir1/1/dir2/2/interval/3";
                break;
              case "3 км. кв.":
                url = $@"https://www.emls.ru/arenda/page{i}.html?query=s/1/r3/1/type/2/rtype/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/4/dir1/1/dir2/2/interval/3";
                break;
              case "4 км. кв.":
                url = $@"https://www.emls.ru/arenda/page{i}.html?query=s/1/r4/1/type/2/rtype/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/4/dir1/1/dir2/2/interval/3";
                break;
              case "5 км. кв.":
                url = $@"https://www.emls.ru/arenda/page{i}.html?query=s/1/r5/1/type/2/rtype/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/4/dir1/1/dir2/2/interval/3";
                break;
              case "6 км. кв.":
                url = $@"https://www.emls.ru/arenda/page{i}.html?query=s/1/r6/1/type/2/rtype/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/4/dir1/1/dir2/2/interval/3";
                break;
            }
            if (!ExecuteParseRent(url, webClient, parser, (string)typeRoom,
              ListDistricts.Where(x => x.Name.ToLower() == distr.Value.ToLower()).First()))
              break;
          }
        }
      }

      MessageBox.Show($"Закончили - {typeRoom}");
    }

    private bool ExecuteParseRent(string url, WebClient webClient, HtmlParser parser, string typeRoom, District district)
    {
      var random = new Random();
      Thread.Sleep(random.Next(2000, 4000));
      //try
      //{
      var responce = webClient.DownloadString(url);
      var document = parser.Parse(responce);
      var tableElements = document.GetElementsByClassName("row1");
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

    private void ParseSheetRent(IHtmlCollection<IElement> collection, string typeRoom, District district)
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
            if (typeRoom == "6 км. кв.")
            {
              var reg = new Regex(@"(\d+-комн\. квартира)");
              var room = reg.Match(divImage.TextContent).Value;
              reg = new Regex(@"(\d+)");
              room = reg.Match(room).Value;
              flat.CountRoom = $@"{room}  км. кв.";
            }

            var square = collection[i].GetElementsByClassName("space-all");
            if (square.Length > 0)
              flat.Square = square[0].TextContent.Replace(".",",").Trim();

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
