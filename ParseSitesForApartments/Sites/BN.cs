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
using ParseSitesForApartments.ParsClasses;
using ParseSitesForApartments.Export;
using ParseSitesForApartments.Export.Creators;
using ParseSitesForApartments.UnionWithBase;

namespace ParseSitesForApartments.Sites
{
  public class BN : BaseParse
  {
    #region Fields

    private static object locker = new object();
    private static object lockerDistrict = new object();
    private int minPage = 1;
    private int maxPage = 17;
    private Dictionary<int, string> district = new Dictionary<int, string>() { { 1, "Адмиралтейский" }, { 2, "Василеостровский" }, { 3, "Выборгский" }, { 5, "Калининский" }, { 4, "Кировский" }, { 16, "Колпинский" }, { 6, "Красногвардейский" }, { 7, "Красносельский" }, { 15, "Кронштадтский" }, { 17, "Курортный" }, { 8, "Московский" }, { 9, "Невский" }, { 10, "Петроградский" }, { 19, "Петродворцовый" }, { 11, "Приморский" }, { 20, "Пушкинский" }, { 12, "Фрунзенский" }, { 13, "Центральный" }, };
    private string filename = @"d:\ParserInfo\Appartament\BNProdam.csv";
    private CoreExport export;
    public delegate void Append(object sender, AppendFlatEventArgs e);
    public event Append OnAppend;
    private readonly UnionParseInfoWithDataBase unionInfo = new UnionParseInfoWithDataBase();

    #endregion

    #region Constructor
    
    public BN(List<District> listDistricts, List<Metro> lisMetro) : base(listDistricts, lisMetro)
    {
      CoreCreator creator = new CsvExportCreator();
      export = creator.FactoryCreate(Filename);
      //CoreCreator creator = new ExcelExportCreator();
      //export = creator.FactoryCreate(Filename);
      OnAppend += export.AddFlatInList;
    }

    #endregion

    #region Properties

    public override string Filename
    {
      get => filename;
      set => filename = value;
    }
    //public override string Filename => @"d:\ParserInfo\Appartament\BNProdam.xlsx";
    public override string FilenameSdam => @"d:\ParserInfo\Appartament\BNSdam.csv";
    public override string FilenameWithinfo => @"d:\ParserInfo\Appartament\BNProdamWithInfo.csv";
    public override string FilenameWithinfoSdam => @"d:\ParserInfo\Appartament\BNSdamWithInfo.csv";
    public override string NameSite => "БН";

    #endregion
    
    Thread studiiThread;
    Thread oneThread;
    Thread twoThread;
    Thread threeThread;
    Thread fourThread;
    Thread studiiThreadOld;
    Thread oneThreadOld;
    Thread twoThreadOld;
    Thread threeThreadOld;
    Thread fourThreadOld;

    public override void ParsingAll()
    {
      if (export is CsvExport)
      {
        using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Create), Encoding.UTF8))
        {
          //sw.WriteLine($@"Район;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние(км);URL");
          sw.WriteLine(@"Район;Улица;Номер;Корпус;Литер;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Дата постройки;Дата реконструкции;Даты кап. ремонты;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов;Расстояние пешком;Время пешком;Расстояние на машине;Время на машине;Откуда взято");
        }
      }
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

      Thread.Sleep(10000);
      var threadCheck = new Thread(CheckCloseThread);
      threadCheck.Start();
    }

    private void CheckCloseThread()
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
      //var excelExport = export as ExcelExport;
      //excelExport?.Save();
    }

    public void ParseStudiiOld()
    {
      var studiiThreadOld = new Thread(ChangeDistrictAndPage);
      studiiThreadOld.Start("Студия");
    }
    public void ParseOneRoomOld()
    {
      var oneThreadOld = new Thread(ChangeDistrictAndPage);
      oneThreadOld.Start("1 км. кв.");
    }
    public void ParseTwoRoomOld()
    {
      var twoThreadOld = new Thread(ChangeDistrictAndPage);
      twoThreadOld.Start("2 км. кв.");
    }
    public void ParseThreeRoomOld()
    {
      var threeThreadOld = new Thread(ChangeDistrictAndPage);
      threeThreadOld.Start("3 км. кв.");
    }
    public void ParseFourRoomOld()
    {
      var fourThreadOld = new Thread(ChangeDistrictAndPage);
      fourThreadOld.Start("4 км. кв.");
    }
    public void ParseStudii()
    {
      var studiiThread = new Thread(ChangeDistrictAndPage);
      studiiThread.Start("Студия Н");
    }
    public void ParseOneRoom()
    {
      var oneThread = new Thread(ChangeDistrictAndPage);
      oneThread.Start("1 км. кв. Н");
    }
    public void ParseTwoRoom()
    {
      var twoThread = new Thread(ChangeDistrictAndPage);
      twoThread.Start("2 км. кв. Н");
    }
    public void ParseThreeRoom()
    {
      var threeThread = new Thread(ChangeDistrictAndPage);
      threeThread.Start("3 км. кв. Н");
    }
    public void ParseFourRoom()
    {
      var fourThread = new Thread(ChangeDistrictAndPage);
      fourThread.Start("4 км. кв. Н");
    }

    private void ChangeDistrictAndPage(object typeRoom)
    {
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
      try
      {
        var responce = webClient.DownloadString(url);
        var document = parser.Parse(responce);
        ParseSheet(typeRoom, document, district);
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

    private void ParseSheet(string typeRoom, IHtmlDocument document, District district)
    {
      var apartaments = document.GetElementsByClassName("object--item");
      var urlElems = document.GetElementsByClassName("object--link-to-object button_mode_one");
      var parseStreet = new ParseStreet();

      for (int i = 0; i < apartaments.Length; i++)
      {
        var flat = new Flat
        {
          Url = $"https://www.bn.ru{urlElems[i].GetAttribute("href")}"
        };
        if (apartaments[i].GetElementsByClassName("object__square").Length > 0)
          flat.Square = apartaments[i].GetElementsByClassName("object__square")[0].TextContent.Trim().Replace(".", ",");
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
          flat.Price = int.Parse($"{ms[0].Value}{ms[1].Value}000");
        else
          flat.Price = int.Parse($"{ms[0].Value}000");

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

        #region Parse Floor

        if (apartaments[i].GetElementsByClassName("object--floor").Length > 0)
        {
          var floor = apartaments[i].GetElementsByClassName("object--floor")[0].TextContent;
          regex = new Regex(@"(\d+)");
          var mas = regex.Matches(floor);
          if (mas.Count > 0)
            flat.Floor = mas[0].Value;
        }

        #endregion
        if (!string.IsNullOrWhiteSpace(flat.Building.Number))
        {
          if (!string.IsNullOrWhiteSpace(flat.Square))
          {
            Monitor.Enter(locker);
            if (string.IsNullOrWhiteSpace(flat.Building.DateBuild))
            {
              unionInfo.UnionInfoProdam(flat);
            }
            OnAppend(this, new AppendFlatEventArgs { Flat = flat });
            Monitor.Exit(locker);
          }
        }
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
              ListDistricts.Where(x => x.Name.ToLower() == distr.Value.ToLower()).First()))
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
          flat.Square = apartaments[i].GetElementsByClassName("object__square")[0].TextContent.Trim();
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

    int count = 0;
  }
}
