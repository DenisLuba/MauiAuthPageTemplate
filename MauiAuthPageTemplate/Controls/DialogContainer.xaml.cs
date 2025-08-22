using System.Windows.Input;

namespace MauiAuthPageTemplate.Controls;

public partial class DialogContainer : ContentView
{
    #region BindableProperties
    #region DialogVerticalOptions
    public static readonly BindableProperty DialogVerticalOptionsProperty =
        BindableProperty.Create(
        propertyName: nameof(DialogVerticalOptions),
        returnType: typeof(LayoutOptions),
        declaringType: typeof(DialogContainer),
        defaultValue: LayoutOptions.Center);
    
    public LayoutOptions DialogVerticalOptions
    {
        get => (LayoutOptions)GetValue(DialogVerticalOptionsProperty);
        set => SetValue(DialogVerticalOptionsProperty, value);
    } 
    #endregion

    #region DialogHorizontalOptions
    public static readonly BindableProperty DialogHorizontalOptionsProperty =
        BindableProperty.Create(
            propertyName: nameof(DialogHorizontalOptions),
            returnType: typeof(LayoutOptions),
            declaringType: typeof(DialogContainer),
            defaultValue: LayoutOptions.Fill);

    public LayoutOptions DialogHorizontalOptions
    {
        get => (LayoutOptions)GetValue(DialogHorizontalOptionsProperty);
        set => SetValue(DialogHorizontalOptionsProperty, value);
    }
    #endregion

    #region DialogMargin
    public static readonly BindableProperty DialogMarginProperty =
        BindableProperty.Create(
            propertyName: nameof(DialogMargin),
            returnType: typeof(Thickness),
            declaringType: typeof(DialogContainer),
            defaultValue: new Thickness(20));

    public Thickness DialogMargin
    {
        get => (Thickness)GetValue(DialogMarginProperty);
        set => SetValue(DialogMarginProperty, value);
    }
    #endregion

    #region DialogPadding
    public static readonly BindableProperty DialogPaddingProperty =
        BindableProperty.Create(
            propertyName: nameof(DialogPadding),
            returnType: typeof(Thickness),
            declaringType: typeof(DialogContainer),
            defaultValue: new Thickness(20));

    public Thickness DialogPadding
    {
        get => (Thickness)GetValue(DialogPaddingProperty);
        set => SetValue(DialogPaddingProperty, value);
    }
    #endregion

    #region DialogCornerRadius
    public static readonly BindableProperty DialogCornerRadiusProperty =
        BindableProperty.Create(
            propertyName: nameof(DialogCornerRadius),
            returnType: typeof(CornerRadius),
            declaringType: typeof(DialogContainer),
            defaultValue: new CornerRadius(16));

    public CornerRadius DialogCornerRadius
    {
        get => (CornerRadius)GetValue(DialogCornerRadiusProperty);
        set => SetValue(DialogCornerRadiusProperty, value);
    }
    #endregion

    #region DialogBoxColor
    public static readonly BindableProperty DialogBoxColorProperty =
BindableProperty.Create(
    propertyName: nameof(DialogBoxColor),
    returnType: typeof(Color),
    declaringType: typeof(DialogContainer),
    defaultValue: Colors.White);

    public Color DialogBoxColor
    {
        get => (Color)GetValue(DialogBoxColorProperty);
        set => SetValue(DialogBoxColorProperty, value);
    } 
    #endregion

    #region DialogBackgroundColor
    public static readonly BindableProperty DialogBackgroundColorProperty =
        BindableProperty.Create(
            propertyName: nameof(DialogBackgroundColor),
            returnType: typeof(Color),
            declaringType: typeof(DialogContainer),
            defaultValue: Color.FromArgb("#80000000"));

    public Color DialogBackgroundColor
    {
        get => (Color)GetValue(DialogBackgroundColorProperty);
        set => SetValue(DialogBackgroundColorProperty, value);
    }
    #endregion

    #region CloseDialogCommand
    /// <summary>
    /// ќпредел€ет <see cref="CloseDialogCommand"/> прив€зываемое свойство.
    /// </summary>
    /// <remarks>Ёто свойство представл€ет собой команду, котора€ может быть выполнена дл€ закрыти€ диалога.</remarks>
    public static readonly BindableProperty CloseDialogCommandProperty =
        BindableProperty.Create(
            propertyName: nameof(CloseDialogCommand),
            returnType: typeof(ICommand),
            declaringType: typeof(DialogContainer),
            defaultValue: null);

    /// <summary>
    /// ѕолучает или устанавливает команду, котора€ выполн€етс€ дл€ закрыти€ диалога.
    /// </summary>
    /// <remarks> оманда может быть св€зана с элементом пользовательского интерфейса, таким как кнопка, чтобы запустить закрытие диалога. 
    /// ”бедитесь, что команда правильно реализована дл€ обработки логики закрыти€ диалога.</remarks>
    public ICommand CloseDialogCommand
    {
        get => (ICommand)GetValue(CloseDialogCommandProperty);
        set => SetValue(CloseDialogCommandProperty, value);
    }
    #endregion
    #endregion

    #region Public Properties
    //  оманда-заглушка
    public ICommand DummyCommand { get; } = new Command(() => { });
    #endregion

    #region Constructor
    public DialogContainer()
    {
        InitializeComponent();
    }
    #endregion
}