namespace StereoApp.Model.Interfaces
{
    public interface ISerializableTo<TSelf, out TSerializedSelf>
        where TSelf : ISerializableTo<TSelf, TSerializedSelf>
        where TSerializedSelf : ISerializableFrom<TSerializedSelf, TSelf>
    {
        public TSerializedSelf ToSerializable();
    }
}
