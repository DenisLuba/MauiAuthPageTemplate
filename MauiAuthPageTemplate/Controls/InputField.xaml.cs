using Microsoft.Maui.Converters;
using System.ComponentModel;
using System.Windows.Input;

namespace MauiAuthPageTemplate.Controls;

public partial class InputField : ContentView
{
	#region BindableProperties
	#region InputText
	public static readonly BindableProperty InputTextProperty =
		BindableProperty.Create(
			propertyName: nameof(InputText),
			returnType: typeof(string),
			declaringType: typeof(InputField),
			defaultValue: string.Empty,
			defaultBindingMode: BindingMode.TwoWay);

	public string InputText
	{
		get => (string)GetValue(InputTextProperty);
		set => SetValue(InputTextProperty, value);
	}
	#endregion

	#region Placeholder
	public static readonly BindableProperty PlaceholderProperty =
		BindableProperty.Create(
			propertyName: nameof(Placeholder),
			returnType: typeof(string),
			declaringType: typeof(InputField),
			defaultValue: string.Empty);

	public string Placeholder
	{
		get => (string)GetValue(PlaceholderProperty);
		set => SetValue(PlaceholderProperty, value);
	}
	#endregion

	#region HintText
	public static readonly BindableProperty HintTextProperty =
		BindableProperty.Create(
			propertyName: nameof(HintText),
			returnType: typeof(string),
			declaringType: typeof(InputField),
			defaultValue: string.Empty);

	public string HintText
	{
		get => (string)GetValue(HintTextProperty);
		set => SetValue(HintTextProperty, value);
	}
	#endregion

	#region IsPassword
	public static readonly BindableProperty IsPasswordProperty =
		BindableProperty.Create(
			propertyName: nameof(IsPassword),
			returnType: typeof(bool),
			declaringType: typeof(InputField),
			defaultValue: false);

	public bool IsPassword
	{
		get => (bool)GetValue(IsPasswordProperty);
		set => SetValue(IsPasswordProperty, value);
	}
	#endregion

	#region KeyboardType
	public static readonly BindableProperty KeyboardTypeProperty =
		BindableProperty.Create(
			propertyName: nameof(KeyboardType),
			returnType: typeof(Keyboard),
			declaringType: typeof(InputField),
			defaultValue: Keyboard.Default);

	[TypeConverter(typeof(KeyboardTypeConverter))]
	public Keyboard KeyboardType
	{
		get => (Keyboard)GetValue(KeyboardTypeProperty);
		set => SetValue(KeyboardTypeProperty, value);
	}
	#endregion

	#region ReturnCommand
	public static readonly BindableProperty ReturnCommandProperty =
		BindableProperty.Create(
			propertyName: nameof(ReturnCommand),
			returnType: typeof(ICommand),
			declaringType: typeof(InputField),
			defaultValue: default(ICommand));

	public ICommand ReturnCommand
	{
		get => (ICommand)GetValue(ReturnCommandProperty);
		set => SetValue(ReturnCommandProperty, value);
	}
	#endregion

	#region ReturnInputType
	public static readonly BindableProperty ReturnInputTypeProperty =
		BindableProperty.Create(
			propertyName: nameof(ReturnInputType),
			returnType: typeof(ReturnType),
			declaringType: typeof(InputField),
			defaultValue: ReturnType.Default);

	public ReturnType ReturnInputType
	{
		get => (ReturnType)GetValue(ReturnInputTypeProperty);
		set => SetValue(ReturnInputTypeProperty, value);
	} 
	#endregion

	#region InputImageSource
	public static readonly BindableProperty InputImageSourceProperty =
		BindableProperty.Create(
			propertyName: nameof(InputImageSource),
			returnType: typeof(string),
			declaringType: typeof(InputField),
			defaultValue: default(string));

	public string InputImageSource
	{
		get => (string)GetValue(InputImageSourceProperty);
		set => SetValue(InputImageSourceProperty, value);
	}
	#endregion 
	#endregion

	#region Constructor
	public InputField()
	{
		InitializeComponent();
	} 
	#endregion
}