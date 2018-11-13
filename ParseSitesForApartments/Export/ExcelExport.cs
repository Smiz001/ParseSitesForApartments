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
      string str =
        @"Район;Улица;Номер;Корпус;Литер;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Дата постройки;Дата реконструкции;Даты кап. ремонтов;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Кол-во встроенных нежилых помещений;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов;Расстояние пешком;Время пешком;Расстояние на машине;Время на машине;Откуда взято";
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

      #endregion
    }
  }
}
