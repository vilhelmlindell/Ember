namespace Ember.UI;

public enum Alignment
{
    TopLeft,
    Top,
    TopRight,
    BottomLeft,
    Bottom,
    BottomRight,
    Left,
    Right,
    Center
}

/// <summary>
/// Describes the dimensions of a control
/// </summary>
public class Layout
{
    protected bool ShouldCalculateLayout = false;
    
    public Layout()
    {
        X = new Length();
        Y = new Length();
        Width = new Length();
        Height = new Length();
        OriginX = new Length();
        OriginY = new Length();

        X.WasChanged += () => ShouldCalculateLayout = true;
        Y.WasChanged += () => ShouldCalculateLayout = true;
        Width.WasChanged += () => ShouldCalculateLayout = true;
        Height.WasChanged += () => ShouldCalculateLayout = true;
        OriginX.WasChanged += () => ShouldCalculateLayout = true;
        OriginY.WasChanged += () => ShouldCalculateLayout = true;
    }

    public Length X { get; set; }
    public Length Y { get; set; }
    public Length Width { get; set; }
    public Length Height { get; set; }
    public Length OriginX { get; set; }
    public Length OriginY { get; set; }

    public void SetPosition(Alignment alignment)
    {
        X.Unit = LengthUnit.Percent;
        Y.Unit = LengthUnit.Percent;
        switch (alignment)
        {
            case Alignment.TopLeft:
                X.Value = 0f;
                Y.Value = 0f;
                break;
            case Alignment.Top:
                X.Value = 0.5f;
                Y.Value = 0f;
                break;
            case Alignment.TopRight:
                X.Value = 1f;
                Y.Value = 0f;
                break;
            case Alignment.BottomLeft:
                X.Value = 0f;
                Y.Value = 1f;
                break;
            case Alignment.Bottom:
                X.Value = 0.5f;
                Y.Value = 1f;
                break;
            case Alignment.BottomRight:
                X.Value = 1f;
                Y.Value = 1f;
                break;
            case Alignment.Left:
                X.Value = 0f;
                Y.Value = 0.5f;
                break;
            case Alignment.Right:
                X.Value = 1f;
                Y.Value = 0.5f;
                break;
            case Alignment.Center:
                X.Value = 0.5f;
                Y.Value = 0.5f;
                break;
        }
    }
    public void SetOrigin(Alignment alignment)
    {
        OriginX.Unit = LengthUnit.Percent;
        OriginY.Unit = LengthUnit.Percent;
        switch (alignment)
        {
            case Alignment.TopLeft:
                OriginX.Value = 0f;
                OriginY.Value = 0f;
                break;
            case Alignment.Top:
                OriginX.Value = 0.5f;
                OriginY.Value = 0f;
                break;
            case Alignment.TopRight:
                OriginX.Value = 1f;
                OriginY.Value = 0f;
                break;
            case Alignment.BottomLeft:
                OriginX.Value = 0f;
                OriginY.Value = 1f;
                break;
            case Alignment.Bottom:
                OriginX.Value = 0.5f;
                OriginY.Value = 1f;
                break;
            case Alignment.BottomRight:
                OriginX.Value = 1f;
                OriginY.Value = 1f;
                break;
            case Alignment.Left:
                OriginX.Value = 0f;
                OriginY.Value = 0.5f;
                break;
            case Alignment.Right:
                OriginX.Value = 1f;
                OriginY.Value = 0.5f;
                break;
            case Alignment.Center:
                OriginX.Value = 0.5f;
                OriginY.Value = 0.5f;
                break;
        }
    }

    protected virtual void CalculateLayout() {}
}
