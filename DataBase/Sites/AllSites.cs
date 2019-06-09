using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Core.Connections;
using Core.MainClasses;
using Core.Proxy;

namespace Core.Sites
{
  public class AllSites:BaseParse
  {
    #region Fileds
    
    private string filename = @"d:\ParserInfo\Appartament\AllSites.csv";
    private BKN bkn;
    private BN bn;
    private ELMS elms;
    private Avito avito;
    private Thread elmsThread;
    private Thread bnThread;
    private Thread bknThread;
    private Thread avitoThread;

    #endregion

    #region Constructor

    public AllSites(List<District> listDistricts, List<Metro> listMetros, List<ProxyInfo> listProxy) : base(listDistricts, listMetros, listProxy)
    {
      bkn = new BKN(listDistricts, listMetros, listProxy);
      bn = new BN(listDistricts, listMetros, listProxy);
      elms = new ELMS(listDistricts, listMetros, listProxy);
      avito = new Avito(listDistricts, listMetros, listProxy);

      bkn.Filename = filename;
      bn.Filename = filename;
      elms.Filename = filename;
      avito.Filename = filename;
    }

    #endregion

    #region Properties

    public override string Filename
    {
      get => filename;
      set
      {
        filename = value;
        bkn.Filename = filename;
        bn.Filename = filename;
        elms.Filename = filename;
        avito.Filename = filename;
      }
    }

    public override string FilenameSdam { get; }
    public override string FilenameWithinfo { get; }
    public override string FilenameWithinfoSdam { get; }
    public override string NameSite { get; }

    #endregion

    public override void ParsingSdamAll()
    {
      throw new NotImplementedException();
    }

    private string ExctractPath()
    {
      string path = string.Empty;
      var arr = Filename.Split('\\');
      path = Filename.Replace(arr[arr.Length-1], "");
      return path;
    }

    private void WaitUntilWorkThread(Thread thread)
    {
      while(thread.IsAlive)
      {
        Thread.Sleep(10000);
      }
    }

