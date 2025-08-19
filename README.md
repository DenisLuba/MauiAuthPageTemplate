# 🚀 MAUI Firebase Auth Template

Многоразовый шаблон проекта на [.NET MAUI](https://learn.microsoft.com/dotnet/maui/) с полной реализацией аутентификации через **Firebase**:

- 🔑 Email/Password регистрация и вход
- 📱 Вход по номеру телефона (SMS)
- 🌐 Google Sign-In
- 🌐 Facebook Sign-In
- 🔁 Сброс пароля
- ✅ MVVM архитектура
- 🧩 DI-сервисы и Shell-навигация
- 🟦 Поддержка Android, iOS, Windows, macOS

---

## 🧱 Структура

```bash
📦 YourApp/
├── 📁 ViewModels/
├── 📁 Pages/
├── 📁 Dialogs/
├── 📁 Converters/
├── 📁 Exceptions/
├── 📁 Services/
├── 📁 Resources/
├── 📁 Platforms/
│   ├── Android/ (MainActivity.cs, google-services.json)
│   └── iOS/ (Info.plist, GoogleService-Info.plist)
├── AppShell.xaml
├── App.xaml.cs
└── MauiProgram.cs

# 🚀 Быстрый старт
## 1. Создай проект на основе шаблона
👉 Нажми Use this template → Назови проект → Клонируй:
```bash
git clone https://github.com/your-username/YourAppName.git
cd YourAppName
```

## 2. Настрой Firebase
### Создай проект на https://console.firebase.google.com

### Включи нужные методы входа:

   * Email/Password

   * Phone (SMS)

   * Google
     
   * Firebase

### Скачай конфиги:

   * google-services.json для Android → положи в Platforms/Android/

   * GoogleService-Info.plist для iOS → положи в Platforms/iOS/

## 3. Проверь настройки проекта

   * Укажи правильные REVERSED_CLIENT_ID в iOS Info.plist

   * Укажи google-services.json путь в Android MainActivity.cs если используется вручную

   * Укажи Bundle ID, совпадающий с Firebase в обоих платформах

## 4. Запусти
```bash
dotnet build
dotnet maui run --device Pixel_5_Android_API_33
```

## 🧪 TODO после клонирования
 Переименовать пространство имён проекта

   * Настроить Firebase SDK
     
   * Для настройки методов входа используй файл README для проекта [AuthenticationMAUI](https://github.com/DenisLuba/AuthenticationMAUI)

   * Заменить ресурсы (иконки, цвета, названия)

   * Обновить MainPage, AppShell под свою логику


