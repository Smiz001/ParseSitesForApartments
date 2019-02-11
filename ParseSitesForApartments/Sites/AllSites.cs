using System;
using System.Collections.Generic;
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

    public override void ParsingAll()
    {
      //TODO нужно переделать обработку сайтов
      // Каждый сайт парсится в свой файл, после того как все спарсилось, все сливается в один файл
      elmsThread = new Thread(elms.ParsingAll);
      elmsThread.Start();
      bnThread = new Thread(bn.ParsingAll);
      bnThread.Start();
      bknThread = new Thread(bkn.ParsingAll);
      bknThread.Start();
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
