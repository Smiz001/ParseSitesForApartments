using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ParseSitesForApartments.UI
{
  public partial class AnalysisResultForm : Form
  {
    private List<Metro> listMetro;

    public AnalysisResultForm(List<Metro> listMetro)
    {
      InitializeComponent();
      this.listMetro = new List<Metro>(listMetro);
    }

    private void btnSelectFile_Click(object sender, EventArgs e)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        //openFileDialog.InitialDirectory = "c:\\";
        openFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
        openFileDialog.FilterIndex = 1;
        openFileDialog.RestoreDirectory = true;
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
          tbPathToFile.Text = openFileDialog.FileName;
        }
      }
    }

    private void btnLoad_Click(object sender, EventArgs e)
    {
      //Проверка разрешения файла

    }
  }
}
