using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Model
{
  public class TypeRoomModel:NotifyClass
  {
    #region Fields
    private bool isSelected;
    private string nameTypeRoom;
    #endregion

    #region Constructors
    public TypeRoomModel()
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
    public string NameTypeRoom
    {
      get => nameTypeRoom;
      set
      {
        if (nameTypeRoom == value) return;
        nameTypeRoom = value;
        OnPropertyChanged("NameTypeRoom");
      }
    }
    #endregion

    #region Methods
    #endregion
  }
}
