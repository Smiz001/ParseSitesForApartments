﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParseSitesForApartments.Sites
{
  public class AllSites:BaseParse
  {
    #region Fileds
    
    private string filename = @"d:\ParserInfo\Appartament\AllSites.csv";
    private BKN bkn;
    private BN bn;
    private ELMS elms;
    private Thread elmsThread;
    private Thread bnThread;
    private Thread bknThread;

    #endregion

    #region Constructor

    public AllSites(List<District> listDistricts, List<Metro> listMetros) : base(listDistricts, listMetros)
    {
      bkn = new BKN(listDistricts, listMetros);
      bn = new BN(listDistricts, listMetros);
      elms = new ELMS(listDistricts, listMetros);

      bkn.Filename = filename;
      bn.Filename = filename;
      elms.Filename = filename;
    }

    #endregion

    #region Properties

    public override string Filename
    {
      get => filename;
      set
      {
        filename = value;
        bkn.Filename = filename;
        bn.Filename = filename;
        elms.Filename = filename;
      }
    }

    public override string FilenameSdam { get; }
    public override string FilenameWithinfo { get; }
    public override string FilenameWithinfoSdam { get; }
    public override string NameSite { get; }

    #endregion

    public override void ParsingSdamAll()
    {
      throw new NotImplementedException();
    }

    private string ExctractPath()
    {
      string path = string.Empty;
      var arr = Filename.Split('\\');
      path = Filename.Replace(arr[arr.Length-1], "");
      return path;
    }

    public override void ParsingAll()
    {
      string path = ExctractPath();
      //TODO нужно переделать обработку сайтов
      // Каждый сайт парсится в свой файл, после того как все спарсилось, все сливается в один файл
      elms.Filename = $@"{path}ELMS.csv";
      //elmsThread = new Thread(elms.ParsingAll);
      //elmsThread.Start();
      bn.Filename = $@"{path}BN.csv";
      //bnThread = new Thread(bn.ParsingAll);
      //bnThread.Start();
      bkn.Filename = $@"{path}BKN.csv";
      //bknThread = new Thread(bkn.ParsingAll);
      //bknThread.Start();

      using (var sw = new StreamWriter(new FileStream(Filename, FileMode.Create), Encoding.UTF8))
      {
        sw.WriteLine(@"Район;Улица;Номер;Корпус;Литер;Кол-во комнат;Площадь;Этаж;Этажей;Цена;Метро;Дата постройки;Дата реконструкции;Даты кап. ремонты;Общая пл. здания, м2;Жилая пл., м2;Пл. нежелых помещений м2;Мансарда м2;Кол-во проживающих;Центральное отопление;Центральное ГВС;Центральное ЭС;Центарльное ГС;Тип Квартир;Кол-во квартир;Дата ТЭП;Виды кап. ремонта;Общее кол-во лифтов;Расстояние пешком;Время пешком;Расстояние на машине;Время на машине;Откуда взято");
        using (var sr = new StreamReader(elms.Filename))
        {
          sr.ReadLine();
          string line;
          while ((line = sr.ReadLine()) != null)
          {
            sw.WriteLine(line);
          }
        }
        File.Delete(elms.Filename);
        using (var sr = new StreamReader(bn.Filename))
        {
          sr.ReadLine();
          string line;
          while ((line = sr.ReadLine()) != null)
          {
            sw.WriteLine(line);
          }
        }
        File.Delete(bn.Filename);
        using (var sr = new StreamReader(bkn.Filename))
        {
          sr.ReadLine();
          string line;
          while ((line = sr.ReadLine()) != null)
          {
            sw.WriteLine(line);
          }
        }
        File.Delete(bkn.Filename);
      }
    }

    public override void ParsingStudii()
    {
      elmsThread = new Thread(elms.ParsingStudii);
      elmsThread.Start();
      bnThread = new Thread(bn.ParsingStudii);
      bnThread.Start();
      bknThread = new Thread(bkn.ParsingStudii);
      bknThread.Start();

    }

    public override void ParsingOne()
    {
      bkn.ParsingOne();
      bn.ParsingOne();
      elms.ParsingOne();
    }

    public override void ParsingTwo()
    {
      bkn.ParsingTwo();
      bn.ParsingTwo();
      elms.ParsingTwo();
    }

    public override void ParsingThree()
    {
      bkn.ParsingThree();
      bn.ParsingThree();
      elms.ParsingThree();
    }

    public override void ParsingFour()
    {
      bkn.ParsingFour();
      bn.ParsingFour();
      elms.ParsingFour();
    }

    public override void ParsingMoreFour()
    {
      bkn.ParsingMoreFour();
      bn.ParsingMoreFour();
      elms.ParsingMoreFour();
    }
  }
}
