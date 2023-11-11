using StereoApp.Model.Interfaces;
using UnityEngine;

namespace StereoApp.Model
{
    public static class JsonSerializable
    {
        public static TSelf FromJson<TSelf, TSerializedSelf>(string json)
            where TSelf : ISerializableTo<TSelf, TSerializedSelf>
            where TSerializedSelf : ISerializableFrom<TSerializedSelf, TSelf>
        {
            return JsonUtility.FromJson<TSerializedSelf>(json).ToActualType();
        }

        public static string ToJson<TSelf, TSerializedSelf>(
            this ISerializableTo<TSelf, TSerializedSelf> self
        )
            where TSelf : ISerializableTo<TSelf, TSerializedSelf>
            where TSerializedSelf : ISerializableFrom<TSerializedSelf, TSelf>
        {
            return JsonUtility.ToJson(self.ToSerializable());
        }
    }
}
