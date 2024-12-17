using Unity.Netcode;

namespace Objects
{
    
    [System.Serializable]
    public struct ClearedMinesData : INetworkSerializable
    {
        public int clearedMinesEasy;
        public int clearedMinesNormal;
        public int clearedMinesHard;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clearedMinesEasy);
            serializer.SerializeValue(ref clearedMinesNormal);
            serializer.SerializeValue(ref clearedMinesHard);
        }
    }
}