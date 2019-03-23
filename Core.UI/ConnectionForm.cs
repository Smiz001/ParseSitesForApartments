using System;
using System.Windows.Forms;
using Core.Connections;

namespace CoreUI
{
  public partial class ConnectionForm : Form
  {
    public ConnectionForm()
    {
      InitializeComponent();
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
      tbPassword.Enabled = cbSqlAut.Checked;
      tbUser.Enabled = cbSqlAut.Checked;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      var connection = ConnetionToSqlServer.Default();
      if (!cbSqlAut.Checked)
      {
        connection.DataBase = tbDataBase.Text;
        connection.Server = tbServerName.Text;
        connection.WindowsAuthentication = !cbSqlAut.Checked;
      }
      else
      {
        connection.DataBase = tbDataBase.Text;
        connection.Server = tbServerName.Text;
        connection.UserName = tbUser.Text;
        connection.Password = tbPassword.Text;
        connection.WindowsAuthentication = !cbSqlAut.Checked;
      }
      connection.Connect();
    }

    private void ConnectionForm_Load(object sender, EventArgs e)
    {
      var connection = ConnetionToSqlServer.Default();
      tbPassword.Text = connection.Password;
      tbServerName.Text = connection.Server;
      tbUser.Text = connection.UserName;
      tbDataBase.Text = connection.DataBase;
      cbSqlAut.Checked = !connection.WindowsAuthentication;
    }
  }
}
