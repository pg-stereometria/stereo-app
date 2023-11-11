namespace StereoApp.Model.Interfaces
{
    public interface ISerializableFrom<TSelf, out TActualType>
        where TSelf : ISerializableFrom<TSelf, TActualType>
        where TActualType : ISerializableTo<TActualType, TSelf>
    {
        public TActualType ToActualType();
    }
}
