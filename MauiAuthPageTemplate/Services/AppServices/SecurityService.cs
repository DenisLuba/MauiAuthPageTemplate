namespace MauiAuthPageTemplate.Services;

public class SecurityService
{
    public string ComputeHash(string input)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hashBytes = System.Security.Cryptography.SHA256.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }

    public async Task SaveHash(string key, string hash)
    {
        await SecureStorage.SetAsync(key, hash);
    }

    public async Task<bool> CheckHash(string key, string hash)
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
}
