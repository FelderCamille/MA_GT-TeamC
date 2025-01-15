using Unity.Collections;
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
        public int clearedMinesEasy;
        public int clearedMinesMedium;
        public int clearedMinesHard;
        public int explodedMines;
        public int notClearedMines;
        public int placedMines;
        public FixedString32Bytes playerName;

        public int ClearedMinesEasyScore => clearedMinesEasy * Constants.Score.ClearMineEasySuccess;
        public int ClearedMinesMediumScore => clearedMinesMedium * Constants.Score.ClearMineMediumSuccess;
        public int ClearedMinesHardScore => clearedMinesHard * Constants.Score.ClearMineHardSuccess;
        public int ExplodedMinesScore => explodedMines * Constants.Score.MineExplosion;
        public int NotClearedMinesScore => notClearedMines * Constants.Score.MineNotCleared;
        public int PlacedMinesScore => placedMines * Constants.Score.MinePlaced;

        public int TotalScore => ClearedMinesEasyScore 
                                 + ClearedMinesMediumScore 
                                 + ClearedMinesHardScore 
                                 + ExplodedMinesScore 
                                 + NotClearedMinesScore
                                 + PlacedMinesScore;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref clearedMinesEasy);
            serializer.SerializeValue(ref clearedMinesMedium);
            serializer.SerializeValue(ref clearedMinesHard);
            serializer.SerializeValue(ref explodedMines);
            serializer.SerializeValue(ref notClearedMines);
            serializer.SerializeValue(ref placedMines);
            serializer.SerializeValue(ref playerName);
        }
    }
}