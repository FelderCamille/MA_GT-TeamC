using Objects;
using Unity.Collections;
using Unity.Netcode;

namespace Core
{
    public class GameParametersManager: NetworkBehaviour
    {
        
        public static GameParametersManager Instance;
        
        private readonly NetworkVariable<int> _budget = new(Constants.GameSettings.DefaultMoney);
        private readonly NetworkVariable<int> _mapTheme = new(0); // MapTheme.Nature = 0; MapTheme.War = 1
        private readonly NetworkVariable<FixedString32Bytes> _player1Name = new("");
        private readonly NetworkVariable<FixedString32Bytes> _player2Name = new("");

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public int Budget
        {
            get => _budget.Value;
            set => _budget.Value = value;
        }
        
        public MapTheme MapTheme
        {
            get => (MapTheme) _mapTheme.Value;
            set => _mapTheme.Value = (int) value;
        }
        
        public string Player1Name
        {
            get => _player1Name.Value.ToString();
            set
            {
                if (value == "") value = Constants.GameSettings.DefaultPlayer1Name;
                _player1Name.Value = value;
            }
        }

        public string Player2Name
        {
            get => _player2Name.Value.ToString();
            private set => _player2Name.Value = value;
        }
        
        [Rpc(SendTo.Server)]
        public void SetPlayer2NameRpc(string value)
        {
            if (value == "") value = Constants.GameSettings.DefaultPlayer2Name;
            Player2Name = value;
        }
    }
}