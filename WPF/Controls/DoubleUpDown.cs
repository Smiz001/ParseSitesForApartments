using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WPF.Controls
{
  [TemplatePart(Name = "PART_TextBox", Type = typeof(TextBox))]
  [TemplatePart(Name = "PART_ButtonUp", Type = typeof(ButtonBase))]
  [TemplatePart(Name = "PART_ButtonDown", Type = typeof(ButtonBase))]
  public class DoubleUpDown : Control
  {

    private TextBox PART_TextBox = new TextBox();

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();

      TextBox textBox = GetTemplateChild("PART_TextBox") as TextBox;
      if (textBox != null)
      {
        PART_TextBox = textBox;
        PART_TextBox.PreviewKeyDown += textBox_PreviewKeyDown;
        PART_TextBox.TextChanged += textBox_TextChanged;
        PART_TextBox.Text = Value.ToString();
        PART_TextBox.MouseWheel += textBox_MouseWheel;
        PART_TextBox.LostFocus += textBox_LostFocus;
      }
      ButtonBase PART_ButtonUp = GetTemplateChild("PART_ButtonUp") as ButtonBase;
      if (PART_ButtonUp != null)
      {
        PART_ButtonUp.Click += buttonUp_Click;
      }
      ButtonBase PART_ButtonDown = GetTemplateChild("PART_ButtonDown") as ButtonBase;
      if (PART_ButtonDown != null)
      {
        PART_ButtonDown.Click += buttonDown_Click;
      }
    }

    static DoubleUpDown()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleUpDown), new FrameworkPropertyMetadata(typeof(DoubleUpDown)));
    }

    public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
        "ValueChanged", RoutingStrategy.Direct,
        typeof(ValueChangedEventHandler), typeof(DoubleUpDown));
    public event ValueChangedEventHandler ValueChanged
    {
      add
      {
        base.AddHandler(DoubleUpDown.ValueChangedEvent, value);
      }
      remove
      {
        base.RemoveHandler(DoubleUpDown.ValueChangedEvent, value);
      }
    }

    public double MaxValue
    {
      get { return (double)GetValue(MaxValueProperty); }
      set { SetValue(MaxValueProperty, value); }
    }
    public static readonly DependencyProperty MaxValueProperty =
        DependencyProperty.Register("MaxValue", typeof(double), typeof(DoubleUpDown), new FrameworkPropertyMetadata(100D, maxValueChangedCallback, coerceMaxValueCallback));
    private static object coerceMaxValueCallback(DependencyObject d, object value)
    {
      double minValue = ((DoubleUpDown)d).MinValue;
      if ((double)value < minValue)
        return minValue;

      return value;
    }
    private static void maxValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      DoubleUpDown numericUpDown = ((DoubleUpDown)d);
      numericUpDown.CoerceValue(MinValueProperty);
      numericUpDown.CoerceValue(ValueProperty);
    }

    public double MinValue
    {
      get { return (double)GetValue(MinValueProperty); }
      set { SetValue(MinValueProperty, value); }
    }
    public static readonly DependencyProperty MinValueProperty =
        DependencyProperty.Register("MinValue", typeof(double), typeof(DoubleUpDown), new FrameworkPropertyMetadata(0D, minValueChangedCallback, coerceMinValueCallback));
    private static object coerceMinValueCallback(DependencyObject d, object value)
    {
      double maxValue = ((DoubleUpDown)d).MaxValue;
      if ((double)value > maxValue)
        return maxValue;

      return value;
    }
    private static void minValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      DoubleUpDown numericUpDown = ((DoubleUpDown)d);
      numericUpDown.CoerceValue(DoubleUpDown.MaxValueProperty);
      numericUpDown.CoerceValue(DoubleUpDown.ValueProperty);
    }

    public double Increment
    {
      get { return (double)GetValue(IncrementProperty); }
      set { SetValue(IncrementProperty, value); }
    }
    public static readonly DependencyProperty IncrementProperty =
        DependencyProperty.Register("Increment", typeof(double), typeof(DoubleUpDown), new FrameworkPropertyMetadata(1D, null, coerceIncrementCallback));
    private static object coerceIncrementCallback(DependencyObject d, object value)
    {
      DoubleUpDown numericUpDown = ((DoubleUpDown)d);
      double i = numericUpDown.MaxValue - numericUpDown.MinValue;
      if ((double)value > i)
        return i;

      return value;
    }

    public double Value
    {
      get { return (double)GetValue(ValueProperty); }
      set { SetValue(ValueProperty, value); }
    }
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(double), typeof(DoubleUpDown), new FrameworkPropertyMetadata(0D, valueChangedCallback, coerceValueCallback), validateValueCallback);
    private static void valueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      DoubleUpDown numericUpDown = (DoubleUpDown)d;
      ValueChangedEventArgs ea =
          new ValueChangedEventArgs(DoubleUpDown.ValueChangedEvent, d, (double)e.OldValue, (double)e.NewValue);
      numericUpDown.RaiseEvent(ea);
      //if (ea.Handled) numericUpDown.Value = (double)e.OldValue;
      //else 
      numericUpDown.PART_TextBox.Text = e.NewValue.ToString();
    }
    private static bool validateValueCallback(object value)
    {
      double val = (double)value;
      if (val > double.MinValue && val < double.MaxValue)
        return true;
      else
        return false;
    }
    private static object coerceValueCallback(DependencyObject d, object value)
    {
      double val = (double)value;
      double minValue = ((DoubleUpDown)d).MinValue;
      double maxValue = ((DoubleUpDown)d).MaxValue;
      double result;
      if (val < minValue)
        result = minValue;
      else if (val > maxValue)
        result = maxValue;
      else
        result = (double)value;

      return result;
    }

    private void buttonUp_Click(object sender, RoutedEventArgs e)
    {
      Value += Increment;
    }
    private void buttonDown_Click(object sender, RoutedEventArgs e)
    {
      Value -= Increment;
    }

    private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Space)
        e.Handled = true;
    }
    private void textBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      int index = PART_TextBox.CaretIndex;
      double result;
      if (!double.TryParse(PART_TextBox.Text, out result))
      {
        var changes = e.Changes.FirstOrDefault();
        PART_TextBox.Text = PART_TextBox.Text.Remove(changes.Offset, changes.AddedLength);
        PART_TextBox.CaretIndex = index > 0 ? index - changes.AddedLength : 0;
      }
      else if (result < MaxValue && result > MinValue)
      {
        Value = result;
        PART_TextBox.CaretIndex = PART_TextBox.Text.Length;
      }
      else
      {
        PART_TextBox.Text = Value.ToString();
        PART_TextBox.CaretIndex = index > 0 ? index - 1 : 0;
      }
      if (string.IsNullOrEmpty(PART_TextBox.Text))
        Value = 0;
    }

    private void textBox_MouseWheel(object sender, MouseWheelEventArgs e)
    {
      if (e.Delta > 0)
        Value += Increment;
      else
        Value -= Increment;
    }
    private void textBox_LostFocus(object sender, RoutedEventArgs e)
    {
      if (string.IsNullOrEmpty(PART_TextBox.Text))
        PART_TextBox.Text = "0";

    }
  }
}
