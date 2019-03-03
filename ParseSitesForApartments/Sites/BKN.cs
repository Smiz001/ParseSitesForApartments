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
using ParseSitesForApartments.ParsClasses;
using ParseSitesForApartments.Export.Creators;
using ParseSitesForApartments.Proxy;
using ParseSitesForApartments.UI;
using ParseSitesForApartments.UnionWithBase;

namespace ParseSitesForApartments.Sites
{
  public class BKN : BaseParse
  {
    #region Fiends

    private int minPage = 1;
    private int maxPage = 100;
    private const string Apartaments = "main Apartments";
    private const string NewApartaments = "main NewApartment";
    private static object locker = new object();
    private static object lockerDistrict = new object();
    private Thread studiiThread;
    private Thread oneThread;
    private Thread twoThread;
    private Thread threeThread;
    private Thread fourThread;
    private Thread fiveThread;
    private Thread studiiRentThread;
    private Thread oneRentThread;
    private Thread twoRentThread;
    private Thread threeRentThread;
    private Thread fourRentThread;
    private Thread fiveRentThread;
    private ProgressForm progress;
    private int count = 1;
    private Dictionary<string, string> districts = new Dictionary<string, string>() { { "admiralteiskii", "Адмиралтейский" }, { "vasileostrovskii", "Василеостровский" }, { "viborgskii", "Выборгский" }, { "kalininskii", "Калининский" }, { "kirovskii", "Кировский" }, { "kolpinskii", "Колпинский" }, { "krasnogvardeiskii", "Красногвардейский" }, { "krasnoselskii", "Красносельский" }, { "kronshtadtskii", "Кронштадтский" }, { "kurortnii", "Курортный" }, { "moskovskii", "Московский" }, { "nevskii", "Невский" }, { "petrogradskii", "Петроградский" }, { "petrodvorcovii", "Петродворцовый" }, { "primorskii", "Приморский" }, { "pushkinskii", "Пушкинский" }, { "frunzenskii", "Фрунзенский" }, { "centralnii", "Центральный" }, };

    private CoreExport export;
    public delegate void Append(object sender, AppendFlatEventArgs e);
    public event Append OnAppend;
    private readonly UnionParseInfoWithDataBase unionInfo = new UnionParseInfoWithDataBase();
    private string filename = @"d:\ParserInfo\Appartament\BKNProdam.csv";

    #endregion

    #region Constructor

    public BKN(List<District> listDistricts, List<Metro> listMetros, List<ProxyInfo> listProxy) : base(listDistricts, listMetros, listProxy)
    {
      //CoreCreator creator = new CsvExportCreator();
      //export = creator.FactoryCreate(Filename);
      //OnAppend += export.AddFlatInList;
    }

    #endregion

    #region Properties

