using System.Windows.Input;

namespace MauiAuthPageTemplate.Controls;

public partial class PinCodeLockView : ContentView
{
    #region Events
    public event EventHandler<string>? PinCodeCompleted; 
    #endregion

    #region Bindable Properties
    public static readonly BindableProperty PinCodeProperty =
        BindableProperty.Create(
            propertyName: nameof(PinCode),
            returnType: typeof(string),
            declaringType: typeof(PinCodeLockView),
            defaultValue: string.Empty);

    public string PinCode
    {
        get => (string)GetValue(PinCodeProperty);
        set => SetValue(PinCodeProperty, value);
    }

    public static readonly BindableProperty AddDigitCommandProperty =
        BindableProperty.Create(
            propertyName: nameof(AddDigitCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(PinCodeLockView));

    public ICommand AddDigitCommand
    {
        get => (ICommand)GetValue(AddDigitCommandProperty);
        set => SetValue(AddDigitCommandProperty, value);
    }

    public static readonly BindableProperty RemoveDigitCommandProperty =
        BindableProperty.Create(
            propertyName: nameof(RemoveDigitCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(PinCodeLockView));

    public ICommand RemoveDigitCommand
    {
        get => (ICommand)GetValue(RemoveDigitCommandProperty);
        set => SetValue(RemoveDigitCommandProperty, value);
    }
    #endregion

    #region Constructor
    public PinCodeLockView()
    {
        InitializeComponent();

        AddDigitCommand = new Command<string>(AddDigit);
        RemoveDigitCommand = new Command(RemoveDigit);
    }
    #endregion

    #region AddDigit Method
    private void AddDigit(string digit)
    {
        if (PinCode.Length < 4)
        {
            PinCode += digit;
        }
        if (PinCode.Length == 4)
        {
            PinCode = string.Empty;

            PinCodeCompleted?.Invoke(this, PinCode);
        }
    }
    #endregion

    #region RemoveDigit Method
    public void RemoveDigit()
    {
        if (!string.IsNullOrEmpty(PinCode))
        {
            PinCode = PinCode[..^1];
        }
    }
    #endregion

    #region Clear Method
    public void Clear()
    {
        PinCode = string.Empty;
    } 
    #endregion
}