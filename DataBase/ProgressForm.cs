using System;
using System.Windows.Forms;

namespace Core
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
    public void UpdateAllProgress(int val)
    {
      pbDownloadInfo.BeginInvoke(
        new Action(() =>
          {
            lbAllCountFlat.Text = val.ToString();
          }
        ));
    }
  }
}
