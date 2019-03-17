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

    private Dictionary<int, string> district = new Dictionary<int, string>()
    {
      {38, "Адмиралтейский"}, {43, "Василеостровский"}, {4, "Выборгский"}, {6, "Калининский"}, {7, "Кировский"},
      {9, "Красногвардейский"}, {8, "Красносельский"}, {12, "Московский"}, {13, "Невский"}, {20, "Петроградский"},
      {14, "Приморский"}, {15, "Фрунзенский"}, {39, "Центральный"},
    };

    private List<Flat> listFlat = new List<Flat>();
    private static object locker = new object();
    private static object lockerUnion = new object();
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
    private int allcount = 1;

    public ELMS(List<District> listDistricts, List<Metro> listMetros, List<ProxyInfo> listProxy) : base(listDistricts,
      listMetros, listProxy)
    {
      //CoreCreator creator = new CsvExportCreator();
      //export = creator.FactoryCreate(Filename);
      //OnAppend += export.AddFlatInList;
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
            sw.WriteLine(
              @"Район;Улица;Номер;Корпус;Литер;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Дата постройки;Дата реконструкции;Даты кап. ремонты;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов;Расстояние пешком;Время пешком;Расстояние на машине;Время на машине;Откуда взято");
          }
        }
      }
    }

    private string ExctractPath()
    {
      string path = string.Empty;
      var arr = Filename.Split('\\');
      path = Filename.Replace(arr[arr.Length - 1], "");
      return path;
    }

    private string CreateExportForRoom(string typeRoom)
    {
      var path = ExctractPath();
      path = $@"{path}{typeRoom}-{DateTime.Now.ToShortDateString()}-{NameSite}.csv";
      if (!File.Exists(Filename))
      {
        File.Delete(path);
      }

      using (var sw = new StreamWriter(new FileStream(path, FileMode.Create), Encoding.UTF8))
      {
        sw.WriteLine(@"Район;Улица;Номер;Корпус;Литер;Кол-во комнат;Площадь;Этаж;Цена;Метро;Откуда взято");
      }

      return path;
    }

    public override void ParsingAll()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("ELMS Все квартиры Продажа");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                studiiThread = new Thread(ChangeDistrictAndPage);
                studiiThread.Start("Студия Н");
                studiiThreadOld = new Thread(ChangeDistrictAndPage);
                studiiThreadOld.Start("Студия");
                oneThreadOld = new Thread(ChangeDistrictAndPage);
                oneThreadOld.Start("1 км. кв.");
                oneThread = new Thread(ChangeDistrictAndPage);
                oneThread.Start("1 км. кв. Н");
                twoThreadOld = new Thread(ChangeDistrictAndPage);
                twoThreadOld.Start("2 км. кв.");
                twoThread = new Thread(ChangeDistrictAndPage);
                twoThread.Start("2 км. кв. Н");
                threeThreadOld = new Thread(ChangeDistrictAndPage);
                threeThreadOld.Start("3 км. кв.");
                threeThread = new Thread(ChangeDistrictAndPage);
                threeThread.Start("3 км. кв. Н");
                fourThreadOld = new Thread(ChangeDistrictAndPage);
                fourThreadOld.Start("4 км. кв.");
                fourThread = new Thread(ChangeDistrictAndPage);
                fourThread.Start("4 км. кв. Н");
                fiveThreadOld = new Thread(ChangeDistrictAndPage);
                fiveThreadOld.Start("5 км. кв.");
                fiveThread = new Thread(ChangeDistrictAndPage);
                fiveThread.Start("5 км. кв. Н");
                sixThreadOld = new Thread(ChangeDistrictAndPage);
                sixThreadOld.Start("6 км. кв.");

                while (true)
                {
                  if (!studiiThreadOld.IsAlive)
                    if (!studiiThread.IsAlive)
                      if (!oneThreadOld.IsAlive)
                        if (!twoThreadOld.IsAlive)
                          if (!threeThreadOld.IsAlive)
                            if (!fourThreadOld.IsAlive)
                              if (!fiveThreadOld.IsAlive)
                                if (!sixThreadOld.IsAlive)
                                  if (!oneThread.IsAlive)
                                    if (!twoThread.IsAlive)
                                      if (!threeThread.IsAlive)
                                        if (!fourThread.IsAlive)
                                          if (!fiveThread.IsAlive)
                                            break;
                }

                studiiThread = new Thread(UnionFlats);
                studiiThread.Start("Студия Н");
                studiiThreadOld = new Thread(UnionFlats);
                studiiThreadOld.Start("Студия");
                oneThreadOld = new Thread(UnionFlats);
                oneThreadOld.Start("1 км. кв.");
                oneThread = new Thread(UnionFlats);
                oneThread.Start("1 км. кв. Н");
                twoThreadOld = new Thread(UnionFlats);
                twoThreadOld.Start("2 км. кв.");
                twoThread = new Thread(UnionFlats);
                twoThread.Start("2 км. кв. Н");
                threeThreadOld = new Thread(UnionFlats);
                threeThreadOld.Start("3 км. кв.");
                threeThread = new Thread(UnionFlats);
                threeThread.Start("3 км. кв. Н");
                fourThreadOld = new Thread(UnionFlats);
                fourThreadOld.Start("4 км. кв.");
                fourThread = new Thread(UnionFlats);
                fourThread.Start("4 км. кв. Н");
                fiveThreadOld = new Thread(UnionFlats);
                fiveThreadOld.Start("5 км. кв.");
                fiveThread = new Thread(UnionFlats);
                fiveThread.Start("5 км. кв. Н");
                sixThreadOld = new Thread(UnionFlats);
                sixThreadOld.Start("6 км. кв.");
                while (true)
                {
                  if (!studiiThreadOld.IsAlive)
                    if (!studiiThread.IsAlive)
                      if (!oneThreadOld.IsAlive)
                        if (!twoThreadOld.IsAlive)
                          if (!threeThreadOld.IsAlive)
                            if (!fourThreadOld.IsAlive)
                              if (!fiveThreadOld.IsAlive)
                                if (!sixThreadOld.IsAlive)
                                  if (!oneThread.IsAlive)
                                    if (!twoThread.IsAlive)
                                      if (!threeThread.IsAlive)
                                        if (!fourThread.IsAlive)
                                          if (!fiveThread.IsAlive)
                                            break;
                }

                IsFinished = true;
                var threadMessage = new Thread(
                  new ThreadStart(() =>
                    {
                      MessageBox.Show("Загрузка завершена");
                    }
                  )
                );
                threadMessage.Start();
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

        //while (threadbackground.IsAlive)
        //  Thread.Sleep(10000);
      }
      else
      {
        progress = new ProgressForm("ELMS Все квартиры Сдача");
        var threadbackground = new Thread(
          new ThreadStart(() =>
          {
            try
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

              while (true)
              {
                if (!studiiRentThread.IsAlive)
                  if (!oneRentThread.IsAlive)
                    if (!twoRentThread.IsAlive)
                      if (!threeRentThread.IsAlive)
                        if (!fourRentThread.IsAlive)
                          if (!fiveRentThread.IsAlive)
                            if (!sixRentThreadOld.IsAlive)
                              break;
              }
              studiiRentThread = new Thread(UnionFlats);
              studiiRentThread.Start("Студия");
              oneRentThread = new Thread(UnionFlats);
              oneRentThread.Start("1 км. кв.");
              twoRentThread = new Thread(UnionFlats);
              twoRentThread.Start("2 км. кв.");
              threeRentThread = new Thread(UnionFlats);
              threeRentThread.Start("3 км. кв.");
              fourRentThread = new Thread(UnionFlats);
              fourRentThread.Start("4 км. кв.");
              fiveRentThread = new Thread(UnionFlats);
              fiveRentThread.Start("5 км. кв.");
              sixRentThreadOld = new Thread(UnionFlats);
              sixRentThreadOld.Start("6 км. кв.");
              while (true)
              {
                if (!studiiRentThread.IsAlive)
                  if (!oneRentThread.IsAlive)
                    if (!twoRentThread.IsAlive)
                      if (!threeRentThread.IsAlive)
                        if (!fourRentThread.IsAlive)
                          if (!fiveRentThread.IsAlive)
                            if (!sixRentThreadOld.IsAlive)
                              break;
              }

              IsFinished = true;
              var threadMessage = new Thread(
                new ThreadStart(() =>
                {
                  MessageBox.Show("Загрузка завершена");
                }
                )
              );
              threadMessage.Start();
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
    }

    public override void ParsingStudii()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("ELMS Студии Продажа");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                studiiThread = new Thread(ChangeDistrictAndPage);
                studiiThread.Start("Студия Н");
                studiiThreadOld = new Thread(ChangeDistrictAndPage);
                studiiThreadOld.Start("Студия");

                while (true)
                {
                  if (!studiiThreadOld.IsAlive)
                  {
                    if (!studiiThread.IsAlive)
                    {
                      break;
                    }
                  }
                }
                listFlat.Clear();
                foreach (var dis in ListDistricts)
                {
                  dis.Buildings.Clear();
                }
                Log.Debug("Start Union Studii");
                studiiThread = new Thread(UnionFlats);
                studiiThread.Start("Студия Н");
                studiiThreadOld = new Thread(UnionFlats);
                studiiThreadOld.Start("Студия");

                while (true)
                {
                  if (!studiiThreadOld.IsAlive)
                  {
                    if (!studiiThread.IsAlive)
                    {
                      break;
                    }
                  }
                }

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
        progress = new ProgressForm("ELMS Студия Сдача");
        var threadbackground = new Thread(
          new ThreadStart(() =>
          {
            try
            {
              studiiRentThread = new Thread(ChangeDistrictAndPageForRent);
              studiiRentThread.Start("Студия");

              while (true)
              {
                if (!studiiRentThread.IsAlive)
                  break;
              }
              studiiRentThread = new Thread(UnionFlats);
              studiiRentThread.Start("Студия");
              while (true)
              {
                if (!studiiRentThread.IsAlive)
                  break;
              }

              IsFinished = true;
              var threadMessage = new Thread(
                new ThreadStart(() =>
                {
                  MessageBox.Show("Загрузка завершена");
                }
                )
              );
              threadMessage.Start();
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
    }

    public override void ParsingOne()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("ELMS 1 км. кв. Продажа");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                oneThreadOld = new Thread(ChangeDistrictAndPage);
                oneThreadOld.Start("1 км. кв.");
                oneThread = new Thread(ChangeDistrictAndPage);
                oneThread.Start("1 км. кв. Н");

                while (true)
                {
                  if (!oneThreadOld.IsAlive)
                  {
                    if (!oneThread.IsAlive)
                      break;
                  }
                }
                listFlat.Clear();
                foreach (var dis in ListDistricts)
                {
                  dis.Buildings.Clear();
                }
                Log.Debug("Start Union 1");
                oneThreadOld = new Thread(UnionFlats);
                oneThreadOld.Start("1 км. кв.");
                oneThread = new Thread(UnionFlats);
                oneThread.Start("1 км. кв. Н");

                while (true)
                {
                  if (!oneThreadOld.IsAlive)
                  {
                    if (!oneThread.IsAlive)
                    {
                      break;
                    }
                  }
                }

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
        progress = new ProgressForm("ELMS 1 км. кв. Сдача");
        var threadbackground = new Thread(
          new ThreadStart(() =>
          {
            try
            {
              oneRentThread = new Thread(ChangeDistrictAndPageForRent);
              oneRentThread.Start("1 км. кв.");
              while (true)
              {
                if (!oneRentThread.IsAlive)
                  break;
              }
              oneRentThread = new Thread(UnionFlats);
              oneRentThread.Start("1 км. кв.");
              while (true)
              {
                if (!oneRentThread.IsAlive)
                  break;
              }

              IsFinished = true;
              var threadMessage = new Thread(
                new ThreadStart(() =>
                {
                  MessageBox.Show("Загрузка завершена");
                }
                )
              );
              threadMessage.Start();
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
    }

    public override void ParsingTwo()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("ELMS 2 км. кв. Продажа");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                twoThreadOld = new Thread(ChangeDistrictAndPage);
                twoThreadOld.Start("2 км. кв.");
                twoThread = new Thread(ChangeDistrictAndPage);
                twoThread.Start("2 км. кв. Н");

                while (true)
                {
                  if (!twoThreadOld.IsAlive)
                  {
                    if (!twoThread.IsAlive)
                      break;
                  }
                }
                listFlat.Clear();
                foreach (var dis in ListDistricts)
                {
                  dis.Buildings.Clear();
                }
                Log.Debug("Start Union 2");
                twoThreadOld = new Thread(UnionFlats);
                twoThreadOld.Start("2 км. кв.");
                twoThread = new Thread(UnionFlats);
                twoThread.Start("2 км. кв. Н");

                while (true)
                {
                  if (!twoThreadOld.IsAlive)
                  {
                    if (!twoThread.IsAlive)
                      break;
                  }
                }

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
        progress = new ProgressForm("ELMS 2 км. кв. Сдача");
        var threadbackground = new Thread(
          new ThreadStart(() =>
          {
            try
            {
              twoRentThread = new Thread(ChangeDistrictAndPageForRent);
              twoRentThread.Start("2 км. кв.");

              while (true)
              {
                if (!twoRentThread.IsAlive)
                  break;
              }
              twoRentThread = new Thread(UnionFlats);
              twoRentThread.Start("2 км. кв.");
              while (true)
              {
                if (!twoRentThread.IsAlive)
                  break;
              }

              IsFinished = true;
              var threadMessage = new Thread(
                new ThreadStart(() =>
                {
                  MessageBox.Show("Загрузка завершена");
                }
                )
              );
              threadMessage.Start();
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
    }

    public override void ParsingThree()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("ELMS 3 км. кв. Продажа");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                threeThreadOld = new Thread(ChangeDistrictAndPage);
                threeThreadOld.Start("3 км. кв.");
                threeThread = new Thread(ChangeDistrictAndPage);
                threeThread.Start("3 км. кв. Н");

                while (true)
                {
                  if (!threeThreadOld.IsAlive)
                  {
                    if (!threeThread.IsAlive)
                      break;
                  }
                }
                listFlat.Clear();
                foreach (var dis in ListDistricts)
                {
                  dis.Buildings.Clear();
                }
                Log.Debug("Start Union 3");
                threeThreadOld = new Thread(UnionFlats);
                threeThreadOld.Start("3 км. кв.");
                threeThread = new Thread(UnionFlats);
                threeThread.Start("3 км. кв. Н");

                while (true)
                {
                  if (!threeThreadOld.IsAlive)
                  {
                    if (!threeThread.IsAlive)
                      break;
                  }
                }

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
        progress = new ProgressForm("ELMS 3 км. кв. Сдача");
        var threadbackground = new Thread(
          new ThreadStart(() =>
          {
            try
            {
              threeRentThread = new Thread(ChangeDistrictAndPageForRent);
              threeRentThread.Start("3 км. кв.");

              while (true)
              {
                if (!threeRentThread.IsAlive)
                  break;
              }
              threeRentThread = new Thread(UnionFlats);
              threeRentThread.Start("3 км. кв.");
              while (true)
              {
                if (!threeRentThread.IsAlive)
                  break;
              }

              IsFinished = true;
              var threadMessage = new Thread(
                new ThreadStart(() =>
                {
                  MessageBox.Show("Загрузка завершена");
                }
                )
              );
              threadMessage.Start();
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
    }

    public override void ParsingFour()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("ELMS 4 км. кв. Продажа");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                fourThreadOld = new Thread(ChangeDistrictAndPage);
                fourThreadOld.Start("4 км. кв.");
                fourThread = new Thread(ChangeDistrictAndPage);
                fourThread.Start("4 км. кв. Н");

                while (true)
                {
                  if (!fourThreadOld.IsAlive)
                  {
                    if (!fourThread.IsAlive)
                      break;
                  }
                }
                listFlat.Clear();
                foreach (var dis in ListDistricts)
                {
                  dis.Buildings.Clear();
                }
                Log.Debug("Start Union 4");
                fourThreadOld = new Thread(UnionFlats);
                fourThreadOld.Start("4 км. кв.");
                fourThread = new Thread(UnionFlats);
                fourThread.Start("4 км. кв. Н");

                while (true)
                {
                  if (!fourThreadOld.IsAlive)
                  {
                    if (!fourThread.IsAlive)
                      break;
                  }
                }

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
        progress = new ProgressForm("ELMS4 км. кв. Сдача");
        var threadbackground = new Thread(
          new ThreadStart(() =>
          {
            try
            {
              fourRentThread = new Thread(ChangeDistrictAndPageForRent);
              fourRentThread.Start("4 км. кв.");

              while (true)
              {
                if (!fourRentThread.IsAlive)
                  break;
              }
              fourRentThread = new Thread(UnionFlats);
              fourRentThread.Start("4 км. кв.");
              while (true)
              {
                if (!fourRentThread.IsAlive)
                  break;
              }

              IsFinished = true;
              var threadMessage = new Thread(
                new ThreadStart(() =>
                {
                  MessageBox.Show("Загрузка завершена");
                }
                )
              );
              threadMessage.Start();
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

                while (true)
                {
                  if (!fiveThreadOld.IsAlive)
                  {
                    if (!fiveThread.IsAlive)
                      break;
                  }
                }
                listFlat.Clear();
                foreach (var dis in ListDistricts)
                {
                  dis.Buildings.Clear();
                }
                Log.Debug("Start Union 5");
                fiveThreadOld = new Thread(UnionFlats);
                fiveThreadOld.Start("5 км. кв.");
                fiveThread = new Thread(UnionFlats);
                fiveThread.Start("5 км. кв. Н");

                while (true)
                {
                  if (!fiveThreadOld.IsAlive)
                  {
                    if (!fiveThread.IsAlive)
                      break;
                  }
                }

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
        progress = new ProgressForm("ELMS Более 4 км. Сдача");
        var threadbackground = new Thread(
          new ThreadStart(() =>
          {
            try
            {
              fiveRentThread = new Thread(ChangeDistrictAndPageForRent);
              fiveRentThread.Start("5 км. кв.");
              sixRentThreadOld = new Thread(ChangeDistrictAndPageForRent);
              sixRentThreadOld.Start("6 км. кв.");

              while (true)
              {
                if (!fiveRentThread.IsAlive)
                  if (!sixRentThreadOld.IsAlive)
                    break;
              }
              fiveRentThread = new Thread(UnionFlats);
              fiveRentThread.Start("5 км. кв.");
              sixRentThreadOld = new Thread(UnionFlats);
              sixRentThreadOld.Start("6 км. кв.");
              while (true)
              {
                if (!fiveRentThread.IsAlive)
                  if (!sixRentThreadOld.IsAlive)
                    break;
              }

              IsFinished = true;
              var threadMessage = new Thread(
                new ThreadStart(() =>
                {
                  MessageBox.Show("Загрузка завершена");
                }
                )
              );
              threadMessage.Start();
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
    }

    public override void ParsingSdamAll()
    {
      //throw new NotImplementedException();
    }

    private void ChangeDistrictAndPage(object typeRoom)
    {
      HtmlParser parser = new HtmlParser();

      var path = CreateExportForRoom(typeRoom.ToString());
      CoreCreator creator = new CsvExportCreator();
      var exportPart = creator.FactoryCreate(path);

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
                url =
                  $@"https://www.emls.ru/new/page{i}.html?query=s/1/dist/{distr.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{distr.Key}/by_room/1";
                break;
              case "1 км. кв.":
                url =
                  $@"https://www.emls.ru/flats/page{i}.html?query=s/1/r1/1/is_auction/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
                break;
              case "1 км. кв. Н":
                url =
                  $@"https://www.emls.ru/new/page{i}.html?query=s/1/dist/{distr.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{distr.Key}/r1/1/by_room/1";
                break;
              case "2 км. кв.":
                url =
                  $@"https://www.emls.ru/flats/page{i}.html?query=s/1/r2/1/is_auction/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
                break;
              case "2 км. кв. Н":
                url =
                  $@"https://www.emls.ru/new/page{i}.html?query=s/1/dist/{distr.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{distr.Key}/r2/1/by_room/1";
                break;
              case "3 км. кв.":
                url =
                  $@"https://www.emls.ru/flats/page{i}.html?query=s/1/r3/1/is_auction/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
                break;
              case "3 км. кв. Н":
                url =
                  $@"https://www.emls.ru/new/page{i}.html?query=s/1/dist/{distr.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{distr.Key}/r3/1/by_room/1";
                break;
              case "4 км. кв. Н":
                url =
                  $@"https://www.emls.ru/new/page{i}.html?query=s/1/dist/{distr.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{distr.Key}/r4/1/by_room/1";
                break;
              case "4 км. кв.":
                url =
                  $@"https://www.emls.ru/flats/page{i}.html?query=s/1/r4/1/is_auction/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
                break;
              case "5 км. кв. Н":
                url =
                  $@"https://www.emls.ru/new/page{i}.html?query=s/1/dist/{distr.Key}/dir2/2/sort2/1/dir1/2/sort1/3/stext/%C0%E4%EC%E8%F0%E0%EB%F2%E5%E9%F1%EA%E8%E9/district/{distr.Key}/r5/1/by_room/1";
                break;
              case "5 км. кв.":
                url =
                  $@"https://www.emls.ru/flats/page{i}.html?query=s/1/r5/1/is_auction/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
                break;
              case "6 км. кв.":
                url =
                  $@"https://www.emls.ru/flats/page{i}.html?query=s/1/r6/1/is_auction/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/1/dir1/2/sort2/3/dir2/1/interval/3";
                break;
            }

            if (!ExecuteParse(url, webClient, parser, (string)typeRoom,
              ListDistricts.Where(x => x.Name.ToLower() == distr.Value.ToLower()).First(), exportPart))
              break;
          }
        }
      }

      //MessageBox.Show($"Закончили - {typeRoom}");
    }

    private bool ExecuteParse(string url, WebClient webClient, HtmlParser parser, string typeRoom, District district,
      CoreExport export)
    {
      var random = new Random();
      Thread.Sleep(random.Next(2000, 4000));
      try
      {
        var responce = webClient.DownloadString(url);
        var document = parser.Parse(responce);
        var tableElements = document.GetElementsByClassName("row1");

        //if (typeRoom.Contains("Н"))
        //  ParseSheetNov(tableElements, typeRoom, district);
        //else
        //  ParseSheet(tableElements, typeRoom, district);

        if (tableElements.Length == 0)
        {
          Log.Debug("tableElements count = 0; URL - {url}");
          return false;
        }
        else
        {
          Log.Debug($"tableElements count = {tableElements.Length}; URL - {url}");
          ParseSheet(tableElements, typeRoom, district, export);
        }

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

    private void ParseSheet(IHtmlCollection<IElement> collection, string typeRoom, District district, CoreExport export)
    {
      Log.Debug($"Call ParseSheet for - {typeRoom}");
      var parseStreet = new ParseStreet();
      for (int i = 0; i < collection.Length; i++)
      {
        //Log.Debug(allcount);
        progress.UpdateAllProgress(allcount);
        allcount++;
        string street = string.Empty;
        string number = string.Empty;
        string structure = string.Empty;
        string liter = string.Empty;
        var flat = new Flat
        {
          CountRoom = typeRoom
        };
        var href = collection[i].ParentElement.GetAttribute("href");
        if (href != "javascript:void(0)")
          flat.Url = $@"https://www.emls.ru{href}";

        //Адрес
        string[] adr = null;
        if (collection[i].GetElementsByClassName("address-geo").Length == 1)
          adr = collection[i].GetElementsByClassName("address-geo")[0].TextContent.Split(',');
        else if (collection[i].GetElementsByClassName("address-geo").Length > 1)
          adr = collection[i].GetElementsByClassName("address-geo")[1].TextContent.Split(',');
        if (adr != null)
        {
          if (adr.Length == 3)
          {
            street = adr[0] + " " + adr[1];
            number = adr[2];
            if (street.Contains(district.Name))
            {
              street = adr[1];
            }
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
        else if (mc.Count == 2)
        {
          liter = mc[1].Value;
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

        //Цена
        if (!typeRoom.Contains("Н"))
        {
          //Если не новостройка
          var pr = collection[i].GetElementsByClassName("price");
          if (pr.Length > 0)
          {
            string priceStr = pr[0].TextContent.Replace("a", "").Replace(" ", "").Replace("\n", "").Replace("\t", "")
              .Replace("/мес", "");
            int price;
            if (int.TryParse(priceStr, out price))
            {
              flat.Price = price;
            }
          }

          //Поиск площади
          var square = collection[i].GetElementsByClassName("space-all");
          if (square.Length > 0)
            flat.Square = square[0].TextContent.Replace(".", ",").Trim();

          //Этаж
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
          //Если новостройка
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
                var square = collection[i].GetElementsByClassName("space-all");
                if (square.Length > 0)
                  flat.Square = square[0].TextContent.Replace(".", ",").Trim();
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
                  flat.Square = sq[0].TextContent.Replace(".", ",").Trim();
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
        }

        //Поиск кол-ва комнат при 6 км. кв.
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
        }

        //street = street.Replace("ул.", "").Replace("ал.", "").Replace("бул.", "").Replace("ш.", "").Replace("пр.", "").Replace("пер.", "").Replace("пр-д", "").Replace(" б", "").Trim();

        number = number.Trim();
        street = street.Trim();
        structure = structure.Trim();
        liter = liter.Trim();
        var st = this.district.Where(x => x.Value == street);
        if (st != null)
        {
          if (st.Count() > 0)
          {
            continue;
          }
        }

        street = parseStreet.Execute(street, district);

        //Поиск дома 
        Log.Debug($"Search Build");
        Building building = null;
        Monitor.Enter(lockerDistrict);
        if (district.Buildings.Count != 0)
        {
          var bldsEnum =
            district.Buildings.Where(x =>
              x.Street == street && x.Number == number && x.Structure == structure && x.Liter == liter);
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
            var mt = metro[0].TextContent.Replace("\n", "").Trim().Replace("пр.", "").Replace("ул.", "")
              .Replace("и-т", "");
            if (mt.Contains("А.Hевского пл."))
              mt = "Площадь Александра Невского";
            else if (mt.Contains("Мужества пл."))
              mt = "Площадь Мужества";
            else if (mt.Contains("Восстания пл."))
              mt = "Площадь Восстания";
            else if (mt.Contains("Ленина пл."))
              mt = "Площадь Ленина";
            else if (mt.Contains("Сенная пл."))
              mt = "Сенная площадь";
            var metroObjEnum = ListMetros.Where(x => mt.ToUpper().Contains(x.Name.ToUpper()));
            if (metroObjEnum.Count() > 0)
            {
              building.MetroObj = metroObjEnum.First();
            }
          }
        }


        if (!string.IsNullOrWhiteSpace(flat.Building.Street))
        {
          if (!string.IsNullOrWhiteSpace(flat.Building.Number))
          {
            if (!string.IsNullOrWhiteSpace(flat.Square))
            {
              bool contain = false;
              Monitor.Enter(locker);
              foreach (var fl in listFlat)
              {
                if (flat.Equals(fl))
                {
                  contain = true;
                }
              }
              Monitor.Exit(locker);

              if (contain)
                flat.CountRoom = flat.CountRoom + " Дубль";
              else
              {
                //OnAppend(this, new AppendFlatEventArgs { Flat = flat });
                export.AddFlatInList(this, new AppendFlatEventArgs { Flat = flat });
                Monitor.Enter(locker);
                progress.UpdateProgress(count);
                count++;
                listFlat.Add(flat);
                Monitor.Exit(locker);
              }
            }
          }
        }
      }
    }

    private void ChangeDistrictAndPageForRent(object typeRoom)
    {
      var path = CreateExportForRoom(typeRoom.ToString());
      CoreCreator creator = new CsvExportCreator();
      var exportPart = creator.FactoryCreate(path);

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
                url =
                  $@"https://www.emls.ru/arenda/page{i}.html?query=s/1/r1/1/type/2/rtype/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/4/dir1/1/dir2/2/interval/3";
                break;
              case "2 км. кв.":
                url =
                  $@"https://www.emls.ru/arenda/page{i}.html?query=s/1/r2/1/type/2/rtype/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/4/dir1/1/dir2/2/interval/3";
                break;
              case "3 км. кв.":
                url =
                  $@"https://www.emls.ru/arenda/page{i}.html?query=s/1/r3/1/type/2/rtype/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/4/dir1/1/dir2/2/interval/3";
                break;
              case "4 км. кв.":
                url =
                  $@"https://www.emls.ru/arenda/page{i}.html?query=s/1/r4/1/type/2/rtype/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/4/dir1/1/dir2/2/interval/3";
                break;
              case "5 км. кв.":
                url =
                  $@"https://www.emls.ru/arenda/page{i}.html?query=s/1/r5/1/type/2/rtype/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/4/dir1/1/dir2/2/interval/3";
                break;
              case "6 км. кв.":
                url =
                  $@"https://www.emls.ru/arenda/page{i}.html?query=s/1/r6/1/type/2/rtype/2/place/address/reg/2/dept/2/dist/{distr.Key}/sort1/4/dir1/1/dir2/2/interval/3";
                break;
            }

            if (!ExecuteParseRent(url, webClient, parser, (string)typeRoom,
              ListDistricts.Where(x => x.Name.ToLower() == distr.Value.ToLower()).First(), exportPart))
              break;
          }
        }
      }
    }

    private bool ExecuteParseRent(string url, WebClient webClient, HtmlParser parser, string typeRoom,
      District district, CoreExport export)
    {
      var random = new Random();
      Thread.Sleep(random.Next(2000, 4000));
      //try
      //{
      var responce = webClient.DownloadString(url);
      var document = parser.Parse(responce);
      var tableElements = document.GetElementsByClassName("row1");
      ParseSheet(tableElements, typeRoom, district, export);
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
              flat.Square = square[0].TextContent.Replace(".", ",").Trim();

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


            flat.Building.Street = flat.Building.Street.Replace("ул.", "").Replace("ал.", "").Replace("бул.", "")
              .Replace("ш.", "").Replace("пр.", "").Replace("пер.", "").Replace("пр-д", "").Replace(" б", "").Trim();

            Monitor.Enter(locker);
            bool flag = false;
            foreach (var bl in listFlat)
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
                listFlat.Add(flat);

                using (var sw = new StreamWriter(new FileStream(FilenameSdam, FileMode.Open), Encoding.UTF8))
                {
                  sw.BaseStream.Position = sw.BaseStream.Length;
                  sw.WriteLine(
                    $@"{town};{flat.Building.Street};{flat.Building.Number};{flat.Building.Structure};{flat.Building.Liter};{flat.CountRoom};{flat.Square};{flat.Price};{flat.Floor};{flat.Building.Metro};{flat.Building.Distance}");
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

    private void UnionFlats(object type)
    {
      Log.Debug($"Start Union {type}");
      var union = new UnionParseInfoWithDataBase();
      var path = ExctractPath();
      path = $@"{path}{type}-{DateTime.Now.ToShortDateString()}-{NameSite}.csv";
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

          District dis = null;
          try
          {
            dis = ListDistricts.Where(x => string.Equals(x.Name, ar[0], StringComparison.CurrentCultureIgnoreCase)).First();
          }
          catch (Exception e)
          {
            Log.Error($@"{e.Message}; ar[0] - {ar[0]}");
          }

          if (dis == null)
            continue;

          Building building = null;
          Monitor.Enter(lockerDistrict);
          try
          {
            if (dis.Buildings.Count != 0)
            {
              var bldsEnum =
                dis.Buildings.Where(x =>
                  x.Street == street && x.Number == number && x.Structure == struc && x.Liter == liter);
              if (bldsEnum.Count() > 0)
                building = bldsEnum.First();
            }

            if (building == null)
            {
              building = new Building
              {
                Street = street,
                Number = number,
                Structure = struc,
                Liter = liter,
                District = dis,
              };
              dis.Buildings.Add(building);
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
}
