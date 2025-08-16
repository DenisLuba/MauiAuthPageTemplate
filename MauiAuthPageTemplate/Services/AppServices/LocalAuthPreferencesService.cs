using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    private const string StorageKey = "local_auth_method";

    public async Task SetAuthMethodAsync(LocalAuthMethod method) =>
        await SecureStorage.SetAsync(StorageKey, ((int)method).ToString());

    public async Task<LocalAuthMethod> GetAuthMethodAsync() =>
        int.TryParse(await SecureStorage.GetAsync(StorageKey), out var flags) 
        ? (LocalAuthMethod)flags
        : LocalAuthMethod.None;

    public void ClearAuthMethod() => SecureStorage.Remove(StorageKey);
}
