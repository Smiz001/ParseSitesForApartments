using DataBase.Connections;

namespace DataBase.Creators
{
  public class SqlServerCreator : CoreCreatorConnection
  {
    public SqlServerCreator()
    {
    }

    public override CoreConnetion FactoryCreate(string connectionString)
    {
      return new ConnetionToSqlServer(connectionString);
    }
  }
}
