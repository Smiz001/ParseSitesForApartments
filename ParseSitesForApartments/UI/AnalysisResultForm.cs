using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ParseSitesForApartments.UI
{
  public partial class AnalysisResultForm : Form
  {

    #region Fields

    private int countFlat;
    private float averPrice;
    private List<float> averPriceForFlat = new List<float>();
    private short priceColumn = -1;
    private short squareColumn = -1;
    private short floorColumn = -1;
    private short dateBuildColumn = -1;
    private short typeBuildColumn = -1;
    private short districtColumn = -1;
    private short distanceCarColumn = -1;
    private short distaneFootColumn = -1;
    private short urlColumn = -1;

    #endregion

    public AnalysisResultForm(List<Metro> listMetro)
    {
      InitializeComponent();
      foreach (var item in listMetro)
      {
        cbMetro.Items.Add(item);
      }

      cbMetro.SelectedIndex = 0;
      cbCountRoom.SelectedIndex = 0;
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
      if (Path.GetExtension(tbPathToFile.Text) == "csv")
      {
        if (CheckHeaderColumnAndSetNumColumns())
        {
          
        }
      }
      else
      {
        MessageBox.Show("Данный файл должен быть с раширением csv","Ошибка",MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private bool CheckHeaderColumnAndSetNumColumns()
    {
      string line = null;
      using (var sr = new StreamReader(tbPathToFile.Text))
      {
        line = sr.ReadLine();
      }
      if (line != null)
      {
        var ar = line.Split(';');
        //Поиск номера колонок
        for (short i = 0; i < ar.Length; i++)
        {
          if (ar[i].ToUpper() == "РАЙОН")
          {
            districtColumn = i;
            break;
          }
        }
        for (short i = 0; i < ar.Length; i++)
        {
          if (ar[i].ToUpper() == "ЦЕНА")
          {
            priceColumn = i;
            break;
          }
        }
        for (short i = 0; i < ar.Length; i++)
        {
          if (ar[i].ToUpper() == "ПЛОЩАДЬ")
          {
            squareColumn = i;
            break;
          }
        }
        for (short i = 0; i < ar.Length; i++)
        {
          if (ar[i].ToUpper() == "ЭТАЖ")
          {
            floorColumn = i;
            break;
          }
        }
        for (short i = 0; i < ar.Length; i++)
        {
          if (ar[i].ToUpper() == "ДАТА ПОСТРОЙКИ")
          {
            dateBuildColumn = i;
            break;
          }
        }
        for (short i = 0; i < ar.Length; i++)
        {
          if (ar[i].ToUpper() == "РАССТОЯНИЕ НА МАШИНЕ")
          {
            distanceCarColumn = i;
            break;
          }
        }
        for (short i = 0; i < ar.Length; i++)
        {
          if (ar[i].ToUpper() == "РАССТОЯНИЕ ПЕШКОМ")
          {
            distaneFootColumn = i;
            break;
          }
        }
        for (short i = 0; i < ar.Length; i++)
        {
          if (ar[i].ToUpper() == "URL")
          {
            urlColumn = i;
            break;
          }
        }

        if (distaneFootColumn == -1)
        {
          MessageBox.Show("Не найдена колонка РАССТОЯНИЕ ПЕШКОМ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
        if (distanceCarColumn == -1)
        {
          MessageBox.Show("Не найдена колонка РАССТОЯНИЕ НА МАШИНЕ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
        if (urlColumn == -1)
        {
          MessageBox.Show("Не найдена колонка URL", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
        if (dateBuildColumn == -1)
        {
          MessageBox.Show("Не найдена колонка ДАТА ПОСТРОЙКИ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
        if (floorColumn == -1)
        {
          MessageBox.Show("Не найдена колонка ЭТАЖ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
        if (squareColumn == -1)
        {
          MessageBox.Show("Не найдена колонка ПЛОЩАДЬ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
        if (priceColumn == -1)
        {
          MessageBox.Show("Не найдена колонка ЦЕНА", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
        if (districtColumn == -1)
        {
          MessageBox.Show("Не найдена колонка РАЙОН", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
      }
      else
      {
        MessageBox.Show("Отсутствуют заголовки в файле", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }

      return true;
    }
  }
}
