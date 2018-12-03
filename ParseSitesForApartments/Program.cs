using log4net;
using log4net.Config;
using System;
using System.Windows.Forms;

namespace ParseSitesForApartments
{
  static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      XmlConfigurator.Configure();
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
      LogManager.Shutdown();
    }
  }
}
