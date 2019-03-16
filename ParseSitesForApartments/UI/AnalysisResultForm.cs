using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace ParseSitesForApartments.UI
{
  public partial class AnalysisResultForm : Form
  {

    #region Fields

    private int countFlat;
    private float averPrice;
    private List<float> averPriceForFlatList = new List<float>();
    private short priceColumn;
    private short squareColumn;
    private short typeRoomColumn;
    private short floorColumn;
    private short dateBuildColumn;
    private short typeBuildColumn;
    private short districtColumn;
    private short distanceCarColumn;
    private short distaneFootColumn;
    private short metroColum;
    private short urlColumn;
    private DataTable table;

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
      priceColumn = -1;
      squareColumn = -1;
      floorColumn = -1;
      dateBuildColumn = -1;
      typeBuildColumn = -1;
      districtColumn = -1;
      distanceCarColumn = -1;
      distaneFootColumn = -1;
      urlColumn = -1;
      typeRoomColumn = -1;
      metroColum = -1;
      Cursor.Current = Cursors.WaitCursor;
      if (Path.GetExtension(tbPathToFile.Text) == ".csv")
      {
        if (CheckHeaderColumnAndSetNumColumns())
        {
          CreateColumns();
          ReadFlatsFromFiles();
        }
      }
      else
      {
        MessageBox.Show("Данный файл должен быть с раширением csv", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      Cursor.Current = Cursors.Default;
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
          if (ar[i].ToUpper() == "ОТКУДА ВЗЯТО")
          {
            urlColumn = i;
            break;
          }
        }
        for (short i = 0; i < ar.Length; i++)
        {
          if (ar[i].ToUpper() == "КОЛ-ВО КОМНАТ")
          {
            typeRoomColumn = i;
            break;
          }
        }
        for (short i = 0; i < ar.Length; i++)
        {
          if (ar[i].ToUpper() == "МЕТРО")
          {
            metroColum = i;
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
        if (typeRoomColumn == -1)
        {
          MessageBox.Show("Не найдена колонка КОЛ-ВО КОМНАТ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
        if (metroColum == -1)
        {
          MessageBox.Show("Не найдена колонка МЕТРО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    private void ReadFlatsFromFiles()
    {
      using (var sr = new StreamReader(tbPathToFile.Text))
      {
        string line = sr.ReadLine();
        if (line != null)
        {
          while ((line = sr.ReadLine()) != null)
          {
            var arLine = line.Split(';');
            var row = table.NewRow();
            int price;
            if (int.TryParse(arLine[priceColumn], out price))
            {
              row["Цена"] = price;
            }

            float square;
            if (float.TryParse(arLine[squareColumn], out square))
            {
              row["Площадь"] = square;
            }

            row["Кол-во комнат"] = arLine[typeRoomColumn];

            short floor;
            if (short.TryParse(arLine[floorColumn], out floor))
            {
              row["Этаж"] = floor;
            }
            row["Год постройки"] = arLine[dateBuildColumn];
            row["Район"] = arLine[districtColumn];
            row["Расстояние пешком"] = arLine[distaneFootColumn];
            row["Расстояние на машине"] = arLine[distanceCarColumn];
            if(!string.IsNullOrWhiteSpace(arLine[urlColumn]))
              row["Url"] = new Uri(arLine[urlColumn]);

            if (!string.IsNullOrWhiteSpace(arLine[metroColum]))
              row["Metro"] = arLine[metroColum];
            table.Rows.Add(row);
          }
        }
      }

      var selectMetro = cbMetro.SelectedItem as Metro;
      //EnumerableRowCollection<DataRow> query = from flat in table.AsEnumerable()
      //  where flat.Field<string>("Metro").Contains(selectMetro.Name)
      //  orderby flat.Field<string>("Цена"), flat.Field<string>("Площадь")
      //  select flat;


      var dv =table.DefaultView;
      
      dv.RowFilter = $"Metro = '{selectMetro.Name}'";

      for (int i = 0; i < dv.Count; i++)
      {
        var price = (int)dv[i]["Цена"];
        var square = (float)dv[i]["Площадь"];
        averPriceForFlatList.Add(price / square);
        countFlat++;
      }

      float sum = 0;
      foreach (var val in averPriceForFlatList)
      {
        sum += val;
      }
      averPriceForFlatList.Clear();
      lbCountFlat.Text = countFlat.ToString();
      averPrice = sum / countFlat;
      lbAveragePriceForSquare.Text = averPrice.ToString();

      dataGridView1.DataSource = dv;
    }

    private void CreateColumns()
    {
      dataGridView1.Rows.Clear();
      dataGridView1.Refresh();
      dataGridView1.DataSource = null;


      table = new DataTable();
      var column = new DataColumn("Цена", typeof(int));
      table.Columns.Add(column);

      column = new DataColumn("Площадь", typeof(float));
      table.Columns.Add(column);

      column = new DataColumn("Кол-во комнат", typeof(string));
      table.Columns.Add(column);

      column = new DataColumn("Этаж", typeof(short));
      table.Columns.Add(column);

      column = new DataColumn("Год постройки", typeof(string));
      table.Columns.Add(column);

      column = new DataColumn("Район", typeof(string));
      table.Columns.Add(column);

      column = new DataColumn("Расстояние пешком", typeof(string));
      table.Columns.Add(column);

      column = new DataColumn("Расстояние на машине", typeof(string));
      table.Columns.Add(column);

      column = new DataColumn("Metro", typeof(string));
      table.Columns.Add(column);

      column = new DataColumn("Url", typeof(Uri));
      table.Columns.Add(column);
    }

    private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
      if (!string.IsNullOrWhiteSpace(cell))
      {
        if (cell.Contains("http"))
        {
          System.Diagnostics.Process.Start(cell);
        }
      }
    }
  }
}
