using System;
using System.Data;
using System.Reflection;
using log4net;

namespace DataBase.Connections
{
  public abstract class CoreConnetion:IDisposable
  {
    protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    public abstract void ExecuteNonQuery(string query);
    public abstract object ExecuteScalar(string query);
    public abstract IDataReader ExecuteReader(string query);

    public abstract void Dispose();
  }
}