    private void UnionFiles()
    {
      using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Create), Encoding.UTF8))
      {
        sw.WriteLine(@"Район;Улица;Номер;Корпус;Литер;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Дата постройки;Дата реконструкции;Даты кап. ремонты;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов;Расстояние пешком, м;Время пешком;Расстояние на машине, м;Время на машине;Откуда взято;Тип дома;Проводился кап.ремонт");
        using (var sr = new StreamReader(elms.Filename))
        {
          sr.ReadLine();
          string line;
          while ((line = sr.ReadLine()) != null)
          {
            sw.WriteLine(line);
          }
        }
        File.Delete(elms.Filename);
        using (var sr = new StreamReader(bn.Filename))
        {
          sr.ReadLine();
          string line;
          while ((line = sr.ReadLine()) != null)
          {
            sw.WriteLine(line);
          }
        }
        File.Delete(bn.Filename);
        using (var sr = new StreamReader(bkn.Filename))
        {
          sr.ReadLine();
          string line;
          while ((line = sr.ReadLine()) != null)
          {
            sw.WriteLine(line);
          }
        }
        File.Delete(bkn.Filename);
      }
      ReadAllPriceForAllRoom(Filename, DateTime.Now);
      ReadPriceForMetroByRoom(Filename, DateTime.Now);
      ReadPriceForDistrictByRoom(Filename, DateTime.Now);
    }
    private void ReadAllPriceForAllRoom(string path, DateTime date)
    {
      using (var sr = new StreamReader(path))
      {
        var dic = new Dictionary<string, List<double>>();
        string line = sr.ReadLine();
        while ((line = sr.ReadLine()) != null)
        {
          var arr = line.Split(';');
          var typeRoom = arr[5];
          List<double> list = null;
          if (!dic.ContainsKey(typeRoom))
          {
            list = new List<double>();
            dic.Add(typeRoom, list);
          }
          else
          {
            list = dic[typeRoom];
          }
          list.Add(double.Parse(arr[9]) / double.Parse(arr[6]));
        }
        var con = ConnetionToSqlServer.Default();
        var insert = $@"insert into dbo.AverPriceForTypeRoom (TypeRoom, AverPrice, Date) values ";
        var countDic = dic.Count;
        int i = 1;
        foreach (var item in dic)
        {
          insert += $@"('{item.Key}', {item.Value.Average().ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US"))}, '{date.Year}-{date.Month}-{date.Day}')";
          if (i != countDic)
          {
            insert += ", ";
            i++;
          }
        }
        con.ExecuteNonQuery(insert);
      }
    }

    private void ReadPriceForMetroByRoom(string path, DateTime date)
    {
      var dic = new Dictionary<string, Dictionary<Metro, List<double>>>();
      using (var sr = new StreamReader(path))
      {
        string line = sr.ReadLine();
        while ((line = sr.ReadLine()) != null)
        {
          var arr = line.Split(';');
          var typeRoom = arr[5];
          Dictionary<Metro, List<double>> selectedDic;
          if (!dic.ContainsKey(typeRoom))
          {
            selectedDic = new Dictionary<Metro, List<double>>();
            dic.Add(typeRoom, selectedDic);
            foreach (var met in ListMetros)
            {
              selectedDic.Add(met, new List<double>());
            }
          }
          else
          {
            selectedDic = dic[typeRoom];
          }
          var metros = ListMetros.Where(x => x.Name == arr[10].Trim());
          if (metros.Count() > 0)
          {
            var metro = metros.First();
            if (selectedDic.ContainsKey(metro))
            {
              selectedDic[metro].Add(double.Parse(arr[9]) / double.Parse(arr[6]));
            }
          }
        }
      }

      var con = ConnetionToSqlServer.Default();
      foreach (var d in dic)
      {
        var insert = $@"insert into dbo.AverPriceForTypeRoomByMetro (TypeRoom, Metro, Date, AverPrice) values ";
        var count = d.Value.Count();
        int i = 1;
        foreach (var item in d.Value)
        {
          if (item.Value.Count > 0)
          {
            insert += $@"('{d.Key}', '{item.Key.Id}', '{date.Year}-{date.Month}-{date.Day}', {item.Value.Average().ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US"))})";
            if (i != count)
              insert += ", ";
          }
          i++;
        }
        if (insert[insert.Length - 2] == ',')
          insert = insert.Remove(insert.Length - 2, 2);
        con.ExecuteNonQuery(insert);
      }
    }

    private void ReadPriceForDistrictByRoom(string path, DateTime date)
    {
      var dic = new Dictionary<string, Dictionary<District, List<double>>>();
      using (var sr = new StreamReader(path))
      {
        string line = sr.ReadLine();
        while ((line = sr.ReadLine()) != null)
        {
          var arr = line.Split(';');
          var typeRoom = arr[5];
          Dictionary<District, List<double>> selectedDic;
          if (!dic.ContainsKey(typeRoom))
          {
            selectedDic = new Dictionary<District, List<double>>();
            dic.Add(typeRoom, selectedDic);
            foreach (var dis in ListDistricts)
            {
              selectedDic.Add(dis, new List<double>());
            }
          }
          else
            selectedDic = dic[typeRoom];

          var dist = ListDistricts.Where(x => x.Name == arr[0].Trim());
          if (dist.Count() > 0)
          {
            var dis = dist.First();
            if (selectedDic.ContainsKey(dis))
            {
              selectedDic[dis].Add(double.Parse(arr[9]) / double.Parse(arr[6]));
            }
          }
        }
      }

      var con = ConnetionToSqlServer.Default();
      foreach (var d in dic)
      {
        var insert = @"insert into [ParseBulding].[dbo].[AverPriceForTypeRoomByDistrict] (TypeRoom, District, Date, AverPrice)
values";
        var count = d.Value.Count();
        int i = 1;
        foreach (var item in d.Value)
        {
          if (item.Value.Count > 0)
          {
            insert += $@"('{d.Key}', '{item.Key.Id}', '{date.Year}-{date.Month}-{date.Day}', {item.Value.Average().ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US"))})";
            if (i != count)
              insert += ", ";
          }
          i++;
        }
        if (insert[insert.Length - 2] == ',')
          insert = insert.Remove(insert.Length - 2, 2);
        con.ExecuteNonQuery(insert);
      }
    }

    public override void ParsingAll()
    {
      string path = ExctractPath();
      //TODO нужно переделать обработку сайтов
      // Каждый сайт парсится в свой файл, после того как все спарсилось, все сливается в один файл

     

      var threadbackground = new Thread(
  new ThreadStart(() =>
  {
    elms.TypeParseFlat = this.TypeParseFlat;
    elms.Filename = $@"{path}ELMS.csv";
    elms.ParsingAll();
    while (!elms.IsFinished)
    { }
    bn.TypeParseFlat = this.TypeParseFlat;
    bn.Filename = $@"{path}BN.csv";
    bn.ParsingAll();
    while (!bn.IsFinished)
    { }
    bkn.TypeParseFlat = this.TypeParseFlat;
    bkn.Filename = $@"{path}BKN.csv";
    bkn.ParsingAll();
    while (!bkn.IsFinished)
    { }

    UnionFiles();
  }
  ));
      threadbackground.Start();
      //elmsThread = new Thread(elms.ParsingAll);
      //elmsThread.Start();
      //WaitUntilWorkThread(elmsThread);
      //bn.TypeParseFlat = this.TypeParseFlat;
      //bn.Filename = $@"{path}BN.csv";
      //bnThread = new Thread(bn.ParsingAll);
      //bnThread.Start();
      //WaitUntilWorkThread(bnThread);
      //bkn.TypeParseFlat = this.TypeParseFlat;
      //bkn.Filename = $@"{path}BKN.csv";
      //bknThread = new Thread(bkn.ParsingAll);
      //bknThread.Start();
      //WaitUntilWorkThread(bknThread);
      //avito.TypeParseFlat = this.TypeParseFlat;
      //avito.Filename = $@"{path}Avito.csv";
      //avitoThread = new Thread(avito.ParsingAll);
      //avitoThread.Start();
      //WaitUntilWorkThread(avitoThread);

    }

    public override void ParsingStudii()
    {
      string path = ExctractPath();
      bn.TypeParseFlat = this.TypeParseFlat;
      bn.Filename = $@"{path}BN.csv";
      bnThread = new Thread(bn.ParsingStudii);
      bnThread.Start();
      WaitUntilWorkThread(bnThread);
      bkn.TypeParseFlat = this.TypeParseFlat;
      bkn.Filename = $@"{path}BKN.csv";
      bknThread = new Thread(bkn.ParsingStudii);
      bknThread.Start();
      WaitUntilWorkThread(bknThread);
      elms.TypeParseFlat = this.TypeParseFlat;
      elms.Filename = $@"{path}ELMS.csv";
      elmsThread = new Thread(elms.ParsingStudii);
      elmsThread.Start();
      WaitUntilWorkThread(elmsThread);
      UnionFiles();
    }

    public override void ParsingOne()
    {
      string path = ExctractPath();
      bn.TypeParseFlat = this.TypeParseFlat;
      bn.Filename = $@"{path}BN.csv";
      bnThread = new Thread(bn.ParsingOne);
      bnThread.Start();
      WaitUntilWorkThread(bnThread);
      bkn.TypeParseFlat = this.TypeParseFlat;
      bkn.Filename = $@"{path}BKN.csv";
      bknThread = new Thread(bkn.ParsingOne);
      bknThread.Start();
      WaitUntilWorkThread(bknThread);
      elms.TypeParseFlat = this.TypeParseFlat;
      elms.Filename = $@"{path}ELMS.csv";
      elmsThread = new Thread(elms.ParsingOne);
      elmsThread.Start();
      WaitUntilWorkThread(elmsThread);
      UnionFiles();
    }

    public override void ParsingTwo()
    {
      string path = ExctractPath();
      bn.TypeParseFlat = this.TypeParseFlat;
      bn.Filename = $@"{path}BN.csv";
      bnThread = new Thread(bn.ParsingTwo);
      bnThread.Start();
      WaitUntilWorkThread(bnThread);
      bkn.TypeParseFlat = this.TypeParseFlat;
      bkn.Filename = $@"{path}BKN.csv";
      bknThread = new Thread(bkn.ParsingTwo);
      bknThread.Start();
      WaitUntilWorkThread(bknThread);
      elms.TypeParseFlat = this.TypeParseFlat;
      elms.Filename = $@"{path}ELMS.csv";
      elmsThread = new Thread(elms.ParsingTwo);
      elmsThread.Start();
      WaitUntilWorkThread(elmsThread);
      UnionFiles();
    }

    public override void ParsingThree()
    {
      string path = ExctractPath();
      bn.TypeParseFlat = this.TypeParseFlat;
      bn.Filename = $@"{path}BN.csv";
      bnThread = new Thread(bn.ParsingThree);
      bnThread.Start();
      WaitUntilWorkThread(bnThread);
      bkn.TypeParseFlat = this.TypeParseFlat;
      bkn.Filename = $@"{path}BKN.csv";
      bknThread = new Thread(bkn.ParsingThree);
      bknThread.Start();
      WaitUntilWorkThread(bknThread);
      elms.TypeParseFlat = this.TypeParseFlat;
      elms.Filename = $@"{path}ELMS.csv";
      elmsThread = new Thread(elms.ParsingThree);
      elmsThread.Start();
      WaitUntilWorkThread(elmsThread);
      UnionFiles();
    }

    public override void ParsingFour()
    {
      string path = ExctractPath();
      bn.TypeParseFlat = this.TypeParseFlat;
      bn.Filename = $@"{path}BN.csv";
      bnThread = new Thread(bn.ParsingFour);
      bnThread.Start();
      WaitUntilWorkThread(bnThread);
      bkn.TypeParseFlat = this.TypeParseFlat;
      bkn.Filename = $@"{path}BKN.csv";
      bknThread = new Thread(bkn.ParsingFour);
      bknThread.Start();
      WaitUntilWorkThread(bknThread);
      elms.TypeParseFlat = this.TypeParseFlat;
      elms.Filename = $@"{path}ELMS.csv";
      elmsThread = new Thread(elms.ParsingFour);
      elmsThread.Start();
      WaitUntilWorkThread(elmsThread);
      UnionFiles();
    }

    public override void ParsingMoreFour()
    {
      string path = ExctractPath();
      bn.TypeParseFlat = this.TypeParseFlat;
      bn.Filename = $@"{path}BN.csv";
      bnThread = new Thread(bn.ParsingMoreFour);
      bnThread.Start();
      WaitUntilWorkThread(bnThread);
      bkn.TypeParseFlat = this.TypeParseFlat;
      bkn.Filename = $@"{path}BKN.csv";
      bknThread = new Thread(bkn.ParsingMoreFour);
      bknThread.Start();
      WaitUntilWorkThread(bknThread);
      elms.TypeParseFlat = this.TypeParseFlat;
      elms.Filename = $@"{path}ELMS.csv";
      elmsThread = new Thread(elms.ParsingMoreFour);
      elmsThread.Start();
      WaitUntilWorkThread(elmsThread);
      UnionFiles();
    }

    protected override void CalcAverPrice()
    {
      throw new NotImplementedException();
    }
  }
}
