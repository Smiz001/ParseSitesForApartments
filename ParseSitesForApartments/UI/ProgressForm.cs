using System;
using System.Windows.Forms;

namespace ParseSitesForApartments.UI
{
  public partial class ProgressForm : Form
  {
    public ProgressForm()
    {
      InitializeComponent();
    }
    
    public void UpdateProgress(int val)
    {
      pbDownloadInfo.BeginInvoke(
          new Action(() =>
          {
            lbCountFlat.Text = val.ToString();
          }
      ));
    }
  }
}
