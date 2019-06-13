using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Model
{
  public class MetroModel:NotifyClass
  {
    #region Fields
    private bool isSelected;
    private Guid guid;
    private string nameMetro;
    #endregion

    #region Constructors
    public MetroModel()
    {
    }
    #endregion

    #region Properties
    public bool IsSelected
    {
      get => isSelected;
      set
      {
        if (isSelected == value) return;
        isSelected = value;
        OnPropertyChanged("IsSelected");
      }
    }
    public string NameMetro
    {
      get => nameMetro;
      set
      {
        if (nameMetro == value) return;
        nameMetro = value;
        OnPropertyChanged("NameMetro");
      }
    }

    public Guid Guid
    {
      get => guid;
      set
      {
        if (guid == value) return;
        guid = value;
        OnPropertyChanged("Guid");
      }
    }
    #endregion

    #region Methods
    #endregion
  }
}
