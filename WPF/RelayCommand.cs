using System;
using System.Diagnostics;
using System.Windows.Input;

namespace WPF
{
  public class RelayCommand<T> : ICommand
  {
    #region Fields

    readonly Action<T> _execute = null;
    readonly Predicate<T> _canExecute = null;
    readonly bool canExecute = false;

    #endregion // Fields

    #region Constructors

    public RelayCommand(Action<T> execute)
      : this(execute, null)
    {
    }

    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    /// <param name="canExecute">The execution status logic.</param>
    public RelayCommand(Action<T> execute, Predicate<T> canExecute)
    {
      if (execute == null)
        throw new ArgumentNullException("execute");

      _execute = execute;
      _canExecute = canExecute;
    }

    public RelayCommand(Action<T> execute, bool canExecute)
    {
      this._execute = execute;
      this.canExecute = canExecute;
    }

    #endregion // Constructors

    #region ICommand Members

    [DebuggerStepThrough]
    public bool CanExecute(object parameter)
    {
      return _canExecute == null ? true : _canExecute((T)parameter);
    }

    public event EventHandler CanExecuteChanged
    {
      add
      {
        if (_canExecute != null)
          CommandManager.RequerySuggested += value;
      }
      remove
      {
        if (_canExecute != null)
          CommandManager.RequerySuggested -= value;
      }
    }

    public void Execute(object parameter)
    {
      _execute((T)parameter);
    }

    #endregion // ICommand Members
  }

  /// <summary>
  /// A command whose sole purpose is to 
  /// relay its functionality to other
  /// objects by invoking delegates. The
  /// default return value for the CanExecute
  /// method is 'true'.
  /// </summary>
  public class RelayCommand : ICommand
  {
    #region Fields

    private Action<object> action;
    readonly Action _execute;
    private bool canExecute = true;
    readonly Func<bool> canExecuteFunc;

    #endregion // Fields

    #region Constructors

    /// <summary>
    /// Creates a new command that can always execute.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    public RelayCommand(Action execute)
      : this(execute, null)
    {
    }


    public RelayCommand(Action<object> action, bool canExecute)
    {
      this.action = action;
      this.canExecute = canExecute;
    }

    public RelayCommand(Action<object> action)
    {
      this.action = action;
    }

    public RelayCommand(Action action, bool canExecute)
    {
      _execute = action;
      this.canExecute = canExecute;
    }

    /// <summary>
    /// Creates a new command.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    /// <param name="canExecute">The execution status logic.</param>
    public RelayCommand(Action execute, Func<bool> canExecute)
    {
      if (execute == null)
        throw new ArgumentNullException("execute");

      _execute = execute;
      canExecuteFunc = canExecute;
    }

    #endregion // Constructors

    #region ICommand Members

    [DebuggerStepThrough]
    public bool CanExecute(object parameter)
    {
      return canExecuteFunc == null ? canExecute : canExecuteFunc();
    }

    public event EventHandler CanExecuteChanged
    {
      add
      {
        if (canExecuteFunc != null)
          CommandManager.RequerySuggested += value;
      }
      remove
      {
        if (canExecuteFunc != null)
          CommandManager.RequerySuggested -= value;
      }
    }

    public void Execute(object parameter)
    {
      if (_execute != null)
      {
        _execute();
        return;
      }
      else if (action != null)
      {
        action(parameter);
        return;
      }
    }

    #endregion // ICommand Members
  }
}
