using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Core.Connections;
using Core.Enum;
using Core.MainClasses;
using Core.Proxy;
using Core.Sites;
using CoreUI;
using log4net;

namespace ParseSitesForApartments
{
  public partial class MainForm : Form
  {
    private List<District> listDistricts = new List<District>();
    private List<Metro> listMetros = new List<Metro>();
    protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private List<ProxyInfo> listProxy = new List<ProxyInfo>();

    public MainForm()
    {
      InitializeComponent();
      listProxy.Add(new ProxyInfo
      {
        Address = "185.233.202.204",
        Port = 9975,
        User = "BGXDdU",
        Password = "vy4ubS"
      });
      listProxy.Add(new ProxyInfo
      {
        Address = "185.233.201.211",
        Port = 9312,
        User = "BGXDdU",
        Password = "vy4ubS"
      });
    }

    private string fileName;
    private void ReadConfig()
    {
      var st = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      var fileCatalog = st + @"\ParseFlat";
      Directory.CreateDirectory(fileCatalog);
      fileName = fileCatalog + @"\parseflat.config";

      if (File.Exists(fileName))
      {
        ImportConfigDataBase inmportConfigDataBase = new ImportConfigDataBase();
        inmportConfigDataBase.Import();
      }
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
      ReadConfig();
      var conform = new ConnectionForm();
      if (conform.ShowDialog() == DialogResult.Cancel)
      {
        this.Close();
      }
      else
      {
        var exportSetting = new ExportConfigDataBase();
        exportSetting.Export(fileName);
      }

      cbChooseParse.SelectedIndex = 0;
      cbTypeRoom.SelectedIndex = 0;
      cbTypeSell.SelectedIndex = 0;

      sfdParseFile.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
      sfdParseFile.FilterIndex = 1;

      Log.Debug("Get Connection");
      var connection = ConnetionToSqlServer.Default();

      string select = "SELECT [ID],[Name] FROM [ParseBulding].[dbo].[District]";
      var reader = connection.ExecuteReader(select);
      if (reader != null)
      {
        while (reader.Read())
        {
          listDistricts.Add(new District { Id = reader.GetGuid(0), Name = reader.GetString(1) });
        }
        reader.Close();
        foreach (var district in listDistricts)
        {
          select = $@"SELECT [Id]
              ,[Name]
              ,[XCoor]
              ,[YCoor]
              ,[IdRegion]
            FROM[ParseBulding].[dbo].[Metro]
            where IdRegion = '{district.Id}'";
          reader = connection.ExecuteReader(select);
          while (reader.Read())
          {
            var metro = new Metro
            {
              Id = reader.GetGuid(0),
              Name = reader.GetString(1),
              XCoor = (float)reader.GetDouble(2),
              YCoor = (float)reader.GetDouble(3)
            };
            district.Metros.Add(metro);
            listMetros.Add(metro);
          }
          reader.Close();
        }
      }
    }

    private void tspmExit_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnSavePath_Click(object sender, EventArgs e)
    {
      string fileName = string.Empty;
      switch (cbChooseParse.SelectedIndex)
      {
        case 0:
          fileName = $"Все сайты - ";
          break;
        case 1:
          fileName = $"ELMS - ";
          break;
        case 2:
          fileName = $"БН - ";
          break;
        case 3:
          fileName = $"БКН - ";
          break;
        case 4:
          fileName = $"Авито - ";
          break;
      }
      switch (cbTypeRoom.SelectedIndex)
      {
        case 0:
          fileName += "Все квартиры ";
          break;
        case 1:
          fileName += "Студии ";
          break;
        case 2:
          fileName += "1 ком. кв. ";
          break;
        case 3:
          fileName += "2 ком. кв. ";
          break;
        case 4:
          fileName += "3 ком. кв. ";
          break;
        case 5:
          fileName += "4 ком. кв. ";
          break;
        case 6:
          fileName += "Больше 4 ком. ";
          break;
      }
      switch (cbTypeSell.SelectedIndex)
      {
        case 0:
          fileName += "Продажа ";
          break;
        case 1:
          fileName += "Сдача ";
          break;
      }
      fileName += DateTime.Now.ToShortDateString();
      //TODO проверить имя
      sfdParseFile.FileName = fileName;

      if (sfdParseFile.ShowDialog() == DialogResult.OK)
      {
        tbSelectedPath.Text = sfdParseFile.FileName;
      }
    }

    private void tpSelectedPath_TextChanged(object sender, EventArgs e)
    {
      if (!string.IsNullOrWhiteSpace(tbSelectedPath.Text))
        btnExecute.Enabled = true;
      else
        btnExecute.Enabled = false;
    }

