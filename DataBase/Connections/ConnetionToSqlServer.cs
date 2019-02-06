using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using log4net;

namespace DataBase.Connections
{
  public class ConnetionToSqlServer
  {
    private static volatile ConnetionToSqlServer connection;// = new Connection();
    private static object syncRoot = new object();

    #region private field

    protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();
    private SqlConnection m_connection;
    private int m_countConnection = 0;

    #endregion

    public static ConnetionToSqlServer Default()
    {
      lock (syncRoot)
      {
        if (connection == null)
          connection = new ConnetionToSqlServer();
      }
      return connection;
    }

    private ConnetionToSqlServer()
    {
      sqlConnectionStringBuilder.IntegratedSecurity = true;
    }

    //public ConnetionToSqlServer()
    //{
    //  Log.Debug($"Connection string - {connectionString}");
    //  connection = new SqlConnection(connectionString);
    //}


    public string UserName
    {
      get
      {
        return sqlConnectionStringBuilder.UserID;
      }
      set
      {
        value = (value ?? string.Empty).Trim();
        if (sqlConnectionStringBuilder.UserID != value)
        {
          sqlConnectionStringBuilder.UserID = value;
        }
      }
    }

    public string Password
    {
      get
      {
        return sqlConnectionStringBuilder.Password;
      }
      set
      {
        value = (value ?? string.Empty).Trim();
        if (sqlConnectionStringBuilder.Password != value)
        {
          sqlConnectionStringBuilder.Password = value;
        }
      }
    }

    public string Server
    {
      get
      {
        return sqlConnectionStringBuilder.DataSource;
      }
      set
      {
        value = (value ?? string.Empty).Trim();
        if (sqlConnectionStringBuilder.DataSource != value)
        {
          sqlConnectionStringBuilder.DataSource = value;
        }
      }
    }

    public string DataBase
    {
      get
      {
        return sqlConnectionStringBuilder.InitialCatalog;
      }
      set
      {
        value = (value ?? string.Empty).Trim();
        if (sqlConnectionStringBuilder.InitialCatalog != value)
        {
          sqlConnectionStringBuilder.InitialCatalog = value;
        }
      }
    }

    public bool SQLAuthentication
    {
      get
      {
        return !WindowsAuthentication;
      }
      set
      {
        WindowsAuthentication = !value;
      }
    }

    public bool WindowsAuthentication
    {
      get
      {
        return sqlConnectionStringBuilder.IntegratedSecurity;
      }
      set
      {
        if (sqlConnectionStringBuilder.IntegratedSecurity != value)
        {
          sqlConnectionStringBuilder.IntegratedSecurity = value;
        }
      }
    }

    public bool Connect()
    {
      m_countConnection++;
      if (m_connection == null)
      {
        m_connection = new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
      }
      if (!IsOpen())
      {
        if (m_countConnection == 1)
        {
          m_connection.Open();
        }
      }
      return IsOpen();

    }

    public void Disconnect()
    {
      m_countConnection--;
      if (m_connection != null)
      {
        if (m_countConnection == 0)
        {
          m_connection.Close();
          m_connection = null;
        }
      }
    }

    public bool IsOpen()
    {
      return (m_connection != null) && (System.Data.ConnectionState.Open == (System.Data.ConnectionState.Open & m_connection.State));
    }

    public SqlCommand GetCommand(string select)
    {
      return new SqlCommand(select, m_connection);
    }

    public SqlCommand GetCommand()
    {
      return m_connection.CreateCommand();
    }

    public int ExecuteNonQuery(string query)
    {
      Log.Debug($"Call {nameof(ExecuteNonQuery)}");
      Log.Debug($"Query - {query}");
      int result;
      Connect();
      try
      {
        result = GetCommand(query).ExecuteNonQuery();
      }
      finally
      {
        Disconnect();
      }
      return result;
    }

    //public override void ExecuteNonQuery(string query)
    //{
    //  Log.Debug($"Call {nameof(ExecuteNonQuery)}");
    //  Log.Debug($"Query - {query}");
    //  try
    //  {
    //    connection.Open();
    //    using (var command = new SqlCommand(query,connection))
    //    {
    //      command.ExecuteNonQuery();
    //    }
    //  }
    //  catch (Exception e)
    //  {
    //    Log.Error($"Exception Message - {e.Message}");
    //    Log.Error($"Stack Trace - {e.StackTrace}");
    //    throw;
    //  }
    //  finally
    //  {
    //    if (connection != null)
    //    {
    //      if(connection.State == ConnectionState.Open)
    //        connection.Close();
    //    }
    //  }
    //}

    //public override object ExecuteScalar(string query)
    //{
    //  object obj = null;
    //  Log.Debug($"Call {nameof(ExecuteScalar)}");
    //  Log.Debug($"Query - {query}");
    //  try
    //  {
    //    connection.Open();
    //    using (var command = new SqlCommand(query, connection))
    //    {
    //      obj = command.ExecuteScalar();
    //    }
    //  }
    //  catch (Exception e)
    //  {
    //    Log.Error($"Exception Message - {e.Message}");
    //    Log.Error($"Stack Trace - {e.StackTrace}");
    //    throw;
    //  }
    //  finally
    //  {
    //    if (connection != null)
    //    {
    //      if (connection.State == ConnectionState.Open)
    //        connection.Close();
    //    }
    //  }

    //  return obj;
    //}

    //public override IDataReader ExecuteReader(string query)
    //{
    //  IDataReader reader = null;
    //  Log.Debug($"Call {nameof(ExecuteScalar)}");
    //  Log.Debug($"Query - {query}");
    //  try
    //  {
    //    if (connection.State != ConnectionState.Open)
    //      connection.Open();
    //    var command = new SqlCommand(query, connection);
    //    reader = command.ExecuteReader();
    //  }
    //  catch (Exception e)
    //  {
    //    Log.Error($"Exception Message - {e.Message}");
    //    Log.Error($"Stack Trace - {e.StackTrace}");
    //    throw;
    //  }
    //  finally
    //  {
    //    if (connection != null)
    //    {
    //      //if (connection.State == ConnectionState.Open)
    //        //connection.Close();
    //    }
    //  }

    //  return reader;
    //}

    //public override void Dispose()
    //{
    //  if(connection != null)
    //    connection.Dispose();
    //}

    public IDataReader ExecuteReader(string select)
    {
      try
      {
        SqlCommand command = GetCommand(select);
        return command.ExecuteReader();
      }
      catch (Exception ex)
      {
        Log.Debug(ex.Message);
        return null;
        //throw;
      }

    }

    public object ExecuteValue(string select)
    {
      object result = null;
      Connect();
      try
      {
        SqlCommand command = GetCommand(select);
        result = command.ExecuteScalar();
        //using (var reader = ExecuteReader(select))
        //{
        //  if (reader != null)
        //  {
        //    if (reader.Read())
        //    {
        //      result = reader.GetValue(0);
        //    }
        //    reader.Close();
        //  }
        //}
      }
      finally
      {
        Disconnect();
      }
      return result;
    }
  }

}
