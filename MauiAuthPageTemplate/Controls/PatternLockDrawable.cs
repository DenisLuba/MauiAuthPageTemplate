namespace MauiAuthPageTemplate.Controls;

public class PatternLockDrawable : IDrawable
{
    #region Private Values
    private const float CircleRadius = 25; // Радиус круга для каждой точки
    private readonly List<PointF> _gridPoints; // Список точек сетки
    private readonly List<int> _selectedPoints = []; // Список выбранных точек (индексы в _gridPoints)
    private PointF? _currentTouch; // Текущее касание (null, если нет касания)
    #endregion

    #region Public Properties
    public IReadOnlyList<int> SelectedPoints => _selectedPoints;
    #endregion

    #region Constructor
    public PatternLockDrawable()
    {
        _gridPoints = GenerateGrid(3, 3, 300, 300);
    }
    #endregion

    #region Draw Method
    /// <summary>
    /// Рисует содержимое на указанный холст в данном прямоугольнике.
    /// </summary>
    /// <param name="canvas">Представляет собой не зависящий от платформы холст, 
    /// на котором можно рисовать 2D-графику, используя типы из пространства имен Microsoft.Maui.Graphics.</param>
    /// <param name="dirtyRect">Представляет собой прямоугольник с координатами x, y, шириной и высотой типа float.</param>
    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeSize = 2; // Толщина линии

        // Рисуем точки
        for (int i = 0; i < _gridPoints.Count; i++)
        {
            var point = _gridPoints[i];
            // Если точка выбрана, будем раскрашивать её синим цветом, иначе серым
            canvas.FillColor = _selectedPoints.Contains(i) ? Colors.Blue : Colors.Gray;
            // В точках рисуем круги, раскрашенные выбранным цветом 
            canvas.FillCircle(point.X, point.Y, CircleRadius);
        }

        // Рисуем линии между выбранными точками
        canvas.StrokeColor = Colors.Blue; // Цвет линии
        for (int i = 0; i < _selectedPoints.Count - 1; i++)
        {
            var pointA = _gridPoints[_selectedPoints[i]];
            var pointB = _gridPoints[_selectedPoints[i + 1]];
            canvas.DrawLine(pointA.X, pointA.Y, pointB.X, pointB.Y);
        }

        // Рисуем линию от последней выбранной точки к текущему касанию
        // если есть хоть одна выбранная точка и текущее касание не равно null
        if (_selectedPoints.Any() && _currentTouch.HasValue)
        {
            var lastPoint = _gridPoints[_selectedPoints.Last()];
            canvas.DrawLine(lastPoint.X, lastPoint.Y, _currentTouch.Value.X, _currentTouch.Value.Y);
        }
    }
    #endregion

    #region StartPath Method
    public void StartPath(PointF touch)
    {
        _selectedPoints.Clear(); // очищаем выбранные точки
        _currentTouch = touch; // сохраняем текущее касание
        TrySelectPoint(touch); // пытаемся добавить точку в список выбранных
    }
    #endregion

    #region UpdatePath Method
    public void UpdatePath(PointF touch)
    {
        _currentTouch = touch; // обновляем текущее касание
        TrySelectPoint(touch); // пытаемся добавить точку в список выбранных
    }
    #endregion

    #region EndPath Method
    public void EndPath()
    {
        _currentTouch = null; // сбрасываем текущее касание
    } 
    #endregion

    #region TrySelectPoint Method
    private void TrySelectPoint(PointF touch)
    {
        for (int i = 0; i < _gridPoints.Count; i++)
        {
            if (!_selectedPoints.Contains(i) &&                        // если точка ещё не выбрана и
                Distance(touch, _gridPoints[i]) <= CircleRadius * 1.5) // если касание близко к точке
            {
                _selectedPoints.Add(i); // добавляем точку в выбранные
                break; // выходим из цикла, чтобы не выбрать несколько точек за одно касание
            }
        }
    }
    #endregion

    #region Distance Method
    private static float Distance(PointF a, PointF b) =>
        (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2)); // расстояние между двумя точками по теореме Пифагора
    #endregion

    #region GenerateGrid Method
    /// <summary>
    /// Возвращает список точек для графического узора экрана блокировки.
    /// </summary>
    /// <param name="rows">Количество строк.</param>
    /// <param name="columns">Количество столбцов.</param>
    /// <param name="width">Ширина сетки.</param>
    /// <param name="height">Высота сетки.</param>
    /// <returns></returns>
    /// 
    private List<PointF> GenerateGrid(int rows, int columns, int width, int height)
    {
        var points = new List<PointF>();
        // расстояние между точками
        float cellWidth = width / columns;
        float cellHeight = height / rows;
        // смещение от края до первой точки
        var x0 = cellWidth / 2;
        var y0 = cellHeight / 2;

        // Заполнение точек
        for (int row = 0; row < rows; row++)
            for (int column = 0; column < columns; column++)
                points.Add(new PointF(x0 + cellWidth * column, y0 + cellHeight * row));

        return points;
    }
    #endregion

    #region Clear Method
    /// <summary>
    /// Очищает выбранные точки и текущее касание.
    /// </summary>
    public void Clear()
    {
        _selectedPoints.Clear();
        _currentTouch = null;
    } 
    #endregion
}
