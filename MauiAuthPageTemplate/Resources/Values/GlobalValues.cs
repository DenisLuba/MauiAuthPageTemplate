namespace MauiAuthPageTemplate;

internal static class GlobalValues
{
    // Значения из Firebase Console и Google Cloud Console. Их нужно заменить на свои.
    // Смотрите документацию по настройке для проекта AuthenticationMAUI (README файл на странице https://github.com/DenisLuba/AuthenticationMAUI).
    internal const string CALLBACK_SCHEME = "mauiauthenticationtemplate"; // Схема обратного вызова для аутентификации через Google. Например, "myapp" для myapp:// (но можно и myapp:// - это будет отредактировано в конструкторе класса FirebaseLoginService)
    internal const string API_KEY = "AIzaSyAW2C23mTiTr0njb4RgvUFUoCOOdF0SQzk"; // Ваш Web API Key из Firebase Console (Firebase Console > Project Settings > General > "Web API Key")
    internal const string AUTH_DOMAIN = "mauiauthenticationtemplate.firebaseapp.com"; // Обычно это your-project-id.firebaseapp.com (Firebase Console > Authentication > Settings > "Authorized domains")
    
    internal const string GOOGLE_CLIENT_ID = "314826889709-ncla25um0ebrof3b91emujf3n15clgg5.apps.googleusercontent.com"; // Ваш Google Client ID (Firebase Console > Authentication > Sign-in method > Google > Web SDK configuration > "Web client ID")
    internal const string REDIRECT_URI = "https://mauiauthenticationtemplate.firebaseapp.com/redirect.html";  // Обычно это "https://your-project-id.firebaseapp.com/__/auth/handler", но "__/auth/handler" меняем на "redirect.html",
                                                                                                              // чтобы получилось "https://your-project-id.firebaseapp.com/redirect.html"
                                                                                                              // (Google Cloud Console > APIs & Services > Credentials > Auth 2.0 Client IDs > Web client (auto created by Google Service) > Authorized redirect URIs)
                                                                                                              // также для Facebook в Valid OAuth Redirect URIs

    internal const bool IS_TEST = true; // Если true, то используется тестовый токен reCAPTCHA, иначе - реальный токен. Тестовый токен не требует проверки и всегда возвращает успешный результат.
    internal const string RECAPTCHA_TOKEN = "test"; // ReCaptcha для аутентификации по номеру телефона
    internal const string SITE_KEY = "6LfOnpgrAAAAAHZCjNkjAwIHlHBSK5c4SfM8cdzP"; // публичный ключ, который выдает Google при регистрации reCAPTCHA (вставляется в HTML/JS и передается в клиент).
    internal const string SECRET_KEY = "6LfOnpgrAAAAAM4TvNXNjC89xggz29ojs3RgyoGl"; // секретный ключ, который выдает Google при регистрации reCAPTCHA (используется только на сервере для проверки токена).

    internal const string FACEBOOK_APP_ID = "768409865679529"; // Ваш Facebook App ID (Facebook for Developers > My Apps > [Your App] > Settings > Basic > App ID)
    internal const string FACEBOOK_APP_SECRET = "45c672bde61b88e7dc533e09de577c7b"; // Ваш Facebook App Secret (Facebook for Developers > My Apps > [Your App] > Settings > Basic > App Secret)



    internal const string MainPage = "//MainPage";
    internal const string AuthPage = "//AuthPage";
    internal const string FingerprintPage = "//FingerprintPage";
    internal const string FaceIdPage = "//FaceIdPage";
    internal const string PatternPage = "//PatternPage";
    internal const string PinCodePage = "//PinCodePage";

    internal const long DefaultTimeout = 100000; // 10 seconds
}

// Примечание: значения CALLBACK_SCHEME, API_KEY, AUTH_DOMAIN, GOOGLE_CLIENT_ID, GOOGLE_REDIRECT_URI, FACEBOOK_APP_ID, FACEBOOK_APP_SECRET должны быть заменены на ваши собственные значения из Firebase Console и Google Cloud Console.