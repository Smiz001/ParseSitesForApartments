using ParseSitesForApartments.Sites;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ParseSitesForApartments
{
  public class UnionParseInfoWithDataBase
  {
    private BaseParse baseSite;
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
                  dateRepair = reader.GetString(4);
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
                  typeApartaments = reader.GetString(16);
                  countApartaments = reader.GetString(17);
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
                  dateRepair = reader.GetString(4);
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
                  typeApartaments = reader.GetString(16);
                  countApartaments = reader.GetString(17);
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
  }
}
