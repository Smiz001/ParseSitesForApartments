using AngleSharp.Dom.Html;
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
using ParseSitesForApartments.ParsClasses;
using ParseSitesForApartments.Export;
using ParseSitesForApartments.Export.Creators;
using ParseSitesForApartments.Proxy;
using ParseSitesForApartments.UI;
using ParseSitesForApartments.UnionWithBase;

namespace ParseSitesForApartments.Sites
{
  public class BN : BaseParse
  {
    #region Fields

    private static object locker = new object();
    private static object lockerDistrict = new object();
    private static object lockerUnion = new object();
    private List<Flat> listFlat = new List<Flat>();

    private int minPage = 1;
    private int maxPage = 17;
    private Dictionary<int, string> district = new Dictionary<int, string>() { { 1, "Адмиралтейский" }, { 2, "Василеостровский" }, { 3, "Выборгский" }, { 5, "Калининский" }, { 4, "Кировский" }, { 16, "Колпинский" }, { 6, "Красногвардейский" }, { 7, "Красносельский" }, { 15, "Кронштадтский" }, { 17, "Курортный" }, { 8, "Московский" }, { 9, "Невский" }, { 10, "Петроградский" }, { 19, "Петродворцовый" }, { 11, "Приморский" }, { 20, "Пушкинский" }, { 12, "Фрунзенский" }, { 13, "Центральный" }, };
    private CoreExport export;
    public delegate void Append(object sender, AppendFlatEventArgs e);
    public event Append OnAppend;
    private readonly UnionParseInfoWithDataBase unionInfo = new UnionParseInfoWithDataBase();
    private Thread studiiThread;
    private Thread oneThread;
    private Thread twoThread;
    private Thread threeThread;
    private Thread fourThread;
    private Thread studiiThreadOld;
    private Thread oneThreadOld;
    private Thread twoThreadOld;
    private Thread threeThreadOld;
    private Thread fourThreadOld;

    private Thread studiiSdamThread;
    private Thread oneSdamThread;
    private Thread twoSdamThread;
    private Thread threeSdamThread;
    private Thread fourSdamThread;
    private ProgressForm progress;
    private int count = 1;
    private int allcount = 1;

    #endregion

    #region Constructor

    public BN(List<District> listDistricts, List<Metro> listMetros, List<ProxyInfo> listProxy) : base(listDistricts, listMetros, listProxy)
    {
      //CoreCreator creator = new ExcelExportCreator();
      //export = creator.FactoryCreate(Filename);
      //CoreCreator creator = new CsvExportCreator();
      //export = creator.FactoryCreate(Filename);
      //OnAppend += export.AddFlatInList;
    }

    #endregion

    #region Properties

    public override string Filename { get; set; }
    //public override string Filename => @"d:\ParserInfo\Appartament\BNProdam.xlsx";
    public override string FilenameSdam => @"d:\ParserInfo\Appartament\BNSdam.csv";
    public override string FilenameWithinfo => @"d:\ParserInfo\Appartament\BNProdamWithInfo.csv";
    public override string FilenameWithinfoSdam => @"d:\ParserInfo\Appartament\BNSdamWithInfo.csv";
    public override string NameSite => "БН";

    #endregion

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

    private void CreateExportSdam()
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

    private string ExctractPath()
    {
      string path = string.Empty;
      var arr = Filename.Split('\\');
      path = Filename.Replace(arr[arr.Length - 1], "");
      return path;
    }

