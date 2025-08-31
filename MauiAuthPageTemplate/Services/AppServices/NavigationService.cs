using MauiAuthPageTemplate.Services.Interfaces;

namespace MauiAuthPageTemplate.Services;

public class NavigationService : INavigationService
{
    #region Private Flags
    // new(1,1) - SemaphoreSlim только с одним начальным активным потоком и максимум только с одним активным потоком.
    private readonly SemaphoreSlim _modalSemaphore = new(1, 1);
    private readonly SemaphoreSlim _pageSemaphore = new(1, 1);
    #endregion

    #region GetNavigation Method
    /// <summary>
    /// Выдает текущий экземпляр навигации от Shell или Application.
    /// </summary>
    /// <returns><see cref="INavigation"/> для обработки навигации на основе стека.</returns>
    private INavigation? GetNavigation() =>
        Application.Current?
        .Windows.FirstOrDefault(window => window.Page is not null)?
        .Page?
        .Navigation;
    #endregion

    #region PushModalAsync Method
    /// <summary>
    /// Кладет модальную страницу в навигационный стек.
    /// </summary>
    /// <param name="page">Модальная страница (<see cref="Page"/>), которая будет положена в стек навигации.</param>
    /// <param name="animated">Параметр типа <see cref="bool"/> который указывает, должен ли переход быть анимирован.</param>
    public async Task PushModalAsync(Page page, bool animated = false)
    {
        var navigation = GetNavigation();
        if (navigation is null || navigation.ModalStack.Count > 0) return;

        // SemaphoreSlim используется, чтобы обеспечить то, что только одна модальная страница может быть положена в стек за 1 раз.

        // WaitAsync используется для асинхронного ожидания момента, когда семафор станет доступен.
        await _modalSemaphore.WaitAsync();
        try
        {
            await navigation.PushModalAsync(page, animated);
        }
        finally
        {
            // Отпускаем семафор, чтобы позволить положить в стек другие модальные страницы.
            _modalSemaphore.Release();
        }
    }
    #endregion

    #region PopModalAsync Method
    /// <summary>
    /// Убирает верхнюю модальную страницу из навигационного стека.
    /// </summary>
    /// <param name="animated">Параметр типа <see cref="bool"/> который указывает, должен ли переход быть анимирован.</param>
    public async Task PopModalAsync(bool animated = false)
    {
        var navigation = GetNavigation();
        if (navigation is null || navigation.ModalStack.Count == 0) return;

        // SemaphoreSlim используется, чтобы обеспечить то, что только одна модальная страница может быть убрана из стека за 1 раз.

        // WaitAsync используется для асинхронного ожидания момента, когда семафор станет доступен.
        await _modalSemaphore.WaitAsync();
        try
        {
            await navigation.PopModalAsync(animated);
        }
        finally
        {
            // Отпускаем семафор, чтобы дать возможность другим модальным станицам быть убранными из стека навигации.
            _modalSemaphore.Release();
        }
    }
    #endregion

    #region PushAsync Method
    /// <summary>
    /// Кладет страницу в навигационный стек.
    /// </summary>
    /// <param name="page">Страница (<see cref="Page"/>), которая будет положена в стек навигации.</param>
    /// <param name="animated">Параметр типа <see cref="bool"/> который указывает, должен ли переход быть анимирован.</param>
    public async Task PushAsync(Page page, bool animated = false)
    {
        var navigation = GetNavigation();
        if (navigation is null) return;

        // SemaphoreSlim используется, чтобы обеспечить то, что только одна страница может быть положена в стек за 1 раз.

        // WaitAsync используется для асинхронного ожидания момента, когда семафор станет доступен.
        await _pageSemaphore.WaitAsync();
        try
        {
            await navigation.PushAsync(page, animated);
        }
        finally
        {
            // Отпускаем семафор, чтобы позволить положить в стек другие страницы.
            _pageSemaphore.Release();
        }
    }
    #endregion

    #region PopAsync Method
    /// <summary>
    /// Убирает верхнюю страницу из навигационного стека.
    /// </summary>
    /// <param name="animated">Параметр типа <see cref="bool"/> который указывает, должен ли переход быть анимирован.</param>
    public async Task PopAsync(bool animated = false)
    {
        var navigation = GetNavigation();
        if (navigation is null || navigation.NavigationStack.Count <= 1) return;

        // SemaphoreSlim используется, чтобы обеспечить то, что только одна страница может быть убрана из стека за 1 раз.

        // WaitAsync используется для асинхронного ожидания момента, когда семафор станет доступен.
        await _pageSemaphore.WaitAsync();
        try
        {
            await navigation.PopAsync(animated);
        }
        finally
        {
            // Отпускаем семафор, чтобы дать возможность другим станицам быть убранными из стека навигации.
            _pageSemaphore.Release();
        }
    }
    #endregion
}
