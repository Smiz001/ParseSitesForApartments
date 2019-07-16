﻿using AngleSharp.Dom;
using Core.Export;
using Core.Export.Creators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using AngleSharp.Html.Parser;
using Core.UnionWithBase;
using Core.Enum;
using Core.MainClasses;
using Core.ParsClasses;
using Core.Proxy;

namespace Core.Sites
{
  public class Avito : BaseParse
  {
    #region Fields

    private int minPage = 1;
    private int maxPage = 100;
    private List<string> stantions = new List<string>() { "Автово", "Адмиралтейская", "Академическая", "Балтийская", "Беговая", "Бухарестская", "Василеостровская", "Владимирская", "Волковская", "Выборгская", "Горьковская", "Гостиный двор", "Гражданский проспект", "Девяткино", "Достоевская", "Елизаровская", "Звёздная", "Звенигородская", "Кировский завод", "Комендантский проспект", "Крестовский остров", "Купчино", "Ладожская", "Ленинский проспект", "Лесная", "Лиговский проспект", "Ломоносовская", "Маяковская", "Международная", "Московская", "Московские ворота", "Нарвская", "Невский проспект", "Новокрестовская", "Новочеркасская", "Обводный канал", "Обухово", "Озерки", "Парк Победы", "Парнас", "Петроградская", "Пионерская", "Площадь Александра Невского", "Площадь Восстания", "Площадь Ленина", "Площадь Мужества", "Политехническая", "Приморская", "Пролетарская", "Проспект Большевиков", "Проспект Ветеранов", "Проспект Просвещения", "Пушкинская", "Рыбацкое", "Садовая", "Сенная площадь", "Спасская", "Спортивная", "Старая Деревня", "Технологический институт", "Удельная", "Улица Дыбенко", "Фрунзенская", "Чёрная речка", "Чернышевская", "Чкаловская", "Электросила" };
    private static object locker = new object();
    private static object lockerDistrict = new object();
    private static object lockerUnion = new object();
    private string filename = @"d:\ParserInfo\Appartament\AvitoProdam.csv";
    private CoreExport export;
    public delegate void Append(object sender, AppendFlatEventArgs e);
    public event Append OnAppend;
    private readonly UnionParseInfoWithDataBase unionInfo = new UnionParseInfoWithDataBase();
    private List<Building> buildings = new List<Building>();

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
    private ProgressForm progress;
    private int count = 1;
    private int allcount = 1;

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

    public Avito(List<District> listDistricts, List<Metro> listMetros, List<ProxyInfo> listProxy) : base(listDistricts, listMetros, listProxy)
    {
    }

