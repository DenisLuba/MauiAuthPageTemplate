using System.Diagnostics;
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

        //if (element == "buttoncommand" && (values[3] is not ICommand || values[4] is not ICommand))
        //    throw new ArgumentException($"An ICommand was expected for the requested values 3 and 4, but an {values[3].GetType()} was received");

        if (element == "buttoncommand")
        {
            var requestCmd = values.ElementAtOrDefault(3) as ICommand;
            var loginCmd = values.ElementAtOrDefault(4) as ICommand;

            return IsVerificationCodeDialog
                ? loginCmd ?? Binding.DoNothing
                : requestCmd ?? Binding.DoNothing;
        }

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

            _ => throw new ArgumentException("Invalid parameter value", nameof(parameter))
        };
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        var element = (parameter as string)?.ToLower() ?? string.Empty;

        // ConvertBack нужен только для Entry.Text, у которой есть двухсторонняя привязка
        if (element != "entrytext")
            throw new NotImplementedException("ConvertBack is only implemented for entrytext");

        if (value is not string phoneNumberOrCode)
            throw new ArgumentException("Expected string value from Entry.Text", nameof(value));

        // Возвращаем нужное значение в нужный индекс [IsVerificationCodeDialog, PhoneNumber, Code] для MultiBinding. 
        // Вот сюда:
        // <MultiBinding Converter="{StaticResource LoginWithPhonePopupConverter}" ConverterParameter="entrytext">
        //    < Binding Path = "IsVerificationCodeDialog" />
        //    < Binding Path = "PhoneNumber" />
        //    < Binding Path = "Code" />
        //</ MultiBinding >
        return [
            Binding.DoNothing, // IsVerificationCodeDialog не трогаем
            phoneNumberOrCode, // PhoneNumber (если IsVerificationCodeDialog == false)
            phoneNumberOrCode // Code (если IsVerificationCodeDialog == true)
        ];
    }
}
