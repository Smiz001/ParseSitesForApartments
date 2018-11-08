using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParseSitesForApartments
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
