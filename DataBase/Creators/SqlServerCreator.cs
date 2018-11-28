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
      throw new System.Exception();
      //return new ConnetionToSqlServer(connectionString);
    }
  }
}
