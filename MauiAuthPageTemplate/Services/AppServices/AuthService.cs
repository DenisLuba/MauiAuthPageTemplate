using AuthenticationMaui.Services;
using AuthenticationMAUI.Models;
using MauiAuthPageTemplate.Exceptions;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MauiAuthPageTemplate.Services;

public class AuthService(ILoginService loginService)
{
    #region Private Constants
    private const string LoginOrEmailKey = "LOGIN_OR_EMAIL_KEY";
    #endregion


    // Методы для аутентификации пользователя


    #region RequestVerificationCodeAsync Method
    /// <summary>
    /// Отправляет запрос на генерацию и доставку проверочного кода на указанный номер телефона.
    /// </summary>
    /// <remarks>Этот метод связывается с внешней службой аутентификации для отправки проверочного кода. 
    /// Убедитесь, что указанный номер телефона действителен и правильно отформатирован.</remarks>
    /// <param name="phoneNumber">Номер телефона, на который будет отправлен проверочный код.</param>
    /// <param name="isTest">Параметр указывает, будет ли выполняться метод в тестовом режиме или на реальном устройстве.</param>
    /// <returns>Результат операции типа <see cref="AuthResponse"/></returns>
    public async Task<AuthResponse> RequestVerificationCodeAsync(string phoneNumber, bool isTest) =>
        await GetAuthResponseAsync(async () =>
            await loginService.RequestVerificationCodeAsync(phoneNumber, GlobalValues.DefaultTimeout, isTest, Shell.Current) 
                ? AuthResult.Successful(null, null) 
                : AuthResult.Failed("Failed to request verification code"));
    #endregion

    #region LoginWithVerificationCodeAsync Method
    /// <summary>
    /// Авторизация с помощью проверочного кода, полученного на номер телефона.
    /// </summary>
    /// <param name="code">Код, полученный пользователем в СМС.</param>
    /// <returns>Результат операции типа <see cref="AuthResponse"/></returns>
    public async Task<AuthResponse> LoginWithVerificationCodeAsync(string code) =>
        await GetAuthResponseAsync(async () =>
            await loginService.LoginWithVerificationCodeAsync(code, GlobalValues.DefaultTimeout));
    #endregion

    #region LoginWithEmailAsync with parameters Method
    /// <summary>
    /// Вход с помощью логина (или электронной почты) и пароля.
    /// </summary>
    /// <param name="loginOrEmail">Логин или электронная почта пользователя.</param>
    /// <param name="password">Пароль (не от почты).</param>
    /// <returns>Результат операции типа <see cref="AuthResponse"/></returns>
    public async Task<AuthResponse> LoginWithEmailAsync(string loginOrEmail, string password) =>
        await GetAuthResponseAsync(async () =>
            await loginService.LoginWithEmailAsync(loginOrEmail, password, GlobalValues.DefaultTimeout));
    #endregion

    #region LoginWithEmailAsync without parameters Method
    /// <summary>
    /// Вход с помощью сохраненных логина (или электронной почты) и пароля.
    /// </summary>
    /// <returns>Результат операции типа <see cref="AuthResponse"/></returns>
    public async Task<AuthResponse> LoginWithEmailAsync()
    {
        return await GetAuthResponseAsync(async () =>
        {
            var (loginOrEmail, password) = await GetCredentialsAsync();
            if (loginOrEmail is null || password is null)
                throw new FailureException("Login, Email or Password is null");

            return await loginService.LoginWithEmailAsync(loginOrEmail, password, GlobalValues.DefaultTimeout);
        });
    }
    #endregion

    #region LoginWithGoogleAsync Method
    /// <summary>
    /// Вход с помощью аккаунта Google.
    /// </summary>
    /// <returns>Результат операции типа <see cref="AuthResponse"/></returns>
    public async Task<AuthResponse> LoginWithGoogleAsync() =>
        await GetAuthResponseAsync(async () =>
            await loginService.LoginWithGoogleAsync(GlobalValues.DefaultTimeout));
    #endregion

    #region LoginWithFacebookAsync Method
    /// <summary>
    /// Вход с помощью аккаунта Facebook.
    /// </summary>
    /// <returns>Результат операции типа <see cref="AuthResponse"/></returns>
    public async Task<AuthResponse> LoginWithFacebookAsync() =>
        await GetAuthResponseAsync(async () =>
            await loginService.LoginWithFacebookAsync(GlobalValues.DefaultTimeout));
    #endregion