    public override void ParsingAll()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("БН Все квартиры");
        var threadbackground = new Thread(
          new ThreadStart(() =>
          {
            try
            {
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

              while (true)
              {
                if (!studiiThreadOld.IsAlive)
                  if (!studiiThread.IsAlive)
                    if (!oneThreadOld.IsAlive)
                      if (!twoThreadOld.IsAlive)
                        if (!threeThreadOld.IsAlive)
                          if (!fourThreadOld.IsAlive)
                            if (!oneThread.IsAlive)
                              if (!twoThread.IsAlive)
                                if (!threeThread.IsAlive)
                                  if (!fourThread.IsAlive)
                                    break;
              }


              studiiThread = new Thread(UnionFlats);
              studiiThread.Start("Студия Н");
              oneThread = new Thread(UnionFlats);
              oneThread.Start("1 км. кв. Н");
              twoThread = new Thread(UnionFlats);
              twoThread.Start("2 км. кв. Н");
              threeThread = new Thread(UnionFlats);
              threeThread.Start("3 км. кв. Н");
              fourThread = new Thread(UnionFlats);
              fourThread.Start("4 км. кв. Н");

              studiiThreadOld = new Thread(UnionFlats);
              studiiThreadOld.Start("Студия");
              oneThreadOld = new Thread(UnionFlats);
              oneThreadOld.Start("1 км. кв.");
              twoThreadOld = new Thread(UnionFlats);
              twoThreadOld.Start("2 км. кв.");
              threeThreadOld = new Thread(UnionFlats);
              threeThreadOld.Start("3 км. кв.");
              fourThreadOld = new Thread(UnionFlats);
              fourThreadOld.Start("4 км. кв.");

              while (true)
              {
                if (!studiiThreadOld.IsAlive)
                  if (!studiiThread.IsAlive)
                    if (!oneThreadOld.IsAlive)
                      if (!twoThreadOld.IsAlive)
                        if (!threeThreadOld.IsAlive)
                          if (!fourThreadOld.IsAlive)
                            if (!oneThread.IsAlive)
                              if (!twoThread.IsAlive)
                                if (!threeThread.IsAlive)
                                  if (!fourThread.IsAlive)
                                    break;
              }
              //Thread.Sleep(10000);
              //CheckCloseThread(-1);
              //export.Execute();

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
        while(threadbackground.IsAlive)
          Thread.Sleep(10000);
      }
      else
      {
        studiiSdamThread = new Thread(ChangeDistrictAndPageForSdam);
        studiiSdamThread.Start("Студия");
        oneSdamThread = new Thread(ChangeDistrictAndPageForSdam);
        oneSdamThread.Start("1 км. кв.");
        twoSdamThread = new Thread(ChangeDistrictAndPageForSdam);
        twoSdamThread.Start("2 км. кв.");
        threeSdamThread = new Thread(ChangeDistrictAndPageForSdam);
        threeSdamThread.Start("3 км. кв.");
        fourSdamThread = new Thread(ChangeDistrictAndPageForSdam);
        fourSdamThread.Start("4 км. кв.");
      }
    }

    public override void ParsingStudii()
    {
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        CreateExport();
        progress = new ProgressForm("БН Студии");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                studiiThreadOld = new Thread(ChangeDistrictAndPage);
                studiiThreadOld.Start("Студия");
                studiiThread = new Thread(ChangeDistrictAndPage);
                studiiThread.Start("Студия Н");

                while (true)
                {
                  if (!studiiThreadOld.IsAlive)
                    if (!studiiThread.IsAlive)
                                      break;
                }

                studiiThread = new Thread(UnionFlats);
                studiiThread.Start("Студия Н");
                studiiThreadOld = new Thread(UnionFlats);
                studiiThreadOld.Start("Студия");
                while (true)
                {
                  if (!studiiThreadOld.IsAlive)
                    if (!studiiThread.IsAlive)
                      break;
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
        CreateExportSdam();
        studiiSdamThread = new Thread(ChangeDistrictAndPageForSdam);
        studiiSdamThread.Start("Студия");
      }
    }

    public override void ParsingOne()
    {
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        CreateExport();

        progress = new ProgressForm("БН 1 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                oneThread = new Thread(ChangeDistrictAndPage);
                oneThread.Start("1 км. кв. Н");
                oneThreadOld = new Thread(ChangeDistrictAndPage);
                oneThreadOld.Start("1 км. кв.");
                while (true)
                {
                  if (!oneThreadOld.IsAlive)
                    if (!oneThread.IsAlive)
                      break;
                }
                oneThread = new Thread(UnionFlats);
                oneThread.Start("1 км. кв. Н");
                oneThreadOld = new Thread(UnionFlats);
                oneThreadOld.Start("1 км. кв.");
                while (true)
                {
                  if (!oneThreadOld.IsAlive)
                    if (!oneThread.IsAlive)
                      break;
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
        CreateExportSdam();
        oneSdamThread = new Thread(ChangeDistrictAndPageForSdam);
        oneSdamThread.Start("1 км. кв.");
      }
    }

    public override void ParsingTwo()
    {
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        CreateExport();
        progress = new ProgressForm("БН 2 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                twoThread = new Thread(ChangeDistrictAndPage);
                twoThread.Start("2 км. кв. Н");
                twoThreadOld = new Thread(ChangeDistrictAndPage);
                twoThreadOld.Start("2 км. кв.");
                while (true)
                {
                  if (!twoThreadOld.IsAlive)
                    if (!twoThread.IsAlive)
                      break;
                }
                twoThread = new Thread(UnionFlats);
                twoThread.Start("2 км. кв. Н");
                twoThreadOld = new Thread(UnionFlats);
                twoThreadOld.Start("2 км. кв.");
                while (true)
                {
                  if (!twoThreadOld.IsAlive)
                    if (!twoThread.IsAlive)
                      break;
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
        CreateExportSdam();
        twoSdamThread = new Thread(ChangeDistrictAndPageForSdam);
        twoSdamThread.Start("2 км. кв.");
      }
    }

    public override void ParsingThree()
    {
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        CreateExport();
        progress = new ProgressForm("БН 3 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                threeThread = new Thread(ChangeDistrictAndPage);
                threeThread.Start("3 км. кв. Н");
                threeThreadOld = new Thread(ChangeDistrictAndPage);
                threeThreadOld.Start("3 км. кв.");
                while (true)
                {
                  if (!threeThreadOld.IsAlive)
                    if (!threeThread.IsAlive)
                      break;
                }
                threeThread = new Thread(UnionFlats);
                threeThread.Start("3 км. кв. Н");
                threeThreadOld = new Thread(UnionFlats);
                threeThreadOld.Start("3 км. кв.");
                while (true)
                {
                  if (!threeThreadOld.IsAlive)
                    if (!threeThread.IsAlive)
                      break;
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
        CreateExportSdam();
        threeSdamThread = new Thread(ChangeDistrictAndPageForSdam);
        threeSdamThread.Start("3 км. кв.");
      }
    }

    public override void ParsingFour()
    {
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        CreateExport();
        progress = new ProgressForm("БН 4 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                fourThread = new Thread(ChangeDistrictAndPage);
                fourThread.Start("4 км. кв. Н");
                fourThreadOld = new Thread(ChangeDistrictAndPage);
                fourThreadOld.Start("4 км. кв.");
                while (true)
                {
                  if (!fourThreadOld.IsAlive)
                    if (!fourThread.IsAlive)
                      break;
                }
                fourThread = new Thread(UnionFlats);
                fourThread.Start("4 км. кв. Н");
                fourThreadOld = new Thread(UnionFlats);
                fourThreadOld.Start("4 км. кв.");
                while (true)
                {
                  if (!fourThreadOld.IsAlive)
                    if (!fourThread.IsAlive)
                      break;
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
        CreateExportSdam();
        fourSdamThread = new Thread(ChangeDistrictAndPageForSdam);
        fourSdamThread.Start("4 км. кв.");
      }
    }

    public override void ParsingMoreFour()
    {
      //Nothing
      //throw new NotImplementedException();
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

    private void ChangeDistrictAndPage(object typeRoom)
    {
      var path = CreateExportForRoom(typeRoom.ToString());
      CoreCreator creator = new CsvExportCreator();
      var exportPart = creator.FactoryCreate(path);

      HtmlParser parser = new HtmlParser();
      using (var webClient = new WebClient())
      {
        webClient.Encoding = Encoding.UTF8;
        string url = "";
        foreach (var distr in district)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            switch (typeRoom)
            {
              case "Студия":
                url =
                  $@"https://www.bn.ru/kvartiry-vtorichka/kkv-0-city_district-{distr.Key}/?from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptNewBuildings=1&exceptPortion=1&formName=secondary&page={i}";
                break;
              case "Студия Н":
                url =
                  $@"https://www.bn.ru/kvartiry-novostroiki/kkv-0-city_district-{distr.Key}/?cpu=kkv-0-city_district-1&kkv%5B0%5D=0&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=newbuild&page={i}";
                break;
              case "1 км. кв.":
                url =
                  $@"https://www.bn.ru/kvartiry-vtorichka/kkv-1-city_district-{distr.Key}/?cpu=kkv-1-city_district-1&kkv%5B0%5D=1&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptNewBuildings=1&exceptPortion=1&formName=secondary&page={i}";
                break;
              case "1 км. кв. Н":
                url =
                  $@"https://www.bn.ru/kvartiry-novostroiki/kkv-1-city_district-{distr.Key}/?cpu=kkv-1-city_district-1&kkv%5B0%5D=1&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=newbuild&page={i}";
                break;
              case "2 км. кв.":
                url =
                  $@"https://www.bn.ru/kvartiry-vtorichka/kkv-2-city_district-{distr.Key}/?cpu=kkv-2-city_district-1&kkv%5B0%5D=2&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptNewBuildings=1&exceptPortion=1&formName=secondary&page={i}";
                break;
              case "2 км. кв. Н":
                url =
                  $@"https://www.bn.ru/kvartiry-novostroiki/kkv-2-city_district-{distr.Key}/?cpu=kkv-2-city_district-1&kkv%5B0%5D=2&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=newbuild&page={i}";
                break;
              case "3 км. кв.":
                url =
                  $@"https://www.bn.ru/kvartiry-vtorichka/kkv-3-city_district-{distr.Key}/?cpu=kkv-3-city_district-1&kkv%5B0%5D=3&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptNewBuildings=1&exceptPortion=1&formName=secondary&page={i}";
                break;
              case "3 км. кв. Н":
                url =
                  $@"https://www.bn.ru/kvartiry-novostroiki/kkv-3-city_district-{distr.Key}/?cpu=kkv-3-city_district-1&kkv%5B0%5D=3&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=newbuild&page={i}";
                break;
              case "4 км. кв. Н":
                url =
                  $@"https://www.bn.ru/kvartiry-novostroiki/kkv-4-city_district-{distr.Key}/?cpu=kkv-4-city_district-1&kkv%5B0%5D=4&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=newbuild&page={i}";
                break;
              case "4 км. кв.":
                url =
                  $@"https://www.bn.ru/kvartiry-vtorichka/kkv-4-city_district-{distr.Key}/?cpu=kkv-4-city_district-1&kkv%5B0%5D=4&city_district%5B0%5D=1&from=&to=&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&preferPhoto=1&exceptNewBuildings=1&exceptPortion=1&formName=secondary&page={i}";
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

    private void ChangeDistrictAndPageForSdam(object typeRoom)
    {
      var path = CreateExportForRoom(typeRoom.ToString());
      CoreCreator creator = new CsvExportCreator();
      var exportPart = creator.FactoryCreate(path);

      HtmlParser parser = new HtmlParser();
      using (var webClient = new WebClient())
      {
        webClient.Encoding = Encoding.UTF8;
        string url = "";
        foreach (var distr in district)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            switch (typeRoom)
            {
              case "Студия":
                url =
                  $@"https://www.bn.ru/arenda-kvartiry/kkv-0-city_district-{distr.Key}/?lease_period%5B%5D=1&floor=0&formName=rent&page={i}";
                break;
              case "1 км. кв.":
                url =
                  $@"https://www.bn.ru/arenda-kvartiry/kkv-1-city_district-{distr.Key}/?lease_period%5B%5D=1&floor=0&formName=rent&page={i}";
                break;
              case "2 км. кв.":
                url =
                  $@"https://www.bn.ru/arenda-kvartiry/kkv-2-city_district-{distr.Key}/?lease_period%5B%5D=1&floor=0&formName=rent&page={i}";
                break;
              case "3 км. кв.":
                url =
                  $@"https://www.bn.ru/arenda-kvartiry/kkv-3-city_district-{distr.Key}/?lease_period%5B%5D=1&floor=0&formName=rent&page={i}";
                break;
              case "4 км. кв.":
                url =
                  $@"https://www.bn.ru/arenda-kvartiry/kkv-4-city_district-{distr.Key}/?lease_period%5B%5D=1&floor=0&formName=rent&page={i}";
                break;
            }
            if (!ExecuteParse(url, webClient, parser, (string)typeRoom,
              ListDistricts.Where(x => x.Name.ToLower() == distr.Value.ToLower()).First(), exportPart))
              break;
          }
        }
      }

      MessageBox.Show($"Закончили - {typeRoom}");
    }

    private bool ExecuteParse(string url, WebClient webClient, HtmlParser parser, string typeRoom, District district, CoreExport export)
    {
      var random = new Random();
      Thread.Sleep(random.Next(2000, 4000));
      try
      {
        var responce = webClient.DownloadString(url);
        var document = parser.Parse(responce);
        ParseSheet(typeRoom, document, district, export);
        if (document.GetElementsByClassName("object--item").Length < 30)
          return false;
        return true;
      }
      catch (Exception e)
      {
        //TODO Если страница долго не отвечает то пропускаем ее
        Thread.Sleep(1000);
        return true;
      }
    }

    private void ParseSheet(string typeRoom, IHtmlDocument document, District district, CoreExport export )
    {
      var apartaments = document.GetElementsByClassName("object--item");
      var urlElems = document.GetElementsByClassName("object--link-to-object button_mode_one");
      var parseStreet = new ParseStreet();

      for (int i = 0; i < apartaments.Length; i++)
      {
        progress.UpdateAllProgress(allcount);
        allcount++;
        var flat = new Flat
        {
          Url = $"https://www.bn.ru{urlElems[i].GetAttribute("href")}"
        };
        if (apartaments[i].GetElementsByClassName("object__square").Length > 0)
          flat.Square = apartaments[i].GetElementsByClassName("object__square")[0].TextContent.Replace("м2","").Replace(".", ",").Trim();
        flat.CountRoom = typeRoom;
        if (typeRoom == "4 км. кв." || typeRoom == "4 км. кв. Н")
        {
          var rx = new Regex(@"(\d+)");
          if (apartaments[i].GetElementsByClassName("object--title").Length > 0)
          {
            flat.CountRoom = $@"{rx.Match(apartaments[i].GetElementsByClassName("object--title")[0].TextContent).Value} км. кв.";
          }
        }

        var regex = new Regex(@"(\d+)");

        var priceString = apartaments[i].GetElementsByClassName("object--price_original")[0].TextContent.Trim();
        var ms = regex.Matches(priceString);
        if (ms.Count > 1)
          if(TypeParseFlat == TypeParseFlat.Sale)
            flat.Price = int.Parse($"{ms[0].Value}{ms[1].Value}000");
          else
            flat.Price = int.Parse($"{ms[0].Value}{ms[1].Value}");
        else 
        if (TypeParseFlat == TypeParseFlat.Sale)
          flat.Price = int.Parse($"{ms[0].Value}000");
        else
          flat.Price = int.Parse($"{ms[0].Value}");

        //var building = district.Buildings.Where(x=> x.Street = )

        #region Parse street

        string street = string.Empty;
        string number = string.Empty;
        string structure = string.Empty;

        street = apartaments[i].GetElementsByClassName("object--address")[0].TextContent.Trim().Replace("Санкт-Петербург, ", "").Replace("Санкт-Петербург г.", "");


        regex = new Regex(@"(\d+к\d+)");
        number = regex.Match(street).Value;
        if (!string.IsNullOrEmpty(number))
        {
          street = street.Replace(number, "");
          regex = new Regex(@"(к\d+)");
          structure = regex.Match(number).Value.Replace("к", "");
          number = number.Replace($"к{structure}", "");
        }
        else
        {
          regex = new Regex(@"(\d+\/\d+)");
          number = regex.Match(street).Value;
          if (!string.IsNullOrEmpty(number))
          {
            street = street.Replace(number, "");
            regex = new Regex(@"(\/\d+)");
            structure = regex.Match(number).Value.Replace(@"/", "");
            number = number.Replace($@"/{structure}", "");
          }
          else
          {
            regex = new Regex(@"(\d+\sк\.\d+)|(\d+к\.\d+)|(\d+\sк\.\s\d+)|(\d+к\.\s\d+)");
            number = regex.Match(street).Value;
            if (!string.IsNullOrEmpty(number))
            {
              street = street.Replace(number, "");
              number = number.Replace(" ", "");
              regex = new Regex(@"(к.\d+)");
              structure = regex.Match(number).Value.Replace(@"к.", "");
              number = number.Replace($@"к.{structure}", "");
            }
            else
            {
              regex = new Regex(@"(ул\.\s\d+$)|(ул\,\s\d+$)");
              number = regex.Match(street).Value;
              if (!string.IsNullOrEmpty(number))
              {
                street = street.Replace(number, "");
                number = number.Replace("ул. ", "").Replace("ул, ", "");
              }
              else
              {
                regex = new Regex(@"(ул\.,\sд\.\s\d+)$");
                number = regex.Match(street).Value;
                if (!string.IsNullOrEmpty(number))
                {
                  street = street.Replace(number, "");
                  number = number.Replace("ул., д. ", "");
                }
                else
                {
                  regex = new Regex(@"(пр\.\,\s\d+$)|(пр\.\s\d+$)$");
                  number = regex.Match(street).Value;
                  if (!string.IsNullOrEmpty(number))
                  {
                    street = street.Replace(number, "");
                    number = number.Replace("пр., ", "").Replace("пр. ", "");
                  }
                  else
                  {
                    regex = new Regex(@"(\,\s\d+$)");
                    number = regex.Match(street).Value;
                    if (!string.IsNullOrEmpty(number))
                    {
                      street = street.Replace(number, "");
                      number = number.Replace(", ", "");
                    }
                    else
                    {
                      regex = new Regex(@"(д\.\s\d+$)|(д\.\d+$)");
                      number = regex.Match(street).Value;
                      if (!string.IsNullOrEmpty(number))
                      {
                        street = street.Replace(number, "");
                        number = number.Replace(" ", "").Replace("д.", "");
                      }
                      else
                      {
                        regex = new Regex(@"(дом\s+\d+$)|(дом\d+$)");
                        number = regex.Match(street).Value;
                        if (!string.IsNullOrEmpty(number))
                        {
                          street = street.Replace(number, "");
                          number = number.Replace(" ", "").Replace("дом", "");
                        }
                        else
                        {
                          regex = new Regex(@"(пер\.\s+\d+)");
                          number = regex.Match(street).Value;
                          if (!string.IsNullOrEmpty(number))
                          {
                            street = street.Replace(number, "");
                            number = number.Replace("пер. ", "");
                          }
                          else
                          {
                            regex = new Regex(@"(наб\.\s+\d+)");
                            number = regex.Match(street).Value;
                            if (!string.IsNullOrEmpty(number))
                            {
                              street = street.Replace(number, "");
                              number = number.Replace("наб. ", "");
                            }
                            else
                            {
                              regex = new Regex(@"(пл\.\s+\d+)");
                              number = regex.Match(street).Value;
                              if (!string.IsNullOrEmpty(number))
                              {
                                street = street.Replace(number, "");
                                number = number.Replace("пл. ", "");
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

        string town = "";
        if (street.Contains("Колпино") || street.Contains("г. Колпино"))
        {
          town = "Колпино";
          street = street.Replace(town, "").Replace("г. Колпино", "");
        }
        else if (street.Contains("Песочный") || street.Contains("пос. Песочный"))
        {
          town = "Песочный";
          street = street.Replace(town, "").Replace("пос. Песочный", "");
        }
        else if (street.Contains("г. Кронштадт"))
        {
          town = "Кронштадт";
          street = street.Replace("г. Кронштадт", "");
        }
        else if (street.Contains("Парголово"))
        {
          town = "Парголово";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Красное Село г"))
        {
          town = "Красное Село г";
          street = street.Replace(town, "");
        }
        else if (street.Contains("г. Пушкин"))
        {
          town = "г. Пушкин";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Пушкин"))
        {
          town = "Пушкин";
          street = street.Replace(town, "");
        }

        else if (street.Contains("Шушары пос"))
        {
          town = "Шушары пос";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Шушары"))
        {
          town = "Шушары";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Металострой пос"))
        {
          town = "Металострой пос";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Металострой"))
        {
          town = "Металострой";
          street = street.Replace(town, "");
        }
        else
        {
          town = "Санкт-Петербург";
          street = street.Replace("СПб", "");
        }

        street = street.Replace("ул.", "").Replace("улица", "").Replace("пр-кт", "").Replace("пр-т", "").Replace("проспект", "").Replace("наб", "").Replace("б-р", "").Replace("б-р/2", "").Replace("б-р/4", "").Replace("проезд", "").Replace("пр", "").Replace("шос к", "").Replace("бульвар", "").Replace(" б", "").Replace("  к", "").Replace("  д", "").Replace("пл", "").Replace(",", "").Replace(".", "").Trim();

        regex = new Regex(@"(\/А\d+А)");
        var str = regex.Match(street).Value;
        if (!string.IsNullOrEmpty(str))
          street = street.Replace(str, "");

        street = street.Replace(district.Name, "").Trim();
        street = parseStreet.Execute(street, district);

        #endregion
        number = number.Trim();
        structure = structure.Trim();

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

        string metro = string.Empty;
        if (apartaments[i].GetElementsByClassName("object--metro").Length > 0)
          metro = apartaments[i].GetElementsByClassName("object--metro")[0].TextContent.Trim();
        var metroObjEnum = ListMetros.Where(x => x.Name.ToUpper() == metro.ToUpper());
        if (metroObjEnum.Count() > 0)
        {
          building.MetroObj = metroObjEnum.First();
        }
        flat.Building = building;

        #region ParseFloor

        if (apartaments[i].GetElementsByClassName("object--floor").Length > 0)
        {
          var floor = apartaments[i].GetElementsByClassName("object--floor")[0].TextContent;
          regex = new Regex(@"(\d+)");
          var mas = regex.Matches(floor);
          if (mas.Count > 0)
            flat.Floor = mas[0].Value;
        }

        #endregion
        //if (apartaments[i].GetElementsByClassName("object--metro-distance").Length > 0)
        //{
        //  regex = new Regex(@"(\d+\.\d+)");
        //  flat.Building.Distance = apartaments[i].GetElementsByClassName("object--metro-distance")[0].TextContent.Replace(",", "").Replace(" ", "");
        //  flat.Building.Distance = regex.Match(flat.Building.Distance).Value;
        //}
        //flat.Building.Distance = flat.Building.Distance.Replace(".", ",");

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

        //if (!string.IsNullOrWhiteSpace(flat.Building.Number))
        //{
        //  if (!string.IsNullOrWhiteSpace(flat.Square))
        //  {
        //    if (!string.IsNullOrWhiteSpace(flat.Building.Street))
        //    {
        //      Monitor.Enter(locker);
        //      if (string.IsNullOrWhiteSpace(flat.Building.DateBuild))
        //      {
        //        unionInfo.UnionInfoProdam(flat);
        //      }
        //      OnAppend(this, new AppendFlatEventArgs { Flat = flat });
        //      progress.UpdateProgress(count);
        //      count++;
        //      Monitor.Exit(locker);
        //    }
        //  }
        //}
      }
    }

    public override void ParsingSdamAll()
    {
      if(export is CsvExport)
      {
        using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Create), Encoding.UTF8))
        {
          sw.WriteLine($@"Район;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние(км);URL");
        }
      }
      var studiiThreadOld = new Thread(ChangeDistrictAndPageSdam);
      studiiThreadOld.Start("Студия");
      var oneThreadOld = new Thread(ChangeDistrictAndPageSdam);
      oneThreadOld.Start("1 км. кв.");
      var twoThreadOld = new Thread(ChangeDistrictAndPageSdam);
      twoThreadOld.Start("2 км. кв.");
      var threeThreadOld = new Thread(ChangeDistrictAndPageSdam);
      threeThreadOld.Start("3 км. кв.");
      var fourThreadOld = new Thread(ChangeDistrictAndPageSdam);
      fourThreadOld.Start("4 км. кв.");
    }

    public void ParseStudiiSdam()
    {
      var studiiThreadOld = new Thread(ChangeDistrictAndPageSdam);
      studiiThreadOld.Start("Студия");
      MessageBox.Show("Закончили студии");
    }
    public void ParseOneSdam()
    {
      var oneThreadOld = new Thread(ChangeDistrictAndPageSdam);
      oneThreadOld.Start("1 км. кв.");
      MessageBox.Show("Закончили 1 км. кв.");
    }
    public void ParseTwoSdam()
    {
      var twoThreadOld = new Thread(ChangeDistrictAndPageSdam);
      twoThreadOld.Start("2 км. кв.");
      MessageBox.Show("Закончили 2 км. кв.");
    }
    public void ParseThreeSdam()
    {
      var threeThreadOld = new Thread(ChangeDistrictAndPageSdam);
      threeThreadOld.Start("3 км. кв.");
      MessageBox.Show("Закончили 3 км. кв.");
    }
    public void ParseFourSdam()
    {
      var fourThreadOld = new Thread(ChangeDistrictAndPageSdam);
      fourThreadOld.Start("4 км. кв.");
      MessageBox.Show("Закончили 4+ км. кв.");
    }

    private void ChangeDistrictAndPageSdam(object typeRoom)
    {
      var path = CreateExportForRoom(typeRoom.ToString());
      CoreCreator creator = new CsvExportCreator();
      var exportPart = creator.FactoryCreate(path);

      HtmlParser parser = new HtmlParser();
      using (var webClient = new WebClient())
      {
        webClient.Encoding = Encoding.UTF8;
        string url = "";
        foreach (var distr in district)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            switch (typeRoom)
            {
              case "Студия":
                url =
                  $@"https://www.bn.ru/arenda-kvartiry/kkv-0-city_district-{distr.Key}/?from=&to=&lease_period%5B%5D=1&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=rent&page={i}";
                break;
              case "1 км. кв.":
                url =
                  $@"https://www.bn.ru/arenda-kvartiry/kkv-1-city_district-{distr.Key}/?from=&to=&lease_period%5B%5D=1&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=rent&page={i}";
                break;
              case "2 км. кв.":
                url =
                  $@"https://www.bn.ru/arenda-kvartiry/kkv-2-city_district-{distr.Key}/?from=&to=&lease_period%5B%5D=1&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=rent&page={i}";
                break;
              case "3 км. кв.":
                url =
                  $@"https://www.bn.ru/arenda-kvartiry/kkv-3-city_district-{distr.Key}/?from=&to=&lease_period%5B%5D=1&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=rent&page={i}";
                break;
              case "4 км. кв.":
                url =
                  $@"https://www.bn.ru/arenda-kvartiry/kkv-4-city_district-{distr.Key}/?from=&to=&lease_period%5B%5D=1&areaFrom=&areaTo=&livingFrom=&livingTo=&kitchenFrom=&kitchenTo=&floor=0&floorFrom=&floorTo=&formName=rent&page={i}";
                break;
            }
            if (!ExecuteParse(url, webClient, parser, (string)typeRoom,
              ListDistricts.Where(x => x.Name.ToLower() == distr.Value.ToLower()).First(), exportPart))
              break;
          }
        }
      }
      MessageBox.Show($"Закончили - {typeRoom}");
    }

    private bool ExecuteParseSdam(string url, WebClient webClient, HtmlParser parser, string typeRoom, District district)
    {
      var random = new Random();
      Thread.Sleep(random.Next(2000, 4000));
      var responce = webClient.DownloadString(url);
      var document = parser.Parse(responce);
      ParseSheetSdam(typeRoom, document, district);
      if (document.GetElementsByClassName("object--item").Length < 30)
        return false;
      return true;
    }

    private void ParseSheetSdam(string typeRoom, IHtmlDocument document, District district)
    {
      var apartaments = document.GetElementsByClassName("object--item");
      var parseStreet = new ParseStreet();

      for (int i = 0; i < apartaments.Length; i++)
      {
        var flat = new Flat();
        if (apartaments[i].GetElementsByClassName("object__square").Length > 0)
          flat.Square = apartaments[i].GetElementsByClassName("object__square")[0].TextContent.Replace("м2", "").Replace(".", ",").Trim();
        flat.CountRoom = typeRoom;
        if (typeRoom == "4 км. кв.")
        {
          var rx = new Regex(@"(\d+)");
          if (apartaments[i].GetElementsByClassName("object--title").Length > 0)
          {
            flat.CountRoom = $@"{rx.Match(apartaments[i].GetElementsByClassName("object--title")[0].TextContent).Value} км. кв.";
          }
        }

        var regex = new Regex(@"(\d+)");

        var priceString = apartaments[i].GetElementsByClassName("object--price_original")[0].TextContent.Trim();
        var ms = regex.Matches(priceString);
        if (ms.Count > 1)
          flat.Price = int.Parse($"{ms[0].Value}{ms[1].Value}000");
        else
          flat.Price = int.Parse($"{ms[0].Value}");


        #region Parse street

        string street = string.Empty;
        string number = string.Empty;
        string structure = string.Empty;

        street = apartaments[i].GetElementsByClassName("object--address")[0].TextContent.Trim().Replace("Санкт-Петербург, ", "").Replace("Санкт-Петербург г.", "");


        regex = new Regex(@"(\d+к\d+)");
        number = regex.Match(street).Value;
        if (!string.IsNullOrEmpty(number))
        {
          street = street.Replace(number, "");
          regex = new Regex(@"(к\d+)");
          structure = regex.Match(number).Value.Replace("к", "");
          number = number.Replace($"к{structure}", "");
        }
        else
        {
          regex = new Regex(@"(\d+\/\d+)");
          number = regex.Match(street).Value;
          if (!string.IsNullOrEmpty(number))
          {
            street = street.Replace(number, "");
            regex = new Regex(@"(\/\d+)");
            structure = regex.Match(number).Value.Replace(@"/", "");
            number = number.Replace($@"/{structure}", "");
          }
          else
          {
            regex = new Regex(@"(\d+\sк\.\d+)|(\d+к\.\d+)|(\d+\sк\.\s\d+)|(\d+к\.\s\d+)");
            number = regex.Match(street).Value;
            if (!string.IsNullOrEmpty(number))
            {
              street = street.Replace(number, "");
              number = number.Replace(" ", "");
              regex = new Regex(@"(к.\d+)");
              structure = regex.Match(number).Value.Replace(@"к.", "");
              number = number.Replace($@"к.{structure}", "");
            }
            else
            {
              regex = new Regex(@"(ул\.\s\d+$)|(ул\,\s\d+$)");
              number = regex.Match(street).Value;
              if (!string.IsNullOrEmpty(number))
              {
                street = street.Replace(number, "");
                number = number.Replace("ул. ", "").Replace("ул, ", "");
              }
              else
              {
                regex = new Regex(@"(ул\.,\sд\.\s\d+)$");
                number = regex.Match(street).Value;
                if (!string.IsNullOrEmpty(number))
                {
                  street = street.Replace(number, "");
                  number = number.Replace("ул., д. ", "");
                }
                else
                {
                  regex = new Regex(@"(пр\.\,\s\d+$)|(пр\.\s\d+$)$");
                  number = regex.Match(street).Value;
                  if (!string.IsNullOrEmpty(number))
                  {
                    street = street.Replace(number, "");
                    number = number.Replace("пр., ", "").Replace("пр. ", "");
                  }
                  else
                  {
                    regex = new Regex(@"(\,\s\d+$)");
                    number = regex.Match(street).Value;
                    if (!string.IsNullOrEmpty(number))
                    {
                      street = street.Replace(number, "");
                      number = number.Replace(", ", "");
                    }
                    else
                    {
                      regex = new Regex(@"(д\.\s\d+$)|(д\.\d+$)");
                      number = regex.Match(street).Value;
                      if (!string.IsNullOrEmpty(number))
                      {
                        street = street.Replace(number, "");
                        number = number.Replace(" ", "").Replace("д.", "");
                      }
                      else
                      {
                        regex = new Regex(@"(дом\s+\d+$)|(дом\d+$)");
                        number = regex.Match(street).Value;
                        if (!string.IsNullOrEmpty(number))
                        {
                          street = street.Replace(number, "");
                          number = number.Replace(" ", "").Replace("дом", "");
                        }
                        else
                        {
                          regex = new Regex(@"(пер\.\s+\d+)");
                          number = regex.Match(street).Value;
                          if (!string.IsNullOrEmpty(number))
                          {
                            street = street.Replace(number, "");
                            number = number.Replace("пер. ", "");
                          }
                          else
                          {
                            regex = new Regex(@"(наб\.\s+\d+)");
                            number = regex.Match(street).Value;
                            if (!string.IsNullOrEmpty(number))
                            {
                              street = street.Replace(number, "");
                              number = number.Replace("наб. ", "");
                            }
                            else
                            {
                              regex = new Regex(@"(пл\.\s+\d+)");
                              number = regex.Match(street).Value;
                              if (!string.IsNullOrEmpty(number))
                              {
                                street = street.Replace(number, "");
                                number = number.Replace("пл. ", "");
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

        string town = "";
        if (street.Contains("Колпино") || street.Contains("г. Колпино"))
        {
          town = "Колпино";
          street = street.Replace(town, "").Replace("г. Колпино", "");
        }
        else if (street.Contains("Песочный") || street.Contains("пос. Песочный"))
        {
          town = "Песочный";
          street = street.Replace(town, "").Replace("пос. Песочный", "");
        }
        else if (street.Contains("г. Кронштадт"))
        {
          town = "Кронштадт";
          street = street.Replace("г. Кронштадт", "");
        }
        else if (street.Contains("Парголово"))
        {
          town = "Парголово";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Красное Село г"))
        {
          town = "Красное Село г";
          street = street.Replace(town, "");
        }
        else if (street.Contains("г. Пушкин"))
        {
          town = "г. Пушкин";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Пушкин"))
        {
          town = "Пушкин";
          street = street.Replace(town, "");
        }

        else if (street.Contains("Шушары пос"))
        {
          town = "Шушары пос";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Шушары"))
        {
          town = "Шушары";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Металострой пос"))
        {
          town = "Металострой пос";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Металострой"))
        {
          town = "Металострой";
          street = street.Replace(town, "");
        }
        else
        {
          town = "Санкт-Петербург";
          street = street.Replace("СПб", "");
        }

        street = street.Replace("ул.", "").Replace("улица", "").Replace("пр-кт", "").Replace("проспект", "").Replace("наб", "").Replace("б-р", "").Replace("б-р/2", "").Replace("б-р/4", "").Replace("проезд", "").Replace("пр", "").Replace("шос к", "").Replace("бульвар", "").Replace(" б", "").Replace("  к", "").Replace("  д", "").Replace("пл", "").Replace(",", "").Replace(".", "").Trim();

        regex = new Regex(@"(\/А\d+А)");
        var str = regex.Match(street).Value;
        if (!string.IsNullOrEmpty(str))
          street = street.Replace(str, "");

        street = parseStreet.Execute(street, district);

        #endregion


        Monitor.Enter(locker);
        if (!string.IsNullOrWhiteSpace(flat.Building.Number))
        {
          using (var sw = new StreamWriter(new FileStream(FilenameSdam, FileMode.Open), Encoding.UTF8))
          {
            sw.BaseStream.Position = sw.BaseStream.Length;
            sw.WriteLine($@"{town};{flat.Building.Street};{flat.Building.Number};{flat.Building.Structure};{flat.Building.Liter};{flat.CountRoom};{flat.Square};{flat.Price};{ flat.Floor};{flat.Building.Metro};{flat.Building.Distance}");
          }
        }
        Monitor.Exit(locker);
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