    private void CreateExport()
    {
      CoreCreator creator = new CsvExportCreator();
      export = creator.FactoryCreate(Filename);
      OnAppend += export.AddFlatInListWithBaseInfo;

      if (export is CsvExport)
      {
        if (!File.Exists(Filename))
        {
          using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Create), Encoding.UTF8))
          {
            sw.WriteLine(@"Район;Улица;Номер;Корпус;Литер;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Дата постройки;Дата реконструкции;Даты кап. ремонты;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов;Расстояние пешком;Время пешком;Расстояние на машине;Время на машине;Откуда взято;Тип дома;Проводился кап.ремонт");
          }
        }
      }
    }

    public override void ParsingAll()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("Авито Все квартиры");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                studiiThread = new Thread(ChangeDistrictAndPage);
                studiiThread.Start("Студия");
                while (studiiThread.IsAlive) { }
                oneThread = new Thread(ChangeDistrictAndPage);
                oneThread.Start("1 км. кв.");
                while (oneThread.IsAlive) { }
                twoThread = new Thread(ChangeDistrictAndPage);
                twoThread.Start("2 км. кв.");
                while (twoThread.IsAlive) { }
                threeThread = new Thread(ChangeDistrictAndPage);
                threeThread.Start("3 км. кв.");
                while (threeThread.IsAlive) { }
                fourThread = new Thread(ChangeDistrictAndPage);
                fourThread.Start("4 км. кв.");
                while (fourThread.IsAlive) { }
                fiveThread = new Thread(ChangeDistrictAndPage);
                fiveThread.Start("5 км. кв.");
                while (fiveThread.IsAlive) { }
                sixThread = new Thread(ChangeDistrictAndPage);
                sixThread.Start("6 км. кв.");
                while (sixThread.IsAlive) { }
                sevenThread = new Thread(ChangeDistrictAndPage);
                sevenThread.Start("7 км. кв.");
                while (sevenThread.IsAlive) { }
                eightThread = new Thread(ChangeDistrictAndPage);
                eightThread.Start("8 км. кв.");
                while (eightThread.IsAlive) { }
                nineThread = new Thread(ChangeDistrictAndPage);
                nineThread.Start("9 км. кв.");
                while (nineThread.IsAlive) { }
                moreNineThread = new Thread(ChangeDistrictAndPage);
                moreNineThread.Start("9 км. кв. +");
                while (moreNineThread.IsAlive) { }

                studiiNewThread = new Thread(ChangeDistrictAndPage);
                studiiNewThread.Start("Студия Н");
                while (studiiNewThread.IsAlive) { }
                oneNewThread = new Thread(ChangeDistrictAndPage);
                oneNewThread.Start("1 км. кв. Н");
                while (oneNewThread.IsAlive) { }
                twoNewThread = new Thread(ChangeDistrictAndPage);
                twoNewThread.Start("2 км. кв. Н");
                while (twoNewThread.IsAlive) { }
                threeNewThread = new Thread(ChangeDistrictAndPage);
                threeNewThread.Start("3 км. кв. Н");
                while (threeNewThread.IsAlive) { }
                fourNewThread = new Thread(ChangeDistrictAndPage);
                fourNewThread.Start("4 км. кв. Н");
                while (fourNewThread.IsAlive) { }
                fiveNewThread = new Thread(ChangeDistrictAndPage);
                fiveNewThread.Start("5 км. кв. Н");
                while (fiveNewThread.IsAlive) { }
                sixNewThread = new Thread(ChangeDistrictAndPage);
                sixNewThread.Start("6 км. кв. Н");
                while (sixNewThread.IsAlive) { }
                sevenNewThread = new Thread(ChangeDistrictAndPage);
                sevenNewThread.Start("7 км. кв. Н");
                while (sevenNewThread.IsAlive) { }
                eightNewThread = new Thread(ChangeDistrictAndPage);
                eightNewThread.Start("8 км. кв. Н");
                while (eightNewThread.IsAlive) { }
                nineNewThread = new Thread(ChangeDistrictAndPage);
                nineNewThread.Start("9 км. кв. Н");
                while (nineNewThread.IsAlive) { }

                //Union
                studiiThread = new Thread(UnionFlats);
                studiiThread.Start("Студия");
                oneThread = new Thread(UnionFlats);
                oneThread.Start("1 км. кв.");
                twoThread = new Thread(UnionFlats);
                twoThread.Start("2 км. кв.");
                threeThread = new Thread(UnionFlats);
                threeThread.Start("3 км. кв.");
                fourThread = new Thread(UnionFlats);
                fourThread.Start("4 км. кв.");
                fiveThread = new Thread(UnionFlats);
                fiveThread.Start("5 км. кв.");
                sixThread = new Thread(UnionFlats);
                sixThread.Start("6 км. кв.");
                sevenThread = new Thread(UnionFlats);
                sevenThread.Start("7 км. кв.");
                eightThread = new Thread(UnionFlats);
                eightThread.Start("8 км. кв.");
                nineThread = new Thread(UnionFlats);
                nineThread.Start("9 км. кв.");
                moreNineThread = new Thread(UnionFlats);
                moreNineThread.Start("9 км. кв. +");

                studiiNewThread = new Thread(UnionFlats);
                studiiNewThread.Start("Студия Н");
                oneNewThread = new Thread(UnionFlats);
                oneNewThread.Start("1 км. кв. Н");
                twoNewThread = new Thread(UnionFlats);
                twoNewThread.Start("2 км. кв. Н");
                threeNewThread = new Thread(UnionFlats);
                threeNewThread.Start("3 км. кв. Н");
                fourNewThread = new Thread(UnionFlats);
                fourNewThread.Start("4 км. кв. Н");
                fiveNewThread = new Thread(UnionFlats);
                fiveNewThread.Start("5 км. кв. Н");
                sixNewThread = new Thread(UnionFlats);
                sixNewThread.Start("6 км. кв. Н");
                sevenNewThread = new Thread(UnionFlats);
                sevenNewThread.Start("7 км. кв. Н");
                eightNewThread = new Thread(UnionFlats);
                eightNewThread.Start("8 км. кв. Н");
                nineNewThread = new Thread(UnionFlats);
                nineNewThread.Start("9 км. кв. Н");

                while (true)
                {
                  if (!studiiThread.IsAlive)
                    if (!oneThread.IsAlive)
                      if (!twoThread.IsAlive)
                        if (!threeThread.IsAlive)
                          if (!fourThread.IsAlive)
                            if (!fiveThread.IsAlive)
                              if (!sixThread.IsAlive)
                                if (!sevenThread.IsAlive)
                                  if (!eightThread.IsAlive)
                                    if (!nineThread.IsAlive)
                                      if (!moreNineThread.IsAlive)
                                        if (!studiiNewThread.IsAlive)
                                          if (!oneNewThread.IsAlive)
                                            if (!twoNewThread.IsAlive)
                                              if (!threeNewThread.IsAlive)
                                                if (!fourNewThread.IsAlive)
                                                  if (!fiveNewThread.IsAlive)
                                                    if (!sixNewThread.IsAlive)
                                                      if (!sevenNewThread.IsAlive)
                                                        if (!eightNewThread.IsAlive)
                                                          if (!nineNewThread.IsAlive)
                                                            break;
                }
                IsFinished = true;
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
                      if (!sixThread.IsAlive)
                      {
                        if (!sevenThread.IsAlive)
                        {
                          if (!eightThread.IsAlive)
                          {
                            if (!nineThread.IsAlive)
                            {
                              if (!moreNineThread.IsAlive)
                              {
                                if (!studiiThread.IsAlive)
                                {
                                  if (!oneNewThread.IsAlive)
                                  {
                                    if (!twoNewThread.IsAlive)
                                    {
                                      if (!threeNewThread.IsAlive)
                                      {
                                        if (!fourNewThread.IsAlive)
                                        {
                                          if (!fiveNewThread.IsAlive)
                                          {
                                            if (!sixNewThread.IsAlive)
                                            {
                                              if (!sevenNewThread.IsAlive)
                                              {
                                                if (!eightNewThread.IsAlive)
                                                {
                                                  if (!nineNewThread.IsAlive)
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
            if (!studiiNewThread.IsAlive)
            {
              break;
            }
          }
        }
      }

      if (countRoom == 1)
      {
        while (true)
        {
          if (!oneThread.IsAlive)
          {
            if (!oneNewThread.IsAlive)
            {
              break;
            }
          }
        }
      }

      if (countRoom == 2)
      {
        while (true)
        {
          if (!twoThread.IsAlive)
          {
            if (!twoNewThread.IsAlive)
            {
              break;
            }
          }
        }
      }

      if (countRoom == 3)
      {
        while (true)
        {
          if (!threeThread.IsAlive)
          {
            if (!threeNewThread.IsAlive)
            {
              break;
            }
          }
        }
      }

      if (countRoom == 4)
      {
        while (true)
        {
          if (!fourThread.IsAlive)
          {
            if (!fourNewThread.IsAlive)
            {
              break;
            }
          }
        }
      }

      if (countRoom == 5)
      {
        while (true)
        {
          if (!fiveThread.IsAlive)
          {
            if (!sixThread.IsAlive)
            {
              if (!sevenThread.IsAlive)
              {
                if (!eightThread.IsAlive)
                {
                  if (!nineThread.IsAlive)
                  {
                    if (!moreNineThread.IsAlive)
                    {
                      if (!fiveNewThread.IsAlive)
                      {
                        if (!sixNewThread.IsAlive)
                        {
                          if (!sevenNewThread.IsAlive)
                          {
                            if (!eightNewThread.IsAlive)
                            {
                              if (!nineNewThread.IsAlive)
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

    public override void ParsingStudii()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("Авито Студии");
        var threadbackground = new Thread(
          new ThreadStart(() =>
          {
            try
            {
              studiiThread = new Thread(ChangeDistrictAndPage);
              studiiThread.Start("Студия");
              while (studiiThread.IsAlive) { }
              studiiNewThread = new Thread(ChangeDistrictAndPage);
              studiiNewThread.Start("Студия Н");
              while (studiiNewThread.IsAlive) { }

              studiiThread = new Thread(UnionFlats);
              studiiThread.Start("Студия");
              studiiNewThread = new Thread(UnionFlats);
              studiiNewThread.Start("Студия Н");
              while (true)
              {
                if (!studiiThread.IsAlive)
                  if (!studiiNewThread.IsAlive)
                    break;
              }
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
        //studiiRentThread = new Thread(ChangeDistrictAndPage);
        //studiiRentThread.Start("Студия");
      }
    }

    public override void ParsingOne()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("Авито 1 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                oneThread = new Thread(ChangeDistrictAndPage);
                oneThread.Start("1 км. кв.");
                oneNewThread = new Thread(ChangeDistrictAndPage);
                oneNewThread.Start("1 км. кв. Н");

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
        //oneRentThread = new Thread(ChangeDistrictAndPageForRent);
        //oneRentThread.Start("1 км. кв.");
      }
    }

    public override void ParsingTwo()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("Авито 2 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                twoThread = new Thread(ChangeDistrictAndPage);
                twoThread.Start("2 км. кв.");
                twoNewThread = new Thread(ChangeDistrictAndPage);
                twoNewThread.Start("2 км. кв. Н");

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
        //twoRentThread = new Thread(ChangeDistrictAndPageForRent);
        //twoRentThread.Start("2 км. кв.");
      }
    }

    public override void ParsingThree()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("Авито 3 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                threeThread = new Thread(ChangeDistrictAndPage);
                threeThread.Start("3 км. кв.");
                threeNewThread = new Thread(ChangeDistrictAndPage);
                threeNewThread.Start("3 км. кв. Н");

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
        //threeRentThread = new Thread(ChangeDistrictAndPageForRent);
        //threeRentThread.Start("3 км. кв.");
      }
    }

    public override void ParsingFour()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("Авито 4 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                fourThread = new Thread(ChangeDistrictAndPage);
                fourThread.Start("4 км. кв.");
                fourNewThread = new Thread(ChangeDistrictAndPage);
                fourNewThread.Start("4 км. кв. Н");

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
        //fourRentThread = new Thread(ChangeDistrictAndPageForRent);
        //fourRentThread.Start("4 км. кв.");
      }
    }

    public override void ParsingMoreFour()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("Авито 5+ км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                fiveThread = new Thread(ChangeDistrictAndPage);
                fiveThread.Start("5 км. кв.");
                sixThread = new Thread(ChangeDistrictAndPage);
                sixThread.Start("6 км. кв.");
                sevenThread = new Thread(ChangeDistrictAndPage);
                sevenThread.Start("7 км. кв.");
                eightThread = new Thread(ChangeDistrictAndPage);
                eightThread.Start("8 км. кв.");
                nineThread = new Thread(ChangeDistrictAndPage);
                nineThread.Start("9 км. кв.");
                moreNineThread = new Thread(ChangeDistrictAndPage);
                moreNineThread.Start("9 км. кв. +");

                fiveNewThread = new Thread(ChangeDistrictAndPage);
                fiveNewThread.Start("5 км. кв. Н");
                sixNewThread = new Thread(ChangeDistrictAndPage);
                sixNewThread.Start("6 км. кв. Н");
                sevenNewThread = new Thread(ChangeDistrictAndPage);
                sevenNewThread.Start("7 км. кв. Н");
                eightNewThread = new Thread(ChangeDistrictAndPage);
                eightNewThread.Start("8 км. кв. Н");
                nineNewThread = new Thread(ChangeDistrictAndPage);
                nineNewThread.Start("9 км. кв. Н");

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
        //fiveRentThread = new Thread(ChangeDistrictAndPageForRent);
        //fiveRentThread.Start("5 км. кв.");
      }
    }

    private void ChangeDistrictAndPage(object typeRoom)
    {
      var path = CreateExportForRoom(typeRoom.ToString());
      CoreCreator creator = new CsvExportCreator();
      var exportPart = creator.FactoryCreate(path);

      HtmlParser parser = new HtmlParser();
      using (var webClient = new WebClient())
      {
        //webClient.Proxy = myProxy;
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
          if (!ExecuteParse(url, webClient, parser, (string)typeRoom, exportPart))
            break;
        }
      }
      //MessageBox.Show($"Закончили - {typeRoom}");
    }

    private bool ExecuteParse(string url, WebClient webClient, HtmlParser parser, string typeRoom, CoreExport export)
    {
      bool result;
      var random = new Random();
      Thread.Sleep(random.Next(8000, 9000));
      try
      {
        Log.Debug("-----------URL-----------");
        Log.Debug(url);
        var responce = webClient.DownloadString(url);
        var document = parser.ParseDocument(responce);

        var collections = document.GetElementsByClassName("description item_table-description");
        if (collections.Length > 0)
        {
          ParsingSheet(typeRoom, collections, export);
          result = true;
        }
        else
          result = false;
      }
      catch (Exception e)
      {
        Log.Error(e.Message);
        //TODO Если страница долго не отвечает то пропускаем ее
        Thread.Sleep(1000);
        if (e.Message == "Запрос был прерван: Соединение было неожиданно закрыто.")
          result = false;
        else
          result = true;
      }
      return result;
    }

  
    private void ParsingSheet(string typeRoom, IHtmlCollection<IElement> collection, CoreExport export)
    {
      var parseStreet = new ParseStreet();
      for (int k = 0; k < collection.Length; k++)
      {
        progress.UpdateAllProgress(allcount);
        allcount++;
        var flat = new Flat();
        string street = string.Empty;
        string number = string.Empty;
        string structure = string.Empty;
        string liter = string.Empty;
        string metro = string.Empty;
        string distanceStr = string.Empty;

        flat.CountRoom = typeRoom;

        flat.Price = int.Parse(collection[k].GetElementsByClassName("price")[0].TextContent.Trim('\n').Trim('₽').Trim().Replace(" ", ""));

        flat.Url = $"https://www.avito.ru{collection[k].GetElementsByClassName("item-description-title-link")[0].GetAttribute("href")}";

        var aboutBuild = collection[k].GetElementsByClassName("item-description-title-link")[0].TextContent;
        var regex = new Regex(@"(\d+\.\d+\s+м²)|(\d+\s+м²)");
        flat.Square = regex.Match(aboutBuild).Value.Replace(".", ",").Replace("м²", "").Trim();
        regex = new Regex(@"(\d+\/\d+)");
        var floor = regex.Match(aboutBuild).Value;
        regex = new Regex(@"(\/\d+)");
        flat.Floor = floor.Replace(regex.Match(floor).Value, "");

        var adress = collection[k].GetElementsByClassName("address");
        if (adress.Length > 0)
        {
          var adr = adress[0];
          street = adr.TextContent.Trim();
          var isHaveMetro = adr.GetElementsByClassName("i-metro");
          if (isHaveMetro.Length > 0)
          {
            var split = street.Split(',');
            if (split.Length > 0)
            {
              metro = street.Split(',')[0];
              regex = new Regex(@"(\d+\.\d+\s+км)|(\d+\s+км)|(\d+\s+м)");
              metro = metro.Replace(regex.Match(metro).Value, "").Trim();
            }
          }

          var distance = collection[k].GetElementsByClassName("c-2");
          if (distance.Length > 0)
            distanceStr = distance[0].TextContent.Trim();
          street = street.Replace(distanceStr, "").Replace("Санкт-Петербург,", "").Replace("посёлок Парголово,", "").Replace("СПб Красное село", "").Replace("г. Ломоносов,", "").Replace("Россия,", "").Replace("Сестрорецк г,", "").Replace("Сестрорецк", "").Replace("Парголово п,", "").Replace("Колпино,", "").Replace("Мурино,", "").Replace("посёлок Шушары,", "").Replace("г. Петергоф,", "");

          #region Удаление лишнего
          regex = new Regex(@"(\,\s+подъезд\s+\d+)|(\,\s+подъезд\d+)");
          var gov = regex.Match(street).Value;
          if (!string.IsNullOrEmpty(gov))
            street = street.Replace(gov, "");

          regex = new Regex(@"(\,\s+стр\. \d+)|(\,\s+стр\.\s+\d+)|(\,\s+стр\.\d+)");
          gov = regex.Match(street).Value;
          if (!string.IsNullOrEmpty(gov))
            street = street.Replace(gov, "");

          #endregion
          var ar = street.Split(',');

          ProcessingBuilding(ref street, ref number, ref structure, ar);
        }


        regex = new Regex(@"(к\d+)");
        structure = regex.Match(number).Value;
        if (string.IsNullOrWhiteSpace(structure))
        {
          regex = new Regex(@"(к \d+)");
          structure = regex.Match(number).Value;
          if (string.IsNullOrWhiteSpace(structure))
          {
            regex = new Regex(@"(кор\.\d+)");
            structure = regex.Match(number).Value;
            if (string.IsNullOrWhiteSpace(structure))
            {

            }
            else
            {
              number = number.Replace(structure, "").Trim();
              structure = structure.Replace("кор.", "");
            }
          }
          else
          {
            number = number.Replace(structure, "");
            structure = structure.Replace("к", "");
          }
        }
        else
        {
          number = number.Replace(structure, "");
          structure = structure.Replace("к", "");
        }

        regex = new Regex(@"(\D$)");
        liter = regex.Match(number).Value;
        if (!string.IsNullOrEmpty(liter))
        {
          number = number.Replace(liter, "");
        }


        street = street.Replace("улица", "").Replace("ул. ", "").Replace("проезд", "").Replace("переулок", "").Replace("переулок", "").Replace("бульвар", "").Replace("б-р", "").Replace("проспект", "пр.").Replace("пр-кт", "пр.").Replace("Васильевского острова", "В.О.").Replace("Васильевского острова", "В.О.").Replace("Петроградской стороны", "П.С.").Replace("ш.", "").Replace("пер", "").Replace(" ул", "").Replace("аллея", "").Replace("дорога ", "").Replace("набережная ", "").Replace("пр-т", "пр.").Trim();

        street = street.Replace("Девяткино , Ленинградская область, Всеволожский район, посёлок  ", "");

        regex = new Regex(@"(^пр\.)");
        var pr = regex.Match(street).Value;
        if (!string.IsNullOrWhiteSpace(pr))
        {
          street = street.Replace(pr, "").Trim() + $" {pr}";
        }

        street = parseStreet.ExecuteWithoutDistrict(street);

        number = number.Trim();
        street = street.Trim();
        structure = structure.Trim();
        Building building = null;
        Monitor.Enter(lockerDistrict);
        if (buildings.Count != 0)
        {
          IEnumerable<Building> bldsEnum;
          if (string.IsNullOrEmpty(liter))
            bldsEnum = buildings.Where(x => x.Street == street && x.Number == number && x.Structure == structure);
          else
            bldsEnum = buildings.Where(x => x.Street == street && x.Number == number && x.Structure == structure && x.Liter == liter);

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
            Liter = liter
          };
          buildings.Add(building);
        }
        Monitor.Exit(lockerDistrict);

        if (building.MetroObj == null)
        {
          var metroObjEnum = ListMetros.Where(x => string.Compare(x.Name, metro, StringComparison.OrdinalIgnoreCase) == 0);
          if (metroObjEnum.Count() > 0)
          {
            building.MetroObj = metroObjEnum.First();
          }
        }
        flat.Building = building;

        if (!string.IsNullOrWhiteSpace(flat.Building.Number))
        {
          int val;
          if (int.TryParse(flat.Building.Number,out val))
          {
            if (!string.IsNullOrWhiteSpace(flat.Square))
            {
              if (!string.IsNullOrWhiteSpace(flat.Building.Street))
              {
                Monitor.Enter(locker);
                export.AddFlatInList(this, new AppendFlatEventArgs { Flat = flat });
                progress.UpdateProgress(count);
                count++;
                Monitor.Exit(locker);
              }
            }
          }
        }
      }
    }
    
    private void ProcessingBuilding(ref string street, ref string number, ref string structure, string[] ar)
    {
      if (ar.Length == 3)
      {
        ar[0] = ar[0].Replace("ин-т", "").Replace("Площадь А.", "").Replace("I", "").Replace("II", "").Trim();
        //ParseMetro(ar[0], flat.Building, connection);

        street = ar[1];
        number = ar[2];
        number = number.Replace("А", "").Replace("А", "").Replace("дом ", "").Replace("д.", "").Trim();
      }
      else if (ar.Length == 1)
      {
        var regex = new Regex(@"(д\. \d+\s+к\.\d+)|(д\.\s+\d+\s+к\.\d+)");
        number = regex.Match(ar[0]).Value;
        if (!string.IsNullOrEmpty(number))
        {
          street = ar[0].Replace(number, "").Trim();
          regex = new Regex(@"(к\.\d+)");
          structure = regex.Match(number).Value;
          number = number.Replace(structure, "").Replace("д. ", "").Replace("д. ", "");
          structure = structure.Replace("к.", "");
        }
        else
        {
          regex = new Regex(@"(д\.\d+\s+к\.\d+)");
          number = regex.Match(ar[0]).Value;
          if (!string.IsNullOrEmpty(number))
          {
            street = ar[0].Replace(number, "").Trim();
            regex = new Regex(@"(к\.\d+)");
            structure = regex.Match(number).Value;
            number = number.Replace(structure, "").Replace("д.", "").Trim();
            structure = structure.Replace("к.", "");
          }
          else
          {
            regex = new Regex(@"(д\. \d+\s+корп\.\s+\d+)|(д\.\s+\d+\s+корп\.\s+\d+)");
            number = regex.Match(ar[0]).Value;
            if (!string.IsNullOrEmpty(number))
            {
              street = ar[0].Replace(number, "").Trim();
              regex = new Regex(@"(корп\.\s+\d+)");
              structure = regex.Match(number).Value;
              number = number.Replace(structure, "").Replace("д. ", "").Replace("д. ", "");
              structure = structure.Replace("корп.", "");
            }
            else
            {
              regex = new Regex(@"(дом\s+\d+\s+корпус\s+\d+)");
              number = regex.Match(ar[0]).Value;
              if (!string.IsNullOrEmpty(number))
              {
                street = ar[0].Replace(number, "").Trim();
                regex = new Regex(@"(корпус\s+\d+)");
                structure = regex.Match(number).Value;
                number = number.Replace(structure, "").Replace("дом ", "").Replace("дом ", "");
                structure = structure.Replace("корпус", "");
              }
              else
              {
                regex = new Regex(@"(дом\s+\d+\s+корп\.\s+\d+)");
                number = regex.Match(ar[0]).Value;
                if (!string.IsNullOrEmpty(number))
                {
                  street = ar[0].Replace(number, "").Trim();
                  regex = new Regex(@"(корп\.\s+\d+)");
                  structure = regex.Match(number).Value;
                  number = number.Replace(structure, "").Replace("дом ", "").Replace("дом ", "");
                  structure = structure.Replace("корп.", "");
                }
                else
                {
                  regex = new Regex(@"(д\.\d+$)");
                  number = regex.Match(ar[0]).Value;
                  if (!string.IsNullOrEmpty(number))
                  {
                    street = ar[0].Replace(number, "").Trim();
                    number = number.Replace("д.", "");
                  }
                  else
                  {
                    regex = new Regex(@"(\d+с\d+$)");
                    number = regex.Match(ar[0]).Value;
                    if (!string.IsNullOrEmpty(number))
                    {
                      street = ar[0].Replace(number, "").Trim();
                    }
                    else
                    {
                      regex = new Regex(@"(д\d+$)");
                      number = regex.Match(ar[0]).Value;
                      if (!string.IsNullOrEmpty(number))
                      {
                        street = ar[0].Replace(number, "").Trim();
                      }
                      else
                      {
                        regex = new Regex(@"(\d+\/\d+)");
                        number = regex.Match(ar[0]).Value;
                        if (!string.IsNullOrEmpty(number))
                        {
                          street = ar[0].Replace(number, "").Trim();
                          structure = number.Split('/')[1];
                          number = number.Split('/')[1];
                        }
                        else
                        {
                          regex = new Regex(@"(\d+$)");
                          number = regex.Match(ar[0]).Value;
                          if (!string.IsNullOrEmpty(number))
                          {
                            street = ar[0].Replace(number, "").Trim();
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
        street = street.Replace("Ул.", "").Replace("г.", "").Replace(".", "").Trim();
      }
      else if (ar.Length == 2)
      {
        if (stantions.Contains(ar[0].Trim()))
        {
          //ParseMetro(ar[0].Trim(), flat.Building, connection);
          street = ar[1];
        }
      }
      else if (ar.Length == 4)
      {
        if (stantions.Contains(ar[0].Trim()))
        {
         // ParseMetro(ar[0].Trim(), flat.Building, connection);
          street = ar[1];
        }
      }
      //else
      //  flat.Building.Street = adres;
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

    //TODO добавить список домов
    private List<Building> listBuildings = new List<Building>();
    private void UnionFlats(object type)
    {
      Log.Debug($"Start Union {type}");
      var union = new UnionParseInfoWithDataBase();
      var path = ExctractPath();
      path = $@"{path}{type}-{DateTime.Now.ToShortDateString()}-{NameSite}.csv";
      if(File.Exists(path))
      {
        using (var sr = new StreamReader(path))
        {
          sr.ReadLine();
          string line;
          while ((line = sr.ReadLine()) != null)
          {
            string street = string.Empty;
            string number = string.Empty;
            string struc = string.Empty;
            string liter = string.Empty;
            string metro = string.Empty;

            var ar = line.Split(';');
            Flat flat = new Flat();
            if (ar.Length == 11)
            {
              flat.Url = ar[10];
            }
            flat.CountRoom = type.ToString();
            flat.Price = int.Parse(ar[8]);
            flat.Floor = ar[7];
            flat.Square = ar[6];

            street = ar[1];
            number = ar[2];
            struc = ar[3];
            liter = ar[4];
            metro = ar[9];

            District dis = null;
            try
            {
              dis = ListDistricts.Where(x => string.Equals(x.Name, ar[0], StringComparison.CurrentCultureIgnoreCase)).First();
            }
            catch (Exception e)
            {
              Log.Error($@"{e.Message}; ar[0] - {ar[0]}");
            }

            //if (dis == null)
            //  continue;

            Building building = null;
            Monitor.Enter(lockerDistrict);
            try
            {
              if (listBuildings.Count != 0)
              {
                var bldsEnum =
                  dis.Buildings.Where(x =>
                    x.Street == street && x.Number == number && x.Structure == struc && x.Liter == liter);
                if (bldsEnum.Count() > 0)
                  building = bldsEnum.First();
              }
              if (building == null)
              {
                Metro metroObj = null;
                var metroObjs = ListMetros.Where(x => string.Compare(x.Name, metro, StringComparison.OrdinalIgnoreCase) == 0 );
                if (metroObjs.Count() > 0)
                  metroObj = metroObjs.First();

                building = new Building
                {
                  Street = street,
                  Number = number,
                  Structure = struc,
                  Liter = liter,
                  District = dis,
                  MetroObj = metroObj
                };
                listBuildings.Add(building);
              }
            }
            finally
            {
              Monitor.Exit(lockerDistrict);
            }

            if (building.Guid == Guid.Empty)
            {
              Monitor.Enter(lockerUnion);
              try
              {
                union.UnionInfo(building);
              }
              finally
              {
                Monitor.Exit(lockerUnion);
              }
            }
            flat.Building = building;
            Monitor.Enter(locker);
            try
            {
              OnAppend(this, new AppendFlatEventArgs { Flat = flat });
            }
            finally
            {
              Monitor.Exit(locker);
            }
          }
        }
        File.Delete(path);
      }
    }

    protected override void CalcAverPrice()
    {
      throw new NotImplementedException();
    }
  }
}
