using System.Xml;
using Core.Connections;

namespace ParseSitesForApartments
{
  public class ExportConfigDataBase
  {
    public ExportConfigDataBase()
    {

    }

    public void Export(string path)
    {
      var doc = new XmlDocument();
      var decl = doc.CreateXmlDeclaration("1.0", "utf-8", "");
      doc.InsertBefore(decl, doc.DocumentElement);
      XmlNode configuration = doc.CreateElement("configuration");

      XmlNode connection = doc.CreateElement("connection");

      var server = doc.CreateAttribute("Server");
      server.InnerText = ConnetionToSqlServer.Default().Server;
      connection.Attributes?.Append(server);

      var dataBase = doc.CreateAttribute("DataBase");
      dataBase.InnerText = ConnetionToSqlServer.Default().DataBase;
      connection.Attributes?.Append(dataBase);

      var sqlAut = doc.CreateAttribute("SQLAuthentication");
      sqlAut.InnerText = ConnetionToSqlServer.Default().SQLAuthentication.ToString();
      connection.Attributes?.Append(sqlAut);

      if (ConnetionToSqlServer.Default().SQLAuthentication)
      {
        var login = doc.CreateAttribute("Login");
        login.InnerText = ConnetionToSqlServer.Default().UserName;
        connection.Attributes?.Append(login);

        var password = doc.CreateAttribute("Password");
        password.InnerText = ConnetionToSqlServer.Default().Password;
        connection.Attributes?.Append(password);
      }

      configuration.AppendChild(connection);
      doc.AppendChild(configuration);
      doc.Save(path);
    }
  }
}
