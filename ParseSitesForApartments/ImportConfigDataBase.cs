using System;
using System.Xml;
using DataBase.Connections;

namespace ParseSitesForApartments
{
  public class ImportConfigDataBase
  {
    private string fileNameConfig = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ParseFlat\parseflat.config";

    public void Import()
    {
      XmlDocument xDoc = new XmlDocument();
      xDoc.Load(fileNameConfig);

      XmlElement xRoot = xDoc.DocumentElement;
      ConnetionToSqlServer.Default().Server = xRoot.ChildNodes[0].Attributes.GetNamedItem("Server").Value;
      ConnetionToSqlServer.Default().DataBase = xRoot.ChildNodes[0].Attributes.GetNamedItem("DataBase").Value;
      var sqlAut = xRoot.ChildNodes[0].Attributes.GetNamedItem("SQLAuthentication")?.Value;
      if (sqlAut == "True")
      {
        ConnetionToSqlServer.Default().SQLAuthentication = true;
        ConnetionToSqlServer.Default().UserName = xRoot.ChildNodes[0].Attributes.GetNamedItem("Login")?.Value;
        ConnetionToSqlServer.Default().Password = xRoot.ChildNodes[0].Attributes.GetNamedItem("Password")?.Value;
      }
    }
  }
}
