using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ParseSitesForApartments.UI
{
  public partial class AnalysisResultForm : Form
  {

    #region Fields

    private int countFlat;
    private double averPrice;
    private List<double> averPriceForFlatList = new List<double>();
    private double averageDeviation;
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
    private short isRepairColumn;
    private DataTable table;
    private List<string> listTypeBuild = new List<string>();

    #endregion

    public AnalysisResultForm(List<Metro> listMetro)
    {
      InitializeComponent();
      foreach (var item in listMetro)
      {
        cbMetro.Items.Add(item);
      }

      //cbMetro.SelectedIndex = 0;
      //cbCountRoom.SelectedIndex = 0;
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
      countFlat = 0;
      averPrice = 0;
      averPriceForFlatList.Clear();
      listTypeBuild.Clear();

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
      isRepairColumn = -1;

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
          if (ar[i].ToUpper() == "РАССТОЯНИЕ НА МАШИНЕ, М")
          {
            distanceCarColumn = i;
            break;
          }
        }
        for (short i = 0; i < ar.Length; i++)
        {
          if (ar[i].ToUpper() == "РАССТОЯНИЕ ПЕШКОМ, М")
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
        for (short i = 0; i < ar.Length; i++)
        {
          if (ar[i].ToUpper() == "ТИП ДОМА")
          {
            typeBuildColumn = i;
            break;
          }
        }
        for (short i = 0; i < ar.Length; i++)
        {
          if (ar[i].ToUpper() == "ПРОВОДИЛСЯ КАП.РЕМОНТ")
          {
            isRepairColumn = i;
            break;
          }
        }

        if (distaneFootColumn == -1)
        {
          MessageBox.Show("Не найдена колонка РАССТОЯНИЕ ПЕШКОМ, М", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
        if (distanceCarColumn == -1)
        {
          MessageBox.Show("Не найдена колонка РАССТОЯНИЕ НА МАШИНЕ, М", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        if (typeBuildColumn == -1)
        {
          MessageBox.Show("Не найдена колонка ТИП ДОМА", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return false;
        }
        if (isRepairColumn == -1)
        {
          MessageBox.Show("Не найдена колонка ПРОВОДИЛСЯ КАП.РЕМОНТ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            double square;
            if (double.TryParse(arLine[squareColumn], out square))
            {
              row["Площадь"] = Math.Round(square,2);
            }

            row["Количество_комнат"] = arLine[typeRoomColumn];

            short floor;
            if (short.TryParse(arLine[floorColumn], out floor))
            {
              row["Этаж"] = floor;
            }
            row["Год постройки"] = arLine[dateBuildColumn];
            row["Район"] = arLine[districtColumn];
            if (string.IsNullOrWhiteSpace(arLine[distaneFootColumn]))
              arLine[distaneFootColumn] = "0";
            row["Расстояние пешком"] = int.Parse(arLine[distaneFootColumn]);

            if (string.IsNullOrWhiteSpace(arLine[distanceCarColumn]))
              arLine[distanceCarColumn] = "0";
            row["Расстояние на машине"] = int.Parse(arLine[distanceCarColumn]);
            if(!string.IsNullOrWhiteSpace(arLine[urlColumn]))
              row["Url"] = new Uri(arLine[urlColumn]);

            if (!string.IsNullOrWhiteSpace(arLine[metroColum]))
              row["Metro"] = arLine[metroColum];

            var type = arLine[typeBuildColumn];
            row["Тип_дома"] = type;
            if(!listTypeBuild.Contains(type))
              listTypeBuild.Add(type);
            row["Проводился кап.ремонт"] = arLine[isRepairColumn];
            table.Rows.Add(row);
          }
          cmbTypeBuild.DataSource = listTypeBuild;
        }
      }
      var dv = table.DefaultView;
      var selectMetro = cbMetro.SelectedItem as Metro;
      
      for (int i = 0; i < dv.Count; i++)
      {
        var price = (int)dv[i]["Цена"];
        var square = (double)dv[i]["Площадь"];
        averPriceForFlatList.Add(price / square);
        countFlat++;
      }

      double sum = 0;
      foreach (var val in averPriceForFlatList)
      {
        sum += val;
      }
      averageDeviation = CalculateAverageDeviation(averPriceForFlatList);
      averPriceForFlatList.Clear();
      lbCountFlat.Text = countFlat.ToString();
      averPrice = Math.Round(sum / countFlat,2);
      lbAveragePriceForSquare.Text = averPrice.ToString();
      lblOtklonenie.Text = averageDeviation.ToString();
      dataGridView1.DataSource = dv;
    }

    private void CreateColumns()
    {
      table?.Clear();
      dataGridView1.DataSource = null;


      table = new DataTable();
      var column = new DataColumn("Цена", typeof(int));
      table.Columns.Add(column);

      column = new DataColumn("Площадь", typeof(double));
      table.Columns.Add(column);

      column = new DataColumn("Количество_комнат", typeof(string));
      table.Columns.Add(column);

      column = new DataColumn("Этаж", typeof(short));
      table.Columns.Add(column);

      column = new DataColumn("Год постройки", typeof(string));
      table.Columns.Add(column);

      column = new DataColumn("Район", typeof(string));
      table.Columns.Add(column);

      column = new DataColumn("Расстояние пешком", typeof(int));
      table.Columns.Add(column);

      column = new DataColumn("Расстояние на машине", typeof(int));
      table.Columns.Add(column);

      column = new DataColumn("Metro", typeof(string));
      table.Columns.Add(column);

      column = new DataColumn("Тип_дома", typeof(string));
      table.Columns.Add(column);

      column = new DataColumn("Проводился кап.ремонт", typeof(string));
      table.Columns.Add(column);

      column = new DataColumn("Url", typeof(Uri));
      table.Columns.Add(column);
    }

    private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.RowIndex > -1 && e.ColumnIndex > -1)
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

    private void cbMetro_SelectedIndexChanged(object sender, EventArgs e)
    {
      WorkToFilter();
    }

    private void cbCountRoom_SelectedIndexChanged(object sender, EventArgs e)
    {
      WorkToFilter();
    }

    private void WorkToFilter()
    {
      if (table == null)return;

      var selectMetro = cbMetro.SelectedItem as Metro;
      var selectTypeRoom = cbCountRoom.SelectedItem;
      var dv = table.DefaultView;
      string filter = string.Empty;
      if (selectTypeRoom == null)
      {
        filter = $"Metro = '{selectMetro.Name}'";
      }
      else
      {
        if ("Все комнаты" == selectTypeRoom.ToString())
          filter = $"Metro = '{selectMetro.Name}'";
        else if (selectTypeRoom.ToString() == "Более 4 км.")
          filter = $"Metro = '{selectMetro.Name}' AND Количество_комнат LIKE '%5%' OR Количество_комнат LIKE '%6%' OR Количество_комнат LIKE '%7%' OR Количество_комнат LIKE '%8%' OR Количество_комнат LIKE '%9%'";
        else
          filter = $"Metro = '{selectMetro.Name}' AND Количество_комнат LIKE '%{selectTypeRoom}%'";
      }

      if (cmbTypeBuild.SelectedIndex > -1)
      {
        var type = cmbTypeBuild.SelectedItem.ToString();
        filter = $@"{filter} AND Тип_дома = '{type}'";
      }
      if (!string.IsNullOrWhiteSpace(filter))
      {
        dv.RowFilter = filter;
      }
      countFlat = 0;
      averPrice = 0;
      //var listForAver = new List<double>();
      for (int i = 0; i < dv.Count; i++)
      {
        var price = (int)dv[i]["Цена"];
        var square = (double)dv[i]["Площадь"];
        averPriceForFlatList.Add(price / square);
        countFlat++;
      }

      double sum = 0;
      lblOtklonenie.Text = CalculateAverageDeviation(averPriceForFlatList).ToString();
      foreach (var val in averPriceForFlatList)
      {
        sum += val;
      }
      averPriceForFlatList.Clear();
      lbCountFlat.Text = countFlat.ToString();
      averPrice = Math.Round(sum / countFlat,2);
      lbAveragePriceForSquare.Text = averPrice.ToString();

      dv.Sort = "Цена ASC, Площадь ASC";

      dataGridView1.DataSource = dv;
    }

    private double CalculateAverageDeviation(List<double> listValue)
    {
      if (listValue.Count > 0)
      {
        double sum = 0;
        var aver = listValue.Average();
        var dev = listValue.Count - 1;
        foreach (var item in listValue)
        {
          sum += Math.Pow(item - aver, 2);
        }

        return Math.Round(Math.Sqrt(sum / dev), 2);
      }

      return 0;
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      WorkToFilter();
    }
  }
}
