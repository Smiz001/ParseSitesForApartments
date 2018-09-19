﻿using AngleSharp.Parser.Html;
using log4net;
using ParseSitesForApartments.Sites;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

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

              sw.WriteLine($@"Район;Улица;Номер;Корпус;Литер;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Дата постройки;Дата реконструкции;Даты кап. ремонты;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Кол-во встроенных нежилых помещений;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов;Расстояние пешком;Время пешком;Расстояние на машине;Время на машине");
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
                  string timeFoor = string.Empty;
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
                    buildingSquare = reader.GetDouble(5).ToString();
                    livingSquare = reader.GetDouble(6).ToString();
                    noLivingSqaure = reader.GetDouble(7).ToString();
                    countFloor = reader.GetInt32(9).ToString();
                    residents = reader.GetInt32(10).ToString();
                    mansardaSquare = reader.GetDouble(11).ToString();
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

                        // string url = $@"https://yandex.ru/maps/2/saint-petersburg/?ll=30.217571%2C59.837282&z=14&mode=routes&rtext={x.ToString().Replace(",", ".")}%2C{y.ToString().Replace(",", ".")}~{xmetro.ToString().Replace(",",".")}%2C{ymetro.ToString().Replace(",", ".")}&rtt=pd";
                        //string url = $@"https://www.google.com/maps/dir/{x.ToString().Replace(",", ".")}+{y.ToString().Replace(",", ".")}/{xmetro.ToString().Replace(",", ".")}+{ymetro.ToString().Replace(",", ".")}/@59.8823321,30.2011912,12z/data=!3m1!4b1!4m10!4m9!1m3!2m2!1d30.18184!2d59.83419!1m3!2m2!1d30.36056!2d59.93152!3e2";
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
                                timeFoor = timeDoc[0].TextContent;
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
set DistanceAndTimeOnFoot = '{disFoot},{timeFoor}'
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
                          timeFoor = arr[1];
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

                  string dateTime = string.Empty;
                  if (dateTep != DateTime.Now)
                    dateTime = dateTep.ToShortDateString();

                  sw.WriteLine($@"{district};{street};{number};{building};{letter};{typeRoom};{square};{floor};{countFloor};{price};{metro};{dateBuild};{dateRecon};{dateRepair};{buildingSquare};{livingSquare};{noLivingSqaure};{mansardaSquare};{residents};{otoplenie};{gvs};{es};{gs};{typeApartaments};{countApartaments};{countInternal};{dateTime};{typeRepair};{countLift};{disFoot};{timeFoor};{disCar};{timeCar}");
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

    public void UnionInfoSdam()
    {
      if (File.Exists(baseSite.FilenameSdam))
      {
        using (var sr = new StreamReader(baseSite.FilenameSdam, Encoding.UTF8))
        {
          using (var sw = new StreamWriter(baseSite.FilenameWithinfoSdam, true, Encoding.UTF8))
          {
            using (var connection = new SqlConnection("Server= localhost; Database= ParseBulding; Integrated Security=True;"))
            {
              connection.Open();

              sw.WriteLine($@"Улица;Номер;Корпус;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Расстояние;Дата постройки;Дата реконструкции;Даты кап. ремонты;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Кол-во встроенных нежилых помещений;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов");
              string line;
              sr.ReadLine();
              while ((line = sr.ReadLine()) != null)
              {
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

                var arr = line.Split(';');
                street = arr[1];
                number = arr[2];
                building = arr[3];
                letter = arr[4];
                typeRoom = arr[5];
                square = arr[6];
                price = arr[7];
                floor = arr[8];
                metro = arr[9];
                distance = arr[10];

                string select = "";
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

                var command = new SqlCommand(select, connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                  dateBuild = reader.GetString(1);
                  dateRecon = reader.GetString(3);
                  dateRepair = reader.GetString(4).Replace("  ", "");
                  buildingSquare = reader.GetDouble(5).ToString();
                  livingSquare = reader.GetDouble(6).ToString();
                  noLivingSqaure = reader.GetDouble(7).ToString();
                  countFloor = reader.GetInt32(9).ToString();
                  residents = reader.GetInt32(10).ToString();
                  mansardaSquare = reader.GetDouble(11).ToString();
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
                }
                reader.Close();

                
                sw.WriteLine($@"{street};{number};{building};{typeRoom};{square};{floor};{countFloor};{price};{metro};{distance};{dateBuild};{dateRecon};{dateRepair};{buildingSquare};{livingSquare};{noLivingSqaure};{mansardaSquare};{residents};{otoplenie};{gvs};{es};{gs};{typeApartaments};{countApartaments};{countInternal};{dateTep.ToShortDateString()};{typeRepair};{countLift}");
              }
            }
          }
        }
      }
      else
      {
        MessageBox.Show("Нет файла с данными");
      }
    }

    private string GetDistanceOnFoot(float x, float y)
    {
      return "";
    }
  }
}
