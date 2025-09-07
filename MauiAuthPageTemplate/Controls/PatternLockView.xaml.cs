using System.Diagnostics;

namespace MauiAuthPageTemplate.Controls;

public partial class PatternLockView : ContentView
{
    #region Private Variables
    private readonly PatternLockDrawable _drawable;
    #endregion

    #region Events
    public event EventHandler<string>? PatternCompleted; 
    #endregion

    #region Constructor
    public PatternLockView()
    {
        InitializeComponent();

        Canvas.Drawable = _drawable = new PatternLockDrawable();

        // Подписки на жесты
        Canvas.StartInteraction += OnStartInteraction; // Начало взаимодействия
        Canvas.DragInteraction += OnDragInteraction; // Перетаскивание
        Canvas.EndInteraction += OnEndInteraction; // Завершение взаимодействия
    }
    #endregion

    #region OnStartInteraction Method
    private void OnStartInteraction(object? sender, TouchEventArgs e)
    {
        _drawable.StartPath(e.Touches.First());// Начинаем новый путь с первой точки касания
        Canvas.Invalidate(); // Перерисовываем Canvas
    } 
    #endregion

    #region OnDragInteraction Method
    private void OnDragInteraction(object? sender, TouchEventArgs e)
    {
        _drawable.UpdatePath(e.Touches.First()); // Обновляем путь с текущей точкой касания
        Canvas.Invalidate(); // Перерисовываем Canvas
    } 
    #endregion

    #region OnEndInteraction Method
    private void OnEndInteraction(object? sender, TouchEventArgs e)
    {
        _drawable.EndPath(); // Завершаем путь
        Canvas.Invalidate(); // Перерисовываем Canvas

        var result = string.Join("", _drawable.SelectedPoints);

        //LOG
        Trace.WriteLine($"PatternLockView - OnEndInteraction: {result}");

        if (_drawable.SelectedPoints.Any()) // Если есть выбранные точки
        {
            PatternCompleted?.Invoke(this, result); // Вызываем событие с результатом 
            Clear();
        }
    }
    #endregion

    #region Clear Method
    /// <summary>
    /// Очищает выбранные точки и перерисовывает Canvas.
    /// </summary>
    public void Clear()
    {
        _drawable.Clear(); // Очищаем выбранные точки
        Canvas.Invalidate(); // Перерисовываем Canvas
    } 
    #endregion
}