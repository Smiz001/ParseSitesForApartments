using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
      bkn.ParsingAll();
      bn.ParsingAll();
      elms.ParsingAll();
    }

    public override void ParsingStudii()
    {
      bkn.ParsingStudii();
      bn.ParsingStudii();
      elms.ParsingStudii();
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
