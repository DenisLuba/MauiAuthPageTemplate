namespace MauiAuthPageTemplate.Services.Interfaces;

public interface INavigationService
{
    #region PushModalAsync Method
    /// <summary>
    /// Pushes a modal page onto the navigation stack.
    /// </summary>
    /// <param name="page">The modal <see cref="Page"/> that will be pushed on the navigation stack.</param>
    /// <param name="animated">A <see cref="bool"/> parameter that indicates whether the transition should be animated.</param>
    Task PushModalAsync(Page page, bool animated = false);
    #endregion

    #region PopModalAsync Method
    /// <summary>
    /// Pops the top modal page off the navigation stack.
    /// </summary>
    /// <param name="animated">A <see cref="bool"/> parameter that indicates whether the transition should be animated.</param>
    Task PopModalAsync(bool animated = false);
    #endregion

    #region PushAsync Method
    /// <summary>
    /// Pushes a page onto the navigation stack.
    /// </summary>
    /// <param name="page">The <see cref="Page"/> that will be pushed on the navigation stack.</param>
    /// <param name="animated">A <see cref="bool"/> parameter that indicates whether the transition should be animated.</param>
    Task PushAsync(Page page, bool animated = false);
    #endregion

    #region PopAsync Method
    /// <summary>
    /// Pops the top page off the navigation stack.
    /// </summary>
    /// <param name="animated">A <see cref="bool"/> parameter that indicates whether the transition should be animated.</param>
    Task PopAsync(bool animated = false); 
    #endregion
}
