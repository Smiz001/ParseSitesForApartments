using AngleSharp.Html.Parser;
using Core.Export;
using Core.Export.Creators;
using Core.MainClasses;
using Core.Proxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;
using AngleSharp.Dom;
using System.Threading;

namespace Core.Sites
{
  public class CIAN:BaseParse
  {
    #region Fields
    public delegate void Append(object sender, AppendFlatEventArgs e);
    public event Append OnAppend;
    private CoreExport export;
    private Dictionary<string, string> district = new Dictionary<string, string>()
    {
      {"Адмиралтейский", "admiralteyskiy-04150"}, {"Василеостровский", "vasileostrovskiy-04149"}, {"Выборгский", "vyborgskiy-04148"}, {"Калининский", "kalininskiy-04147"}, {"Кировский", "kirovskiy-04146"},{"Колпинский","kolpinskiy-04145" },
      {"Красногвардейский", "krasnogvardeyskiy-04144"}, {"Красносельский", "krasnoselskiy-04143"},{"Кронштадтский", "kronshtadtskiy-04142" },{"Курортный ","kurortnyy-04141" }, {"Московский", "moskovskiy-04140"}, {"Невский", "nevskiy-04139"}, {"Петроградский", "petrogradskiy-04138"},{ "Петродворцовый ", "petrodvorcovyy-04137"},
      {"Приморский", "primorskiy-04136"},{"Пушкинский ","pushkinskiy-04135" }, {"Фрунзенский", "frunzenskiy-04134"}, {"Центральный", "centralnyy-04133"},
    };
    private int firstPage = 0;
    private int lastPage = 100;
    #endregion

    #region Constructors
    public CIAN(List<District> listDistricts, List<Metro> listMetros, List<ProxyInfo> listProxy) : base(listDistricts,
      listMetros, listProxy)
    {
    }
    #endregion

    #region Properties
    public override string NameSite => throw new NotImplementedException();
    public override string Filename { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public override string FilenameSdam => throw new NotImplementedException();

    public override string FilenameWithinfo => throw new NotImplementedException();

    public override string FilenameWithinfoSdam => throw new NotImplementedException();
    #endregion

    #region Methods
    public override void ParsingAll()
    {
      throw new NotImplementedException();
    }

    public override void ParsingFour()
    {
      throw new NotImplementedException();
    }

    public override void ParsingMoreFour()
    {
      throw new NotImplementedException();
    }

    public override void ParsingOne()
    {
      throw new NotImplementedException();
    }

    public override void ParsingSdamAll()
    {
      throw new NotImplementedException();
    }

    public override void ParsingStudii()
    {
      throw new NotImplementedException();
    }

    public override void ParsingThree()
    {
      throw new NotImplementedException();
    }

    public override void ParsingTwo()
    {
      throw new NotImplementedException();
    }

    protected override void CalcAverPrice()
    {
      throw new NotImplementedException();
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
            sw.WriteLine(@"Район;Улица;Номер;Корпус;Литер;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Дата постройки;Дата реконструкции;Даты кап. ремонты;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов;Расстояние пешком, м;Время пешком;Расстояние на машине, м;Время на машине;Откуда взято;Тип дома;Проводился кап.ремонт");
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
          for (int i = firstPage; i < lastPage; i++)
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
        var document = parser.ParseDocument(responce);
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

    }
    #endregion
  }
}
