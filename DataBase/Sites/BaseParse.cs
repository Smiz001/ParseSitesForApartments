﻿using System.Collections.Generic;
using System.Reflection;
using Core.Enum;
using Core.MainClasses;
using log4net;
using Core.Proxy;
using System;
using System.IO;
using System.Text;

namespace Core.Sites
{
  public abstract class BaseParse
  {
    //public CoreExport export;
    //public delegate void Append(object sender, AppendFlatEventArgs e);
    //public event Append OnAppend;

    public BaseParse(List<District> listDistricts, List<Metro> listMetros, List<ProxyInfo> listProxy)
    {
      ListDistricts = new List<District>(listDistricts);
      ListMetros = new List<Metro>(listMetros);
      if(listProxy != null)
        ListProxy = new List<ProxyInfo>(listProxy);
    }

    protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    protected List<District> ListDistricts { get; private set; }
    protected List<Metro> ListMetros { get; private set; }
    protected List<ProxyInfo> ListProxy { get; private set; }
    public abstract string Filename{get; set; }
    public abstract string FilenameSdam { get; }
    public abstract string FilenameWithinfo { get; }
    public abstract string FilenameWithinfoSdam { get; }
    public abstract string NameSite { get; }
    public TypeParseFlat TypeParseFlat { get; set; }
    public bool IsFinished { get; set; } = false;

    public abstract void ParsingAll();
    public abstract void ParsingStudii();
    public abstract void ParsingOne();
    public abstract void ParsingTwo();
    public abstract void ParsingThree();
    public abstract void ParsingFour();
    public abstract void ParsingMoreFour();

    protected abstract void CalcAverPrice();
    public abstract void ParsingSdamAll();

    protected string ExctractPath()
    {
      string path = string.Empty;
      var arr = Filename.Split('\\');
      path = Filename.Replace(arr[arr.Length - 1], "");
      return path;
    }

    protected string CreateExportForRoom(string typeRoom)
    {
      var path = ExctractPath();
      path = $@"{path}{typeRoom}-{DateTime.Now.ToShortDateString()}-{NameSite}.csv";
      if (!File.Exists(Filename))
      {
        File.Delete(path);
      }

      using (var sw = new StreamWriter(new FileStream(path, FileMode.Create), Encoding.UTF8))
      {
        sw.WriteLine(@"Район;Улица;Номер;Корпус;Литер;Кол-во комнат;Площадь;Этаж;Цена;Метро;Откуда взято");
      }

      return path;
    }
  }
}
