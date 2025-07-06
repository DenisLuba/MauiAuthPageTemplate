using MauiAuthPageTemplate.Services.Interfaces;

namespace MauiAuthPageTemplate.Services;

public class NavigationService : INavigationService
{
    #region Private Flags
    // new(1,1) - SemaphoreSlim with an initial count of 1 active thread and a maximum count of 1 active thread.
    private readonly SemaphoreSlim _modalSemaphore = new(1, 1);
    private readonly SemaphoreSlim _pageSemaphore = new(1, 1);
    #endregion

    #region GetNavigation Method
    /// <summary>
    /// Gets the current navigation instance from the Shell or Application.
    /// </summary>
    /// <returns>The <see cref="INavigation"/> for handling stack-based navigation.</returns>
    private INavigation? GetNavigation() =>
        Application.Current?
        .Windows.FirstOrDefault(window => window.Page is not null)?
        .Page?
        .Navigation;
    #endregion

    #region PushModalAsync Method
    /// <summary>
    /// Pushes a modal page onto the navigation stack.
    /// </summary>
    /// <param name="page">The modal <see cref="Page"/> that will be pushed on the navigation stack.</param>
    /// <param name="animated">A <see cref="bool"/> parameter that indicates whether the transition should be animated.</param>
    public async Task PushModalAsync(Page page, bool animated = false)
    {
        var navigation = GetNavigation();
        if (navigation is null || navigation.ModalStack.Count > 0) return;

        // SemaphoreSlim is used to ensure that only one modal can be pushed at a time.

        // WaitAsync is used to asynchronously wait for the semaphore to be available.
        await _modalSemaphore.WaitAsync();
        try
        {
            await navigation.PushModalAsync(page, animated);
        }
        finally
        {
            // Release the semaphore to allow other modals to be pushed.
            _modalSemaphore.Release();
        }
    }
    #endregion

    #region PopModalAsync Method
    /// <summary>
    /// Pops the top modal page off the navigation stack.
    /// </summary>
    /// <param name="animated">A <see cref="bool"/> parameter that indicates whether the transition should be animated.</param>
    public async Task PopModalAsync(bool animated = false)
    {
        var navigation = GetNavigation();
        if (navigation is null || navigation.ModalStack.Count == 0) return;

        // SemaphoreSlim is used to ensure that only one modal can be popped at a time.

        // WaitAsync is used to asynchronously wait for the semaphore to be available.
        await _modalSemaphore.WaitAsync();
        try
        {
            await navigation.PopModalAsync(animated);
        }
        finally
        {
            // Release the semaphore to allow other modals to be popped.
            _modalSemaphore.Release();
        }
    }
    #endregion

    #region PushAsync Method
    /// <summary>
    /// Pushes a page onto the navigation stack.
    /// </summary>
    /// <param name="page">The <see cref="Page"/> that will be pushed on the navigation stack.</param>
    /// <param name="animated">A <see cref="bool"/> parameter that indicates whether the transition should be animated.</param>
    public async Task PushAsync(Page page, bool animated = false)
    {
        var navigation = GetNavigation();
        if (navigation is null) return;

        // SemaphoreSlim is used to ensure that only one page can be pushed at a time.

        // WaitAsync is used to asynchronously wait for the semaphore to be available.
        await _pageSemaphore.WaitAsync();
        try
        {
            await navigation.PushAsync(page, animated);
        }
        finally
        {
            // Release the semaphore to allow other pages to be pushed.
            _pageSemaphore.Release();
        }
    }
    #endregion

    #region PopAsync Method
    /// <summary>
    /// Pops the top page off the navigation stack.
    /// </summary>
    /// <param name="animated">A <see cref="bool"/> parameter that indicates whether the transition should be animated.</param>
    public async Task PopAsync(bool animated = false)
    {
        var navigation = GetNavigation();
        if (navigation is null || navigation.NavigationStack.Count <= 1) return;

        // SemaphoreSlim is used to ensure that only one page can be popped at a time.

        // WaitAsync is used to asynchronously wait for the semaphore to be available.
        await _pageSemaphore.WaitAsync();
        try
        {
            await navigation.PopAsync(animated);
        }
        finally
        {
            // Release the semaphore to allow other pages to be popped.
            _pageSemaphore.Release();
        }
    }
    #endregion
}
