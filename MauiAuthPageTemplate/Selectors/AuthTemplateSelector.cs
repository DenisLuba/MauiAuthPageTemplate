using MauiAuthPageTemplate.ViewModels;

namespace MauiAuthPageTemplate.Selectors;

public class AuthTemplateSelector : DataTemplateSelector
{
    public DataTemplate? PatternTemplate { get; set; }
    public DataTemplate? PinTemplate { get; set; }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (item is LocalAuthDialogViewModel viewModel)
        {
            if (viewModel.IsPattern) return PatternTemplate ?? throw new NullReferenceException("PatternTemplate is null");
            if (viewModel.IsPinCode) return PinTemplate ?? throw new NullReferenceException("PinTemplate is null");
        }
        throw new NullReferenceException("LocalAuthDialogViewModel item is null.");
    }
}
