using Unity.Netcode;

namespace Objects
{
    
    [System.Serializable]
    public struct LandmineDifficultyData : INetworkSerializable
    {
        public int difficulty;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref difficulty);
        }
    }
}