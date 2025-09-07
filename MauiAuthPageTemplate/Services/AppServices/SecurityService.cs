namespace MauiAuthPageTemplate.Services;

public class SecurityService
{
    #region ComputeHash Method
    /// <summary>
    /// Вычисляет SHA-256 хэш для заданной строки и возвращает его в виде строки Base64.
    /// </summary>
    public string ComputeHash(string input)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }
    #endregion

    #region SaveHashAsync Method
    /// <summary>
    /// Сохраняет хэш в безопасном хранилище с использованием указанного ключа.
    /// </summary>
    public async Task SaveHashAsync(string key, string hash)
    {
        await SecureStorage.SetAsync(key, hash);
    }
    #endregion

    #region CheckHashAsync Method
    /// <summary>
    /// Проверяет, соответствует ли предоставленный хэш хэшу, сохраненному в безопасном хранилище под указанным ключом.
    /// </summary>
    public async Task<bool> CheckHashAsync(string key, string hash)
    {
        try
        {
            var storedHash = await SecureStorage.GetAsync(key);
            return storedHash == hash;
        }
        catch (Exception)
        {
            // Обрабатывает потенциальные исключения, например, когда устройство не поддерживает безопасное хранилище
            return false;
        }
    }
    #endregion

    #region HasHashAsync Method
    /// <summary>
    /// Проверяет, существует ли хэш в безопасном хранилище под указанным ключом.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<bool> HasHashAsync(string key)
    {
        try
        {
            var storedHash = await SecureStorage.GetAsync(key);
            return !string.IsNullOrEmpty(storedHash);
        }
        catch (Exception)
        {
            // Обрабатывает потенциальные исключения, например, когда устройство не поддерживает безопасное хранилище
            return false;
        }
    } 
    #endregion
}
