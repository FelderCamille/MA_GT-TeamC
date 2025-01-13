using Unity.Netcode;

namespace Objects
{
    
    [System.Serializable]
    public struct MinesStatisticalData : INetworkSerializable
    {
        public int clearedMinesEasy;
        public int clearedMinesNormal;
        public int clearedMinesHard;
        public int explodedMines;
        public int placedMines;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clearedMinesEasy);
            serializer.SerializeValue(ref clearedMinesNormal);
            serializer.SerializeValue(ref clearedMinesHard);
            serializer.SerializeValue(ref explodedMines);
            serializer.SerializeValue(ref placedMines);
        }
    }
}