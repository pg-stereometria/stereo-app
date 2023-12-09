namespace StereoApp.Model
{
    public abstract class SolidFigure
    {
        // Unity's .NET does not currently support covariant return types
        public abstract SerializedSolidFigure ToSerializableFigure();
    }

    public abstract class SerializedSolidFigure
    {
        // Unity's .NET does not currently support covariant return types
        public abstract SolidFigure ToActualFigure();
    }
}
