using System;
using System.Windows.Forms;

namespace ParseSitesForApartments.UI
{
  public partial class ProgressForm : Form
  {
    public ProgressForm(string title)
    {
      InitializeComponent();
      Text += $" {title}";
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