    #region RegisterWithEmailAsync Method
    /// <summary>
    /// Регистрация с помощью логина, электронной почты и пароля.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="email">Электронная почта пользователя.</param>
    /// <param name="password">Пароль (не от почты).</param>
    /// <returns>Результат операции типа <see cref="AuthResponse"/></returns>
    public async Task<AuthResponse> RegisterWithEmailAsync(string login, string email, string password) =>
        await GetAuthResponseAsync(async () =>
            await loginService.RegisterWithEmailAsync(EditString(login), EditString(email), password, GlobalValues.DefaultTimeout));
    #endregion

    #region SendPasswordResetEmailAsync Method
    /// <summary>
    /// Сброс пароля пользователя по электронной почте.
    /// </summary>
    /// <param name="email">Электронная почта пользователя, на которую придет новый пароль.</param>
    /// <returns>Результат операции типа <see cref="AuthResponse"/></returns>
    public async Task<AuthResponse> SendPasswordResetEmailAsync(string email) =>
        await GetAuthResponseAsync(async () =>
        {
            await loginService.SendPasswordResetEmailAsync(EditString(email), GlobalValues.DefaultTimeout);

            await RemoveCredentialsAsync();

            return AuthResult.Successful(null, null);
        });
    #endregion

    #region LogoutAsync Method
    /// <summary>
    /// Выход из учетной записи. Удаление сохраненных записей об учетной записи из памяти устройства.
    /// </summary>
    /// <returns>Результат операции типа <see cref="AuthResponse"/></returns>
    public async Task<AuthResponse> LogoutAsync() =>
        await GetAuthResponseAsync(async () =>
        {
            loginService.Logout();

            await RemoveCredentialsAsync();

            return AuthResult.Successful(null, null);
        });
    #endregion


    // Вспомогательные методы для работы с логином (или электронной почтой) и паролем в SecureStorage


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
    /// <param name="asyncMethod">Асинхронная операция, которая возвращает результат типа Т.</param>
    /// <returns>Возвращает результат операции.</returns>
    private static async Task<T> StartWaitingAsync<T>(Func<Task<T>> asyncMethod)
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

    #region GetAuthResponseAsync Method
    /// <summary>
    /// Получает результат операции аутентификации и возвращает соответствующий AuthResponse.
    /// </summary>
    /// <param name="asyncMethod">Аснхронная операция, которая возвращает результат типа <see cref="AuthResult"/>?</param>
    /// <returns>Результат операции типа <see cref="AuthResponse"/></returns>
    private static async Task<AuthResponse> GetAuthResponseAsync(Func<Task<AuthResult?>> asyncMethod)
    {
        try
        {
            var authResult = await StartWaitingAsync(async () => await asyncMethod())
                ?? throw new Exception("AuthResult is null");

            if (!authResult.Success)
                throw new FailureException(authResult.ErrorMessage ?? "The AuthResult is unsuccessful. ErrorMessage is null");

            // Сохраняем refreshToken и idToken в SecureStorage для дальнейшего использования,
            // например, для авторизации через ПИН-КОД, графический узор, FaceID или отпечаток пальца
            if (authResult.UserData is not null && authResult.Tokens is not null)
            {
                SecureStorage.Remove(GlobalValues.REFRESH_TOKEN);
                SecureStorage.Remove(GlobalValues.ID_TOKEN);

                await SecureStorage.SetAsync(GlobalValues.REFRESH_TOKEN, authResult.Tokens.RefreshToken);
                await SecureStorage.SetAsync(GlobalValues.ID_TOKEN, authResult.Tokens.IdToken);
            }

                return new SuccessResponse(authResult.UserData, authResult.Tokens);
        }
        catch (NoInternetException ex)
        {
            Trace.WriteLine(ex.Message);
            return new NoInternetResponse();
        }
        catch (TimeoutException ex)
        {
            Trace.WriteLine(ex.Message);
            return new NoInternetResponse();
        }
        catch (FailureException ex)
        {
            Trace.WriteLine(ex.Message);
            return new FailureResponse(ex.Message);
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.Message);
            return new UnknownErrorResponse();
        }
    }
}
#endregion


// Классы для возврата результата операции аутентификации


#region AuthResponse
public abstract record AuthResponse(Result Result);

public enum Result
{
    Success,
    Failure,
    NoInternetConnection,
    UnknownError
}

public record SuccessResponse(UserAuthData? UserAuthData, AuthTokens? AuthTokens)
    : AuthResponse(Result.Success);

public record FailureResponse(string ErrorMessage)
    : AuthResponse(Result.Failure);

public record NoInternetResponse()
    : AuthResponse(Result.NoInternetConnection);

public record UnknownErrorResponse()
    : AuthResponse(Result.UnknownError);
#endregion