    private BaseParse parser = null;
    private void btnExecute_Click(object sender, EventArgs e)
    {
      switch (cbChooseParse.SelectedIndex)
      {
        case 0:
          parser = new AllSites(listDistricts, listMetros, listProxy);
          break;
        case 1:
          parser = new ELMS(listDistricts, listMetros, listProxy);
          break;
        case 2:
          parser = new BN(listDistricts, listMetros, listProxy);
          break;
        case 3:
          parser = new BKN(listDistricts, listMetros, listProxy);
          break;
        case 4:
          parser = new Avito(listDistricts, listMetros, listProxy);
          break;
      }
      parser.Filename = tbSelectedPath.Text;

      switch (cbTypeSell.SelectedIndex)
      {
        case 0:
          parser.TypeParseFlat = TypeParseFlat.Sale;
          break;
        case 1:
          parser.TypeParseFlat = TypeParseFlat.Rent;
          break;
      }

      switch (cbTypeRoom.SelectedIndex)
      {
        case 0:
          parser.ParsingAll();
          break;
        case 1:
          parser.ParsingStudii();
          break;
        case 2:
          parser.ParsingOne();
          break;
        case 3:
          parser.ParsingTwo();
          break;
        case 4:
          parser.ParsingThree();
          break;
        case 5:
          parser.ParsingFour();
          break;
        case 6:
          parser.ParsingMoreFour();
          break;
      }
    }

    private void AnalysisResaltToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var analys = new AnalysisResultForm(listMetros);
      analys.ShowDialog();
    }

