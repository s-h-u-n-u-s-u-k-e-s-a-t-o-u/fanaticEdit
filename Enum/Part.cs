namespace fanaticEdit.Enum;

/// <summary>
/// パート区分
/// </summary>
public enum Part
{
    Main = 1,
    Encore = 2
}


public static class PartExtensions
{
    public static int ToInt(this Part part)
    {      
        return (int)part;
    }
}

public static class PartParser { 
    public static Part Parse(int value) {
        return value switch
        {
            1 => Part.Main,
            2 => Part.Encore,
            _ => throw new ArgumentOutOfRangeException(nameof(Part), $"Invalid part value: {value}")
        };
    }
}
