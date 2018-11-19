using System;
using System.Data;
using System.Data.SqlClient;

namespace DataBase.Connections
{
  public class ConnetionToSqlServer:CoreConnetion
  {
    private static volatile ConnetionToSqlServer connection;// = new Connection();
    private static object syncRoot = new Object();

    #region private field
    
    private SqlConnectionStringBuilder m_builder = new SqlConnectionStringBuilder();
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

    public ConnetionToSqlServer(string connectionString)
    {
      Log.Debug($"Connection string - {connectionString}");
      connection = new SqlConnection(connectionString);
    }

    public override void ExecuteNonQuery(string query)
    {
      Log.Debug($"Call {nameof(ExecuteNonQuery)}");
      Log.Debug($"Query - {query}");
      try
      {
        connection.Open();
        using (var command = new SqlCommand(query,connection))
        {
          command.ExecuteNonQuery();
        }
      }
      catch (Exception e)
      {
        Log.Error($"Exception Message - {e.Message}");
        Log.Error($"Stack Trace - {e.StackTrace}");
        throw;
      }
      finally
      {
        if (connection != null)
        {
          if(connection.State == ConnectionState.Open)
            connection.Close();
        }
      }
    }

    public override object ExecuteScalar(string query)
    {
      object obj = null;
      Log.Debug($"Call {nameof(ExecuteScalar)}");
      Log.Debug($"Query - {query}");
      try
      {
        connection.Open();
        using (var command = new SqlCommand(query, connection))
        {
          obj = command.ExecuteScalar();
        }
      }
      catch (Exception e)
      {
        Log.Error($"Exception Message - {e.Message}");
        Log.Error($"Stack Trace - {e.StackTrace}");
        throw;
      }
      finally
      {
        if (connection != null)
        {
          if (connection.State == ConnectionState.Open)
            connection.Close();
        }
      }

      return obj;
    }

    public override IDataReader ExecuteReader(string query)
    {
      IDataReader reader = null;
      Log.Debug($"Call {nameof(ExecuteScalar)}");
      Log.Debug($"Query - {query}");
      try
      {
        if (connection.State != ConnectionState.Open)
          connection.Open();
        var command = new SqlCommand(query, connection);
        reader = command.ExecuteReader();
      }
      catch (Exception e)
      {
        Log.Error($"Exception Message - {e.Message}");
        Log.Error($"Stack Trace - {e.StackTrace}");
        throw;
      }
      finally
      {
        if (connection != null)
        {
          //if (connection.State == ConnectionState.Open)
            //connection.Close();
        }
      }

      return reader;
    }

    public override void Dispose()
    {
      if(connection != null)
        connection.Dispose();
    }
  }
}
