using System.Text.RegularExpressions;

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
      else
        return street;
    }
  }
}
