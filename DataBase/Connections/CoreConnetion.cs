using System;
using System.Data;

namespace DataBase.Connections
{
  public abstract class CoreConnetion:IDisposable
  {
    public abstract void ExecuteNonQuery(string query);
    public abstract object ExecuteScalar(string query);
    public abstract IDataReader ExecuteReader(string query);

    public abstract void Dispose();
  }
}
