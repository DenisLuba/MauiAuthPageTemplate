namespace MauiAuthPageTemplate;

internal static class GlobalValues
{
    // Значения из Firebase Console и Google Cloud Console. Их нужно заменить на свои.
    // Смотрите документацию по настройке для проекта AuthenticationMAUI (README файл на странице https://github.com/DenisLuba/AuthenticationMAUI).
    internal const string CALLBACK_SCHEME = "todolist"; // Схема обратного вызова для аутентификации через Google. Например, "myapp" для myapp:// (но можно и myapp:// - это будет отредактировано в конструкторе класса FirebaseLoginService)
    internal const string API_KEY = "AIzaSyBoY2s3D4UsYTzh5z2VeVUpFuOS0R9Z5LE"; // Ваш Web API Key из Firebase Console (Firebase Console > Project Settings > General > "Web API Key")
    internal const string AUTH_DOMAIN = "todolist-6b8ae.firebaseapp.com"; // Обычно это your-project-id.firebaseapp.com (Firebase Console > Authentication > Settings > "Authorized domains")
    internal const string GOOGLE_CLIENT_ID = "327439117326-cn4emri10a9s3m1lfqvr5gp8ged1isms.apps.googleusercontent.com"; // Ваш Google Client ID (Firebase Console > Authentication > Sign-in method > Google > Web SDK configuration > "Web client ID")
    internal const string GOOGLE_REDIRECT_URI = "https://todolist-6b8ae.firebaseapp.com/redirect.html";  // Обычно это "https://your-project-id.firebaseapp.com/__/auth/handler", но "__/auth/handler" меняем на "redirect.html",
                                                                                                                                                                    // чтобы получилось "https://your-project-id.firebaseapp.com/redirect.html"
                                                                                                                                                                    // (Google Cloud Console > APIs & Services > Credentials > Auth 2.0 Client IDs > Web client (auto created by Google Service) > Authorized redirect URIs)

    internal const string MainPage = "//MainPage";
    internal const string AuthPage = "//AuthPage";

    internal const long DefaultTimeout = 10000; // 10 seconds
}
