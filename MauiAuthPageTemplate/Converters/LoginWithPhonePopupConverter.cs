using System.Globalization;
using System.Windows.Input;
using MauiAuthPageTemplate.Resources.Strings.LoginWithPhonePopupResources;

namespace MauiAuthPageTemplate.Converters;

public class LoginWithPhonePopupConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 3)         
            throw new ArgumentException("Expected at least 3 values: isPhone, phone number, and verification code.", nameof(values));
        
        bool IsVerificationCodeDialog = values[0] is bool b && b;

        var element = (parameter as string)?.ToLower() ?? string.Empty;

        if (element == "buttoncommand" && values.Length < 5)
            throw new ArgumentException("Expected at least 5 values for button commands.", nameof(values));

        if (element == "buttoncommand" && (values[3] is not ICommand || values[4] is not ICommand))
            throw new ArgumentException($"An ICommand was expected for the requested values 3 and 4, but an {values[3].GetType()} was received");

        return element switch
        {
            "label" => IsVerificationCodeDialog
            ? ResourcesLoginWithPhonePopup.enter_verification_code
            : ResourcesLoginWithPhonePopup.enter_phone_number,

            "image" => IsVerificationCodeDialog
            ? "verification_code_logo.png"
            : "phone_number_logo.png",

            "entrytext" => IsVerificationCodeDialog
            ? values[2] // Verification code 
            : values[1], // Phone number  

            "entryplaceholder" => IsVerificationCodeDialog
            ? ResourcesLoginWithPhonePopup.verification_code
            : ResourcesLoginWithPhonePopup.phone_number,

            "buttontext" => IsVerificationCodeDialog
            ? ResourcesLoginWithPhonePopup.log_in
            : ResourcesLoginWithPhonePopup.send_verification_code,

            "buttoncommand" => IsVerificationCodeDialog
            ? values[4] as ICommand // LoginWithVerificationCodeCommand
                ?? throw new ArgumentException("Expected RelayCommand for phone number entry", nameof(values))
            : values[3] as ICommand // RequestVerificationCodeCommand
                ?? throw new ArgumentException("Expected RelayCommand for phone number entry", nameof(values)),

            _ => throw new ArgumentException("Invalid parameter value", nameof(parameter))
        };
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
