using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace ParseSitesForApartments.Export
{
  public class ExcelExport:CoreExport
  {
    public ExcelExport(string filename) : base(filename)
    {
    }

    public override void ExecuteWithBaseInfo()
    {
      throw new NotImplementedException();
    }

    public override void Execute()
    {
      Application excel;
      Workbook workbook;
      Worksheet worksheet;
      if (!File.Exists(Filename))
      {
        try
        {
          excel = new Application();
          excel.Visible = false;
          excel.DisplayAlerts = false;

          workbook = excel.Workbooks.Add(Type.Missing);
          worksheet = (Worksheet)workbook.ActiveSheet;
          worksheet.Name = "Основная информация";

          CreateTitle(worksheet);
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
      }
    }

    public virtual void AddFilesInList(object sender, AppendFlatEventArgs arg)
    {
      base.AddFilesInList(sender, arg);
      if (listFlats.Count == 50)
      {
        Execute();
      }
    }

    private void CreateTitle(Worksheet sheet)
    {
      sheet.Cells[1, 1] = "Район";
      sheet.Cells[1, 2] = "Улица";
      sheet.Cells[1, 3] = "Номер";
      sheet.Cells[1, 4] = "Корпус";
      sheet.Cells[1, 5] = "Литер";
      sheet.Cells[1, 6] = "Кол-во комнат";
      sheet.Cells[1, 7] = "Площадь";
      sheet.Cells[1, 8] = "Цена";
      sheet.Cells[1, 9] = "Этаж";
      sheet.Cells[1, 10] = "Этажей";
      sheet.Cells[1, 11] = "Цена";
      sheet.Cells[1, 12] = "Метро";
      sheet.Cells[1, 13] = "Дата постройки";
      sheet.Cells[1, 14] = "Дата реконструкции";
      sheet.Cells[1, 15] = "Даты кап. ремонтов";
      sheet.Cells[1, 16] = "Общая пл. здания, м2";
      sheet.Cells[1, 17] = "Жилая пл., м2";
      sheet.Cells[1, 18] = "Пл. нежелых помещений м2";
      sheet.Cells[1, 19] = "Мансарда м2";
      sheet.Cells[1, 20] = "Кол-во проживающих";
      sheet.Cells[1, 21] = "Центральное отопление";
      sheet.Cells[1, 22] = "Центральное ГВС";
      sheet.Cells[1, 23] = "Центральное ЭС";
      sheet.Cells[1, 24] = "Центарльное ГС";
      sheet.Cells[1, 25] = "Тип Квартир";
      sheet.Cells[1, 26] = "Кол-во квартир";
      sheet.Cells[1, 27] = "Кол-во встроенных нежилых помещений";
      sheet.Cells[1, 28] = "Дата ТЭП";
      sheet.Cells[1, 29] = "Виды кап. ремонта";
      sheet.Cells[1, 30] = "Общее кол-во лифтов";
      sheet.Cells[1, 31] = "Расстояние пешком";
      sheet.Cells[1, 32] = "Время пешком";
      sheet.Cells[1, 33] = "Расстояние на машине";
      sheet.Cells[1, 34] = "Время на машине";
      sheet.Cells[1, 35] = "Ссылка";

      #region Установление ширины колонок

      var range = sheet.Range[sheet.Cells[1, 1], sheet.Cells[1, 1]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 2], sheet.Cells[1,2]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 3], sheet.Cells[1, 3]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 4], sheet.Cells[1, 4]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 5], sheet.Cells[1, 5]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 6], sheet.Cells[1, 6]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 7], sheet.Cells[1, 7]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 8], sheet.Cells[1, 8]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 9], sheet.Cells[1, 9]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 10], sheet.Cells[1, 10]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 11], sheet.Cells[1, 11]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 12], sheet.Cells[1, 12]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 13], sheet.Cells[1, 13]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 14], sheet.Cells[1, 14]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 15], sheet.Cells[1, 15]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 16], sheet.Cells[1, 16]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 17], sheet.Cells[1, 17]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 18], sheet.Cells[1, 18]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 19], sheet.Cells[1, 19]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 20], sheet.Cells[1, 20]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 21], sheet.Cells[1, 21]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 22], sheet.Cells[1, 22]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 23], sheet.Cells[1, 23]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 24], sheet.Cells[1, 24]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 25], sheet.Cells[1, 25]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 26], sheet.Cells[1, 26]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 27], sheet.Cells[1, 27]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 28], sheet.Cells[1, 28]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 29], sheet.Cells[1, 29]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 30], sheet.Cells[1, 30]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 31], sheet.Cells[1, 31]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 32], sheet.Cells[1, 32]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 33], sheet.Cells[1, 33]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 34], sheet.Cells[1, 34]];
      range.EntireColumn.ColumnWidth = 30;

      range = sheet.Range[sheet.Cells[1, 35], sheet.Cells[1, 35]];
      range.EntireColumn.ColumnWidth = 30;

      #endregion
    }
  }
}
