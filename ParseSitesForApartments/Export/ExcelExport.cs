using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace ParseSitesForApartments.Export
{
  public class ExcelExport:CoreExport
  {
    private int startRow = 1;
    private bool isCreatedFile = false;
    public ExcelExport(string filename) : base(filename)
    {
    }

    public override void ExecuteWithBaseInfo()
    {
      throw new NotImplementedException();
    }


    private Application excel = null;
    private Workbook workbook = null;
    private Worksheet worksheet = null;
    public override void Execute()
    {
      if (!isCreatedFile)
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
          foreach (var flat in listFlats)
          {
            AddRow(flat, worksheet);
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message);
        }
        isCreatedFile = true;
      }
      else
      {
        foreach (var flat in listFlats)
        {
          AddRow(flat, worksheet);
        }
      }
      listFlats.Clear();
    }

    public override void AddFlatInList(object sender, AppendFlatEventArgs arg)
    {
      base.AddFlatInList(sender, arg);
      if (listFlats.Count == 50)
      {
        Execute();
      }
    }

    private void CreateTitle(Worksheet sheet)
    {
      if (sheet == null)
        throw new NullReferenceException("Объект sheet не существует");
      sheet.Cells[startRow, 1] = "Район";
      sheet.Cells[startRow, 2] = "Улица";
      sheet.Cells[startRow, 3] = "Номер";
      sheet.Cells[startRow, 4] = "Корпус";
      sheet.Cells[startRow, 5] = "Литер";
      sheet.Cells[startRow, 6] = "Кол-во комнат";
      sheet.Cells[startRow, 7] = "Площадь";
      sheet.Cells[startRow, 8] = "Цена";
      sheet.Cells[startRow, 9] = "Этаж";
      sheet.Cells[startRow, 10] = "Этажей";
      sheet.Cells[startRow, 11] = "Цена";
      sheet.Cells[startRow, 12] = "Метро";
      sheet.Cells[startRow, 13] = "Дата постройки";
      sheet.Cells[startRow, 14] = "Дата реконструкции";
      sheet.Cells[startRow, 15] = "Даты кап. ремонтов";
      sheet.Cells[startRow, 16] = "Общая пл. здания, м2";
      sheet.Cells[startRow, 17] = "Жилая пл., м2";
      sheet.Cells[startRow, 18] = "Пл. нежелых помещений м2";
      sheet.Cells[startRow, 19] = "Мансарда м2";
      sheet.Cells[startRow, 20] = "Кол-во проживающих";
      sheet.Cells[startRow, 21] = "Центральное отопление";
      sheet.Cells[startRow, 22] = "Центральное ГВС";
      sheet.Cells[startRow, 23] = "Центральное ЭС";
      sheet.Cells[startRow, 24] = "Центарльное ГС";
      sheet.Cells[startRow, 25] = "Тип Квартир";
      sheet.Cells[startRow, 26] = "Кол-во квартир";
      sheet.Cells[startRow, 27] = "Кол-во встроенных нежилых помещений";
      sheet.Cells[startRow, 28] = "Дата ТЭП";
      sheet.Cells[startRow, 29] = "Виды кап. ремонта";
      sheet.Cells[startRow, 30] = "Общее кол-во лифтов";
      sheet.Cells[startRow, 31] = "Расстояние пешком";
      sheet.Cells[startRow, 32] = "Время пешком";
      sheet.Cells[startRow, 33] = "Расстояние на машине";
      sheet.Cells[startRow, 34] = "Время на машине";
      sheet.Cells[startRow, 35] = "Ссылка";

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

    private void AddRow(Flat flat, Worksheet sheet)
    {
      startRow++;
      sheet.Cells[startRow, 1] = flat.Building?.District?.Name;
      sheet.Cells[startRow, 2] = flat.Building?.Street;
      sheet.Cells[startRow, 3] = flat.Building?.Number;
      sheet.Cells[startRow, 4] = flat.Building?.Structure;
      sheet.Cells[startRow, 5] = flat.Building?.Liter;
      sheet.Cells[startRow, 6] = flat.CountRoom;
      sheet.Cells[startRow, 7] = flat.Square;
      sheet.Cells[startRow, 8] = flat.Price;
      sheet.Cells[startRow, 9] = flat.Floor;
      sheet.Cells[startRow, 10] = flat.Building.CountFloor;
      sheet.Cells[startRow, 11] = flat.Price;
      sheet.Cells[startRow, 12] = flat.Building?.MetroObj?.Name;
      sheet.Cells[startRow, 13] = flat.Building.DateBuild;
      sheet.Cells[startRow, 14] = flat.Building.DateReconstruct;
      sheet.Cells[startRow, 15] = flat.Building.DateRepair;
      sheet.Cells[startRow, 16] = flat.Building.BuildingSquare;
      sheet.Cells[startRow, 17] = flat.Building.LivingSquare;
      sheet.Cells[startRow, 18] = flat.Building.NoLivingSqaure;
      sheet.Cells[startRow, 19] = flat.Building.MansardaSquare;
      //sheet.Cells[startRow, 20] = flat.Building.CountInternal;
      sheet.Cells[startRow, 21] = flat.Building.Otoplenie;
      sheet.Cells[startRow, 22] = flat.Building.Gvs;
      sheet.Cells[startRow, 23] = flat.Building.Es;
      sheet.Cells[startRow, 24] = flat.Building.Gs;
      sheet.Cells[startRow, 25] = flat.Building.TypeApartaments;
      sheet.Cells[startRow, 26] = flat.Building.CountApartaments;
      sheet.Cells[startRow, 27] = flat.Building.CountInternal;
      sheet.Cells[startRow, 28] = flat.Building.DateTep;
      sheet.Cells[startRow, 29] = flat.Building.TypeRepair;
      sheet.Cells[startRow, 30] = flat.Building.CountLift;
      sheet.Cells[startRow, 31] = flat.Building.DistanceOnFoot;
      sheet.Cells[startRow, 32] = flat.Building.TimeOnFootToMetro;
      sheet.Cells[startRow, 33] = flat.Building.DistanceOnCar;
      sheet.Cells[startRow, 34] = flat.Building.TimeOnCarToMetro;
      sheet.Cells[startRow, 35] = flat.Url;
    }

    public void Save()
    {
      if (listFlats.Count > 0)
      {
        Execute();
      }
      if (excel != null && workbook != null)
      {
        workbook.SaveAs(Filename); ;
        workbook.Close();
        excel.Quit();
      }
    }
  }
}
