using AngleSharp.Parser.Html;
using log4net;
using ParseSitesForApartments.Sites;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace ParseSitesForApartments
{
  public class UnionParseInfoWithDataBase
  {
    private BaseParse baseSite;
    private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public UnionParseInfoWithDataBase(BaseParse baseSite)
    {
      this.baseSite = baseSite;
    }

    public void UnionInfoProdam()
    {
      if (File.Exists(baseSite.Filename))
      {
        using (var sr = new StreamReader(baseSite.Filename, Encoding.UTF8))
        {
          using (var sw = new StreamWriter(baseSite.FilenameWithinfo, true, Encoding.UTF8))
          {
            using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
            {
              connection.Open();

              sw.WriteLine(@"Район;Улица;Номер;Корпус;Литер;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Дата постройки;Дата реконструкции;Даты кап. ремонты;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Кол-во встроенных нежилых помещений;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов;Расстояние пешком;Время пешком;Расстояние на машине;Время на машине;Откуда взято");
              string line = "";
              sr.ReadLine();
              string select = "";
              while ((line = sr.ReadLine()) != null)
              {
                SqlDataReader reader =null;
                try
                {
                  string district = string.Empty;
                  string street = string.Empty;
                  string number = string.Empty;
                  string building = string.Empty;
                  string letter = string.Empty;
                  string typeRoom = string.Empty;
                  string square = string.Empty;
                  string floor = string.Empty;
                  string countFloor = string.Empty;
                  string price = string.Empty;
                  string metro = string.Empty;
                  string distance = string.Empty;
                  string dateBuild = string.Empty;
                  string dateRecon = string.Empty;
                  string dateRepair = string.Empty;
                  string buildingSquare = string.Empty;
                  string livingSquare = string.Empty;
                  string noLivingSqaure = string.Empty;
                  string residents = string.Empty;
                  string mansardaSquare = string.Empty;
                  string otoplenie = string.Empty;
                  string gvs = string.Empty;
                  string es = string.Empty;
                  string gs = string.Empty;
                  string typeApartaments = string.Empty;
                  string countApartaments = string.Empty;
                  string countInternal = string.Empty;
                  DateTime dateTep = DateTime.Now;
                  string typeRepair = string.Empty;
                  string countLift = string.Empty;
                  float x = 0;
                  float y = 0;
                  string metroInBase = string.Empty;
                  string distanceOnFoot = string.Empty;
                  string distanceOnCar = string.Empty;
                  float xmetro = 0;
                  float ymetro = 0;
                  Guid IdBuilding = Guid.Empty;
                  Guid? metroId = Guid.Empty;
                  string disFoot = string.Empty;
                  string timeFoot = string.Empty;
                  string disCar = string.Empty;
                  string timeCar = string.Empty;

                  var arr = line.Split(';');
                  district = arr[0];
                  street = arr[1].Replace("«","").Replace("«", "»");
                  number = arr[2].Replace("«", "").Replace("«", "»");
                  building = arr[3];
                  letter = arr[4];
                  typeRoom = arr[5];
                  square = arr[6];
                  price = arr[7];
                  floor = arr[8];
                  metro = arr[9];
                  distance = arr[10];

                  if (string.IsNullOrWhiteSpace(letter))
                  {
                    if (string.IsNullOrWhiteSpace(building))
                    {
                      select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumber '{street}', '{number}'";
                    }
                    else
                    {
                      select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumberAndBuilbind '{street}', '{number}', '{building}'";
                    }
                  }
                  else
                  {
                    if (string.IsNullOrWhiteSpace(building))
                    {
                      select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumberAndLetter '{street}', '{number}', '{letter}'";
                    }
                    else
                    {
                      select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumberAndBuilbindAndLetter '{street}', '{number}', '{building}', '{letter}'";
                    }
                  }
                  Log.Debug("----------------------------");
                  Log.Debug(select);
                  var command = new SqlCommand(select, connection);
                  reader = command.ExecuteReader();
                  while (reader.Read())
                  {
                    if (string.IsNullOrEmpty(district))
                      district = reader.GetString(0);

                    dateBuild = reader.GetString(1);
                    dateRecon = reader.GetString(3);
                    dateRepair = reader.GetString(4).Replace("  ", "");
                    buildingSquare = reader.GetDouble(5).ToString(CultureInfo.CurrentCulture);
                    livingSquare = reader.GetDouble(6).ToString(CultureInfo.CurrentCulture);
                    noLivingSqaure = reader.GetDouble(7).ToString(CultureInfo.CurrentCulture);
                    countFloor = reader.GetInt32(9).ToString();
                    residents = reader.GetInt32(10).ToString();
                    mansardaSquare = reader.GetDouble(11).ToString(CultureInfo.CurrentCulture);
                    otoplenie = reader.GetBoolean(12).ToString();
                    gvs = reader.GetBoolean(13).ToString();
                    es = reader.GetBoolean(14).ToString();
                    gs = reader.GetBoolean(15).ToString();
                    typeApartaments = reader.GetString(16).Replace("  ", "");
                    countApartaments = reader.GetString(17).Replace("  ", "");
                    countInternal = reader.GetInt32(18).ToString();
                    dateTep = reader.GetDateTime(19);
                    typeRepair = reader.GetString(21);
                    countLift = reader.GetInt32(22).ToString();

                    x = (float)reader.GetDouble(24);
                    y = (float)reader.GetDouble(25);

                    distanceOnFoot = reader.GetString(26);
                    distanceOnCar = reader.GetString(27);
                    metroInBase = reader.GetString(28);
                    xmetro = (float)reader.GetDouble(29);
                    ymetro = (float)reader.GetDouble(30);
                    IdBuilding = reader.GetGuid(31);
                  }
                  reader.Close();

                  if (!string.IsNullOrEmpty(dateBuild))
                  {
                    if (!string.IsNullOrEmpty(metro))
                    {
                      if (string.IsNullOrEmpty(metroInBase))
                      {
                        select = $@"SELECT [XCoor]
    ,[YCoor]
	  ,Id
  FROM [ParseBulding].[dbo].[Metro]
  WHERE NAME like '%{metro}%'";
                        
                        Log.Debug("----------------------------");
                        Log.Debug(select);
                        command = new SqlCommand(select, connection);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                          xmetro = (float)reader.GetDouble(0);
                          ymetro = (float)reader.GetDouble(1);
                          metroId = reader.GetGuid(2);
                        }
                        reader.Close();
                        
                        string update = $@"update [ParseBulding].[dbo].MainInfoAboutBulding
set Metro = '{metroId}'
where ID='{IdBuilding}'";

                        Log.Debug("----------------------------Обновления метро----------------------------");
                        Log.Debug(update);

                        command = new SqlCommand(update, connection);
                        command.ExecuteNonQuery();
                        
                        if(xmetro >1 && ymetro > 1)
                        {
                          string url = $@"https://2gis.ru/spb/routeSearch/rsType/pedestrian/from/{y.ToString().Replace(",", ".")}%2C{x.ToString().Replace(",", ".")}%7C{x.ToString().Replace(",", ".")}%20{y.ToString().Replace(",", ".")}%7Cgeo/to/{ymetro.ToString().Replace(",", ".")}%2C{xmetro.ToString().Replace(",", ".")}%7C{xmetro.ToString().Replace(",", ".")}%20{ymetro.ToString().Replace(",", ".")}%7Cgeo?queryState=center%2F30.352666%2C59.920495%2Fzoom%2F17%2FrouteTab";

                          string urlCar = $@"https://2gis.ru/spb/routeSearch/rsType/car/from/{y.ToString().Replace(",", ".")}%2C{x.ToString().Replace(",", ".")}%7C{x.ToString().Replace(",", ".")}%20{y.ToString().Replace(",", ".")}%7Cgeo/to/{ymetro.ToString().Replace(",", ".")}%2C{xmetro.ToString().Replace(",", ".")}%7C{xmetro.ToString().Replace(",", ".")}%20{ymetro.ToString().Replace(",", ".")}%7Cgeo?queryState=center%2F30.235319%2C59.854278%2Fzoom%2F14%2FrouteTab";

                          Log.Debug("----------------------------URL----------------------------");
                          Log.Debug(url);
                          using (var webClient = new WebClient())
                          {
                            var random = new Random();
                            Thread.Sleep(random.Next(2000, 4000));

                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                            webClient.Encoding = Encoding.UTF8;
                            var responce = webClient.DownloadString(url);
                            var parser = new HtmlParser();
                            var document = parser.Parse(responce);

                            var timeDoc = document.GetElementsByClassName("autoResults__routeHeaderContentDuration");
                            if(timeDoc.Length > 0)
                            {
                              var disDoc = document.GetElementsByClassName("autoResults__routeHeaderContentLength");
                              if(disDoc.Length > 0)
                              {
                                timeFoot = timeDoc[0].TextContent;
                                disFoot = disDoc[0].TextContent;

                                if(disFoot.Contains("км"))
                                {
                                  var regex = new Regex(@"(\d+,\d+)");
                                  var km = regex.Match(disFoot).Value;
                                  km = km.Replace(".", "").Replace(",", "") + "00";
                                  if (km == "00")
                                  {
                                    regex = new Regex(@"(\d+)");
                                    km = regex.Match(disFoot).Value;
                                    km = km.Replace(".", "").Replace(",", "") + "000";
                                  }

                                  disFoot = km + " м";
                                }
                                update = $@"update [ParseBulding].[dbo].[MainInfoAboutBulding]
set DistanceAndTimeOnFoot = '{disFoot},{timeFoot}'
where ID='{IdBuilding}'";

                                Log.Debug("----------------------------");
                                Log.Debug(update);
                                command = new SqlCommand(update, connection);
                                command.ExecuteNonQuery();
                              }
                            }
                            responce = webClient.DownloadString(urlCar);
                            document = parser.Parse(responce);
                            timeDoc = document.GetElementsByClassName("autoResults__routeHeaderContentDuration");
                            if(timeDoc.Length>0)
                            {
                              var disDoc = document.GetElementsByClassName("autoResults__routeHeaderContentLength");
                              if(disDoc.Length>0)
                              {
                                if(timeDoc.Length == 2)
                                {
                                  timeCar = timeDoc[1].TextContent;
                                  disCar = disDoc[1].TextContent;
                                }
                                else
                                {
                                  timeCar = timeDoc[0].TextContent;
                                  disCar = disDoc[0].TextContent;
                                }
                                if (disCar.Contains("км"))
                                {
                                  var regex = new Regex(@"(\d+,\d+)");
                                  var km = regex.Match(disCar).Value;
                                  km = km.Replace(".", "").Replace(",", "") + "00";
                                  if (km == "00")
                                  {
                                    regex = new Regex(@"(\d+)");
                                    km = regex.Match(disCar).Value;
                                    km = km.Replace(".", "").Replace(",", "") + "000";
                                  }
                                  disCar = km + " м";
                                }
                                update = $@"update [ParseBulding].[dbo].[MainInfoAboutBulding]
set DistanceAndTimeOnCar = '{disCar},{timeCar}'
where ID='{IdBuilding}'";

                                Log.Debug("----------------------------");
                                Log.Debug(update);
                                command = new SqlCommand(update, connection);
                                command.ExecuteNonQuery();
                              }
                            }
                          }
                        }
                      }
                      else
                      {
                        if (string.IsNullOrEmpty(distanceOnCar))
                        {
                          //Если пустое расстояние
                          string urlCar = $@"https://2gis.ru/spb/routeSearch/rsType/car/from/{y.ToString().Replace(",", ".")}%2C{x.ToString().Replace(",", ".")}%7C{x.ToString().Replace(",", ".")}%20{y.ToString().Replace(",", ".")}%7Cgeo/to/{ymetro.ToString().Replace(",", ".")}%2C{xmetro.ToString().Replace(",", ".")}%7C{xmetro.ToString().Replace(",", ".")}%20{ymetro.ToString().Replace(",", ".")}%7Cgeo?queryState=center%2F30.235319%2C59.854278%2Fzoom%2F14%2FrouteTab";

                          using (var webClient = new WebClient())
                          {
                            var random = new Random();
                            Thread.Sleep(random.Next(2000, 4000));

                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                            webClient.Encoding = Encoding.UTF8;
                            var responce = webClient.DownloadString(urlCar);
                            var parser = new HtmlParser();
                            var document = parser.Parse(responce);

                            var timeDoc = document.GetElementsByClassName("autoResults__routeHeaderContentDuration");
                            if (timeDoc.Length > 0)
                            {
                              var disDoc = document.GetElementsByClassName("autoResults__routeHeaderContentLength");
                              if (disDoc.Length > 0)
                              {
                                timeCar = timeDoc[0].TextContent;
                                disCar = disDoc[0].TextContent;

                                if (timeDoc.Length == 2)
                                {
                                  timeCar = timeDoc[1].TextContent;
                                  disCar = disDoc[1].TextContent;
                                }
                                else
                                {
                                  timeCar = timeDoc[0].TextContent;
                                  disCar = disDoc[0].TextContent;
                                }
                                if (disCar.Contains("км"))
                                {
                                  var regex = new Regex(@"(\d+,\d+)");
                                  var km = regex.Match(disCar).Value;
                                  km = km.Replace(".", "").Replace(",", "") + "00";
                                  if (km == "00")
                                  {
                                    regex = new Regex(@"(\d+)");
                                    km = regex.Match(disCar).Value;
                                    km = km.Replace(".", "").Replace(",", "") + "000";
                                  }
                                  disCar = km + " м";
                                }
                                string update = $@"update [ParseBulding].[dbo].[MainInfoAboutBulding]
set DistanceAndTimeOnCar = '{disCar},{timeCar}'
where ID='{IdBuilding}'";

                                Log.Debug("----------------------------");
                                Log.Debug(update);
                                command = new SqlCommand(update, connection);
                                command.ExecuteNonQuery();
                              }
                            }
                          }
                        }
                        arr = distanceOnFoot.Split(',');
                        if(arr.Length > 1)
                        {
                          disFoot = arr[0];
                          timeFoot = arr[1];
                        }
                        arr = distanceOnCar.Split(',');
                        if(arr.Length > 1)
                        {
                          disCar = arr[0];
                          timeCar = arr[1];
                        }
                      }
                    }
                  }
                  if(!string.IsNullOrEmpty(distanceOnCar) && string.IsNullOrEmpty(disCar))
                  {
                    var arrr = distanceOnCar.Split(',');
                    disCar = arrr[0];
                    timeCar = arrr[1];
                  }
                  if(metroId == Guid.Empty)
                  {
                    metroId = null;
                  }


                  //Поиск координат у домов которые не нашлись в базе и нахождения у них расстояния до метро
                  if(string.IsNullOrWhiteSpace(dateBuild))
                  {
                    Metro metroObj =null;
                    if(xmetro < 1 || ymetro < 1)
                    {
                      metroObj = GetCoorMetroFromBase(metro, connection);
                    }
                    if (metroObj != null)
                    {
                      var address = $@"Санкт-Петербург {street}, {number}к{building} лит.{letter}";
                      Log.Debug(address);
                      var coords = GetCoorForBuildig(address);
                      if (coords != null)
                      {
                        string url = $@"https://2gis.ru/spb/routeSearch/rsType/pedestrian/from/{coords[1].ToString().Replace(",", ".")}%2C{coords[0].ToString().Replace(",", ".")}%7C{coords[0].ToString().Replace(",", ".")}%20{coords[1].ToString().Replace(",", ".")}%7Cgeo/to/{metroObj.YCoor.ToString().Replace(",", ".")}%2C{metroObj.XCoor.ToString().Replace(",", ".")}%7C{metroObj.XCoor.ToString().Replace(",", ".")}%20{metroObj.YCoor.ToString().Replace(",", ".")}%7Cgeo?queryState=center%2F30.352666%2C59.920495%2Fzoom%2F17%2FrouteTab";

                        string urlCar = $@"https://2gis.ru/spb/routeSearch/rsType/car/from/{coords[1].ToString().Replace(",", ".")}%2C{coords[0].ToString().Replace(",", ".")}%7C{coords[0].ToString().Replace(",", ".")}%20{coords[1].ToString().Replace(",", ".")}%7Cgeo/to/{metroObj.YCoor.ToString().Replace(",", ".")}%2C{metroObj.XCoor.ToString().Replace(",", ".")}%7C{metroObj.XCoor.ToString().Replace(",", ".")}%20{metroObj.YCoor.ToString().Replace(",", ".")}%7Cgeo?queryState=center%2F30.235319%2C59.854278%2Fzoom%2F14%2FrouteTab";

                        using (var webClient = new WebClient())
                        {
                          var random = new Random();
                          Thread.Sleep(random.Next(2000, 4000));

                          ServicePointManager.Expect100Continue = true;
                          ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                          ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                          webClient.Encoding = Encoding.UTF8;
                          Log.Debug(url);
                          var responce = webClient.DownloadString(url);
                          var parser = new HtmlParser();
                          var document = parser.Parse(responce);

                          var timeDoc = document.GetElementsByClassName("autoResults__routeHeaderContentDuration");
                          if (timeDoc.Length > 0)
                          {
                            var disDoc = document.GetElementsByClassName("autoResults__routeHeaderContentLength");
                            if (disDoc.Length > 0)
                            {
                              timeFoot = timeDoc[0].TextContent;
                              disFoot = disDoc[0].TextContent;

                              if (disFoot.Contains("км"))
                              {
                                var regex = new Regex(@"(\d+,\d+)");
                                var km = regex.Match(disFoot).Value;
                                km = km.Replace(".", "").Replace(",", "") + "00";
                                if (km == "00")
                                {
                                  regex = new Regex(@"(\d+)");
                                  km = regex.Match(disFoot).Value;
                                  km = km.Replace(".", "").Replace(",", "") + "000";
                                }

                                disFoot = km + " м";
                              }
                            }
                          }
                          responce = webClient.DownloadString(urlCar);
                          document = parser.Parse(responce);
                          timeDoc = document.GetElementsByClassName("autoResults__routeHeaderContentDuration");
                          if (timeDoc.Length > 0)
                          {
                            var disDoc = document.GetElementsByClassName("autoResults__routeHeaderContentLength");
                            if (disDoc.Length > 0)
                            {
                              if (timeDoc.Length == 2)
                              {
                                timeCar = timeDoc[1].TextContent;
                                disCar = disDoc[1].TextContent;
                              }
                              else
                              {
                                timeCar = timeDoc[0].TextContent;
                                disCar = disDoc[0].TextContent;
                              }
                              if (disCar.Contains("км"))
                              {
                                var regex = new Regex(@"(\d+,\d+)");
                                var km = regex.Match(disCar).Value;
                                km = km.Replace(".", "").Replace(",", "") + "00";
                                if (km == "00")
                                {
                                  regex = new Regex(@"(\d+)");
                                  km = regex.Match(disCar).Value;
                                  km = km.Replace(".", "").Replace(",", "") + "000";
                                }
                                disCar = km + " м";
                              }
                            }
                          }
                        }

                        string insert = $@"insert into [ParseBulding].[dbo].[MainInfoAboutBulding] (Id, Street, Number, Bulding, Letter, DistrictId, DateBulding, SeriesID,[CountCommApartament],[DateReconstruct],[DateRepair],[BuldingArea],[LivingArea],[NoLivingArea],[Stairs],[Storeys] ,[Residents],[MansardArea] ,[HeatingCentral],[HotWaterCentral],[ElectroCentral],[GascCntral],[FlatType],[FlatNum],[InternalNum],[TepCreateDate],[ManagCompanyId],[Failure],[RepairJob],[LiftCount],[BasementArea],[Xcoor],[Ycoor],[Metro],[DistanceAndTimeOnFoot],[DistanceAndTimeOnCar])
values(newid(),'{street}','{number}','{building}','{letter}','A0CC3147-65B0-472D-9300-96D6A7364F68','','856E5C0B-F9C0-4F06-AD81-2405BF8357A6',0,'','',0,0,0,0,0,0,0,0,0,0,0,'','',null,null,'CE2CC208-8F15-44C5-94CC-29F5A971C196',0,'',0,0,{coords[0].ToString().Replace(",",".")},{coords[1].ToString().Replace(",", ".")},'{metroObj.Id}','{disFoot}, {timeFoot}','{disCar}, {timeCar}')";
                        Log.Debug(insert);
                        command = new SqlCommand(insert, connection);
                        command.ExecuteNonQuery();
                      }
                    }
                  }
                  

                  string dateTime = string.Empty;
                  if (dateTep != DateTime.Now)
                    dateTime = dateTep.ToShortDateString();

                  sw.WriteLine($@"{district};{street};{number};{building};{letter};{typeRoom};{square};{floor};{countFloor};{price};{metro};{dateBuild};{dateRecon};{dateRepair};{buildingSquare};{livingSquare};{noLivingSqaure};{mansardaSquare};{residents};{otoplenie};{gvs};{es};{gs};{typeApartaments};{countApartaments};{countInternal};{dateTime};{typeRepair};{countLift};{disFoot};{timeFoot};{disCar};{timeCar};{baseSite.NameSite}");
                }
                catch (SqlException ex)
                {
                  Log.Error("----------------------------");
                  Log.Error(ex.Message);
                  if (reader != null)
                  {
                    if (!reader.IsClosed)
                      reader.Close();
                  }
                }
                catch (Exception ex)
                {
                  Log.Error("----------------------------");
                  Log.Error(ex.Message);
                  if (reader != null)
                  {
                    if (!reader.IsClosed)
                      reader.Close();
                  }
                }
              }
            }
          }
        }
      }
      else
      {
        MessageBox.Show("Нет файла с данными");
      }
      MessageBox.Show("Готово");
    }

    private float[] GetCoorForBuildig(string adress)
    {
      var yandex = new Yandex();
      var doc1 = yandex.SearchObjectByAddress(adress);
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
            return new float[]{x,y};
          }
        }
      }
      File.Delete(@"D:\Coord.xml");
      return null;
    }

    private Metro GetCoorMetroFromBase(string metroName, SqlConnection connection)
    {
      string select = $@"SELECT [XCoor]
    ,[YCoor]
	  ,Id
  FROM [ParseBulding].[dbo].[Metro]
  WHERE NAME like '%{metroName}%'";

      Log.Debug("----------------------------");
      Log.Debug(select);
      var command = new SqlCommand(select, connection);
      var reader = command.ExecuteReader();
      Metro metro= null;
      while (reader.Read())
      {
        metro = new Metro();
        metro.XCoor = (float)reader.GetDouble(0);
        metro.YCoor = (float)reader.GetDouble(1);
        metro.Id = reader.GetGuid(2);
        metro.Name = metroName;
      }
      reader.Close();
      return metro;
    }

    public void UnionInfoSdam()
    {
      if (File.Exists(baseSite.Filename))
      {
        using (var sr = new StreamReader(baseSite.FilenameSdam, Encoding.UTF8))
        {
          using (var sw = new StreamWriter(baseSite.FilenameWithinfoSdam, true, Encoding.UTF8))
          {
            using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
            {
              connection.Open();

              sw.WriteLine(@"Район;Улица;Номер;Корпус;Литер;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Дата постройки;Дата реконструкции;Даты кап. ремонты;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Кол-во встроенных нежилых помещений;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов;Расстояние пешком;Время пешком;Расстояние на машине;Время на машине;Откуда взято");
              string line = "";
              sr.ReadLine();
              string select = "";
              while ((line = sr.ReadLine()) != null)
              {
                SqlDataReader reader = null;
                try
                {
                  string district = string.Empty;
                  string street = string.Empty;
                  string number = string.Empty;
                  string building = string.Empty;
                  string letter = string.Empty;
                  string typeRoom = string.Empty;
                  string square = string.Empty;
                  string floor = string.Empty;
                  string countFloor = string.Empty;
                  string price = string.Empty;
                  string metro = string.Empty;
                  string distance = string.Empty;
                  string dateBuild = string.Empty;
                  string dateRecon = string.Empty;
                  string dateRepair = string.Empty;
                  string buildingSquare = string.Empty;
                  string livingSquare = string.Empty;
                  string noLivingSqaure = string.Empty;
                  string residents = string.Empty;
                  string mansardaSquare = string.Empty;
                  string otoplenie = string.Empty;
                  string gvs = string.Empty;
                  string es = string.Empty;
                  string gs = string.Empty;
                  string typeApartaments = string.Empty;
                  string countApartaments = string.Empty;
                  string countInternal = string.Empty;
                  DateTime dateTep = DateTime.Now;
                  string typeRepair = string.Empty;
                  string countLift = string.Empty;
                  float x = 0;
                  float y = 0;
                  string metroInBase = string.Empty;
                  string distanceOnFoot = string.Empty;
                  string distanceOnCar = string.Empty;
                  float xmetro = 0;
                  float ymetro = 0;
                  Guid IdBuilding = Guid.Empty;
                  Guid? metroId = Guid.Empty;
                  string disFoot = string.Empty;
                  string timeFoot = string.Empty;
                  string disCar = string.Empty;
                  string timeCar = string.Empty;

                  var arr = line.Split(';');
                  district = arr[0];
                  street = arr[1].Replace("«", "").Replace("«", "»");
                  number = arr[2].Replace("«", "").Replace("«", "»");
                  building = arr[3];
                  letter = arr[4];
                  typeRoom = arr[5];
                  square = arr[6];
                  price = arr[7];
                  floor = arr[8];
                  metro = arr[9];
                  distance = arr[10];

                  if (string.IsNullOrWhiteSpace(letter))
                  {
                    if (string.IsNullOrWhiteSpace(building))
                    {
                      select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumber '{street}', '{number}'";
                    }
                    else
                    {
                      select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumberAndBuilbind '{street}', '{number}', '{building}'";
                    }
                  }
                  else
                  {
                    if (string.IsNullOrWhiteSpace(building))
                    {
                      select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumberAndLetter '{street}', '{number}', '{letter}'";
                    }
                    else
                    {
                      select = $"EXEC dbo.MainInfoAboutBuldingByStreetAndNumberAndBuilbindAndLetter '{street}', '{number}', '{building}', '{letter}'";
                    }
                  }
                  Log.Debug("----------------------------");
                  Log.Debug(select);
                  var command = new SqlCommand(select, connection);
                  reader = command.ExecuteReader();
                  while (reader.Read())
                  {
                    if (string.IsNullOrEmpty(district))
                      district = reader.GetString(0);

                    dateBuild = reader.GetString(1);
                    dateRecon = reader.GetString(3);
                    dateRepair = reader.GetString(4).Replace("  ", "");
                    buildingSquare = reader.GetDouble(5).ToString(CultureInfo.CurrentCulture);
                    livingSquare = reader.GetDouble(6).ToString(CultureInfo.CurrentCulture);
                    noLivingSqaure = reader.GetDouble(7).ToString(CultureInfo.CurrentCulture);
                    countFloor = reader.GetInt32(9).ToString();
                    residents = reader.GetInt32(10).ToString();
                    mansardaSquare = reader.GetDouble(11).ToString(CultureInfo.CurrentCulture);
                    otoplenie = reader.GetBoolean(12).ToString();
                    gvs = reader.GetBoolean(13).ToString();
                    es = reader.GetBoolean(14).ToString();
                    gs = reader.GetBoolean(15).ToString();
                    typeApartaments = reader.GetString(16).Replace("  ", "");
                    countApartaments = reader.GetString(17).Replace("  ", "");
                    countInternal = reader.GetInt32(18).ToString();
                    dateTep = reader.GetDateTime(19);
                    typeRepair = reader.GetString(21);
                    countLift = reader.GetInt32(22).ToString();

                    x = (float)reader.GetDouble(24);
                    y = (float)reader.GetDouble(25);

                    distanceOnFoot = reader.GetString(26);
                    distanceOnCar = reader.GetString(27);
                    metroInBase = reader.GetString(28);
                    xmetro = (float)reader.GetDouble(29);
                    ymetro = (float)reader.GetDouble(30);
                    IdBuilding = reader.GetGuid(31);
                  }
                  reader.Close();

                  if (!string.IsNullOrEmpty(dateBuild))
                  {
                    if (!string.IsNullOrEmpty(metro))
                    {
                      if (string.IsNullOrEmpty(metroInBase))
                      {
                        select = $@"SELECT [XCoor]
    ,[YCoor]
	  ,Id
  FROM [ParseBulding].[dbo].[Metro]
  WHERE NAME like '%{metro}%'";

                        Log.Debug("----------------------------");
                        Log.Debug(select);
                        command = new SqlCommand(select, connection);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                          xmetro = (float)reader.GetDouble(0);
                          ymetro = (float)reader.GetDouble(1);
                          metroId = reader.GetGuid(2);
                        }
                        reader.Close();

                        string update = $@"update [ParseBulding].[dbo].MainInfoAboutBulding
set Metro = '{metroId}'
where ID='{IdBuilding}'";

                        Log.Debug("----------------------------Обновления метро----------------------------");
                        Log.Debug(update);

                        command = new SqlCommand(update, connection);
                        command.ExecuteNonQuery();

                        if (xmetro > 1 && ymetro > 1)
                        {
                          string url = $@"https://2gis.ru/spb/routeSearch/rsType/pedestrian/from/{y.ToString().Replace(",", ".")}%2C{x.ToString().Replace(",", ".")}%7C{x.ToString().Replace(",", ".")}%20{y.ToString().Replace(",", ".")}%7Cgeo/to/{ymetro.ToString().Replace(",", ".")}%2C{xmetro.ToString().Replace(",", ".")}%7C{xmetro.ToString().Replace(",", ".")}%20{ymetro.ToString().Replace(",", ".")}%7Cgeo?queryState=center%2F30.352666%2C59.920495%2Fzoom%2F17%2FrouteTab";

                          string urlCar = $@"https://2gis.ru/spb/routeSearch/rsType/car/from/{y.ToString().Replace(",", ".")}%2C{x.ToString().Replace(",", ".")}%7C{x.ToString().Replace(",", ".")}%20{y.ToString().Replace(",", ".")}%7Cgeo/to/{ymetro.ToString().Replace(",", ".")}%2C{xmetro.ToString().Replace(",", ".")}%7C{xmetro.ToString().Replace(",", ".")}%20{ymetro.ToString().Replace(",", ".")}%7Cgeo?queryState=center%2F30.235319%2C59.854278%2Fzoom%2F14%2FrouteTab";

                          Log.Debug("----------------------------URL----------------------------");
                          Log.Debug(url);
                          using (var webClient = new WebClient())
                          {
                            var random = new Random();
                            Thread.Sleep(random.Next(2000, 4000));

                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                            webClient.Encoding = Encoding.UTF8;
                            var responce = webClient.DownloadString(url);
                            var parser = new HtmlParser();
                            var document = parser.Parse(responce);

                            var timeDoc = document.GetElementsByClassName("autoResults__routeHeaderContentDuration");
                            if (timeDoc.Length > 0)
                            {
                              var disDoc = document.GetElementsByClassName("autoResults__routeHeaderContentLength");
                              if (disDoc.Length > 0)
                              {
                                timeFoot = timeDoc[0].TextContent;
                                disFoot = disDoc[0].TextContent;

                                if (disFoot.Contains("км"))
                                {
                                  var regex = new Regex(@"(\d+,\d+)");
                                  var km = regex.Match(disFoot).Value;
                                  km = km.Replace(".", "").Replace(",", "") + "00";
                                  if (km == "00")
                                  {
                                    regex = new Regex(@"(\d+)");
                                    km = regex.Match(disFoot).Value;
                                    km = km.Replace(".", "").Replace(",", "") + "000";
                                  }

                                  disFoot = km + " м";
                                }
                                update = $@"update [ParseBulding].[dbo].[MainInfoAboutBulding]
set DistanceAndTimeOnFoot = '{disFoot},{timeFoot}'
where ID='{IdBuilding}'";

                                Log.Debug("----------------------------");
                                Log.Debug(update);
                                command = new SqlCommand(update, connection);
                                command.ExecuteNonQuery();
                              }
                            }
                            responce = webClient.DownloadString(urlCar);
                            document = parser.Parse(responce);
                            timeDoc = document.GetElementsByClassName("autoResults__routeHeaderContentDuration");
                            if (timeDoc.Length > 0)
                            {
                              var disDoc = document.GetElementsByClassName("autoResults__routeHeaderContentLength");
                              if (disDoc.Length > 0)
                              {
                                if (timeDoc.Length == 2)
                                {
                                  timeCar = timeDoc[1].TextContent;
                                  disCar = disDoc[1].TextContent;
                                }
                                else
                                {
                                  timeCar = timeDoc[0].TextContent;
                                  disCar = disDoc[0].TextContent;
                                }
                                if (disCar.Contains("км"))
                                {
                                  var regex = new Regex(@"(\d+,\d+)");
                                  var km = regex.Match(disCar).Value;
                                  km = km.Replace(".", "").Replace(",", "") + "00";
                                  if (km == "00")
                                  {
                                    regex = new Regex(@"(\d+)");
                                    km = regex.Match(disCar).Value;
                                    km = km.Replace(".", "").Replace(",", "") + "000";
                                  }
                                  disCar = km + " м";
                                }
                                update = $@"update [ParseBulding].[dbo].[MainInfoAboutBulding]
set DistanceAndTimeOnCar = '{disCar},{timeCar}'
where ID='{IdBuilding}'";

                                Log.Debug("----------------------------");
                                Log.Debug(update);
                                command = new SqlCommand(update, connection);
                                command.ExecuteNonQuery();
                              }
                            }
                          }
                        }
                      }
                      else
                      {
                        if (string.IsNullOrEmpty(distanceOnCar))
                        {
                          //Если пустое расстояние
                          string urlCar = $@"https://2gis.ru/spb/routeSearch/rsType/car/from/{y.ToString().Replace(",", ".")}%2C{x.ToString().Replace(",", ".")}%7C{x.ToString().Replace(",", ".")}%20{y.ToString().Replace(",", ".")}%7Cgeo/to/{ymetro.ToString().Replace(",", ".")}%2C{xmetro.ToString().Replace(",", ".")}%7C{xmetro.ToString().Replace(",", ".")}%20{ymetro.ToString().Replace(",", ".")}%7Cgeo?queryState=center%2F30.235319%2C59.854278%2Fzoom%2F14%2FrouteTab";

                          using (var webClient = new WebClient())
                          {
                            var random = new Random();
                            Thread.Sleep(random.Next(2000, 4000));

                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                            webClient.Encoding = Encoding.UTF8;
                            var responce = webClient.DownloadString(urlCar);
                            var parser = new HtmlParser();
                            var document = parser.Parse(responce);

                            var timeDoc = document.GetElementsByClassName("autoResults__routeHeaderContentDuration");
                            if (timeDoc.Length > 0)
                            {
                              var disDoc = document.GetElementsByClassName("autoResults__routeHeaderContentLength");
                              if (disDoc.Length > 0)
                              {
                                timeCar = timeDoc[0].TextContent;
                                disCar = disDoc[0].TextContent;

                                if (timeDoc.Length == 2)
                                {
                                  timeCar = timeDoc[1].TextContent;
                                  disCar = disDoc[1].TextContent;
                                }
                                else
                                {
                                  timeCar = timeDoc[0].TextContent;
                                  disCar = disDoc[0].TextContent;
                                }
                                if (disCar.Contains("км"))
                                {
                                  var regex = new Regex(@"(\d+,\d+)");
                                  var km = regex.Match(disCar).Value;
                                  km = km.Replace(".", "").Replace(",", "") + "00";
                                  if (km == "00")
                                  {
                                    regex = new Regex(@"(\d+)");
                                    km = regex.Match(disCar).Value;
                                    km = km.Replace(".", "").Replace(",", "") + "000";
                                  }
                                  disCar = km + " м";
                                }
                                string update = $@"update [ParseBulding].[dbo].[MainInfoAboutBulding]
set DistanceAndTimeOnCar = '{disCar},{timeCar}'
where ID='{IdBuilding}'";

                                Log.Debug("----------------------------");
                                Log.Debug(update);
                                command = new SqlCommand(update, connection);
                                command.ExecuteNonQuery();
                              }
                            }
                          }
                        }
                        arr = distanceOnFoot.Split(',');
                        if (arr.Length > 1)
                        {
                          disFoot = arr[0];
                          timeFoot = arr[1];
                        }
                        arr = distanceOnCar.Split(',');
                        if (arr.Length > 1)
                        {
                          disCar = arr[0];
                          timeCar = arr[1];
                        }
                      }
                    }
                  }
                  if (!string.IsNullOrEmpty(distanceOnCar) && string.IsNullOrEmpty(disCar))
                  {
                    var arrr = distanceOnCar.Split(',');
                    disCar = arrr[0];
                    timeCar = arrr[1];
                  }
                  if (metroId == Guid.Empty)
                  {
                    metroId = null;
                  }


                  //Поиск координат у домов которые не нашлись в базе и нахождения у них расстояния до метро
                  if (string.IsNullOrWhiteSpace(dateBuild))
                  {
                    Metro metroObj = null;
                    if (xmetro < 1 || ymetro < 1)
                    {
                      metroObj = GetCoorMetroFromBase(metro, connection);
                    }
                    if (metroObj != null)
                    {
                      var address = $@"Санкт-Петербург {street}, {number}к{building} лит.{letter}";
                      Log.Debug(address);
                      var coords = GetCoorForBuildig(address);
                      if (coords != null)
                      {
                        string url = $@"https://2gis.ru/spb/routeSearch/rsType/pedestrian/from/{coords[1].ToString().Replace(",", ".")}%2C{coords[0].ToString().Replace(",", ".")}%7C{coords[0].ToString().Replace(",", ".")}%20{coords[1].ToString().Replace(",", ".")}%7Cgeo/to/{metroObj.YCoor.ToString().Replace(",", ".")}%2C{metroObj.XCoor.ToString().Replace(",", ".")}%7C{metroObj.XCoor.ToString().Replace(",", ".")}%20{metroObj.YCoor.ToString().Replace(",", ".")}%7Cgeo?queryState=center%2F30.352666%2C59.920495%2Fzoom%2F17%2FrouteTab";

                        string urlCar = $@"https://2gis.ru/spb/routeSearch/rsType/car/from/{coords[1].ToString().Replace(",", ".")}%2C{coords[0].ToString().Replace(",", ".")}%7C{coords[0].ToString().Replace(",", ".")}%20{coords[1].ToString().Replace(",", ".")}%7Cgeo/to/{metroObj.YCoor.ToString().Replace(",", ".")}%2C{metroObj.XCoor.ToString().Replace(",", ".")}%7C{metroObj.XCoor.ToString().Replace(",", ".")}%20{metroObj.YCoor.ToString().Replace(",", ".")}%7Cgeo?queryState=center%2F30.235319%2C59.854278%2Fzoom%2F14%2FrouteTab";

                        using (var webClient = new WebClient())
                        {
                          var random = new Random();
                          Thread.Sleep(random.Next(2000, 4000));

                          ServicePointManager.Expect100Continue = true;
                          ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                          ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                          webClient.Encoding = Encoding.UTF8;
                          Log.Debug(url);
                          var responce = webClient.DownloadString(url);
                          var parser = new HtmlParser();
                          var document = parser.Parse(responce);

                          var timeDoc = document.GetElementsByClassName("autoResults__routeHeaderContentDuration");
                          if (timeDoc.Length > 0)
                          {
                            var disDoc = document.GetElementsByClassName("autoResults__routeHeaderContentLength");
                            if (disDoc.Length > 0)
                            {
                              timeFoot = timeDoc[0].TextContent;
                              disFoot = disDoc[0].TextContent;

                              if (disFoot.Contains("км"))
                              {
                                var regex = new Regex(@"(\d+,\d+)");
                                var km = regex.Match(disFoot).Value;
                                km = km.Replace(".", "").Replace(",", "") + "00";
                                if (km == "00")
                                {
                                  regex = new Regex(@"(\d+)");
                                  km = regex.Match(disFoot).Value;
                                  km = km.Replace(".", "").Replace(",", "") + "000";
                                }

                                disFoot = km + " м";
                              }
                            }
                          }
                          responce = webClient.DownloadString(urlCar);
                          document = parser.Parse(responce);
                          timeDoc = document.GetElementsByClassName("autoResults__routeHeaderContentDuration");
                          if (timeDoc.Length > 0)
                          {
                            var disDoc = document.GetElementsByClassName("autoResults__routeHeaderContentLength");
                            if (disDoc.Length > 0)
                            {
                              if (timeDoc.Length == 2)
                              {
                                timeCar = timeDoc[1].TextContent;
                                disCar = disDoc[1].TextContent;
                              }
                              else
                              {
                                timeCar = timeDoc[0].TextContent;
                                disCar = disDoc[0].TextContent;
                              }
                              if (disCar.Contains("км"))
                              {
                                var regex = new Regex(@"(\d+,\d+)");
                                var km = regex.Match(disCar).Value;
                                km = km.Replace(".", "").Replace(",", "") + "00";
                                if (km == "00")
                                {
                                  regex = new Regex(@"(\d+)");
                                  km = regex.Match(disCar).Value;
                                  km = km.Replace(".", "").Replace(",", "") + "000";
                                }
                                disCar = km + " м";
                              }
                            }
                          }
                        }

                        string insert = $@"insert into [ParseBulding].[dbo].[MainInfoAboutBulding] (Id, Street, Number, Bulding, Letter, DistrictId, DateBulding, SeriesID,[CountCommApartament],[DateReconstruct],[DateRepair],[BuldingArea],[LivingArea],[NoLivingArea],[Stairs],[Storeys] ,[Residents],[MansardArea] ,[HeatingCentral],[HotWaterCentral],[ElectroCentral],[GascCntral],[FlatType],[FlatNum],[InternalNum],[TepCreateDate],[ManagCompanyId],[Failure],[RepairJob],[LiftCount],[BasementArea],[Xcoor],[Ycoor],[Metro],[DistanceAndTimeOnFoot],[DistanceAndTimeOnCar])
values(newid(),'{street}','{number}','{building}','{letter}','A0CC3147-65B0-472D-9300-96D6A7364F68','','856E5C0B-F9C0-4F06-AD81-2405BF8357A6',0,'','',0,0,0,0,0,0,0,0,0,0,0,'','',null,null,'CE2CC208-8F15-44C5-94CC-29F5A971C196',0,'',0,0,{coords[0].ToString().Replace(",", ".")},{coords[1].ToString().Replace(",", ".")},'{metroObj.Id}','{disFoot}, {timeFoot}','{disCar}, {timeCar}')";
                        Log.Debug(insert);
                        command = new SqlCommand(insert, connection);
                        command.ExecuteNonQuery();
                      }
                    }
                  }


                  string dateTime = string.Empty;
                  if (dateTep != DateTime.Now)
                    dateTime = dateTep.ToShortDateString();

                  sw.WriteLine($@"{district};{street};{number};{building};{letter};{typeRoom};{square};{floor};{countFloor};{price};{metro};{dateBuild};{dateRecon};{dateRepair};{buildingSquare};{livingSquare};{noLivingSqaure};{mansardaSquare};{residents};{otoplenie};{gvs};{es};{gs};{typeApartaments};{countApartaments};{countInternal};{dateTime};{typeRepair};{countLift};{disFoot};{timeFoot};{disCar};{timeCar};{baseSite.NameSite}");
                }
                catch (SqlException ex)
                {
                  Log.Error("----------------------------");
                  Log.Error(ex.Message);
                  if (reader != null)
                  {
                    if (!reader.IsClosed)
                      reader.Close();
                  }
                }
                catch (Exception ex)
                {
                  Log.Error("----------------------------");
                  Log.Error(ex.Message);
                  if (reader != null)
                  {
                    if (!reader.IsClosed)
                      reader.Close();
                  }
                }
              }
            }
          }
        }
      }
      else
      {
        MessageBox.Show("Нет файла с данными");
      }
      MessageBox.Show("Готово");
    }

    private string GetDistanceOnFoot(float x, float y)
    {
      return "";
    }
  }
}
