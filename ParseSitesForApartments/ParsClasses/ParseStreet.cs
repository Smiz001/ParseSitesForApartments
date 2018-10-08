﻿using System.Text.RegularExpressions;

namespace ParseSitesForApartments.ParsClasses
{
  public class ParseStreet
  {
    public string Execute(string street, District district)
    {
      var str = street.ToLower();
      Regex regex;
      if (str.Contains("мойк"))
        return "наб.реки Мойки";
      else if (str.Contains("красноармейская"))
      {
        regex = new Regex(@"(\d+)");
        string streetNum = regex.Match(street).Value;
        if (!string.IsNullOrEmpty(streetNum))
        {
          return $"{streetNum}-я Красноармейская";
        }
        else
          return "Красноармейская";
      }
      else if(str.Contains("кадетская"))
      {
        return "Кадетская линия В.О.";
      }
      else if (str.Contains("косая"))
      {
        return "Косая линия В.О.";
      }
      else if (str.Contains("линия") && ((str.Contains("васильевского") || str.Contains("во") || district.Name == "Василеостровский")))
      {
        regex = new Regex(@"(\d+)");
        string streetNum = regex.Match(street).Value;
        if (!string.IsNullOrEmpty(streetNum))
        {
          return $"{streetNum}-я линия В.О.";
        }
        else
          return "линия В.О.";
      }
      else if (str.Contains("большой пс"))
      {
        return "Большой П.С. пр.";
      }
      else if (str.Contains("большой во") || (str.Contains("большой") && district.Name== "Василеостровский"))
      {
        return "Большой пр. В.О.";
      }
      else if (str.Contains("казачий"))
      {
        return "Б. Казачий пер.";
      }
      else if (str.Contains("спасский"))
      {
        return "Спасский пер.";
      }
      else if (str.Contains("советская"))
      {
        regex = new Regex(@"(\d+)");
        string streetNum = regex.Match(street).Value;
        if (!string.IsNullOrEmpty(streetNum))
        {
          return $"{streetNum}-я Советская";
        }
        else
          return "Советская";
      }
      else if (str.Contains("муринский"))
      {
        regex = new Regex(@"(\d+)");
        string streetNum = regex.Match(street).Value;
        if (!string.IsNullOrEmpty(streetNum))
        {
          return $"{streetNum}-й Муринский";
        }
        else
          return "Муринский";
      }
      else if (str.Contains("предпортовый"))
      {
        regex = new Regex(@"(\d+)");
        string streetNum = regex.Match(street).Value;
        if (!string.IsNullOrEmpty(streetNum))
        {
          return $"{streetNum}-й Предпортовый";
        }
        else
          return "Предпортовый";
      }
      else if (str.Contains("грибоедова") && (district.Name == "﻿Адмиралтейский" || district.Name == "﻿﻿Центральный"))
      {
        return "Грибоедова наб.кан.";
      }
      else if (str.Contains("гривцова"))
      {
        return "Гривцова пер.";
      }
      else if (str.Contains("фонтанки"))
      {
        return "Фонтанки реки наб.";
      }
      else if (str.Contains("адмиралтейского"))
      {
        return "Адмиралтейского канала наб.";
      }
      else if (str.Contains("обводного"))
      {
        return "Обводного кан. наб.";
      }
      else if (str.Contains("бринько"))
      {
        return "Бринько пер.";
      }
      else if (str.Contains("макаренко"))
      {
        return "Макаренко пер.";
      }
      else if (str.Contains("прядильный"))
      {
        return "Прядильный пер.";
      }
      else if (str.Contains("пряжки"))
      {
        return "Наб. р. Пряжки";
      }
      else
        return street;
    }
  }
}