    public override string Filename
    {
      get => filename;
      set => filename = value;
    }
    public override string FilenameSdam => @"d:\ParserInfo\Appartament\BKNSdam.csv";
    public override string FilenameWithinfo => @"d:\ParserInfo\Appartament\BKNProdamWithInfo.csv";
    public override string FilenameWithinfoSdam => @"d:\ParserInfo\Appartament\BKNSdamWithInfo.csv";
    public override string NameSite => "БКН";

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
            //sw.WriteLine($@"Район;Улица;Номер;Корпус;Литера;Кол-во комнат;Площадь;Цена;Этаж;Метро;Расстояние(км);URL");
            sw.WriteLine(@"Район;Улица;Номер;Корпус;Литер;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Дата постройки;Дата реконструкции;Даты кап. ремонты;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов;Расстояние пешком;Время пешком;Расстояние на машине;Время на машине;Откуда взято");
          }
        }
      }
    }

    public override void ParsingAll()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("БКН Все квартиры");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                studiiThread = new Thread(ChangeDistrictAndPage);
                studiiThread.Start("Студия");
                oneThread = new Thread(ChangeDistrictAndPage);
                oneThread.Start("1 км. кв.");
                twoThread = new Thread(ChangeDistrictAndPage);
                twoThread.Start("2 км. кв.");
                threeThread = new Thread(ChangeDistrictAndPage);
                threeThread.Start("3 км. кв.");
                fourThread = new Thread(ChangeDistrictAndPage);
                fourThread.Start("4 км. кв.");
                fiveThread = new Thread(ChangeDistrictAndPage);
                fiveThread.Start("5 км. кв.");

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
                      break;
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
            break;
          }
        }
      }
    }

    public override void ParsingStudii()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("БКН Студии");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                studiiThread = new Thread(ChangeDistrictAndPage);
                studiiThread.Start("Студия");

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
        studiiRentThread = new Thread(ChangeDistrictAndPage);
        studiiRentThread.Start("Студия");
      }
    }

    public override void ParsingOne()
    {
      CreateExport();
      if (TypeParseFlat == TypeParseFlat.Sale)
      {
        progress = new ProgressForm("БКН 1 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                oneThread = new Thread(ChangeDistrictAndPage);
                oneThread.Start("1 км. кв.");

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
        progress = new ProgressForm("БКН 2 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                twoThread = new Thread(ChangeDistrictAndPage);
                twoThread.Start("2 км. кв.");

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
        progress = new ProgressForm("БКН 3 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                threeThread = new Thread(ChangeDistrictAndPage);
                threeThread.Start("3 км. кв.");

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
        progress = new ProgressForm("БКН 4 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                fourThread = new Thread(ChangeDistrictAndPage);
                fourThread.Start("4 км. кв.");

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
        progress = new ProgressForm("БКН 5 км. кв.");
        var threadbackground = new Thread(
          new ThreadStart(() =>
            {
              try
              {
                fiveThread = new Thread(ChangeDistrictAndPage);
                fiveThread.Start("5 км. кв.");

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
      }
    }

    private void ChangeDistrictAndPage(object typeRoom)
    {
      HtmlParser parser = new HtmlParser();
      using (var webClient = new WebClient())
      {
        webClient.Encoding = Encoding.UTF8;
        string url = "";
        foreach (var distr in districts)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            switch (typeRoom)
            {
              case "Студия":
                url =
                  $@"https://www.bkn.ru/prodazha/vtorichka/studii/{distr.Key}-raion?page={i}";
                break;
              case "1 км. кв.":
                url =
                  $@"https://www.bkn.ru/prodazha/vtorichka/odnokomnatnye-kvartiry/{distr.Key}-raion?page={i}";
                break;
              case "2 км. кв.":
                url =
                  $@"https://www.bkn.ru/prodazha/vtorichka/dvuhkomnatnye-kvartiry/{distr.Key}-raion?page={i}";
                break;
              case "3 км. кв.":
                url =
                  $@"https://www.bkn.ru/prodazha/vtorichka/trehkomnatnye-kvartiry/{distr.Key}-raion?page={i}";
                break;
              case "4 км. кв.":
                url =
                  $@"https://www.bkn.ru/prodazha/vtorichka/chetyrehkomnatnye-kvartiry/{distr.Key}-raion?page={i}";
                break;
              case "5 км. кв.":
                url =
                  $@"https://www.bkn.ru/prodazha/vtorichka/pyatikomnatnye-kvartiry/{distr.Key}-raion?page={i}";
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

    private void ChangeDistrictAndPageForRent(object typeRoom)
    {
      HtmlParser parser = new HtmlParser();
      using (var webClient = new WebClient())
      {
        webClient.Encoding = Encoding.UTF8;
        string url = "";
        foreach (var distr in districts)
        {
          for (int i = minPage; i < maxPage; i++)
          {
            switch (typeRoom)
            {
              case "Студия":
                url =
                  $@"https://www.bkn.ru/arenda/vtorichka/studii/{distr.Key}-raion?page={i}";
                break;
              case "1 км. кв.":
                url =
                  $@"https://www.bkn.ru/arenda/vtorichka/odnokomnatnye-kvartiry/{distr.Key}-raion?page={i}";
                break;
              case "2 км. кв.":
                url =
                  $@"https://www.bkn.ru/arenda/vtorichka/dvuhkomnatnye-kvartiry/{distr.Key}-raion?page={i}";
                break;
              case "3 км. кв.":
                url =
                  $@"https://www.bkn.ru/arenda/vtorichka/trehkomnatnye-kvartiry/{distr.Key}-raion?page={i}";
                break;
              case "4 км. кв.":
                url =
                  $@"https://www.bkn.ru/arenda/vtorichka/chetyrehkomnatnye-kvartiry/{distr.Key}-raion?page={i}";
                break;
              case "5 км. кв.":
                url =
                  $@"https://www.bkn.ru/arenda/vtorichka/pyatikomnatnye-kvartiry/{distr.Key}-raion?page={i}";
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
        Log.Debug("-----------URL-----------");
        Log.Debug(url);
        var responce = webClient.DownloadString(url);
        var document = parser.Parse(responce);
        var col = document.GetElementsByClassName(Apartaments);
        if (col.Length == 0)
          return false;
        ParseSheet(typeRoom, col, district);
        return true;
      }
      catch (Exception e)
      {
        //TODO Если страница долго не отвечает то пропускаем ее
        Thread.Sleep(1000);
        return true;
      }
    }

    private void ParseSheet(string typeRoom, IHtmlCollection<IElement> collection, District district)
    {
      var parseStreet = new ParseStreet();
      for (int j = 0; j < collection.Length; j++)
      {
        string town = string.Empty;
        var flat = new Flat();
        flat.CountRoom = typeRoom;

        string street = string.Empty;
        string number = string.Empty;
        string structure = string.Empty;
        string liter = string.Empty;
        string metro = string.Empty;
        string distance = string.Empty;

        var priceDiv = collection[j].GetElementsByClassName("price overflow");
        //if (priceDiv.Length == 0)
        //  break;
        //else
        //{
        var regex = new Regex(@"(\d+\,\d+\sм2)|(\d+\sм2)");
        var title = collection[j].GetElementsByClassName("title")[0].TextContent;
        flat.Square = regex.Match(title).Value.Replace(".", ",").Replace("м2", "");
        if (typeRoom == "5 км. кв.")
        {
          regex = new Regex(@"(\d\-)");
          flat.CountRoom = regex.Match(title).Value.Replace("-", " км. кв.");
        }

        //Get URL
        var elems = collection[j].GetElementsByClassName("col-xs-7 nopadding");
        if (elems.Length > 0)
        {
          elems = elems[0].GetElementsByClassName("name");
          if (elems.Length > 0)
          {
            elems = elems[0].GetElementsByTagName("a");
            var elem = elems[0];
            flat.Url = $@"https://www.bkn.ru{elem.GetAttribute("href")}";
          }
        }

        regex = new Regex(@"(\d+)");
        var ms = regex.Matches(priceDiv[0].TextContent);
        if (ms.Count == 3)
        {
          var price = int.Parse($"{ms[0].Value}{ms[1].Value}{ms[2].Value}");
          flat.Price = price;
        }
        else if (ms.Count == 2)
        {
          var price = int.Parse($"{ms[0].Value}{ms[1].Value}");
          flat.Price = price;
        }

        regex = new Regex(@"(\d+)");
        ms = regex.Matches(collection[j].GetElementsByClassName("floor overflow")[0].TextContent);
        if (ms.Count > 0)
          flat.Floor = ms[0].Value;
        else
          flat.Floor = "";

        street = collection[j].GetElementsByClassName("overflow")[3].TextContent;

        if (street.Contains("Заводская"))
        {
          string a = "";
        }

        if (street.Contains("Сестрорецк г."))
        {
          town = "Сестрорецк г.";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Шушары пос."))
        {
          town = "Шушары пос.";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Петергоф г."))
        {
          town = "Петергоф г.";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Пушкин г."))
        {
          town = "Пушкин г.";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Зеленогорск г."))
        {
          town = "Зеленогорск г.";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Металлострой пос."))
        {
          town = "Металлострой пос.";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Колпино г."))
        {
          town = "Колпино г.";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Парголово пос."))
        {
          town = "Парголово пос.";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Красное Село г."))
        {
          town = "Красное Село г.";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Понтонный пос"))
        {
          town = "Понтонный пос";
          street = street.Replace(town, "");
        }
        else if (street.Contains("г. Петергоф"))
        {
          town = "Петергоф г.";
          street = street.Replace("г. Петергоф", "");
        }
        else if (street.Contains("Ломоносов г."))
        {
          town = "Ломоносов г.";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Стрельна г."))
        {
          town = "Стрельна г.";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Павловск г."))
        {
          town = "Павловск г.";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Кронштадт г."))
        {
          town = "Кронштадт г.";
          street = street.Replace(town, "");
        }
        else if (street.Contains("Санкт-Петербург г."))
        {
          town = "Санкт-Петербург г.";
          street = street.Replace(town, "");
        }
        else
          town = "Санкт-Петербург г.";

        regex = new Regex(@"(д\.\s+\d+\,\s+к\.\s+\d+)");
        number = regex.Match(street).Value;

        if (!string.IsNullOrWhiteSpace(number))
        {
          street = street.Replace(number, "");
          regex = new Regex(@"(к\.\s+\d+)");
          structure = regex.Match(number).Value;
          number = number.Replace(structure, "");

          structure = structure.Replace("к.", "").Trim();
          number = number.Replace("д.", "").Replace(",", "").Trim();
        }
        else
        {
          regex = new Regex(@"(д\.\s+\d+К\d+)");
          number = regex.Match(street).Value;

          if (string.IsNullOrWhiteSpace(number))
          {
            regex = new Regex(@"(прд\.\,\d+)");
            number = regex.Match(street).Value;
            if (string.IsNullOrWhiteSpace(number))
            {
              regex = new Regex(@"(д\.\d+\s+к\.\d+)");
              number = regex.Match(street).Value;

              if (string.IsNullOrWhiteSpace(number))
              {
                regex = new Regex(@"(д\.\s+\d+$)");
                number = regex.Match(street).Value;

                if (string.IsNullOrWhiteSpace(number))
                {
                  regex = new Regex(@"(д\.\s+\d+\/\d+)");
                  number = regex.Match(street).Value;
                  if (string.IsNullOrWhiteSpace(number))
                  {
                    regex = new Regex(@"(д\.\d+\,\s+к\.\d+)");
                    number = regex.Match(street).Value;
                    if (string.IsNullOrWhiteSpace(number))
                    {
                      regex = new Regex(@"(д\.\s+\d+\s+лит\.\s+\D)");
                      number = regex.Match(street).Value;
                      if (string.IsNullOrWhiteSpace(number))
                      {
                        regex = new Regex(@"(д\.\d+\s+К\,\s+к\.\d+)");
                        number = regex.Match(street).Value;
                        if (string.IsNullOrWhiteSpace(number))
                        {
                          regex = new Regex(@"(д\.\d+)");
                          number = regex.Match(street).Value;
                          if (string.IsNullOrWhiteSpace(number))
                          {
                            regex = new Regex(@"(\d+\s+к\.\d+)");
                            number = regex.Match(street).Value;
                            if (string.IsNullOrWhiteSpace(number))
                            {
                              regex = new Regex(@"(д\.\s+\d+\D\s+к\.\s+\d+)");
                              number = regex.Match(street).Value;
                              if (string.IsNullOrWhiteSpace(number))
                              {
                                regex = new Regex(@"(д\.\s+\d+)");
                                number = regex.Match(street).Value;
                                if (string.IsNullOrWhiteSpace(number))
                                {
                                }
                                else
                                {
                                  street = street.Replace(number, "");
                                  number = number.Replace("д.", "").Trim();
                                }
                              }
                              else
                              {
                                street = street.Replace(number, "");
                                regex = new Regex(@"(к\.\s+\d+)");
                                structure = regex.Match(number).Value;
                                number = number.Replace(structure, "").Trim();
                                regex = new Regex(@"(д\.\s+\d+)");
                                var num = regex.Match(number).Value;
                                liter = number.Replace(num, "");
                                number = num.Replace("д.", "").Trim();
                              }
                            }
                            else
                            {
                              street = street.Replace(number, "");
                              regex = new Regex(@"(к\.\d+)");
                              structure = regex.Match(number).Value;
                              number = number.Replace(structure, "").Trim();
                              structure = structure.Replace("к.", "");
                            }
                          }
                          else
                          {
                            street = street.Replace(number, "");
                            number = number.Replace("д.", "");
                          }
                        }
                        else
                        {
                          street = street.Replace(number, "");
                          regex = new Regex(@"(к\.\d+)");
                          structure = regex.Match(number).Value;
                          number = number.Replace(structure, "").Trim();
                          structure = structure.Replace("к.", "");
                          regex = new Regex(@"(д\.\d+)");
                          number = regex.Match(number).Value.Replace("д.", "").Trim();
                        }
                      }
                      else
                      {
                        street = street.Replace(number, "");
                        regex = new Regex(@"(лит\.\s+\D)");
                        liter = regex.Match(number).Value;
                        number = number.Replace(liter, "").Replace("д.", "").Trim();
                        liter = liter.Replace("лит.", "").Trim();
                      }
                    }
                    else
                    {
                      street = street.Replace(number, "");
                      regex = new Regex(@"(к\.\d+)");
                      structure = regex.Match(number).Value;
                      number = number.Replace(structure, "").Replace("д.", "").Replace(",", "").Trim();
                      structure = structure.Replace("к.", "");
                    }

                  }
                  else
                  {
                    street = street.Replace(number, "");
                    var arr = number.Split('/');
                    number = arr[0].Replace("д.", "").Trim();
                    structure = arr[1];
                  }
                }
                else
                {
                  street = street.Replace(number, "");
                  number = number.Replace("д.", "").Trim();
                }
              }
              else
              {
                street = street.Replace(number, "");
                regex = new Regex(@"(к\.\d+)");
                structure = regex.Match(number).Value;
                number = number.Replace(structure, "").Replace("д.", "").Trim();
                structure = structure.Replace("к.", "");
              }
            }
            else
            {
              street = street.Replace(number, "");
              number = number.Replace("прд.,", "");
            }
          }
          else
          {
            street = street.Replace(number, "");
            regex = new Regex(@"(К\d+)");
            structure = regex.Match(number).Value;
            number = number.Replace(structure, "").Replace("д.", "");
            structure = structure.Replace("К", "");
          }
        }

        metro = collection[j].GetElementsByClassName("subwaystring")[0].TextContent;

        regex = new Regex(@"(\d+\sмин.\sна\sтранспорте)|(\d+\sмин\.\sпешком)");
        distance = regex.Match(metro).Value;

        street = street.Replace("ул.", "").Replace("просп.", "").Replace("пр-кт", "").Replace("пер.", "").Replace("шос.", "").Replace("пр.", "").Replace("лит. а", "").Replace("лит. А", "").Replace("стр. 3", "").Replace("стр. 1", "").Replace("стр. 2", "").Replace("б-р.", "").Replace(" б", "").Replace("пр-д", "").Replace("тер.", "").Replace("пл.", "").Replace(",", "").Replace(".", "").Replace("-1", "").Trim();


        regex = new Regex(@"(к\d+)");
        var struc = regex.Match(street).Value;
        if (!string.IsNullOrEmpty(struc))
        {
          structure = struc.Replace("к", "");
          street = street.Replace(struc, "");
        }

        regex = new Regex(@"(кП\d+)");
        var kp = regex.Match(street).Value;
        if (!string.IsNullOrEmpty(kp))
          street = street.Replace(kp, "").Trim();

        if (!string.IsNullOrWhiteSpace(distance))
          metro = metro.Replace(distance, "").Replace("●", "").Replace(",", "").Trim();

        regex = new Regex(@"(\d+ пеш)");
        var forDelete = regex.Match(metro).Value;
        if (!string.IsNullOrEmpty(forDelete))
        {
          metro = metro.Replace(forDelete, "");
        }

        regex = new Regex(@"(\d+ тр)");
        forDelete = regex.Match(metro).Value;
        if (!string.IsNullOrEmpty(forDelete))
        {
          metro = metro.Replace(forDelete, "");
        }

        regex = new Regex(@"(\d+ ост, \d+ остановок)");
        forDelete = regex.Match(metro).Value;
        if (!string.IsNullOrEmpty(forDelete))
        {
          metro = metro.Replace(forDelete, "");
        }
        metro = metro.Replace("Пл.", "").Replace("пр.", "").Replace("Пр.", "").Replace("●", "").Replace("Пл. А.", "").Trim();

        if (string.IsNullOrWhiteSpace(number))
        {
          regex = new Regex(@"(д\s+\d+\s+корпус\s+\d+)");
          number = regex.Match(street).Value;
          if (string.IsNullOrEmpty(number))
          {
            regex = new Regex(@"(д\s+\d+)");
            number = regex.Match(street).Value;
            if (string.IsNullOrWhiteSpace(number))
            {
            }
            else
            {
              street = street.Replace(number, "").Trim();
              number = number.Replace("д", "").Trim();
            }
          }
          else
          {
            street = street.Replace(number, "").Trim();
            regex = new Regex(@"(корпус\s+\d+)");
            structure = regex.Match(number).Value.Replace("корпус", "");
            number = number.Replace($"корпус{structure}", "").Replace("д", "").Trim();
          }
        }

        street = parseStreet.Execute(street, district);

        Building building = null;
        Monitor.Enter(lockerDistrict);
        if (district.Buildings.Count != 0)
        {
          IEnumerable<Building> bldsEnum;
          if (string.IsNullOrEmpty(liter))
            bldsEnum = district.Buildings.Where(x => x.Street == street && x.Number == number && x.Structure == structure);
          else
            bldsEnum = district.Buildings.Where(x => x.Street == street && x.Number == number && x.Structure == structure && x.Liter == liter);

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
            Liter = liter
          };
          district.Buildings.Add(building);
        }
        Monitor.Exit(lockerDistrict);

        if (building.MetroObj == null)
        {
          var metroObjEnum = ListMetros.Where(x => x.Name.ToUpper().Contains(metro.ToUpper()));
          if (metroObjEnum.Count() > 0)
          {
            building.MetroObj = metroObjEnum.First();
          }
        }
        flat.Building = building;


        if (!string.IsNullOrWhiteSpace(flat.Building.Number))
        {
          if (!string.IsNullOrWhiteSpace(flat.Square))
          {
            if (!string.IsNullOrWhiteSpace(flat.Building.Street))
            {
              Monitor.Enter(locker);
              if (string.IsNullOrWhiteSpace(flat.Building.DateBuild))
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

        //Monitor.Enter(locker);
        //if (string.IsNullOrWhiteSpace(flat.Building.DateBuild))
        //{
        //  unionInfo.UnionInfoProdam(flat);
        //}
        //OnAppend(this, new AppendFlatEventArgs { Flat = flat });
        //Monitor.Exit(locker);

        //flat.Building.Street = parseStreet.Execute()
        //  Monitor.Enter(locker);
        //  progress.UpdateProgress(count);
        //  count++;
        //  if (!string.IsNullOrEmpty(flat.Building.Number))
        //  {
        //    using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Open), Encoding.UTF8))
        //    {
        //      sw.BaseStream.Position = sw.BaseStream.Length;
        //      sw.WriteLine($@"{town};{flat.Building.Street};{flat.Building.Number};{flat.Building.Structure};{flat.Building.Liter};{flat.CountRoom};{flat.Square};{flat.Price};{flat.Floor};{flat.Building.Metro};{flat.Building.Distance}");
        //    }
        //  }
        //  Monitor.Exit(locker);
        ////}
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
                //ParseSheet("Студия Н", newApartment);
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
                    // ParseSheet("1 км. кв. Н", newApartment);
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
                //ParseSheet("2 км. кв. Н", newApartment);
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
                // ParseSheet("3 км. кв. Н", newApartment);
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