    private void tsmUpdateTypeBuilding_Click(object sender, EventArgs e)
    {
      string path = string.Empty;
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
        openFileDialog.FilterIndex = 1;
        openFileDialog.RestoreDirectory = true;
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
          path = openFileDialog.FileName;
        }
      }

      var threadbackground = new Thread(
        new ThreadStart(() =>
        {
          if (Path.GetExtension(path) == ".csv")
          {
            if (CheckHeaderColumnAndSetNumColumns(path))
            {
              using (var sr = new StreamReader(path))
              {
                string line = sr.ReadLine();
                if (line != null)
                {
                  var con = ConnetionToSqlServer.Default();
                  while ((line = sr.ReadLine()) != null)
                  {
                    var arLine = line.Split(';');
                    //Парсинг адреса
                    var adress = arLine[1].Split(',');
                    string street = adress[0].Trim();
                    string number = string.Empty;
                    string structure = string.Empty;
                    string liter = string.Empty;
                    if (adress.Length > 1)
                    {
                      if (adress[1].Contains("корп."))
                      {
                        var regex = new Regex(@"(корп\.\s+\d+)");
                        var val = regex.Match(adress[1]).Value;
                        if (string.IsNullOrEmpty(val))
                        {
                          regex = new Regex(@"(корп\.\d+)");
                          val = regex.Match(adress[1]).Value;
                        }
                        structure = val.Replace("корп.", "").Trim();
                        try
                        {
                          adress[1] = adress[1].Replace(val, "");
                        }
                        catch
                        {
                          Log.Error(arLine[1]);
                        }
                      }

                      if (adress[1].Contains("литер"))
                      {
                        var str = adress[1].Replace("литер", "").Replace(" ", "");
                        var regex = new Regex(@"(\D)");
                        var val = regex.Match(str).Value;
                        if (!string.IsNullOrWhiteSpace(val))
                        {
                          if (val == "/" || (val == "-"))
                          {
                            if (regex.Matches(str).Count > 1)
                            {
                              val = regex.Matches(str)[1].Value;
                            }
                          }
                          liter = val;
                          if (!string.IsNullOrEmpty(val))
                            number = str.Replace(val, "");
                        }
                      }
                      else
                        number = adress[1];
                    }

                    //Парсинг типа дома
                    var type = arLine[2].Split(',');
                    string typeBuild = string.Empty;
                    string isRepair = string.Empty;
                    if (type.Length == 2)
                    {
                      typeBuild = type[0];
                      if (type[1].Contains("капитальный"))
                      {
                        isRepair = type[1];
                      }
                    }
                    else
                    {
                      typeBuild = type[0];
                    }

                    string update = string.Empty;
                    string exists = string.Empty;
                    if (!string.IsNullOrEmpty(structure))
                    {
                      if (!string.IsNullOrEmpty(liter))
                      {
                        exists = $@"if exists (select * from [dbo].[MainInfoAboutBulding] where [Street] = '{street}' and  and Street = '{street}' and Number = '{number}' and Bulding = '{structure}' and Letter = '{liter}')
	select 1 as IsExist
else
	select 0 as IsExist";
                        update = $@"update [ParseBulding].[dbo].[MainInfoAboutBulding]
  set TypeBuild = '{typeBuild}',
  IsRepair = '{isRepair}'
  where Street = '{street}' and Number = '{number}' and Bulding = '{structure}' and Letter = '{liter}'";
                      }
                      else
                      {
                        exists = $@"if exists (select * from [dbo].[MainInfoAboutBulding] where [Street] = '{street}' and  and Number = '{number}' and Bulding = '{structure}')
	select 1 as IsExist
else
	select 0 as IsExist";
                        update = $@"update [ParseBulding].[dbo].[MainInfoAboutBulding]
  set TypeBuild = '{typeBuild}',
  IsRepair = '{isRepair}'
  where Street = '{street}' and Number = '{number}' and Bulding = '{structure}'";
                      }
                    }
                    else
                    {
                      if (!string.IsNullOrEmpty(liter))
                      {
                        exists = $@"if exists (select * from [dbo].[MainInfoAboutBulding] where Street = '{street}' and Number = '{number}' and Letter = '{liter}')
	select 1 as IsExist
else
	select 0 as IsExist";
                        update = $@"update [ParseBulding].[dbo].[MainInfoAboutBulding]
  set TypeBuild = '{typeBuild}',
  IsRepair = '{isRepair}'
  where Street = '{street}' and Number = '{number}' and Letter = '{liter}'";
                      }
                      else
                      {
                        exists = $@"if exists (select * from [dbo].[MainInfoAboutBulding] where Street = '{street}' and Number = '{number}')
	select 1 as IsExist
else
	select 0 as IsExist";
                        update = $@"update [ParseBulding].[dbo].[MainInfoAboutBulding]
  set TypeBuild = '{typeBuild}',
  IsRepair = '{isRepair}'
  where Street = '{street}' and Number = '{number}'";
                      }
                    }

                    var exi = (int) con.ExecuteValue(exists);
                    if(exi == 1)
                      con.ExecuteNonQuery(update);
                    else
                    {
                      Log.Debug($"Не найден дом - {line}");
                    }
                  }
                }
              }
            }
          }
          else
          {
            MessageBox.Show("Данный файл должен быть с раширением csv", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
        }
        ));
      threadbackground.Start();
    }

    private bool CheckHeaderColumnAndSetNumColumns(string path)
    {
      string line = null;
      using (var sr = new StreamReader(path))
      {
        line = sr.ReadLine();
      }
      if (line != null)
      {

      }
      else
      {
        MessageBox.Show("Файл пустой", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      return true;
    }

    private void ЗагрузитьДанныеПоСреднейСтоимостиToolStripMenuItem_Click(object sender, EventArgs e)
    {
      string path =string.Empty;
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
        openFileDialog.FilterIndex = 1;
        openFileDialog.RestoreDirectory = true;
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
          path = openFileDialog.FileName;
        }
      }
      if(!string.IsNullOrEmpty(path))
      {
        ReadAllPriceForAllRoom(path);
        ReadPriceForMetroByRoom(path);
        ReadPriceForDistrictByRoom(path);
      }
    }

    private void ReadAllPriceForAllRoom(string path)
    {
      using (var sr = new StreamReader(path))
      {
        var listStudii = new List<double>();
        var listOne = new List<double>();
        var listTwo = new List<double>();
        var listThree = new List<double>();
        var listFour = new List<double>();
        var listMoreFour = new List<double>();
        string line = sr.ReadLine();
        while ((line = sr.ReadLine()) != null)
        {
          var arr = line.Split(';');
          var typeRoom = arr[5];
          if (typeRoom.Contains("Студия"))
          {
            listStudii.Add(double.Parse(arr[9]));
          }
          else if (typeRoom.Contains("1 км."))
          {
            listOne.Add(double.Parse(arr[9]));
          }
          else if (typeRoom.Contains("2 км."))
          {
            listTwo.Add(double.Parse(arr[9]));
          }
          else if (typeRoom.Contains("3 км."))
          {
            listThree.Add(double.Parse(arr[9]));
          }
          else if (typeRoom.Contains("4 км."))
          {
            listFour.Add(double.Parse(arr[9]));
          }
          else
          {
            listMoreFour.Add(double.Parse(arr[9]));
          }
        }
      }
    }

    private void ReadPriceForMetroByRoom(string path)
    {
      var studii = new { Name = "Студия", Dictionary = new Dictionary<string, List<double>>() };
      var one = new { Name = "1 км.", Dictionary = new Dictionary<string, List<double>>() };
      var two = new { Name = "2 км.", Dictionary = new Dictionary<string, List<double>>() };
      var three = new { Name = "3 км.", Dictionary = new Dictionary<string, List<double>>() };
      var four = new { Name = "4 км.", Dictionary = new Dictionary<string, List<double>>() };
      var five = new { Name = "Более 4 км.", Dictionary = new Dictionary<string, List<double>>() };

      foreach (var metro in listMetros)
      {
        studii.Dictionary.Add(metro.Name, new List<double>());
        one.Dictionary.Add(metro.Name, new List<double>());
        two.Dictionary.Add(metro.Name, new List<double>());
        three.Dictionary.Add(metro.Name, new List<double>());
        four.Dictionary.Add(metro.Name, new List<double>());
        five.Dictionary.Add(metro.Name, new List<double>());
      }
      using (var sr = new StreamReader(path))
      {
        string line = sr.ReadLine();
        while ((line = sr.ReadLine()) != null)
        {
          var arr = line.Split(';');
          var metro = arr[10].Trim();
          if(!string.IsNullOrEmpty(metro))
          {
            var typeRoom = arr[5];
            if (typeRoom.Contains("Студия"))
            {
              var list = studii.Dictionary[metro];
              list.Add(double.Parse(arr[9]));
            }
            else if (typeRoom.Contains("1 км."))
            {
              var list = one.Dictionary[metro];
              list.Add(double.Parse(arr[9]));
            }
            else if (typeRoom.Contains("2 км."))
            {
              var list = two.Dictionary[metro];
              list.Add(double.Parse(arr[9]));
            }
            else if (typeRoom.Contains("3 км."))
            {
              var list = three.Dictionary[metro];
              list.Add(double.Parse(arr[9]));
            }
            else if (typeRoom.Contains("4 км."))
            {
              var list = four.Dictionary[metro];
              list.Add(double.Parse(arr[9]));
            }
            else
            {
              var list = five.Dictionary[metro];
              list.Add(double.Parse(arr[9]));
            }
          }
        }
      }
    }

    private void ReadPriceForDistrictByRoom(string path)
    {
      var studii = new { Name = "Студия", Dictionary = new Dictionary<string, List<double>>() };
      var one = new { Name = "1 км.", Dictionary = new Dictionary<string, List<double>>() };
      var two = new { Name = "2 км.", Dictionary = new Dictionary<string, List<double>>() };
      var three = new { Name = "3 км.", Dictionary = new Dictionary<string, List<double>>() };
      var four = new { Name = "4 км.", Dictionary = new Dictionary<string, List<double>>() };
      var five = new { Name = "Более 4 км.", Dictionary = new Dictionary<string, List<double>>() };

      foreach (var district in listDistricts)
      {
        studii.Dictionary.Add(district.Name, new List<double>());
        one.Dictionary.Add(district.Name, new List<double>());
        two.Dictionary.Add(district.Name, new List<double>());
        three.Dictionary.Add(district.Name, new List<double>());
        four.Dictionary.Add(district.Name, new List<double>());
        five.Dictionary.Add(district.Name, new List<double>());
      }
      using (var sr = new StreamReader(path, Encoding.UTF8))
      {
        string line = sr.ReadLine();
        while ((line = sr.ReadLine()) != null)
        {
          var arr = line.Split(';');
          var distr = arr[0].Trim();
          ////Слово скопиравано
          //if (distr == "﻿Адмиралтейский")
          //{
          //  continue;
          //}
          ////Полностью на русском
          //if (distr == "Адмиралтейский")
          //{
          //  var a1 = distr.GetHashCode();
          //  var a2 = "﻿Адмиралтейский".GetHashCode();
          //} 
          //if (distr == "Адмиpалтейcкий") { } //Английская p и c
          //if (distr == "Адмиралтейcкий") { }
          if (!string.IsNullOrEmpty(distr))
          {
            var typeRoom = arr[5];
            if (typeRoom.Contains("Студия"))
            {
              if(studii.Dictionary.ContainsKey(distr))
              {
                var list = studii.Dictionary[distr];
                list.Add(double.Parse(arr[9]));
              }
            }
            else if (typeRoom.Contains("1 км."))
            {
              if (one.Dictionary.ContainsKey(distr))
              {
                var list = one.Dictionary[distr];
                list.Add(double.Parse(arr[9]));
              }
            }
            else if (typeRoom.Contains("2 км."))
            {
              if (two.Dictionary.ContainsKey(distr))
              {
                var list = two.Dictionary[distr];
                list.Add(double.Parse(arr[9]));
              }
            }
            else if (typeRoom.Contains("3 км."))
            {
              if (three.Dictionary.ContainsKey(distr))
              {
                var list = three.Dictionary[distr];
                list.Add(double.Parse(arr[9]));
              }
            }
            else if (typeRoom.Contains("4 км."))
            {
              if (four.Dictionary.ContainsKey(distr))
              {
                var list = four.Dictionary[distr];
                list.Add(double.Parse(arr[9]));
              }
            }
            else
            {
              if (five.Dictionary.ContainsKey(distr))
              {
                var list = five.Dictionary[distr];
                list.Add(double.Parse(arr[9]));
              }
            }
          }
        }
      }
    }
  }
}
