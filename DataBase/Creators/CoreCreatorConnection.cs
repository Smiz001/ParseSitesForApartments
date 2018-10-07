using DataBase.Connections;

namespace DataBase.Creators
{
  public abstract class CoreCreatorConnection
  {
    public abstract CoreConnetion FactoryCreate(string connectionString);
  }
}
