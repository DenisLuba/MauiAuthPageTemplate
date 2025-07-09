using AuthenticationMaui.Services;
using System.Diagnostics;
using System.Text.RegularExpressions;
using MauiAuthPageTemplate.Exceptions;

namespace MauiAuthPageTemplate.Services;

public class AuthService(ILoginService loginService)
{
    #region Private Constants
    private const string LoginOrEmailKey = "LOGIN_OR_EMAIL_KEY";
    #endregion

    #region RequestVerificationCodeAsync Method
    /// <summary>
    /// Отправляет запрос на генерацию и доставку проверочного кода на указанный номер телефона.
    /// </summary>
    /// <remarks>Этот метод связывается с внешней службой аутентификации для отправки проверочного кода. 
    /// Убедитесь, что указанный номер телефона действителен и правильно отформатирован.</remarks>
    /// <param name="phoneNumber">Номер телефона, на который будет отправлен проверочный код.</param>
    /// <returns>Результат операции типа <see cref="Enum"/> <see cref="Result"/></returns>
    /// <exception cref="Exception">Выбрасывается, если запрос завершился неудачей или ответ не содержит необходимой информации о сессии.</exception>
    public async Task<Result> RequestVerificationCodeAsync(string phoneNumber)
    {
        try
        {
            var isRequested = await StartWaitingAsync(async () =>  
                await loginService.RequestVerificationCodeAsync(phoneNumber, GlobalValues.DefaultTimeout));

            if (isRequested) return Result.Success;

            return Result.Failure;
        }
        catch (NoInternetException ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.NoInternetConnection;
        }
        catch (TimeoutException ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.NoInternetConnection;
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.Failure;
        }
    }
    #endregion

    #region LoginWithVerificationCodeAsync Method
    /// <summary>
    /// Авторизация с помощью проверочного кода, полученного на номер телефона.
    /// </summary>
    /// <param name="code">Код, полученный пользователем в СМС.</param>
    /// <returns>Результат операции типа <see cref="Enum"/> <see cref="Result"/></returns>
    public async Task<Result> LoginWithVerificationCodeAsync(string code)
    {
        try
        {
            var isLogin = await StartWaitingAsync(async () =>
                await loginService.LoginWithVerificationCodeAsync(code, GlobalValues.DefaultTimeout));

            if (!isLogin)
                throw new Exception("Login with the phone failed.");

            return Result.Success;
        }
        catch (NoInternetException ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.NoInternetConnection;
        }
        catch (TimeoutException ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.NoInternetConnection;
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.Failure;
        }
    }
    #endregion

    #region LoginWithEmailAsync with parameters Method
    /// <summary>
    /// Вход с помощью логина (или электронной почты) и пароля.
    /// </summary>
    /// <param name="loginOrEmail">Логин или электронная почта пользователя.</param>
    /// <param name="password">Пароль (не от почты).</param>
    /// <returns>Результат операции типа <see cref="Enum"/> <see cref="Result"/></returns>
    public async Task<Result> LoginWithEmailAsync(string loginOrEmail, string password)
    {
        try
        {
            var result = await StartWaitingAsync(async () =>
            {
                var isLogin = await loginService.LoginWithEmailAsync(loginOrEmail, password, GlobalValues.DefaultTimeout);

                if (!isLogin)
                    throw new Exception("Login with Email failed.");

                await SetCredentialsAsync(loginOrEmail, password);

                return true;
            });

            if (result)
                return Result.Success;
            else
                return Result.Failure;
        }
        catch (NoInternetException ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.NoInternetConnection;
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.Failure;
        }
    }
    #endregion

    #region LoginWithEmailAsync without parameters Method
    /// <summary>
    /// Вход с помощью сохраненных логина (или электронной почты) и пароля.
    /// </summary>
    /// <returns>Результат операции типа <see cref="Enum"/> <see cref="Result"/></returns>
    public async Task<Result> LoginWithEmailAsync()
    {
        try
        {
            var (loginOrEmail, password) = await GetCredentialsAsync();
            if (loginOrEmail is null || password is null) return Result.Failure;

            var isLogin = await loginService.LoginWithEmailAsync(loginOrEmail, password, GlobalValues.DefaultTimeout);

            if (!isLogin)
                throw new Exception("Login with Email failed.");

            return Result.Success;
        }
        catch (NoInternetException ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.NoInternetConnection;
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.Failure;
        }
    }
    #endregion

    #region LoginWithGoogleAsync Method
    /// <summary>
    /// Вход с помощью аккаунта Google.
    /// </summary>
    /// <returns>Результат операции типа <see cref="Enum"/> <see cref="Result"/></returns>
    public async Task<Result> LoginWithGoogleAsync()
    {
        try
        {
            var isLogin = await StartWaitingAsync(async () => await loginService.LoginWithGoogleAsync(GlobalValues.DefaultTimeout));

            if (!isLogin)
                throw new Exception("Login with Google failed.");

            return Result.Success;
        }
        catch (NoInternetException ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.NoInternetConnection;
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.Failure;
        }
    }
    #endregion

    #region LoginWithFacebookAsync Method
    /// <summary>
    /// Вход с помощью аккаунта Facebook.
    /// </summary>
    /// <returns>Результат операции типа <see cref="Enum"/> <see cref="Result"/></returns>
    public async Task<Result> LoginWithFacebookAsync()
    {
        try
        {
            var isLogin = await StartWaitingAsync(async() => await loginService.LoginWithFacebookAsync(GlobalValues.DefaultTimeout));

            if (!isLogin)
                throw new Exception("Login with Google failed.");

            return Result.Success;
        }
        catch (NoInternetException ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.NoInternetConnection;
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.Failure;
        }
    }
    #endregion

