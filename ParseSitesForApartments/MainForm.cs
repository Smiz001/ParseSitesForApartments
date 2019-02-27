using ParseSitesForApartments.Sites;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DataBase.Connections;
using ParseSitesForApartments.Enum;
using ParseSitesForApartments.UI;

namespace ParseSitesForApartments
{
  public partial class MainForm : Form
  {
    private List<District> listDistricts = new List<District>();
    private List<Metro> listMetros = new List<Metro>();

    public MainForm()
    {
      InitializeComponent();
    }

    private void GetCoordBuilding(string inputFile, string outputFile)
    {
      if(!string.IsNullOrEmpty(inputFile))
      {
        if (File.Exists(inputFile))
        {
          using (var sr = new StreamReader(inputFile, Encoding.UTF8))
          {
            using (var sw = new StreamWriter(outputFile, true, Encoding.UTF8))
            {
              var yandex = new Yandex();
              string line;
              sr.ReadLine();
              while ((line = sr.ReadLine()) != null)
              {
                var arr = line.Split(';');
                var doc1 = yandex.SearchObjectByAddress($@"Санкт-Петербург {arr[1]}, {arr[2]}к{arr[3]} лит.{arr[4]}");
                using (var sw1 = new StreamWriter(@"D:\Coord.xml", false, System.Text.Encoding.UTF8))
                {
                  sw1.WriteLine(doc1);
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(@"D:\Coord.xml");
                var root = doc.DocumentElement;
                var GeoObjectCollection = root.GetElementsByTagName("GeoObjectCollection")[0];
                if (GeoObjectCollection.ChildNodes.Count > 1)
                {
                  var featureMember = GeoObjectCollection.ChildNodes[1];
                  if (featureMember.ChildNodes.Count > 0)
                  {
                    var GeoObject = featureMember.ChildNodes[0];
                    if (GeoObject.ChildNodes.Count > 4)
                    {
                      var Point = GeoObject.ChildNodes[4];
                      var coor = Point.InnerText.Split(' ');
                      float x = float.Parse(coor[1].Replace(".", ","));
                      float y = float.Parse(coor[0].Replace(".", ","));
                      sw.WriteLine($@"{arr[0]};{arr[1]};{arr[2]};{arr[3]};{arr[4]};{x};{y}");
                    }
                  }
                }
                File.Delete(@"D:\Coord.xml");
              }
            }
          }
        }
      }
    }
    
    private List<Metro> metroStantions = new List<Metro>() { new Metro { Name = "Автово" }, new Metro { Name = "Адмиралтейская" }, new Metro { Name = "Академическая" }, new Metro { Name = "Балтийская" }, new Metro { Name = "Беговая" }, new Metro { Name = "Бухарестская" }, new Metro { Name = "Василеостровская" }, new Metro { Name = "Владимирская" }, new Metro { Name = "Волковская" }, new Metro { Name = "Выборгская" }, new Metro { Name = "Горьковская" }, new Metro { Name = "Гостиный двор" }, new Metro { Name = "Гражданский проспект" }, new Metro { Name = "Девяткино" }, new Metro { Name = "Достоевская" }, new Metro { Name = "Елизаровская" }, new Metro { Name = "Звёздная" }, new Metro { Name = "Звенигородская" }, new Metro { Name = "Кировский завод" }, new Metro { Name = "Комендантский проспект" }, new Metro { Name = "Крестовский остров" }, new Metro { Name = "Купчино" }, new Metro { Name = "Ладожская" }, new Metro { Name = "Ленинский проспект" }, new Metro { Name = "Лесная" }, new Metro { Name = "Лиговский проспект" }, new Metro { Name = "Ломоносовская" }, new Metro { Name = "Маяковская" }, new Metro { Name = "Международная" }, new Metro { Name = "Московская" }, new Metro { Name = "Московские ворота" }, new Metro { Name = "Нарвская" }, new Metro { Name = "Невский проспект" }, new Metro { Name = "Новокрестовская" }, new Metro { Name = "Новочеркасская" }, new Metro { Name = "Обводный канал" }, new Metro { Name = "Обухово" }, new Metro { Name = "Озерки" }, new Metro { Name = "Парк Победы" }, new Metro { Name = "Парнас" }, new Metro { Name = "Петроградская" }, new Metro { Name = "Пионерская" }, new Metro { Name = "Площадь Александра Невского 1" }, new Metro { Name = "Площадь Восстания" }, new Metro { Name = "Площадь Ленина" }, new Metro { Name = "Площадь Мужества" }, new Metro { Name = "Политехническая" }, new Metro { Name = "Приморская" }, new Metro { Name = "Пролетарская" }, new Metro { Name = "Проспект Большевиков" }, new Metro { Name = "Проспект Ветеранов" }, new Metro { Name = "Проспект Просвещения" }, new Metro { Name = "Пушкинская" }, new Metro { Name = "Рыбацкое" }, new Metro { Name = "Садовая" }, new Metro { Name = "Сенная площадь" }, new Metro { Name = "Спасская" }, new Metro { Name = "Спортивная" }, new Metro { Name = "Старая Деревня" }, new Metro { Name = "Технологический институт 1" }, new Metro { Name = "Удельная" }, new Metro { Name = "Улица Дыбенко" }, new Metro { Name = "Фрунзенская" }, new Metro { Name = "Чёрная речка" }, new Metro { Name = "Чернышевская" }, new Metro { Name = "Чкаловская" }, new Metro { Name = "Электросила" } };

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
          parser = new AllSites(listDistricts, listMetros);
          break;
        case 1:
          parser = new ELMS(listDistricts, listMetros);
          break;
        case 2:
          parser = new BN(listDistricts, listMetros);
          break;
        case 3:
          parser = new BKN(listDistricts, listMetros);
          break;
        case 4:
          parser = new Avito(listDistricts, listMetros);
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
  }
}
