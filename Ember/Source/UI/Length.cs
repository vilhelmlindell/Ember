using System;
namespace Ember.UI;

public enum LengthUnit
{
    Pixels,
    Percent
}

public class Length
{
    private float _value;
    private LengthUnit _unit;

    public Length()
    {
        _value = 0f;
        _unit = LengthUnit.Pixels;
    }
    public Length(float value = 0f, LengthUnit lengthUnit = LengthUnit.Pixels)
    {
        Value = value;
        Unit = lengthUnit;
    }
    
    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            WasChanged?.Invoke();
        }
    }
    public LengthUnit Unit
    {
        get => _unit;
        set
        {
            _unit = value;
            WasChanged?.Invoke();
        }
    }

    public Action WasChanged;
}