    #region RegisterWithEmailAsync Method
    /// <summary>
    /// Регистрация с помощью логина, электронной почты и пароля.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="password">Пароль (не от почты).</param>
    /// <returns>Результат операции типа <see cref="Enum"/> <see cref="Result"/></returns>
    public async Task<Result> RegisterWithEmailAsync(string login, string email, string password)
    {
        try
        {
            var result = await StartWaitingAsync(async () =>
            {
                var isRegistered = await loginService.RegisterWithEmailAsync(EditString(login), EditString(email), password, GlobalValues.DefaultTimeout);

                if (!isRegistered)
                    throw new Exception("Register with Email failed.");

                await SetCredentialsAsync(EditString(email), password);

                return true;
            });

            if (result)
                return Result.Success;
            else
                return Result.Failure;
        }
        catch (NoInternetException ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.NoInternetConnection;
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.Failure;
        }
    }
    #endregion

    #region SendPasswordResetEmailAsync Method
    /// <summary>
    /// Сброс пароля пользователя по электронной почте.
    /// </summary>
    /// <param name="email">Электронная почта пользователя, на которую придет новый пароль.</param>
    /// <returns>true, если сброс пароля прошел успешно, иначе - false.</returns>
    public async Task<Result> SendPasswordResetEmailAsync(string email)
    {
        try
        {
            var result = await StartWaitingAsync(async () =>
            {
                await loginService.SendPasswordResetEmailAsync(EditString(email), GlobalValues.DefaultTimeout);

                await RemoveCredentialsAsync();

                return true;
            });

            if (result)
                return Result.Success;
            else
                return Result.Failure;
        }
        catch (NoInternetException ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.NoInternetConnection;
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.Failure;
        }
    }
    #endregion

    #region LogoutAsync Method
    /// <summary>
    /// Выход из учетной записи. Удаление сохраненных записей об учетной записи из памяти устройства.
    /// </summary>
    /// <returns>Результат операции типа <see cref="Enum"/> <see cref="Result"/></returns>
    public async Task<Result> LogoutAsync()
    {
        try
        {
            var result = await StartWaitingAsync(async () =>
            {
                loginService.Logout();

                await RemoveCredentialsAsync();

                return true;
            });

            if (result)
                return Result.Success;
            else
                return Result.Failure;
        }
        catch (NoInternetException ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.NoInternetConnection;
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
            return Result.Failure;
        }
    }
    #endregion

    #region GetCredentialsAsync Method
    /// <summary>
    /// Получить сохраненные в SecureStorage логин (или электронную почту) и пароль.
    /// </summary>
    /// <returns>Логин (или электронная почта) и пароль. Или (null, null), если нет сохраненного логина.</returns>
    private async Task<(string? loginOrEmail, string? password)> GetCredentialsAsync()
    {
        var loginOrEmail = await SecureStorage.GetAsync(LoginOrEmailKey);
        if (string.IsNullOrEmpty(loginOrEmail)) return (null, null);

        var password = await SecureStorage.GetAsync(loginOrEmail);

        return (loginOrEmail, password);
    }
    #endregion

    #region SetCredentialsAsync Method
    /// <summary>
    /// Сохранить в SecureStorage логин (или электронную почту) и пароль.
    /// </summary>
    /// <param name="loginOrEmail">Логин или электронная почта пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    private async Task SetCredentialsAsync(string loginOrEmail, string password)
    {
        var loginTask = SecureStorage.SetAsync(LoginOrEmailKey, EditString(loginOrEmail));
        var passwordTask = SecureStorage.SetAsync(EditString(loginOrEmail), password);

        await Task.WhenAll(loginTask, passwordTask);
    }
    #endregion

    #region RemoveCredentialsAsync Method
    /// <summary>
    /// Удаляет логин (или электронную почту) и пароль в SecureStorage.
    /// </summary>
    private async Task RemoveCredentialsAsync()
    {
        var (loginOrEmail, _) = await GetCredentialsAsync();
        if (loginOrEmail is null) return;

        SecureStorage.Remove(LoginOrEmailKey);
        SecureStorage.Remove(loginOrEmail);
    }
    #endregion

    #region EditString Method
    /// <summary>
    /// Корректирует строку, удаляя пробелы, и конвертируя ее в нижний регистр.
    /// </summary>
    /// <param name="text">Строка для коррекции.</param>
    /// <returns>Откорректированная строка без пробелов и в нижнем регистре.</returns>
    private string EditString(string text)
        => Regex.Replace(text, @"\s+", string.Empty).ToLower();
    #endregion

    #region StartWaitingAsync Method
    /// <summary>
    /// Перед началом выполнения операции задает значение переменной App.IsBusy равным true. 
    /// После окончания операции возвращает переменной App.IsBusy значение false.
    /// </summary>
    /// <param name="asyncMethod">Асинхронная операция, которая возвращает результат типа bool.</param>
    /// <returns>Возвращает результат операции типа bool.</returns>
    private static async Task<bool> StartWaitingAsync(Func<Task<bool>> asyncMethod)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            throw new NoInternetException();

        var currentApp = Application.Current as App;

        if (currentApp is not null)
            currentApp.IsBusy = true;
        else
            throw new Exception("Application.Current is not of type App or is null.");

        try
        {
            var result = await asyncMethod();
            return result;
        }
        finally
        {
            if (currentApp is not null)
                currentApp.IsBusy = false;
            else
                throw new Exception("Application.Current is not of type App or is null.");
        }
    }
    #endregion
}

public enum Result
{
    Success,
    Failure,
    NoInternetConnection,
    UnknownError
}
