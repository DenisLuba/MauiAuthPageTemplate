namespace MauiAuthPageTemplate.Services;

[Flags]
public enum LocalAuthMethod
{
    None = 0,
    Fingerprint = 1 << 0,
    FaceId = 1 << 1,
    Pattern = 1 << 2,
    PinCode = 1 << 3
}

public class LocalAuthPreferencesService
{
    #region Constants
    private const string SELECTED_ENTER_METHOD = "local_auth_method"; 
    #endregion

    #region SetAuthMethodAsync Method
    /// <summary>
    /// Устанавливает метод входа в приложение.
    /// </summary>
    /// <param name="method">Метод входа в приложение.</param>
    /// <returns></returns>
    public async Task SetAuthMethodAsync(LocalAuthMethod method) =>
        await SecureStorage.SetAsync(SELECTED_ENTER_METHOD, ((int)method).ToString());
    #endregion

    #region GetAuthMethodAsync Method
    /// <summary>
    /// Возвращает метод входа в приложение.
    /// </summary>
    /// <returns>Метод входа в приложение.</returns>
    public async Task<LocalAuthMethod> GetAuthMethodAsync() =>
        int.TryParse(await SecureStorage.GetAsync(SELECTED_ENTER_METHOD), out var flags)
        ? (LocalAuthMethod)flags
        : LocalAuthMethod.None;
    #endregion

    #region CleanAuthMethod Method
    /// <summary>
    /// Очищает сохраненные методы входа в приложение
    /// </summary>
    public void ClearAuthMethod() => SecureStorage.Remove(SELECTED_ENTER_METHOD);
    #endregion
}
