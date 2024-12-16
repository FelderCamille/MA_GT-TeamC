using Unity.Netcode;

namespace Objects
{
    [System.Serializable]
    public struct GameResultsData : INetworkSerializable
    {
        public PlayerResultData player1Result;
        public PlayerResultData player2Result;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref player1Result);
            serializer.SerializeValue(ref player2Result);
        }
    }

    [System.Serializable]
    public struct PlayerResultData : INetworkSerializable
    {
        public ulong clientId;
        public int clearedMines;
        public int explodedMines;
        public int ClearedMinesScore => clearedMines * Constants.Score.ClearMineSuccess;
        public int ExplodedMinesScore => explodedMines * Constants.Score.MineExplosion;
        public int TotalScore => ClearedMinesScore + ExplodedMinesScore;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref clearedMines);
            serializer.SerializeValue(ref explodedMines);
        }
    }
}